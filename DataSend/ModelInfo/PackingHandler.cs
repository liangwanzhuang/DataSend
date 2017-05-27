using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;
using System.Data.SqlClient;

using System.Collections;


namespace ModelInfo
{
    public class PackingHandler
    {
        public DataBaseLayer db = new DataBaseLayer();
        public int AddPacking(int DecoctingNum, string PackPer)
        {
            int end = 0;
            /// <summary>
            /// 添加包装信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>
            /// 
            #region 当没有收到煎药完成指令和开始排液指令，触发判断煎药是否完成未完成将煎药时间设置为开始煎药时间+25MIN+煎药方案时间
            try
            {
                //包装流程修改，若煎药流程结束，扫码开始包装，包装代数为实际代数+多包代数。
                //多包代数
                string str11 = "select top 1 package_machine_nums from dbo.tb_sys_add_setting";
                SqlDataReader sr11 = db.get_Reader(str11);
                int more_num = 0;
                if (sr11.Read())
                {

                    more_num = Convert.ToInt32(sr11["package_machine_nums"].ToString());
                  //  string sql12 = "update prescription set packagenum =packagenum+" + more_num.ToString() + " where id = '" + DecoctingNum.ToString().Trim() + "'";//+" and "+"'"++"'";
                  //  db.cmd_Execute(sql12);
                    string sql13 = "update  tisaneinfo  set tisanestatus ='1' where pid ='" + DecoctingNum.ToString().Trim() + "'";
                    db.cmd_Execute(sql13);
                }
                sr11.Close();
                db.sp_Execute_no_return("sp_auto_tisane", DecoctingNum);
            }
            catch
            {
                end = 0;
            }
            #endregion

            String sql = "";

            string per = PackPer.Substring(6);
            string employeeid = "";
            string str7 = "select id from employee where EmNumAName ='" + PackPer + "'";
            SqlDataReader sr7 = db.get_Reader(str7);

            if (sr7.Read())
            {

                employeeid = sr7["id"].ToString();

            }

            string str = "select pid from tisaneinfo where  tisanestatus = 1 and  pid = '" + DecoctingNum + "'";
            //and pid not in (select pid from InvalidPrescription)
            SqlDataReader sr = db.get_Reader(str);
            System.DateTime now = new System.DateTime();
            now = System.DateTime.Now;
            #region if 1
            if (sr.Read())
            {
                string result = sr["pid"].ToString();
                int a = Convert.ToInt32(result);

                string str1 = "select * from Packing where DecoctingNum = '" + a + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                #region 是否包装判断
                if (sr1.Read())
                {
                    string start = "select Starttime from Packing where DecoctingNum = '" + a + "'";
                    SqlDataReader starttime = db.get_Reader(start);

                    string starttime2 = "";
                    if (starttime.Read())
                    {
                        string tate = "select Fpactate from Packing where DecoctingNum = '" + a + "'";
                        SqlDataReader tate1 = db.get_Reader(tate);

                        if (tate1.Read())
                        {
                            if (tate1["Fpactate"].ToString() == "0")
                            {
                                starttime2 = starttime["Starttime"].ToString();
                                //获取包装时间
                                DateTime starttime1 = Convert.ToDateTime(starttime2);//开始时间
                                string now1 = now.ToString("yyyy-MM-dd HH:mm:ss");
                                DateTime d3 = Convert.ToDateTime(now1);//把当前时间转变为datetime
                                TimeSpan d4 = d3.Subtract(starttime1);//开始时间-当前时间

                                int fpack = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());//差值转换为分钟数
                                string packtime = fpack.ToString();
                                //sql = "INSERT INTO [Packing](DecoctingNum,packtime,Fpactate) VALUES('" + a + "','" + fpack + "','" + 2 + "')";
                                sql = "Update Packing set PacTime='" + now + "',  Fpactate ='1' where  DecoctingNum =  '" + a + "'";
                                if (db.cmd_Execute(sql) == 1)
                                {
                                    string sql1 = "update prescription set curstate = '包装完成'  where id = '" + a + "'";
                                    if (db.cmd_Execute(sql1) == 1)
                                    {
                                        string sql2 = "INSERT INTO [Delivery](DecoctingNum,Sendstate) VALUES('" + a + "','0')";
                                        if (db.cmd_Execute(sql2) == 1)
                                        {
                                            string str5 = "select machineid from tisaneunit where pid = '" + DecoctingNum + "'";
                                            SqlDataReader sr01 = db.get_Reader(str5);
                                            if (sr01.Read())
                                            {
                                                string machineid = sr01["machineid"].ToString();
                                                //更改包装机状态
                                                string sq6 = "select unitnum from machine where id = '" + machineid + "'";
                                                SqlDataReader sr6 = db.get_Reader(sq6);
                                                if (sr6.Read())
                                                {
                                                    string unitnum = sr6["unitnum"].ToString();
                                                    string sq7 = "update machine set status ='空闲' where unitnum = '" + unitnum + "' and mark =1";
                                                    db.cmd_Execute(sq7);
                                                }

                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                sql = "";
                            }
                        }

                    }

                }
                #endregion 

                #region 未开始包装判断
                else
                {

                    string sql2 = "INSERT INTO [Packing](DecoctingNum,Starttime,Fpactate,Pacpersonnel,employeeid) VALUES('" + a + "','" + now + "','" + 0 + "','" + per + "','" + employeeid + "')";
                    //开始包装
                    BaseInfo.Insert_PackCmd(now, db, DecoctingNum.ToString());
                    //SqlDataReader tate13 = db.get_Reader(sql2);
                    if (db.cmd_Execute(sql2) == 1)
                    {
                        sql = "update prescription set doperson ='" + per + "',curstate = '开始包装'  where id = '" + a + "'";
                        db.cmd_Execute(sql);
                        // sql = "update prescription set doperson ='" + per + "',curstate = '开始包装'  where id = '" + a + "'";
                        //db.cmd_Execute(sql2);


                        //把机组表里的包装状态设为1
                        string now1 = now.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime d3 = Convert.ToDateTime(now1);//把当前时间转变为datetime

                        sql = "update tisaneunit set packstatus = 1,endDate ='" + d3 + "' where pid ='" + a + "'";
                        db.cmd_Execute(sql);


                        //更改煎药机状态为空闲
                        //string changetisanestatu = "update machine set status ='空闲' where id = (select machineid from tisaneunit where pid = '"+a+"')";
                        // db.cmd_Execute(changetisanestatu);

                        //更改包装机状态为忙碌
                        //  string changepackstatu = "update machine set status ='忙碌' where id = (select machineid from tisaneunit where pid = '" + a + "')";
                        // db.cmd_Execute(changepackstatu);



                        string machineid = "";
                        string unitnum = "";
                        string str5 = "select machineid from tisaneunit where pid = '" + DecoctingNum + "'";
                        SqlDataReader sr5 = db.get_Reader(str5);


                        if (sr5.Read())
                        {

                            machineid = sr5["machineid"].ToString();
                            //更改包装机状态
                            string sq6 = "select unitnum from machine where id = '" + machineid + "'";
                            SqlDataReader sr6 = db.get_Reader(sq6);
                            if (sr6.Read())
                            {
                                unitnum = sr6["unitnum"].ToString();
                                string sq7 = "update machine set status ='忙碌' where unitnum = '" + unitnum + "'and mark =1";
                                db.cmd_Execute(sq7);
                            }

                        }

                        string sql9 = "update machine set status ='空闲' where id = '" + machineid + "'";
                        db.cmd_Execute(sql9);


                    }
                }
                #endregion
            }
            #endregion
            else
            {
                sql = "";

            }

            if (sql == "")
            {
                end = 0;
            }
            else
            {
               // end = db.cmd_Execute(sql);
                end = 1;
            }
            return end;

        }
        #region 通过权限查询调剂人员
        public SqlDataReader findNameAll()
        {
            string sql = "select * from  Employee where Role ='5' or  Role ='0'";

            return db.get_Reader(sql);
        }

        #endregion
        #region 更新时获取包装信息
        public DataTable getpackingInfo(int id)
        {

            string sql = "select DecoctingNum,Pacpersonnel,PacTime,Fpactate,Starttime,Timeset from packing where id = '" + id + "'";
            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(sql);


            return dt;
        }
        #endregion
        #region 更新包装信息
        public int updatePackingInfo(int DecoctingNum, string Pacpersonnel, string PacTime, string Fpactate, string Starttime, string Timeset)
        {

            int end = 0;
            int a = 0;
            string sql = "";
            string str = "select id from tisaneinfo where id = '" + DecoctingNum + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                string result = sr["id"].ToString();
                a = Convert.ToInt32(result);
                sql = "update Packing set  DecoctingNum = '" + a + "',Pacpersonnel='" + Pacpersonnel + "',PacTime='" + PacTime + "',Fpactate='" + Fpactate + "',Starttime='" + Starttime + "',Timeset='" + Timeset + "' ";
                end = db.cmd_Execute(sql);
            }
            else
            {
                end = 0;
            }


            return end;
        }
        #endregion
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public DataTable SearchPacking()
        {
            string strSql = "select id ,DecoctingNum,Pacpersonnel,PacTime,Fpactate,Starttime,Timeset from Packing";
            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        //煎药警告
        public string packwarning()
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql3 = "";
            sql3 = "select ID,Pspnum,customid,delnum,(select top 1 packwarning from warning where hospitalid = p.Hospitalid and type=0) as packwarning,(select top 1 doingtime from bubble where pid = p.id) as doingtime,(SELECT top 1 bubbleperson FROM bubble WHERE pid = p.id) as bp,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql3 += "diagresult,(select top 1 warningtime from tisaneinfo where pid = p.id) as warningtime,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate";
            sql3 += " from prescription as p where id in (select pid from tisaneinfo where tisanestatus = 1)";

            SqlDataReader sr3 = db.get_Reader(sql3);//煎药完成的所有信息

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
                string d1 = sr3["packwarning"].ToString();//包装警告时间

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



                string d1 = list1[i].ToString();//包装警告时间

                DateTime d2 = Convert.ToDateTime(list2[i].ToString());//取药时间


                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d3 = Convert.ToDateTime(strY);//当前时间



                TimeSpan d4 = d2.Subtract(d3);//取药时间- 当前时间



                //取药时间- 当前时间
                int time = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());
                //包装警告时间
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
                    string strsql1 = "update tisaneinfo set warningstatus = 1,warningtype='包装预警' where pid = '" + list3[i] + "'";

                    db.cmd_Execute(strsql1);
                    //  if (list5[i].ToString() == "1970-1-1 0:00:00")
                    if (list5[i].ToString() == "" || Convert.ToDateTime(list5[i].ToString()).ToString("yyyy-MM-dd HH:mm:ss") == "1970-01-01 00:00:00")
                    {
                        string strsql2 = "update tisaneinfo set warningtime ='" + warningtime + "' where pid = '" + list3[i] + "'";

                        db.cmd_Execute(strsql2);
                    }


                }
                else
                {
                    string strsql2 = "update tisaneinfo set warningstatus = 0,warningtype='暂无预警',warningtime ='1970-1-1 00:00:00' where pid = '" + list3[i] + "'";

                    db.cmd_Execute(strsql2);
                }


            }
            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");



            string str2 = "";
            string str = "select pid from tisaneinfo where tisanestatus =1 and warningstatus = 1 and pid not in (select Decoctingnum from packing) and pid in (select id from prescription as p where  p.dotime between '" + strS + "' and  '" + strS2 + "')  ";
            SqlDataReader sr = db.get_Reader(str);

            while (sr.Read())
            {
                str2 += sr["pid"].ToString() + ",";

            }
            return str2;
        }



    }

}
