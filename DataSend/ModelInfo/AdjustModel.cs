using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;
namespace ModelInfo
{
    public class AdjustModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        //TEST
        #region 删除调剂信息
        ///// <summary>
        ///// 查询所有调剂信息
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>
        public int delAdjust(int id)
        {
            string sql = "delete from adjust where id="+id;

            return db.cmd_Execute(sql);

        }
        #endregion

        #region 更新调剂信息
        ///// <summary>
        ///// 更新调剂信息
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>
        public int updateAdjust(int id,string wordcontent, string wordDate, string workload)
        {
            string sql = "update adjust set wordcontent='" + wordcontent + "',wordDate='" + wordDate + "',workload='" + workload + "' where id=" + id;

            return db.cmd_Execute(sql);

        }
        ///// <summary>
        ///// 更新调剂状态
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>
        public int updateAdjust(int id,int status,string endDate,string userName,string tisaneNum)
        {
            string sql = "update adjust set status='" + status + "',endDate='"+endDate+"' where id=" + id;
            string sql2 = "update prescription set curstate = '调剂完成'  where id = '" + tisaneNum + "'";;
            db.cmd_Execute(sql2);
            return db.cmd_Execute(sql);

        }
        #endregion
         #region 添加调剂信息
        ///// <summary>
        ///// 更新调剂信息
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>
        public int addAdjust( string barcode, string SwapPer)
        {
           // string sql = "insert into adjust(employeeId,wordDate,barcode,wordcontent) values('" + userid + "','" + wordDate + "','" + barcode + "','" + wordcontent + "')";

           // return db.cmd_Execute(sql);
          
             int  barcode1 =Convert.ToInt32( barcode.Substring(4,10));
            String sql = "";
            int end = 0;
            string per = SwapPer.Substring(6);
            string employeeid = "";
           
            string str3 = "select id from employee where EmNumAName ='" + SwapPer + "'";
            SqlDataReader sr3 = db.get_Reader(str3);

            if (sr3.Read())
            {

                employeeid = sr3["id"].ToString();

            }
         
         //   string str1 = " select  prescriptionId  from PrescriptionCheckState  where  prescriptionId not in (select pid from InvalidPrescription) and  checkStatus =1 and  prescriptionId =" + barcode1;
          //  string str1 = "select id  from prescription where  id in (select prescriptionId from adjust where status in('1','0'))  and  id = '" + barcode1 + "'";
            string str1 = "select id  from prescription where   id = '" + barcode1 + "'";

            SqlDataReader sr1 = db.get_Reader(str1);

            if (sr1.Read())
           {
               string str = "select prescriptionId from adjust as a  where barcode ='" + barcode.Trim()+"'";
                SqlDataReader sr = db.get_Reader(str);
                System.DateTime now = new System.DateTime();
                now = System.DateTime.Now;
                if (sr.Read())
                {


                    string str2 = "select status from adjust  where  barcode='" + barcode.Trim()+"'";
                    SqlDataReader sr2 = db.get_Reader(str2);
                    if (sr2.Read())
                    {
                   
                        if (sr2["status"].ToString() == "0")
                        {
                            AdjustModel am = new AdjustModel();
                            DataTable dt = am.findAdjustBybarcode(barcode1.ToString());

                            sql = "update adjust set status= 1 ,endDate='" + now + "' where barcode='" + barcode.Trim()+"'";
                            if (db.cmd_Execute(sql) == 1)
                            {
                                sql = "update prescription set curstate = '调剂完成'  where id = '" + barcode1 + "'";

                            }
                        }
                        else {

                            sql = "";
                        }
                    }
                }else
                {
                    sql = "insert into adjust(wordDate,barcode,wordcontent,prescriptionId,SwapPer,employeeId) values('" + now + "','" + barcode + "',' 调剂 ','" + barcode1 + "','" + per + "','" + employeeid + "')";
                    if (db.cmd_Execute(sql) == 1)
                    {
                        sql = "update prescription set doperson ='" + per + "',curstate = '开始调剂'  where id = '" + barcode1 + "'";
                     //db.cmd_Execute(sql2);
                    }
                }
            }
           else {
                sql = "";
           }

            if (sql== "")
            {

                end = 0;   
            }
            else
            {
                end = db.cmd_Execute(sql);
               
            }
            return end; ;

        }
        public int addAdjust(int userid, string wordDate, string barcode, string wordcontent, string tisaneNum, string imgname, string userName)
        {
            string sql = "insert into adjust(employeeId,wordDate,barcode,wordcontent,prescriptionId,imgname,SwapPer) values('" + userid + "','" + wordDate + "','" + barcode + "','" + wordcontent + "','" + tisaneNum + "','" + imgname + "','" + userName + "')";
            string sql2 = "update prescription set doperson ='" + userName + "',curstate = '开始调剂'  where id = '" + tisaneNum + "'";
            db.cmd_Execute(sql2);
            return db.cmd_Execute(sql);

        }

        
        #endregion
        
        #region 查询调剂信息
        ///// <summary>
        ///// 查询所有调剂信息
        ///// </summary>
        ///// <param name="status">0未完成,1已完成,2全部</param>
        ///// <param name="begindate">开始日期</param>
        ///// <param name="enddate">结束日期</param>
        ///// <param name="eName">员工姓名</param>
        ///// <returns>DataTable对象</returns>
        public DataTable findRecipeInfo(int status,string begindate,string enddate,string eName)
        {
            string sql = "select  max(id) as id,SwapPer,convert(varchar, wordDate, 111) as wordDate,'调剂' AS wordcontent, count(wordDate)  as workload from adjust as a  where 1=1 ";

            if (status != 2 )
            {

                sql += " and a.status=" + status;

            }
            if (begindate != null && begindate.Length > 0)
            {
                DateTime d = Convert.ToDateTime(begindate);
                string strB = d.ToString("yyyy/MM/dd  00:00:00");
                sql += " and a.wordDate>='" + strB + "'";

            }
            if (enddate != null && enddate.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(enddate);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += " and a.wordDate<='" + strE + "'";

            }


            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }
            sql += "GROUP BY  SwapPer,CONVERT(varchar, wordDate, 111)";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }


        public int findRecipeInfots(int status, string begindate, string enddate, string eName)
        {
            string sql = "select count(*) from  ( select  max(id) as id,SwapPer,convert(varchar, wordDate, 111) as wordDate,'调剂' AS wordcontent, count(wordDate)  as workload from adjust as a  where 1=1 ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (begindate != null && begindate.Length > 0)
            {
                DateTime d = Convert.ToDateTime(begindate);
                string strB = d.ToString("yyyy/MM/dd  00:00:00");
                sql += " and a.wordDate>='" + strB + "'";

            }
            if (enddate != null && enddate.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(enddate);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += " and a.wordDate<='" + strE + "'";

            }


            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }
            sql += "GROUP BY  SwapPer,CONVERT(varchar, wordDate, 111)";
            sql += ") as b";
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;

        }


        public DataTable findRecipeInfofy(int status, string begindate, string enddate, string eName, int ts, int page)
        {
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select  max(id) as id,SwapPer,convert(varchar, wordDate, 111) as wordDate,'调剂' AS wordcontent, count(wordDate)  as workload from adjust as a  where 1=1 ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (begindate != null && begindate.Length > 0)
            {
                DateTime d = Convert.ToDateTime(begindate);
                string strB = d.ToString("yyyy/MM/dd  00:00:00");
                sql += " and a.wordDate>='" + strB + "'";

            }
            if (enddate != null && enddate.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(enddate);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += " and a.wordDate<='" + strE + "'";

            }


            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }
            sql += "GROUP BY  SwapPer,CONVERT(varchar, wordDate, 111)";
            sql+= ") t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(sql);
            return dt;

        }

        public DataTable findRecipeInfo()
        {
            string sql = "select  '调剂' AS wordcontent,convert(varchar, wordDate, 111) as wordDate,count(wordDate) AS workload from adjust as a left join employee as e on a.employeeId=e.id where 1=1 GROUP BY CONVERT(varchar, a.wordDate, 111)";

            


            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
        ///// <summary>
        ///// 查询所有调剂信息
        ///// </summary>
        ///// <param name="status">0未完成,1已完成,2全部</param>
        ///// <param name="begindate">开始日期</param>
        ///// <param name="enddate">结束日期</param>
        ///// <param name="eName">员工姓名</param>
        ///// <returns>DataTable对象</returns>
        public DataTable findAdjustById(int id)
        {
            string sql = "select a.id,wordcontent,convert(varchar, wordDate, 111) as wordDate,workload,employeeId,prescriptionId,status from adjust as a left join employee as e on a.employeeId=e.id where a.id="+id;

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }

        ///// <summary>
        ///// 根据条码查询调剂信息
        ///// </summary>
        ///// <param name="status">0未完成,1已完成,2全部</param>
        ///// <param name="begindate">开始日期</param>
        ///// <param name="enddate">结束日期</param>
        ///// <param name="eName">员工姓名</param>
        ///// <returns>DataTable对象</returns>
        public DataTable findAdjustBybarcode(string barcode)
        {
            string sql = "select a.id,wordcontent,convert(varchar, wordDate, 111) as wordDate,workload,employeeId,prescriptionId,status,barcode from adjust as a  where a.barcode='" + barcode + "'";

            db.write_log_txt("调剂扫码："+sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
        #endregion
        #region 通过权限查询调剂人员
        public SqlDataReader findNameAll()
        {
            string sql = "select * from  Employee where Role ='1' or  Role ='0' ";

            return db.get_Reader(sql);
        }
        public DataTable findNumById(string SwapPer)
        {
            string sql = "select EName from Employee where JobNum ='"+SwapPer+"'";

            return db.get_DataTable(sql);
        }
        #endregion
        /// <summary>
        /// 调剂大屏显示
        /// </summary>
        /// <returns></returns>
        public DataTable AdjustMonitors()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");
            DataBaseLayer db = new DataBaseLayer();
            string sql = @"select p.id, Pspnum,customid,delnum,h.Hnum ,h.Hname,Hospitalid,a.SwapPer,a.wordDate,a.endDate,a.status,p.name,sex,age,department,inpatientarea,ward,sickbed,
                           diagresult,takenum,getdrugtime,getdrugnum,decscheme,dose,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,
                           ordertime,curstate,RemarksA,RemarksB
                           from prescription as p 
                           right join adjust as a on p.ID=a.prescriptionId
                           left join Hospital as h on  h.ID=p.Hospitalid and h.DrugSendDisplayState='0'
                           where   p.ID in (select prescriptionId from adjust ) 
                           and p.ID not in (select pid from Audit ) and p.dotime between '" + strS + "' and '" + strS2 + "'";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        public DataTable AdjustMonitors_2()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");
            DataBaseLayer db = new DataBaseLayer();
            string sql = @"select p.id, Pspnum,customid,delnum,h.Hnum ,h.Hname,Hospitalid,a.SwapPer,a.wordDate,a.endDate,a.status,p.name,sex,age,department,inpatientarea,ward,sickbed,
                           diagresult,takenum,getdrugtime,getdrugnum,decscheme,dose,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,
                           ordertime,curstate,RemarksA,RemarksB
                           from prescription as p 
                           right join adjust as a on p.ID=a.prescriptionId
                           left join Hospital as h on  h.ID=p.Hospitalid and h.DrugSendDisplayState='0'
                           where   p.ID in (select prescriptionId from adjust ) 
                           and p.ID not in (select pid from Audit ) and p.dotime between '" + strS + "' and '" + strS2 + "'";
            db.write_log_txt("综合大屏调剂显示"+sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        public DataTable AdjustMonitors_1()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");
            DataBaseLayer db = new DataBaseLayer();
            string sql = @"   select p.id,h.Hname,p.name,a.SwapPer,p.dotime,a.wordDate,a.status
                           from prescription as p 
                           right join adjust as a on p.ID=a.prescriptionId
                           left join Hospital as h on  h.ID=p.Hospitalid and h.DrugSendDisplayState='0'
                           where 

                         p.curstate in('开始调剂','调剂完成') and 
                         p.ID not in (select pid from Audit )
                             and p.dotime >='" + strS + "' and p.dotime<='" + strS2 + "'";
            db.write_log_txt("调剂大屏显示SQL：" + sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        //大屏显示调剂信息

        public string getAdjustinfo()
        {

            DataBaseLayer db = new DataBaseLayer();

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");


            string str = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "'";
            SqlDataReader sdr = db.get_Reader(str);
            string ct = "";
            if (sdr.Read())
            {
                ct = sdr["ct"].ToString();
            }


            // string str2 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select decoctingnum from delivery where sendstate =0)";
            string str2 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select prescriptionId from adjust where status =0) AND (Hospitalid IN (SELECT   ID FROM Hospital WHERE (DrugSendDisplayState = '0')))";



            SqlDataReader sdr2 = db.get_Reader(str2);
            string ct2 = "";
            if (sdr2.Read())
            {
                ct2 = sdr2["ct"].ToString();
            }

            string str3 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select prescriptionId from adjust where status =1)";
            SqlDataReader sdr3 = db.get_Reader(str3);
            string ct3 = "";
            if (sdr3.Read())
            {
                ct3 = sdr3["ct"].ToString();
            }

            string result = "";
            result = "当日接单数:" + ct + "  　" + "已调剂:" + ct3 + "";



            return result;
        }
       
    }
}
