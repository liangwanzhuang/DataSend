using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Collections;
using System.Data;

namespace ModelInfo
{
    public class Bubbleinfo
    {
        public DataBaseLayer db = new DataBaseLayer();
        //泡药环节手动添加条码 new
        public int addbubble_new(string tisanebarcode, string bubbleperson, string mark, string wateryield)
        {
            string tisaneid = tisanebarcode.Substring(4, 10);

            string originaltisaneid = tisaneid.TrimStart('0');

            //获取处方泡药时间
            int soaktime = 0;
            string s_sql = "select top 1 soaktime from prescription where id ='" + originaltisaneid.Trim() + "'";
            SqlDataReader sr_s = db.get_Reader(s_sql);
            if (sr_s.Read())
            {
                soaktime = Int16.Parse(sr_s["soaktime"].ToString());
            }
            String sql = "";
            string per = bubbleperson.Substring(6);


            string employeeid = "";
            string str4 = "select id from employee where EmNumAName ='" + bubbleperson + "'";
            SqlDataReader sr4 = db.get_Reader(str4);
            if (sr4.Read())
            {
                employeeid = sr4["id"].ToString();
            }

            int isneedreview = db.cmd_Execute_value("select isneedreview from isneedcheck");
            if (isneedreview == 1)//1 不经过复核环节，0经过
            {
                string reviewsql = "insert into Audit(ReviewPer,AuditTime,barcode,pid,imgname,AuditStatus,employeeId) values('自动复核','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + tisanebarcode + "','" + tisaneid + "','','1','0')";
                string reviewsql2 = "update prescription set doperson ='自动复核', curstate = '复核'  where id = '" + tisaneid + "'";
                db.cmd_Execute(reviewsql);
                db.cmd_Execute(reviewsql2);
            }

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            string strtime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");//
            int end = 0;//正在泡
            string str2 = "select * from prescription where  id = '" + originaltisaneid + "' and (curstate='" + "复核".Trim() + "' or " + "curstate='" + "开始泡药".Trim() + "')";
            //not in (select pid from InvalidPrescription) and
            SqlDataReader sr2 = db.get_Reader(str2);

            if (sr2.Read())
            {

                string str5 = "select * from Audit where  pid='" + originaltisaneid + "'";

                SqlDataReader sr5 = db.get_Reader(str5);

                if (sr5.Read())
                {

                    string str = "select * from bubble where pid = '" + originaltisaneid + "'";
                    SqlDataReader sr = db.get_Reader(str);
                    if (sr.Read())
                    {
                        //根据处方号获取当前时间与开始泡药时间间隔
                        int flag_minute = 0;
                        string s_sql_m = "select DATEDIFF(MINUTE,starttime,GETDATE()) as min_b from bubble where pid='" + originaltisaneid.Trim() + "'";
                        SqlDataReader sr_sm = db.get_Reader(s_sql_m);
                        if (sr_sm.Read())
                        {

                            flag_minute = Int16.Parse(sr_sm["min_b"].ToString());

                        }
                        if (sr["bubblestatus"].ToString() == "1")
                        {
                            end = 4;//泡药已完成
                        }
                        else
                        {

                            //添加强制泡药开关判断，关闭时执行下面代码
                            string flag_global = "1";//默认强制泡药开关开启 0为关闭
                            string sql_global = "select top 1 global_medi_is_flag from  dbo.tb_sys_add_setting where  sys_add_id='1'";
                            SqlDataReader sr_global = db.get_Reader(sql_global);
                            if (sr_global.Read())
                            {
                                try
                                {
                                    flag_global = sr_global["global_medi_is_flag"].ToString();
                                }
                                catch
                                {
                                    flag_global = "1";
                                }
                            }
                            //强制泡药开关关闭状态
                            if (flag_global == "0" || (flag_global == "1" && flag_minute > soaktime))
                            {
                                DateTime d1 = Convert.ToDateTime(strtime);//当前时间
                                DateTime d2 = Convert.ToDateTime(sr["starttime"].ToString());//开始时间

                                TimeSpan d3 = d1.Subtract(d2);//泡药时间

                                int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());//转化为分钟数


                                string str1 = "update bubble set bubblestatus =1,endDate='" + strtime + "',doingtime='" + doingtime + "' where pid = '" + originaltisaneid + "'";

                                if (db.cmd_Execute(str1) == 1)
                                {
                                    end = 2;//泡药成功，但分配机组不成功
                                    //此处添加煎药机选择模式判断
                                    string flag_dec_machine = "1";//默认煎药机选择模式为：自动分配煎药机
                                    string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
                                    dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
                                    SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);
                                    if (sr_dec_machine_select.Read())
                                    {
                                        try
                                        {

                                            flag_dec_machine = sr_dec_machine_select["decocting_machine_mode_select"].ToString();

                                        }
                                        catch
                                        {
                                            flag_dec_machine = "1";
                                        }
                                    }
                                    //分配煎药机模式
                                    if (flag_dec_machine == "1")
                                    {
                                        //  string machineid = distributionmachine();
                                        string machineid = "";
                                        DataTable t = findMachineByStartAndFree();//优先分配空闲煎药机，
                                        try
                                        {
                                            machineid = t.Rows[0]["machineid"].ToString();
                                        }
                                        catch
                                        {
                                            //如果煎药机都处于忙碌状态执行分配忙碌煎药机算法
                                            DataTable tt = findMachineByStartAndFree_busy();//优先分配空闲煎药机，
                                            machineid = tt.Rows[0]["machineid"].ToString();
                                        }




                                        string unitnum = "select unitnum from machine where id = '" + machineid + "'";
                                        SqlDataReader sdr10 = db.get_Reader(unitnum);
                                        string ut = "";
                                        if (sdr10.Read())
                                        {
                                            ut = sdr10["unitnum"].ToString();
                                        }

                                        machineid = db.sp_Execute_ave("sp_cf_jz", "@jz_id").ToString();

                                        string str3 = "insert into tisaneunit(pid,machineid,unitnum) values('" + originaltisaneid + "','" + machineid + "','" + ut + "')";

                                        if (db.cmd_Execute(str3) == 1)
                                        {
                                            end = 6;//泡药成功，且成功分配机组

                                            string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + originaltisaneid + "'";

                                            db.cmd_Execute(sql7);//更新处方表里的当前状态
                                        }
                                    }
                                    //不分配煎药机模式
                                    if (flag_dec_machine == "0")
                                    {
                                        /* string machineid = distributionmachine();
                                         string unitnum = "select unitnum from machine where id = '" + machineid + "'";
                                         SqlDataReader sdr10 = db.get_Reader(unitnum);
                                         string ut = "";
                                         if (sdr10.Read())
                                         {
                                             ut = sdr10["unitnum"].ToString();
                                         }

                                         machineid = db.sp_Execute_ave("sp_cf_jz", "@jz_id").ToString();

                                         string str3 = "insert into tisaneunit(pid,machineid,unitnum) values('" + originaltisaneid + "','" + machineid + "','" + ut + "')";

                                         if (db.cmd_Execute(str3) == 1)
                                         {
                                             end = 8;//泡药成功且未分配机组

                                             string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + originaltisaneid + "'";

                                             db.cmd_Execute(sql7);//更新处方表里的当前状态
                                             string sql8 = "update tisaneunit set machineid='0'  where pid = '" + originaltisaneid + "'";

                                             db.cmd_Execute(sql8);//更新处方表里的当前状态

                                             string sql9 = "update tisaneinfo set machineid='0'  where pid = '" + originaltisaneid + "'";

                                             db.cmd_Execute(sql9);//更新处方表里的当前状态
                                         }
                                         */

                                        //    DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//当前时间
                                        //  DateTime d2 = Convert.ToDateTime(dt.Rows[0]["starttime"].ToString());//开始时间

                                        //   TimeSpan d3 = d1.Subtract(d2);//泡药时间

                                        //   int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());//转化为分钟数

                                        //   DataBaseLayer db = new DataBaseLayer();
                                        string sql_no = "update bubble set bubblestatus='" + "1" + "',endDate=" + "getdate()" + ",doingtime='" + doingtime + "' where pid='" + originaltisaneid.ToString().Trim() + "'";

                                        int count = db.cmd_Execute(sql_no);
                                        // string sql4 = "select pid from bubble where pid=" + originaltisaneid.ToString().Trim() + "'";
                                        // DataTable dt4 = db.get_DataTable(sql4);
                                        string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + originaltisaneid.ToString() + "'";

                                        // db.cmd_Execute(sql7);//更新处方表里的当前状态
                                        //  result = -10;//不分配煎药机模式下，返回值。
                                    }
                                }

                            }
                            //强制泡药开关开启状态


                            else if (flag_global == "1" && flag_minute < soaktime)
                            {
                                end = 9;//强制泡药已开启，无法强制结束泡药流程。
                            }
                            else { end = 0; }
                        }

                    }
                    else
                    {
                        sql = "INSERT INTO [Bubble](pid,starttime,bubbleperson,mark,waterYield,employeeId) VALUES('" + originaltisaneid + "','" + strtime + "','" + per + "','" + mark + "','" + wateryield + "','" + employeeid + "')";

                        if (db.cmd_Execute(sql) == 1)
                        {
                            end = 1;//正在泡
                            string sql7 = "update prescription set doperson ='" + per + "',curstate = '开始泡药'  where id = '" + originaltisaneid + "'";

                            if (db.cmd_Execute(sql7) == 1) //更新处方表里
                            {
                                //删除复合表里的复合的数据

                                // string sql8 = "delete from AgainPrescriptionCheckState where prescriptionId = '" + originaltisaneid + "'";
                                //   db.cmd_Execute(sql8);
                            }

                        }

                    }

                }
                else
                {
                    end = 7;//该煎药单号还没完成复核
                }
            }
            else
            {
                end = 3;//煎药单号不存在
            }


            return end;
        }
        public int addbubble(string tisanebarcode, string bubbleperson, string mark, string wateryield)
        {
            string tisaneid = tisanebarcode.Substring(4, 10);

            string originaltisaneid = tisaneid.TrimStart('0');

            String sql = "";

            string per = bubbleperson.Substring(6);


            string employeeid = "";
            string str4 = "select id from employee where EmNumAName ='" + bubbleperson + "'";
            SqlDataReader sr4 = db.get_Reader(str4);

            if (sr4.Read())
            {

                employeeid = sr4["id"].ToString();

            }

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            string strtime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");//

            int end = 0;//正在泡
            string str2 = "select * from prescription where id not in (select pid from InvalidPrescription) and id = '" + originaltisaneid + "'";
            SqlDataReader sr2 = db.get_Reader(str2);

            if (sr2.Read())
            {

                string str5 = "select * from Audit where  pid='" + originaltisaneid + "'";

                SqlDataReader sr5 = db.get_Reader(str5);

                if (sr5.Read())
                {

                    string str = "select * from bubble where pid = '" + originaltisaneid + "'";
                    SqlDataReader sr = db.get_Reader(str);
                    if (sr.Read())
                    {
                        if (sr["bubblestatus"].ToString() == "1")
                        {
                            end = 4;//泡药已完成
                        }
                        else
                        {

                            //添加强制泡药开关判断，关闭时执行下面代码
                            string flag_global = "1";//默认强制泡药开关开启 0为关闭
                            string sql_global = "select top 1 global_medi_is_flag from  dbo.tb_sys_add_setting where  sys_add_id='1'";
                            SqlDataReader sr_global = db.get_Reader(sql_global);
                            if (sr_global.Read())
                            {
                                try
                                {
                                    flag_global = sr_global["global_medi_is_flag"].ToString();
                                }
                                catch
                                {
                                    flag_global = "1";
                                }
                            }
                            //强制泡药开关关闭状态
                            if (flag_global == "0")
                            {
                                DateTime d1 = Convert.ToDateTime(strtime);//当前时间
                                DateTime d2 = Convert.ToDateTime(sr["starttime"].ToString());//开始时间

                                TimeSpan d3 = d1.Subtract(d2);//泡药时间

                                int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());//转化为分钟数


                                string str1 = "update bubble set bubblestatus =1,endDate='" + strtime + "',doingtime='" + doingtime + "' where pid = '" + originaltisaneid + "'";

                                if (db.cmd_Execute(str1) == 1)
                                {
                                    end = 2;//泡药成功，但分配机组不成功
                                    //此处添加煎药机选择模式判断
                                    string flag_dec_machine = "1";//默认煎药机选择模式为：自动分配煎药机
                                    string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
                                    dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
                                    SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);
                                    if (sr_dec_machine_select.Read())
                                    {
                                        try
                                        {

                                            flag_dec_machine = sr_dec_machine_select["global_medi_is_flag"].ToString();

                                        }
                                        catch
                                        {
                                            flag_dec_machine = "1";
                                        }
                                    }
                                    //分配煎药机模式
                                    if (flag_dec_machine == "1")
                                    {
                                        string machineid = distributionmachine();
                                        string unitnum = "select unitnum from machine where id = '" + machineid + "'";
                                        SqlDataReader sdr10 = db.get_Reader(unitnum);
                                        string ut = "";
                                        if (sdr10.Read())
                                        {
                                            ut = sdr10["unitnum"].ToString();
                                        }

                                        machineid = db.sp_Execute_ave("sp_cf_jz", "@jz_id").ToString();

                                        string str3 = "insert into tisaneunit(pid,machineid,unitnum) values('" + originaltisaneid + "','" + machineid + "','" + ut + "')";

                                        if (db.cmd_Execute(str3) == 1)
                                        {
                                            end = 6;//泡药成功，且成功分配机组

                                            string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + originaltisaneid + "'";

                                            db.cmd_Execute(sql7);//更新处方表里的当前状态
                                        }
                                    }
                                    //不分配煎药机模式
                                    if (flag_dec_machine == "0")
                                    {
                                        string machineid = distributionmachine();
                                        string unitnum = "select unitnum from machine where id = '" + machineid + "'";
                                        SqlDataReader sdr10 = db.get_Reader(unitnum);
                                        string ut = "";
                                        if (sdr10.Read())
                                        {
                                            ut = sdr10["unitnum"].ToString();
                                        }

                                        machineid = db.sp_Execute_ave("sp_cf_jz", "@jz_id").ToString();

                                        string str3 = "insert into tisaneunit(pid,machineid,unitnum) values('" + originaltisaneid + "','" + machineid + "','" + ut + "')";

                                        if (db.cmd_Execute(str3) == 1)
                                        {
                                            end = 8;//泡药成功且未分配机组

                                            string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + originaltisaneid + "'";

                                            db.cmd_Execute(sql7);//更新处方表里的当前状态
                                            string sql8 = "update tisaneunit set machineid='0'  where pid = '" + originaltisaneid + "'";

                                            db.cmd_Execute(sql8);//更新处方表里的当前状态

                                            string sql9 = "update tisaneinfo set machineid='0'  where pid = '" + originaltisaneid + "'";

                                            db.cmd_Execute(sql9);//更新处方表里的当前状态
                                        }
                                    }
                                }

                            }
                            //强制泡药开关开启状态
                            else if (flag_global == "1")
                            {
                                end = 9;//强制泡药已开启，无法强制结束泡药流程。
                            }
                            else { end = 0; }
                        }

                    }
                    else
                    {
                        sql = "INSERT INTO [Bubble](pid,starttime,bubbleperson,mark,waterYield,employeeId) VALUES('" + originaltisaneid + "','" + strtime + "','" + per + "','" + mark + "','" + wateryield + "','" + employeeid + "')";

                        if (db.cmd_Execute(sql) == 1)
                        {
                            end = 1;//正在泡
                            string sql7 = "update prescription set doperson ='" + per + "',curstate = '开始泡药'  where id = '" + originaltisaneid + "'";

                            if (db.cmd_Execute(sql7) == 1) //更新处方表里
                            {
                                //删除复合表里的复合的数据

                                // string sql8 = "delete from AgainPrescriptionCheckState where prescriptionId = '" + originaltisaneid + "'";
                                //   db.cmd_Execute(sql8);
                            }

                        }

                    }

                }
                else
                {
                    end = 7;//该煎药单号还没完成复核
                }
            }
            else
            {
                end = 3;//煎药单号不存在
            }


            return end;

        }
        #region 泡药大屏显示信息

        public DataTable DrugDisplayInfo()
        {

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "select * from (select (select pid from bubble where pid= p.id) as ID,Pspnum,customid,delnum,(select bubblestatus from bubble where pid= p.id) as bubblestatus,(select doingtime from bubble where pid = p.id) as doingtime,(select starttime from bubble where pid = p.id) as starttime,(select endDate from bubble where pid = p.id) as endDate,(select warningstatus from bubble where pid = p.id) as warningstatus,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(SELECT mark FROM bubble WHERE pid = p.id) as mark,(select hnum from hospital as h where h.id = p.hospitalid ) as hnum,(select hname from hospital as h where h.id = p.hospitalid  ) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql += "diagresult,(select wateryield from bubble where pid= p.id) as wateryield,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,RemarksA,RemarksB";
            sql += " from prescription as p where  id not in (select pid from InvalidPrescription) and  id in (select pid from bubble ) and id not in (select pid from tisaneinfo ) and p.Hospitalid  in (select id from hospital where DrugDisplayState='0') ) as bu where bu.starttime between '" + strS + "' and '" + strS2 + "' order by starttime desc";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        public DataTable DrugDisplayInfo_1()
        {

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间

            //string strS = "2016-03-01 00:00:00";
            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");


            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "select * from (select (select top 1 pid from bubble where pid= p.id) as ID,Pspnum,customid,delnum,(select bubblestatus from bubble where pid= p.id) as bubblestatus,(select top 1 doingtime from bubble where pid = p.id) as doingtime,(select top 1 starttime from bubble where pid = p.id) as starttime,(select top 1 endDate from bubble where pid = p.id) as endDate,(select top 1 warningstatus from bubble where pid = p.id) as warningstatus,(SELECT top 1 bubbleperson FROM bubble WHERE pid = p.id) as bp,(SELECT top 1 mark FROM bubble WHERE pid = p.id) as mark,(select top 1 hnum from hospital as h where h.id = p.hospitalid ) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid  ) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql += "diagresult,(select wateryield from bubble where pid= p.id) as wateryield,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,RemarksA,RemarksB";
            sql += " from prescription as p where  p.curstate in('开始泡药','泡药完成') and  id in (select pid from bubble ) and id not in (select pid from tisaneinfo ) and p.Hospitalid  in (select id from hospital where DrugDisplayState='0') )as bu where bu.starttime between '" + strS + "' and '" + strS2 + "' order by bu.starttime desc";
            db.write_log_txt("大屏泡药SQL: " + sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion

        #region 煎药机算法
        //   DataBaseLayer ddd = new DataBaseLayer();
        public int findMachineByStartAndFree_str()
        {
            int num = 0;
            // int get_int = 0;
            string str1 = "SELECT  count(id) as num  FROM machine WHERE mark = 0 AND usingstatus = '启用' AND status = '空闲' and mark = 0 ";
            db.write_log_txt(str1);
            num = db.cmd_Execute_value(str1);
            db.write_log_txt(num.ToString());
            if (num > 0)
            {
                string str2 = "SELECT   top 1 id  FROM machine WHERE (mark = 0) AND (usingstatus = '启用') AND (status = '空闲') and mark = 0 order by id desc";
                db.write_log_txt(str2);
                return db.cmd_Execute_value(str2);
            }
            else
            {
                //按已煎处方分配煎药机
                string str_1 = "select top 1 isnull(b.machineid,0) as num from " +
 " (select a.machineid,count(a.pid) as num from  (select p.ID,t.pid,t.machineid  from prescription p inner join tisaneinfo t on t.pid =p.id where p.curstate ='煎药完成' ) a " +
 " group by a.machineid ) b ORDER BY b.num asc";
                db.write_log_txt("分配煎药机—1：" + str_1);
                d.write_log_txt("分配煎药机—1—1" + db.cmd_Execute(str_1).ToString());
                int flag = db.cmd_Execute(str_1);
                if (flag > 0)
                {
                    d.write_log_txt("分配煎药机—1—1-1" + flag.ToString());
                    return flag;
                }
                else if (flag == 0)
                {
                    string str_2 = "select top 1 isnull(b.machineid,0) as num from " +
 " (select a.machineid,count(a.pid) as num from  (select p.ID,t.pid,t.machineid  from prescription p inner join tisaneinfo t on t.pid =p.id where p.curstate ='开始煎药' ) a " +
 " group by a.machineid ) b ORDER BY b.num asc";
                    db.write_log_txt("分配煎药机—2：" + str_2);
                    if (db.cmd_Execute(str_2) > 0)
                    {
                        return db.cmd_Execute(str_2);
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion

        #region 根据处方id查询分配的煎药机

        public DataTable findTisaneunitByPid(int pid)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";

            string str_s = "select count(id) as mid from prescription where id='" + pid.ToString() + "' and " + "curstate in(" + "'" + "泡药完成" + "','" + "开始煎药" + "')";
            db.write_log_txt("煎药忙碌分配煎药机默认错误1：" + str_s);
            SqlDataReader s1 = db.get_Reader(str_s);
            string p_c = "0";
            if (s1.Read())
            {
                p_c = s1["mid"].ToString();
            }
            db.write_log_txt("煎药忙碌分配煎药机默认错误2：" + p_c);
            if (p_c != "0")
            {

                string sql_m = "  SELECT   id, pid, machineid, packstatus, roomnum, unitnum,(SELECT top 1 machinename FROM machine WHERE (id = t.machineid)) as machinename, endDate, (SELECT " +
                "top 1 machinename FROM machine WHERE (id = t.machineid)) AS mname FROM tisaneunit AS t WHERE pid='" + pid + "'";
                SqlDataReader s2 = db.get_Reader(sql_m);
                string m = "0";
                if (s2.Read())
                {
                    m = s2["machineid"].ToString();
                }
                if (m == "0")
                {
                    string sql1 = "delete from tisaneunit where pid =" + pid.ToString();
                    db.write_log_txt("煎药忙碌分配煎药机默认错误3：" + sql1);
                    db.cmd_Execute(sql1);
                }
            }

            string sql = "SELECT   id, pid, machineid, packstatus, roomnum, unitnum,(SELECT machinename FROM machine WHERE (id = t.machineid)) as machinename, endDate, (SELECT machinename FROM machine WHERE (id = t.machineid)) AS mname FROM tisaneunit AS t WHERE pid=" + pid;



            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion

        public int insert_tisaneunit(string pid, string machineid, string packstatus)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "insert into tisaneunit(pid,machineid,packstatus) select '" + pid + "'," + "'" + machineid + "'," + "'" + packstatus + "'";

            db.write_log_txt("插入煎药相关信息：" + sql);

            return db.cmd_Execute(sql);
        }



        #region 修改煎药机

        public int updateTisaneunitByMachineid(int id, int machineid)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "update tisaneunit set machineid=" + machineid.ToString().Trim() + " where pid=" + id.ToString().Trim();
            return db.cmd_Execute(sql);
        }
        #endregion
        /*  #region 修改煎药机

        public int updateTisaneunitByMachineid_c(int id, int machineid)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "update tisaneunit set machineid=" + machineid + " where pid=" + id;



            return db.cmd_Execute(sql);
        }
        #endregion
       */

        public int updateBubble(int id, int status, string endDate, string starttime)
        {

            DateTime d1 = Convert.ToDateTime(endDate);//当前时间
            DateTime d2 = Convert.ToDateTime(starttime);//开始时间

            TimeSpan d3 = d1.Subtract(d2);//泡药时间

            int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());//转化为分钟数


            string sql = "update bubble set bubblestatus='" + status + "',endDate='" + endDate + "',doingtime='" + doingtime + "' where id=" + id.ToString().Trim();
            int count = db.cmd_Execute(sql);
            if (count > 0)
            {

                string sql4 = "select pid from bubble where id=" + id.ToString().Trim();
                DataTable dt4 = db.get_DataTable(sql4);
                if (dt4.Rows.Count > 0)
                {
                    //string machineid = distributionmachine();
                    DataTable t = findMachineByStartAndFree_busy();
                    string machineid = t.Rows[0]["machineid"].ToString();
                    string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
                    string flag_dec_machine = "1";
                    string str3 = "";
                    dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
                    SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);
                    if (sr_dec_machine_select.Read())
                    {
                        try
                        {

                            flag_dec_machine = sr_dec_machine_select["decocting_machine_mode_select"].ToString();

                        }
                        catch
                        {
                            flag_dec_machine = "1";
                        }
                    }
                    //分配煎药机模式
                    if (flag_dec_machine == "1")
                    {
                        str3 = "insert into tisaneunit(pid,machineid) values('" + dt4.Rows[0]["pid"].ToString() + "','" + machineid + "')";
                    }
                    if (flag_dec_machine == "0")
                    {
                        // str3 = "insert into tisaneunit(pid,machineid) values('" + dt4.Rows[0]["pid"].ToString() + "','" +"0" + "')";
                    }


                    if (db.cmd_Execute(str3) == 1)
                    {

                        string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + dt4.Rows[0]["pid"].ToString() + "'";

                        db.cmd_Execute(sql7);//更新处方表里的当前状态


                    }
                }




            }


            return count;

        }


        //扫描枪调用
        DataBaseLayer d = new DataBaseLayer();
        public int updateBubble_scan(int id, int status, string endDate, string starttime, string machineid)
        {
            // d.write_log_txt("1");
            DateTime d1 = Convert.ToDateTime(endDate);//当前时间
            DateTime d2 = Convert.ToDateTime(starttime);//开始时间

            TimeSpan d3 = d1.Subtract(d2);//泡药时间
            string[] arydoingtime = d3.TotalMinutes.ToString().Split('.');//转化为分钟数
            string sql = "update bubble set bubblestatus='" + status + "',endDate='" + endDate + "',doingtime='" + arydoingtime[0] + "' where id=" + id.ToString().Trim();
            d.write_log_txt(sql);
            int count = db.cmd_Execute(sql);
            //  d.write_log_txt(count.ToString());
            if (count > 0)
            {

                string sql4 = "select pid from bubble where id=" + id.ToString().Trim();
                DataTable dt4 = db.get_DataTable(sql4);
                if (dt4.Rows.Count > 0)
                {
                    // string machineid = distributionmachine();
                    string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
                    string flag_dec_machine = "1";
                    string str3 = "";
                    dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
                    SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);

                    if (sr_dec_machine_select.Read())
                    {
                        try
                        {

                            flag_dec_machine = sr_dec_machine_select["decocting_machine_mode_select"].ToString();

                        }
                        catch
                        {
                            flag_dec_machine = "1";
                        }
                    }
                    //分配煎药机模式
                    if (flag_dec_machine == "1")
                    {
                        str3 = "insert into tisaneunit(pid,machineid) values('" + dt4.Rows[0]["pid"].ToString() + "','" + machineid + "')";
                        d.write_log_txt(str3);
                        if (db.cmd_Execute(str3) == 1)
                        {

                            string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + dt4.Rows[0]["pid"].ToString() + "'";

                            db.cmd_Execute(sql7);//更新处方表里的当前状态


                        }
                    }
                    //不分配煎药机模式
                    if (flag_dec_machine == "0")
                    {
                        string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + dt4.Rows[0]["pid"].ToString() + "'";

                        db.cmd_Execute(sql7);//更新处方表里的当前状态
                    }



                }




            }


            return count;

        }
        public int updateBubble_scan_auto(int id, int status, string endDate, string starttime, string machineid)
        {
            // d.write_log_txt("1");
            DateTime d1 = Convert.ToDateTime(endDate);//当前时间
            DateTime d2 = Convert.ToDateTime(starttime);//开始时间
            d.write_log_txt("ID:" + id.ToString());
            TimeSpan d3 = d1.Subtract(d2);//泡药时间
            string[] arydoingtime = d3.TotalMinutes.ToString().Split('.');//转化为分钟数
            string sql = "update bubble set bubblestatus='" + status + "',endDate='" + endDate + "',doingtime='" + arydoingtime[0] + "' where id=" + id.ToString().Trim();

            int count = db.cmd_Execute(sql);
            //  d.write_log_txt(count.ToString());
            // if (count > 0)
            // {

            string sql4 = "select pid from bubble where id=" + id.ToString();
            db.write_log_txt("scan_auto:" + sql4);
            DataTable dt4 = db.get_DataTable(sql4);

            if (dt4.Rows.Count > 0)
            {
                // string machineid = distributionmachine();

                string str3 = "";
                str3 = "insert into tisaneunit(pid,machineid) values('" + dt4.Rows[0]["pid"].ToString() + "','" + machineid + "')";
                d.write_log_txt("测试泡药完成自动分配煎药机：" + str3);
                if (db.cmd_Execute(str3) == 1)
                {
                    string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + dt4.Rows[0]["pid"].ToString() + "'";
                    db.cmd_Execute(sql7);//更新处方表里的当前状态
                }
                //   }
            }
            return count;

        }
        public int addbubble(int userid, string wordDate, string barcode, string wordcontent, string tisaneNum, string imgname, string waterYield, string userName)
        {
            string sql = "insert into bubble(employeeId,starttime,barcode,wordcontent,pid,imgname,waterYield,bubblestatus,doingtime,bubbleperson) values('" + userid.ToString().Trim() + "','" + wordDate + "','" + barcode + "','" + wordcontent + "','" + tisaneNum + "','" + imgname + "','" + waterYield + "','0','0" + "','" + userName + "')";
            string sql2 = "update prescription set doperson ='" + userName + "',curstate = '开始泡药'  where id = '" + tisaneNum + "'";
            db.cmd_Execute(sql2);
            return db.cmd_Execute(sql);

        }
        public DataTable findBubbleBybarcode(string barcode)
        {
            string sql = "select * from bubble  where pid='" + barcode + "'";

            DataTable dt = db.get_DataTable(sql);
            db.write_log_txt(sql);
            return dt;

        }
        public DataTable getBubbleInfo(int userid, string date)
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
             + ",b.bubblestatus bstatus,b.waterYield bwaterYield,b.starttime bstarttime,b.endDate bendDate,b.doingtime bdoingtime,(SELECT top 1 machinename FROM machine WHERE (id = (SELECT top 1  machineid FROM tisaneunit AS tisaneunit_1 WHERE (pid = p.id)))) as machinename  from prescription as p inner join bubble b on p.id=b.pid where b.employeeId=" + userid + " and CONVERT(varchar, b.starttime, 120) like '%" + date + "%' order by p.ID desc";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }
        public DataTable getBubbleInfo_scan(int userid, string date, string hospital_name)
        {
            update_soak_doingtime();
            //添加异常泡药校正
             string sql = "";
             string flag = "";

             string sql_1 = "";
             if (hospital_name != "0")
             {
                 sql_1 = "'" + hospital_name.Replace("_", "','") + "'";
             }
             else
             {
                 sql_1 = "select id from hospital";
             }

             string s_id = "";
             string sql2 = "select top 1 jobnum from Employee where id = '" +userid.ToString() + "'";
             SqlDataReader sr2 = db.get_Reader(sql2);//正在泡药的处方号的开始时间                     
             while (sr2.Read())
             {
                 s_id = sr2["jobnum"].ToString();
             }
             sr2.Close();
             flag = db.get_role_by_userid(Int32.Parse(s_id));
             if (flag != "0")
             {
                 sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
                 + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                 + ",b.bubblestatus bstatus,b.waterYield bwaterYield,b.starttime bstarttime,b.endDate bendDate,b.doingtime bdoingtime,(SELECT top 1 machinename FROM machine WHERE (id = (SELECT top 1  machineid FROM tisaneunit AS tisaneunit_1 WHERE (pid = p.id)))) as machinename,(select top 1 f_str from dbo.tb_alarm_info_1) as f_str  from prescription as p inner join bubble b on p.id=b.pid  " +
               "  left join hospital hs on hs.id = p.hospitalid  where b.employeeId=" + userid + " and b.starttime>='".TrimEnd() + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "'" + " and hs.id in("+sql_1  +")" +" order by  b.starttime desc";
             }
             else
             {
                 sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
                 + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                 + ",b.bubblestatus bstatus,b.waterYield bwaterYield,b.starttime bstarttime,b.endDate bendDate,b.doingtime bdoingtime,(SELECT top 1 machinename FROM machine WHERE (id = (SELECT top 1  machineid FROM tisaneunit AS tisaneunit_1 WHERE (pid = p.id)))) as machinename,(select top 1 f_str from dbo.tb_alarm_info_1)  as f_str   from prescription as p inner join bubble b on p.id=b.pid left join hospital hs on hs.id = p.hospitalid where " +
                 "  b.starttime>='".TrimEnd() + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "'"+ " and hs.id in("+sql_1  +")" + " order by  b.starttime desc";
             }
        db.write_log_txt("PDA泡药显示~~~~~~~~~：" + sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;
        }
        //添加 20160331 泡药已泡时间 
        public void update_soak_doingtime()
        {
            //煎药单号list
            List<string> l_pid = new List<string>();
            l_pid.Clear();
            //  string strsql = "select distinct pid from bubble where bubblestatus = 0";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string strsql = "SELECT p.ID FROM prescription AS p INNER JOIN bubble AS b ON p.ID = b.pid AND DATEDIFF(minute, b.starttime, GETDATE()) >= p.soaktime WHERE   (b.bubblestatus = 0) AND (CONVERT(varchar, p.dotime, 120) LIKE '%" + date + "%') ORDER BY DATEDIFF(minute, b.starttime, GETDATE()) DESC";
            db.write_log_txt("获取泡药显示信息 开始泡药-----");
            db.write_log_txt(strsql);
            db.write_log_txt("获取泡药显示信息 开始泡药-----");
            SqlDataReader sr = db.get_Reader(strsql);//正在泡药的处方号的开始时间
            // if (sr.Read())
            //  {
            while (sr.Read())
            {
                l_pid.Add(sr["ID"].ToString());
            }
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime d2 = Convert.ToDateTime(strY);//当前时间
            for (int i = 0; i < l_pid.Count; i++)
            {
                string sql = "select top 1 starttime from bubble where pid = '" + l_pid[i].ToString() + "'";
                SqlDataReader sr1 = db.get_Reader(sql);//正在泡药的处方号的开始时间           
                string a = "";
                db.write_log_txt("L_PID：" + l_pid[i].ToString());
                while (sr1.Read())
                {
                    a = sr1["starttime"].ToString();
                }
                sr1.Close();
                DateTime d1 = Convert.ToDateTime(a);//正在泡药的处方号的开始时间
                //  db.write_log_txt("流程已开始时间d1：" + d1.ToString());
                TimeSpan d3 = d2.Subtract(d1);//当前时间-开始时间=已泡药时间  int.Parse(na.Substring(0,na.IndexOf('.')));

                string[] arydoingtime = d3.TotalMinutes.ToString().Split('.');//转化为分钟数
                string strsql1 = "update bubble set doingtime= '" + arydoingtime[0] + "' where pid = '" + l_pid[i].ToString() + "'";

                db.cmd_Execute(strsql1);
            }
            //已完成状态
            List<string> l_pid_e = new List<string>();
            l_pid_e.Clear();
            //  string strsql_e = "select distinct pid from bubble where bubblestatus = 1 and starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "'";
            //  string strsql_e = "select distinct pid from bubble where bubblestatus = 1 and starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "'";
            string strsql_e = "SELECT p.ID FROM prescription AS p INNER JOIN bubble AS b ON p.ID = b.pid AND DATEDIFF(minute, b.starttime, GETDATE()) >= p.soaktime WHERE b.doingtime='0' and  (b.bubblestatus =1) AND (CONVERT(varchar, p.dotime, 120) LIKE '%" + date + "%') ORDER BY p.id DESC";
            db.write_log_txt("获取泡药显示信息 完成泡药-----");
            db.write_log_txt(strsql_e);
            db.write_log_txt("获取泡药显示信息 完成泡药-----");
            SqlDataReader sre = db.get_Reader(strsql_e);//正在泡药的处方号的开始时间
            // if (sr.Read())
            //  {
            while (sre.Read())
            {
                l_pid_e.Add(sre["ID"].ToString());
            }


            //  DateTime d2e = Convert.ToDateTime(strY);//当前时间
            for (int j = 0; j < l_pid_e.Count; j++)
            {
                string p = "select distinct soaktime from prescription where id='" + l_pid_e[j].ToString().Trim() + "'";
                db.write_log_txt("获取泡药时间：" + p);
                SqlDataReader sp = db.get_Reader(p);//正在泡药的处方号的开始时间
                // if (sr.Read())
                //  {
                string p_s = "0";
                while (sp.Read())
                {
                    p_s = sp["soaktime"].ToString();
                }
                string see = "update bubble set doingtime= '" + p_s + "' where pid = '" + l_pid_e[j].ToString() + "'";
                db.write_log_txt("修改已完成泡药时间：" + see);
                db.cmd_Execute(see);
            }
            //  Bubbleinfo bi = new Bubbleinfo();
            //   DataTable dataTable = bi.getBubbleInfo_scan(Convert.ToInt32(id), DateTime.Now.ToString("yyyy-MM-dd"));

            //   Response.Write("{\"code\":\"0\",\"msg\":\"操作成功\",\"data\":" + DataTableToJson.ToJson(dataTable) + ",\"warning\":\"" + warningid + "\",\"timeout\":\"" + timeout + "\"}");

            //添加异常泡药校正
            #region
            //自动结束泡药串行校正
            //
            // string sql_ad = "select b.pid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成'"+
            //  " and DATEDIFF(minute, b.starttime, b.endDate ) < p.soaktime  " + " where  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
            string sql_ad = "select b.pid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成'" +
   " and DATEDIFF(minute, b.starttime, b.endDate ) < p.soaktime" + " where  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
            db.write_log_txt("校正条件：" + sql_ad);
            SqlDataReader srad = db.get_Reader(sql_ad);//正在泡药的处方号的开始时间       
            List<string> l_a = new List<string>();
            l_a.Clear();
            while (srad.Read())
            {
                l_a.Add(srad["pid"].ToString());
                db.write_log_txt("校正PID：" + srad["pid"].ToString());
            }
            for (int k = 0; k < l_a.Count; k++)
            {
                //校正BUBBLE update
                string s_u_b = "update bubble set enddate=NULL,doingtime=0,bubblestatus=0 where pid='" + l_a[k].ToString().Trim() + "'";
                //校正PRESCRIPTION  update
                string s_u_p = "update prescription set curstate='开始泡药' where id='" + l_a[k].ToString().Trim() + "'";
                db.write_log_txt("校正SQL:" + "PID:" + l_a[k].ToString() + "~~" + s_u_b + "----" + s_u_p);
                db.cmd_Execute(s_u_b);
                db.cmd_Execute(s_u_p);
            }

            //获取强制泡药开关状态
            string flag_onoff = get_globaldrug_status();
            if (flag_onoff == "开启")
            {
                //已泡时间小于浸泡时间校正
                string sql_ad1 = "select b.pid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成'" +
       " and b.doingtime < p.soaktime and b.doingtime>'0'" + " where  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
                db.write_log_txt("已泡时间小于浸泡时间校正校正条件：" + sql_ad1);
                SqlDataReader srad1 = db.get_Reader(sql_ad1);//正在泡药的处方号的开始时间
                List<string> l_a1 = new List<string>();
                l_a1.Clear();
                while (srad1.Read())
                {
                    l_a1.Add(srad1["pid"].ToString());
                    db.write_log_txt("已泡时间小于浸泡时间校正PID：" + srad1["pid"].ToString());
                }
                for (int k = 0; k < l_a1.Count; k++)
                {
                    //校正BUBBLE update
                    string s_u_b1 = "update  b set b.doingtime=p.soaktime from bubble b inner join prescription p on p.id=b.pid where b.pid='" + l_a1[k].ToString().Trim() + "'";
                    db.write_log_txt("已泡时间小于浸泡时间校正SQL:" + "PID:" + l_a1[k].ToString() + "~~" + s_u_b1);
                    db.cmd_Execute(s_u_b1);
                }
                //历史数据校正
                string sql_ad11 = "select b.pid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成'" +
       " and b.doingtime>'0' and DATEDIFF(minute, b.starttime, b.endDate ) > p.soaktime*2" + " where  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
                db.write_log_txt("已泡时间小于浸泡时间校正校正条件：" + sql_ad11);
                SqlDataReader srad11 = db.get_Reader(sql_ad11);//正在泡药的处方号的开始时间       
                List<string> l_a11 = new List<string>();
                l_a11.Clear();
                while (srad11.Read())
                {
                    l_a11.Add(srad11["pid"].ToString());
                    db.write_log_txt("历史数据校正PID：" + srad11["pid"].ToString());
                }
                for (int k = 0; k < l_a11.Count; k++)
                {
                    //校正BUBBLE update
                    string s_u_b11 = "update bubble set enddate=DATEADD(MINUTE,convert(int,doingtime),starttime) where pid='" + l_a11[k].ToString().Trim() + "'";
                    db.write_log_txt("历史数据校正SQL:" + "PID:" + l_a11[k].ToString() + "~~" + s_u_b11);
                    db.cmd_Execute(s_u_b11);
                }
                //校正完毕没有浸泡时间大于已泡时间
                string sql_ad111 = "select b.pid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成'" +
    " and b.doingtime>'0' and b.doingtime> p.soaktime" + " where  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
                db.write_log_txt("已泡时间小于浸泡时间校正校正条件：" + sql_ad111);
                SqlDataReader srad111 = db.get_Reader(sql_ad111);//正在泡药的处方号的开始时间       
                List<string> l_a111 = new List<string>();
                l_a111.Clear();
                while (srad111.Read())
                {
                    l_a111.Add(srad111["pid"].ToString());
                    db.write_log_txt("历史数据校正PID：" + srad111["pid"].ToString());
                }
                for (int k = 0; k < l_a111.Count; k++)
                {
                    //校正BUBBLE update
                    string s_u_b111_1 = "update  b set b.doingtime=p.soaktime from bubble b inner join prescription p on p.id=b.pid where b.pid='" + l_a111[k].ToString().Trim() + "'";
                    db.cmd_Execute(s_u_b111_1);
                    string s_u_b111 = "update bubble set enddate=DATEADD(MINUTE,convert(int,doingtime),starttime) where pid='" + l_a111[k].ToString().Trim() + "'";
                    db.write_log_txt("浸泡时间大于已泡时间SQL:" + "PID:" + l_a111[k].ToString() + "~~" + s_u_b111);
                    db.cmd_Execute(s_u_b111);
                }
                //校正完毕分配煎药机数据校正
                string flag_dec_machine = "1";//默认煎药机选择模式为：自动分配煎药机
                string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
                dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
                SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);
                if (sr_dec_machine_select.Read())
                {
                    try
                    {
                        flag_dec_machine = sr_dec_machine_select["decocting_machine_mode_select"].ToString();
                    }
                    catch
                    {
                        flag_dec_machine = "1";
                    }
                }
                //自动分配煎药机模式
                if (flag_dec_machine == "1")
                {
                    string sql_ad1112 = "  select b.pid,u.machineid from prescription p INNER JOIN bubble b ON b.pid =p.ID and b.bubblestatus ='1'  and p.curstate='泡药完成' " +
      "and b.doingtime>'0' left join tisaneunit u on u.pid=b.pid where u.machineid is null and  b.starttime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00".TrimEnd() + "' order by p.ID desc";
                    db.write_log_txt("已泡时间小于浸泡时间校正校正条件：" + sql_ad1112);
                    SqlDataReader srad1112 = db.get_Reader(sql_ad1112);//正在泡药的处方号的开始时间       
                    List<string> l_a1112 = new List<string>();
                    l_a1112.Clear();
                    while (srad1112.Read())
                    {
                        l_a1112.Add(srad1112["pid"].ToString());
                        db.write_log_txt("历史数据校正PID：" + srad1112["pid"].ToString());
                    }
                    for (int k = 0; k < l_a1112.Count; k++)
                    {
                        //校正BUBBLE update
                        //判断tisaneunit是否存在PID
                        string stu = "select count(id) as cid from tisaneunit where pid='" + l_a1112[k].ToString().Trim() + "'";
                        SqlDataReader sdt = db.get_Reader(stu);//正在泡药的处方号的开始时间      
                        string flag_un = "0";
                        while (sdt.Read())
                        {
                            flag_un = sdt["cid"].ToString();
                        }
                        //tisaneunit存在PID，修改machineid
                        string machineid = db.sp_Execute_ave_1("sp_cf_jz").ToString();
                        if (flag_un != "0")
                        {

                            string us1 = "update tisaneunit set machineid='" + machineid.Trim() + "' where pid='" + l_a1112[k].ToString().Trim() + "'";
                         //   db.write_log_txt("历史数据校正SQL:" + "PID:" + l_a1112[k].ToString() + "~~" + us1);
                            db.cmd_Execute(us1);
                        }
                        //tisaneunit不存在PID，添加并修改machineid
                        if (flag_un == "0")
                        {
                            //插入煎药机
                            string sit = "insert into tisaneunit(pid,machineid,packstatus) values('" + l_a1112[k].ToString().Trim() + "','" + machineid + "','" + "1" + "')";
                            //  string us1 = "update tisaneunit set machineid='" + machineid.Trim() + "' where pid='" + l_a1112[k].ToString().Trim() + "'";
                       //     db.write_log_txt("历史数据校正SQL:" + "PID:" + l_a1112[k].ToString() + "~~" + sit);
                            db.cmd_Execute(sit);
                        }

                    }
                }
            }
            #endregion
        }
        #region 获取泡药超时煎药单号

        public string getTimeoutNumber(string date)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "SELECT p.ID FROM prescription AS p INNER JOIN bubble AS b ON p.ID = b.pid AND DATEDIFF(minute, b.starttime, GETDATE()) >= p.soaktime WHERE   (b.bubblestatus = 0) AND (b.bubblestatus = 0) AND (CONVERT(varchar, p.dotime, 120) LIKE '%" + date + "%') ORDER BY DATEDIFF(minute, b.starttime, GETDATE()) DESC";


            SqlDataReader sr = db.get_Reader(sql);
            string str = "";
            int index = 0;
            while (sr.Read())
            {

                str += sr["ID"].ToString() + ",";
                index++;
                if (index == 3)
                {
                    break;
                }
            }

            return str;
        }

        public string getTimeoutNumber_c(string date)
        {
            DataBaseLayer db = new DataBaseLayer();
            // string sql = "select id ,  (select Pspnum from prescription as p p.id = b.pid )as Pspnum, (select getdrugnum from prescription as p p.id = b.pid )as getdrugnum, (select getdrugtime from prescription as p p.id = b.pid )as getdrugtime,bubbleperson as bp,starttime,doingtime,bubblestatus from bubble as b";
            string sql = "SELECT p.ID FROM prescription AS p INNER JOIN bubble AS b ON p.ID = b.pid AND DATEDIFF(minute, b.starttime, GETDATE()) >= p.soaktime WHERE   (b.bubblestatus = 0) AND (CONVERT(varchar, p.dotime, 120) LIKE '%" + date + "%') ORDER BY DATEDIFF(minute, b.starttime, GETDATE()) DESC";
            db.write_log_txt("新测试sql：" + sql);
            //添加超时的处方id
            List<string> l_p = new List<string>();
            l_p.Clear();
            SqlDataReader sr = db.get_Reader(sql);
            string str = "";
            int index = 0;
            while (sr.Read())
            {

                str += sr["ID"].ToString() + ",";
                l_p.Add(sr["ID"].ToString());
                db.write_log_txt("新测试：" + sr["ID"].ToString());
                index++;
                if (index == 3)
                {
                    break;
                }
            }
            db.write_log_txt("index_num:" + index);
            #region 添加自动结束泡药并分配煎药机功能
            //根据泡药超时的处方号自动结束泡药并根据分配煎药机开关状态分配煎药机组
            //判断系统设置自动分配煎药机组开关是否打开。
            string flag_dec_machine = "1";//默认煎药机选择模式为：自动分配煎药机
            string dec_machine_select;//1:自动分配煎药机 0：不分配煎药机
            dec_machine_select = "select top 1 decocting_machine_mode_select from dbo.tb_sys_add_setting where sys_add_id='1'";
            SqlDataReader sr_dec_machine_select = db.get_Reader(dec_machine_select);
            if (sr_dec_machine_select.Read())
            {
                try
                {
                    flag_dec_machine = sr_dec_machine_select["decocting_machine_mode_select"].ToString();
                }
                catch
                {
                    flag_dec_machine = "1";
                }
            }
            //自动分配煎药机模式
            db.write_log_txt("测试自动分配煎药机模式:" + flag_dec_machine);
            if (flag_dec_machine == "1")
            {
                //  string machine = "[]";
                for (int j = 0; j < l_p.Count; j++)
                {
                    //  DataTable t = findMachineByStartAndFree_busy();
                    //   db.write_log_txt("测试自动分配煎药机模式是否有空闲煎药机:" + t.Rows.Count.ToString());
                    // if (t.Rows.Count > 0)
                    // {
                    //      machine = DataTableToJson.ToJson(t);
                    //   }

                    db.write_log_txt("自动泡药结束分配煎药机：" + l_p[j].ToString());
                    DataTable dt = findBubbleBybarcode(l_p[j].ToString().Trim());
                    //  db.write_log_txt("end：" + l_p[j].ToString()); 

                    string machineid = db.sp_Execute_ave_1("sp_cf_jz").ToString();
                    //  db.write_log_txt("----" + machineid);
                    // d.write_log_txt(Convert.ToInt32(dt.Rows[0]["id"].ToString()).ToString() + "--" + "1" + "--" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + 
                    //   dt.Rows[0]["starttime"].ToString() + "---" + machineid);
                    //  updateBubble_scan_auto(Convert.ToInt32(dt.Rows[0]["id"].ToString()), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["starttime"].ToString(), t.Rows[0]["machineid"].ToString());   
                    db.write_log_txt("执行updateBubble_scan_auto");
                    updateBubble_scan_auto(Convert.ToInt32(dt.Rows[0]["id"].ToString()), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["starttime"].ToString(), machineid);
                }

            }

            if (flag_dec_machine == "0")
            {
                db.write_log_txt("执行不分配煎药机");
                for (int j = 0; j < l_p.Count; j++)
                {
                    DataTable dt = findBubbleBybarcode(l_p[j].ToString().Trim());
                    //  DataTable t = findMachineByStartAndFree_busy();
                    DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//当前时间
                    DateTime d2 = Convert.ToDateTime(dt.Rows[0]["starttime"].ToString());//开始时间
                    TimeSpan d3 = d1.Subtract(d2);//泡药时间
                    int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());//转化为分钟数     

                    db.write_log_txt("异常 begin--");
                    string sql2 = "update bubble set bubblestatus='" + "1" + "',endDate=" + "getdate()" + ",doingtime='" + doingtime + "' where id='" + dt.Rows[0]["id"].ToString().Trim() + "'";
                    db.write_log_txt(sql2);
                    int count = db.cmd_Execute(sql2);
                    string sql4 = "select pid from bubble where id=" + dt.Rows[0]["id"].ToString().Trim() + "'";
                    db.write_log_txt(sql4);
                    DataTable dt4 = db.get_DataTable(sql4);
                    string sql7 = "update prescription set curstate = '泡药完成'  where id = '" + dt4.Rows[0]["pid"].ToString() + "'";
                    db.write_log_txt(sql7);
                    db.write_log_txt("异常 END--");
                    db.cmd_Execute(sql7);//更新处方表里的当前状态
                }
            }
            #endregion
            return str;
        }
        #endregion

        public DataTable getBubbleInfo(string name, int status, string STime, string Ksbq, string Psnum, string HostaileName)
        {

            ArrayList list = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();


            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            if (status == 0)//正在泡
            {
                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d2 = Convert.ToDateTime(strY);//当前时间

                string strsql = "select starttime from bubble where bubblestatus = 0";
                SqlDataReader sr = db.get_Reader(strsql);//正在泡药的处方号的开始时间

                while (sr.Read())
                {
                    string a = sr["starttime"].ToString();

                    list.Add(a);
                }

                for (int i = 0; i < list.Count; i++)
                {

                    string a = list[i].ToString();

                    DateTime d1 = Convert.ToDateTime(a);//正在泡药的处方号的开始时间

                    TimeSpan d3 = d2.Subtract(d1);//当前时间-开始时间=已泡药时间
                    int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());

                    string strsql1 = "update bubble set doingtime= '" + doingtime + "' where starttime = '" + a + "'";

                    db.cmd_Execute(strsql1);
                }
            }

            string sql = "";

            sql = "select  ID,Pspnum,delnum,(select bubblestatus from bubble where pid= p.id) as bubblestatus,(select bubblewarning from warning where hospitalid = p.Hospitalid and type=0) as bubblewarning,(select doingtime from bubble where pid = p.id) as doingtime,(select warningstatus from bubble where pid = p.id) as warningstatus,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(SELECT mark FROM bubble WHERE pid = p.id) as mark,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql += "diagresult,(select wateryield from bubble where pid= p.id) as wateryield,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,RemarksA,RemarksB";
            sql += ",(select starttime from bubble where pid= p.id) as starttime ,(select endDate from bubble where pid= p.id) as endDate from prescription as p where  " +
                // id not in (select pid from InvalidPrescription) and  
             "id in (select pid from bubble where 1=1";

            if (name != "0")
            {

                sql += "and bubbleperson ='" + name + "'";
            }
            if (status != 9)
            {
                sql += "and bubblestatus ='" + status + "'";
            }

            sql += ")";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                sql += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (Psnum != null && Psnum.Length > 0)
            {
                sql += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                sql += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                sql += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            sql += " order by ID desc";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }

        public int getBubbleInfots(string name, int status, string STime, string Ksbq, string Psnum, string HostaileName)
        {

            ArrayList list = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();


            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            if (status == 0)//正在泡
            {
                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d2 = Convert.ToDateTime(strY);//当前时间

                string strsql = "select starttime from bubble where bubblestatus = 0";
                SqlDataReader sr = db.get_Reader(strsql);//正在泡药的处方号的开始时间

                while (sr.Read())
                {
                    string a = sr["starttime"].ToString();

                    list.Add(a);
                }

                for (int i = 0; i < list.Count; i++)
                {

                    string a = list[i].ToString();

                    DateTime d1 = Convert.ToDateTime(a);//正在泡药的处方号的开始时间

                    TimeSpan d3 = d2.Subtract(d1);//当前时间-开始时间=已泡药时间
                    int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());

                    string strsql1 = "update bubble set doingtime= '" + doingtime + "' where starttime = '" + a + "'";

                    db.cmd_Execute(strsql1);
                }
            }

            string sql = "";

            sql = "select  count(*)  from prescription as p   where  " +
                // id not in (select pid from InvalidPrescription) and  
             "p.id in (select pid from bubble where 1=1";

            if (name != "0")
            {

                sql += "and bubbleperson ='" + name + "'";
            }
            if (status != 9)
            {
                sql += "and bubblestatus ='" + status + "'";
            }

            sql += ")";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                sql += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (Psnum != null && Psnum.Length > 0)
            {
                sql += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                sql += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                sql += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));

            return count;
        }

        public DataTable getBubbleInfofy(string name, int status, string STime, string Ksbq, string Psnum, string HostaileName, int ts, int page)
        {

            ArrayList list = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();


            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            if (status == 0)//正在泡
            {
                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d2 = Convert.ToDateTime(strY);//当前时间

                string strsql = "select starttime from bubble where bubblestatus = 0";
                SqlDataReader sr = db.get_Reader(strsql);//正在泡药的处方号的开始时间

                while (sr.Read())
                {
                    string a = sr["starttime"].ToString();

                    list.Add(a);
                }

                for (int i = 0; i < list.Count; i++)
                {

                    string a = list[i].ToString();

                    DateTime d1 = Convert.ToDateTime(a);//正在泡药的处方号的开始时间

                    TimeSpan d3 = d2.Subtract(d1);//当前时间-开始时间=已泡药时间
                    int doingtime = Convert.ToInt32(d3.Days.ToString()) * 24 * 60 + Convert.ToInt32(d3.Hours.ToString()) * 60 + Convert.ToInt32(d3.Minutes.ToString());

                    string strsql1 = "update bubble set doingtime= '" + doingtime + "' where starttime = '" + a + "'";

                    db.cmd_Execute(strsql1);
                }
            }

            string sql = "";

            sql = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from (  select  ID,Pspnum,delnum,(select bubblestatus from bubble where pid= p.id) as bubblestatus,(select bubblewarning from warning where hospitalid = p.Hospitalid and type=0) as bubblewarning,(select doingtime from bubble where pid = p.id) as doingtime,(select warningstatus from bubble where pid = p.id) as warningstatus,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(SELECT mark FROM bubble WHERE pid = p.id) as mark,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql += "diagresult,(select wateryield from bubble where pid= p.id) as wateryield,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,RemarksA,RemarksB";
            sql += ",(select starttime from bubble where pid= p.id) as starttime ,(select endDate from bubble where pid= p.id) as endDate from prescription as p where  " +
                // id not in (select pid from InvalidPrescription) and  
             "id in (select pid from bubble where 1=1";

            if (name != "0")
            {

                sql += "and bubbleperson ='" + name + "'";
            }
            if (status != 9)
            {
                sql += "and bubblestatus ='" + status + "'";
            }

            sql += ")";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                sql += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (Psnum != null && Psnum.Length > 0)
            {
                sql += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                sql += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                sql += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            sql += ") t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;
        }

        #region 导出泡药信息
        public DataTable getBubbleInfoDao(string name, int status)
        {
            string sql = "";
            sql = "select  ID,Pspnum,delnum,(select case  convert(nvarchar(50),bubblestatus) when 1 then '泡药完成' when 0 then '开始泡药' else convert(nvarchar(50),bubblestatus) end from bubble where pid= p.id) as bubblestatus,(select doingtime from bubble where pid = p.id) as doingtime,(select starttime from bubble where pid= p.id) as starttime,(select endDate from bubble where pid= p.id) as endDate,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(SELECT mark FROM bubble WHERE pid = p.id) as mark,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,name,case convert(nvarchar(50), sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql += "diagresult,(select wateryield from bubble where pid= p.id) as wateryield,dose,takenum,getdrugtime,getdrugnum,takemethod,case decscheme when 1 then '微压（密闭）解表（15min)' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end ,oncetime,twicetime,packagenum,dotime,dtbcompany,dtbaddress,dtbphone,case dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' else dtbtype end,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,RemarksA,";
            sql += "RemarksB from prescription as p where id not in (select pid from InvalidPrescription) and  id in (select pid from bubble where 1=1";

            if (name != "0")
            {
                sql += "and bubbleperson ='" + name + "'";
            }
            if (status != 2)
            {
                sql += "and bubblestatus ='" + status + "'";

            }

            sql += ")";
            sql += " order by ID desc";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }

        #endregion
        public DataTable getBubbleInfo(int id)
        {

            string sql = "select hospitalid,pspnum ,(select bubbleperson from bubble where pid = '" + id + "') as bp from prescription where id = '" + id + "'";
            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(sql);


            return dt;
        }



        public int updateBubbleInfo(int id, string bubbleman)
        {
            //update tb set UserName="XXXXX" where UserID="aasdd"
            int end = 0;

            string sql = "";
            string str = "select * from bubble where pid= '" + id + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                //string result = sr["id"].ToString();
                //a = Convert.ToInt32(result);
                sql = "update bubble set bubbleperson = '" + bubbleman + "' where pid = '" + id + "' ";
                end = db.cmd_Execute(sql);
            }
            else
            {
                end = 0;
            }


            return end;
        }


        public int deleteBubbleInfo(int id)
        {
            string strSql = "delete from bubble where pid = '" + id + "'";
            int n = db.cmd_Execute(strSql);


            return n;
        }
        #region 查询开启并空闲的煎药机
        public DataTable findMachineByStartAndFree()
        {
            string str = "SELECT   id, status, roomnum, usingstatus, healthstatus, disinfectionstatus, machinename, mark, unitnum, machineroom, "
                + "macaddress, checkman, checktime, equipmenttype, pid FROM machine WHERE (mark = 0) AND (usingstatus = '启用') AND (status = '空闲') and mark = 0";
            return db.get_DataTable(str);
        }
        #endregion
        /*  select top 1 b.machineid from  (select a.machineid,count(a.id) as num from       (select  t.* from machine m inner join  tisaneunit t on m.id =t.machineid  inner join 
        prescription p on p.ID=t.pid  where m.mark = 0 and  m.usingstatus = '启用' and m.status='忙碌' and t.packstatus ='0'
        ) a group by a.machineid ) b order by b.num asc*/
        #region 查询忙碌且待煎处方最少的煎药机
        public DataTable findMachineByStartAndFree_busy()
        {
            string str = " select top 1 b.machineid, c.status, c.roomnum,c.usingstatus,c.healthstatus, c.disinfectionstatus, c.machinename, c.mark,c.unitnum, c.machineroom, "
                + "c.macaddress,c.checkman, c.checktime,c.equipmenttype, c.pid  from  (select a.machineid,count(a.id) as num from   (select  t.* from machine m inner join  tisaneunit t on m.id =t.machineid " + "inner join   prescription p on p.ID=t.pid  where m.mark = 0 and  m.usingstatus = '启用' and m.status='忙碌' and t.packstatus ='0'" +
      " ) a group by a.machineid ) b" +
      " inner join machine c on c.id=b.machineid "
      + "order by b.num asc";
            db.write_log_txt(str);
            return db.get_DataTable(str);
        }
        #endregion

        //分配机组的算法
        public string distributionmachine()
        {
            ArrayList list = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            //找出机组
            string str = "select id from machine where mark = 0 and usingstatus = '启用' and status='空闲'";

            SqlDataReader sr = db.get_Reader(str);
            //煎药机
            while (sr.Read())
            {
                string id = sr["id"].ToString();
                int machineid = Convert.ToInt32(id);
                list.Add(machineid);
            }
            string str1 = "";
            string str2 = "";
            string num = "";
            string num1 = "";
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            for (int i = 0; i < list.Count; i++)
            {
                str1 = "select count(machineid) as num  from tisaneunit where machineid ='" + list[i] + "'";
                SqlDataReader sr1 = db.get_Reader(str1);//从煎药表里找到每个煎药机被分配的处方的数量

                str2 = "select count(*) as num1 from  tisaneunit  where packstatus = 1 and machineid ='" + list[i] + "'";
                SqlDataReader sr2 = db.get_Reader(str2);//每个煎药机被分配且已包装好的处方的数量



                if (sr1.Read())
                {
                    num = sr1["num"].ToString();
                }
                else
                {
                    num = "0";
                }
                count1 = Convert.ToInt32(num);
                if (sr2.Read())
                {
                    num1 = sr2["num1"].ToString();
                }
                else
                {
                    num1 = "0";
                }
                count2 = Convert.ToInt32(num1);



                count = count1 - count2;//每个煎药机被分配处方的数量-这些处方已包装的数量

                list1.Add(count);

            }


            int min = 10000;
            int pos = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                if (Convert.ToInt32(list1[i]) < min)
                {
                    min = Convert.ToInt32(list1[i]);
                    pos = i;
                }
            }
            return list[pos].ToString();//最先分配的煎药机的id
        }


        //泡药警告
        public string bubblewarning()
        {
            string sql3 = "";
            sql3 = "select ID,Pspnum,customid,delnum,( select top 1 bubblewarning from warning where hospitalid = p.Hospitalid and type=0) as bubblewarning,(select  top 1 doingtime from bubble where pid = p.id) as doingtime,(SELECT top 1 bubbleperson FROM bubble WHERE pid = p.id) as bp,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql3 += "diagresult,(select top 1 warningtime from Audit where pid = p.id) as warningtime,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate";
            sql3 += " from prescription as p where id in (select pid from Audit)";

            SqlDataReader sr3 = db.get_Reader(sql3);//复核通过的所有信息

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间
            string warningtime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            ArrayList list2 = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list3 = new ArrayList();
            ArrayList list4 = new ArrayList();
            ArrayList list5 = new ArrayList();



            while (sr3.Read())
            {

                // sr3["bubblewarning"].ToString();
                // sr3["getdrugtime"].ToString();
                string d1 = sr3["bubblewarning"].ToString();//泡药警告时间

                list1.Add(d1);

                string drugtime = sr3["getdrugtime"].ToString();//得到该处方号的取药时间
                //  DateTime d2 = Convert.ToDateTime(sr3["getdrugtime"].ToString());
                list2.Add(drugtime);


                string id = sr3["ID"].ToString();//当前id煎药单号
                list3.Add(id);



                string hospitalid = sr3["hospitalid"].ToString();

                list4.Add(hospitalid);

                string awarningtime = sr3["warningtime"].ToString();

                list5.Add(awarningtime);

            }
            for (int i = 0; i < list2.Count; i++)
            {



                string sql8 = "select status from warning where hospitalid = '" + list4[i] + "'";

                SqlDataReader sr8 = db.get_Reader(sql8);
                string status = "";//医院预警开关状态

                if (sr8.Read())
                {
                    status = sr8["status"].ToString();

                }




                string d1 = list1[i].ToString();//泡药警告时间

                DateTime d2 = Convert.ToDateTime(list2[i].ToString());//取药时间


                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d3 = Convert.ToDateTime(strY);//当前时间



                TimeSpan d4 = d2.Subtract(d3);//取药时间- 当前时间



                //取药时间- 当前时间
                int time = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());
                //泡药警告时间
                if (d1 == "")
                {
                    d1 = "-10000000";
                }
                int time2 = Convert.ToInt32(d1);
                if (status == "0")
                {
                    time2 = -10000000;
                }


                if (time < time2)
                {
                    string strsql1 = "update Audit set bubblewarningstatus = 1,warningtype='泡药预警' where pId = '" + list3[i] + "'";

                    db.cmd_Execute(strsql1);

                    // if (list5[i].ToString() == "1970-1-1 0:00:00")
                    if (list5[i].ToString() == "" || Convert.ToDateTime(list5[i].ToString()).ToString("yyyy-MM-dd HH:mm:ss") == "1970-01-01 00:00:00")
                    {
                        string strsql2 = "update Audit set warningtime ='" + warningtime + "' where pId = '" + list3[i] + "'";

                        db.cmd_Execute(strsql2);
                    }
                }
                else
                {
                    string strsql2 = "update Audit set bubblewarningstatus = 0,warningtype='暂无预警',warningtime='1970-1-1 00:00:00' where pId = '" + list3[i] + "'";

                    db.cmd_Execute(strsql2);
                }


            }
            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");

            string str2 = "";
            string str = "select pId from  Audit where bubblewarningstatus = 1 and pid not in (select pid from bubble) and pid in (select id from prescription as p where  p.dotime between '" + strS + "' and  '" + strS2 + "')";
            SqlDataReader sr = db.get_Reader(str);

            while (sr.Read())
            {

                str2 += sr["pId"].ToString() + ",";

            }
            return str2;
        }
        #region 通过权限查询人员
        public SqlDataReader findNameAll()
        {
            string sql = "select * from  Employee where Role ='3' or  Role ='0'";

            return db.get_Reader(sql);
        }

        #endregion



        public bool checkisdone(string id)
        {

            bool result = false;
            string str = "select bubblestatus from bubble where pid ='" + id + "' ";

            SqlDataReader sdr = db.get_Reader(str);

            if (sdr.Read())
            {
                if (sdr["bubblestatus"].ToString() == "1")
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }




            return result;

        }

        //返回强制泡药开关状态
        public string get_globaldrug_status()
        {
            string result = "-1";
            string str = "SELECT TOP 1 global_medi_is_flag  FROM tb_sys_add_setting";

            SqlDataReader sdr = db.get_Reader(str);

            if (sdr.Read())
            {
                if (sdr["global_medi_is_flag"].ToString() == "1")
                {
                    result = "开启";
                }
                else
                {
                    result = "关闭";
                }
            }
            return result;
        }

        //返回泡药完成煎药机选择模式
        public string get_globaldrug_select_model()
        {
            string result = "-1";
            string str = "SELECT TOP 1 decocting_machine_mode_select  FROM tb_sys_add_setting";

            SqlDataReader sdr = db.get_Reader(str);

            if (sdr.Read())
            {
                if (sdr["decocting_machine_mode_select"].ToString() == "1")
                {
                    result = "分配煎药机";
                }
                else
                {
                    result = "不分配煎药机";
                }

            }
            return result;
        }
        //根据处方号返回设定泡药时间   select soaktime,* from dbo.prescription where ID='199'
        public string get_drug_soaktime_by_pid(string pid)
        {
            string result = "-1";
            string str = "select top 1 soaktime from dbo.prescription where ID='" + pid.Trim() + "'";
            db.write_log_txt(str);
            SqlDataReader sdr = db.get_Reader(str);

            if (sdr.Read())
            {

                result = sdr["soaktime"].ToString().Trim();
                db.write_log_txt(result);

            }
            return result;
        }
        //修改泡药状态到完成
        //  update prescription set curstate ='泡药完成' where id=@cf_id and Hospitalid =@hospital_id  
        // update bubble set bubblestatus ='1',endDate =GETDATE()  where pid=@cf_id 
        public string update_drug_global_status(string pid)
        {
            string result = "-1";
            string str1 = "update prescription set curstate ='泡药完成' where id='" + pid.Trim() + "'";
            string str2 = "update bubble set bubblestatus ='1',endDate =GETDATE()  where pid='" + pid.Trim() + "'";
            try
            {
                db.cmd_Execute(str1);
                db.cmd_Execute(str2);
                result = "1";
            }
            catch
            {
                result = "-1";

            }
            return result;
        }
        //根据泡药条形码返回药方编号
        public string get_pid_by_barcode(string barcode)
        {
            string result = "-1";
            string str = "select top 1 pid from dbo.bubble where barcode='" + barcode.Trim() + "'";

            SqlDataReader sdr = db.get_Reader(str);

            if (sdr.Read())
            {

                result = sdr["pid"].ToString().Trim();


            }
            return result;
        }


        //此处添加获取煎药室空闲煎药机信息
        #region
        public string get_room_message()
        {
            string alarm_str = "";
            string str = "select top 1 f_str from tb_alarm_info_1";
            SqlDataReader sdr = db.get_Reader(str);
            if (sdr.Read())
            {
                alarm_str = sdr["f_str"].ToString().Trim();
            }
            return alarm_str;
        }
        #endregion


        //此处添加煎药机条码转换判断 该条码存在返回1，否则返回0
        #region
        public string get_m_id_by_m_id(string m_id)
        {
            string id = "0";
            m_id = m_id.TrimStart('0');
            string str = "select top 1 id from machine where m_id='" + m_id.Trim() + "'";
            SqlDataReader sdr = db.get_Reader(str);
            if (sdr.Read())
            {
                id = sdr["id"].ToString().Trim();
            }
            return id;
        }
        #endregion
        //根据煎药机条码获取煎药机状态
        #region
        public string get_m_status_by_m_id(string m_id)
        {
            string id = "0";
            m_id = m_id.TrimStart('0');
            string str = "select top 1 status from machine where m_id='" + m_id.Trim() + "'";
            SqlDataReader sdr = db.get_Reader(str);
            if (sdr.Read())
            {
                id = sdr["status"].ToString().Trim();
            }
            return id;
        }
        #endregion
        //判断煎药机条码否是机器编号
        public string get_m_id_is(string mid)
        {
            string id = "0";

            string str = "select count(id) as mid from machine where id='" + mid.Trim() + "'";
            SqlDataReader sdr = db.get_Reader(str);
            if (sdr.Read())
            {
                id = sdr["mid"].ToString().Trim();
            }
            return id;
        }
    }
}
