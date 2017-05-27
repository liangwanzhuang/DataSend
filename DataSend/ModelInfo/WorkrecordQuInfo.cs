using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using SQLDAL;
using System.Collections;
using System.Data;

namespace ModelInfo
{
    public class WorkrecordQuInfo
    {
        public DataBaseLayer db = new DataBaseLayer();
        #region 添加工作记录查询信息
        public int AddWorkrecordQuInfo(Int64 DecoctingNum)
        {
            String sql = "";
            int end = 0;
            string str = "select id  from prescription where   id  = '" + DecoctingNum + "'";
            SqlDataReader sr = db.get_Reader(str);

            if (sr.Read())
            {
                string str1 = "select *  from WorkrecordQuery where   pid  = '" + DecoctingNum + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read())
                {
                    sql = "";
                }
                else
                {
                    sql = "INSERT INTO [WorkrecordQuery](pid) VALUES('" + DecoctingNum + "')";
                }
            }
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
                end = db.cmd_Execute(sql);
            }
            return end;
            /* String sql = "";
             int end = 0;
             string str = "select id  from prescription where  id in (Select prescriptionId from AgainPrescriptionCheckState  where checkStatus = 1) and  id  = '" + DecoctingNum + "'";
             SqlDataReader sr = db.get_Reader(str);

             if (sr.Read())
             {
                 string result = sr["id"].ToString();
                 int a = Convert.ToInt32(result);

                 string str1 = "select * from WorkrecordQuery where pid = '" + a + "'";
                 SqlDataReader sr1 = db.get_Reader(str1);

                 if (sr1.Read())
                 {
                     sql = "";
                 }
                 else
                 {

                     sql = "INSERT INTO [WorkrecordQuery](pid) VALUES('" + a + "')";
                 }
             }
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
                 end = db.cmd_Execute(sql);
             }
             return end; ;*/
        }
        #endregion

        #region 删除工作记录查询信息
        public int deleteWorkrecordQuInfo(int id)
        {
            string strSql = "delete from WorkrecordQuery where pid = '" + id + "'";
            int n = db.cmd_Execute(strSql);


            return n;
        }
        #endregion
        #region 更新时获取工作记录查询信息
        public DataTable getWorkrecordqueryInfo(int id)
        {

            string sql = "select hospitalid,pspnum  from prescription where id = '" + id + "'";
            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(sql);


            return dt;
        }
        #endregion
        #region 更新工作记录查询信息
        public int updateWorkrecordqueryInfo(int id, int hospitalSelect, string pspnum)
        {

            int end = 0;
            int a = 0;
            string sql = "";
            string str = "select id from prescription where hospitalid = '" + hospitalSelect + "' and pspnum = '" + pspnum + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                string result = sr["id"].ToString();
                a = Convert.ToInt32(result);
                sql = "update WorkrecordQuery set pid='" + a + "' where pid = '" + id + "' ";
                end = db.cmd_Execute(sql);
            }
            else
            {
                end = 0;
            }


            return end;
        }
        #endregion



        //复核警告
        public string recheckwarning()
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql3 = "";
            sql3 = "select ID,Pspnum,customid,delnum,(select top 1 recheckwarning from warning where hospitalid = p.Hospitalid and type=0) as recheckwarning,(select top 1 doingtime from bubble where pid = p.id) as doingtime,(SELECT top 1 bubbleperson FROM bubble WHERE pid = p.id) as bp,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql3 += "diagresult,(select top 1 warningtime from adjust where prescriptionId =p.id) as warningtime,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate";
            sql3 += " from prescription as p where id in (select prescriptionId from adjust where status =1)";

            SqlDataReader sr3 = db.get_Reader(sql3);//调剂完成的所有信息

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
                string d1 = sr3["recheckwarning"].ToString();//复核警告时间

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
                //复核警告时间
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
                    string strsql1 = "update adjust set warningstatus = 1,warningtype='复核预警' where prescriptionid = '" + list3[i] + "'";

                    db.cmd_Execute(strsql1);

                    // if (list5[i].ToString() == "1970-1-1 0:00:00")
                    if (list5[i].ToString() == "" || Convert.ToDateTime(list5[i].ToString()).ToString("yyyy-MM-dd HH:mm:ss") == "1970-01-01 00:00:00")
                    {
                        string strsql2 = "update adjust set warningtime ='" + warningtime + "' where prescriptionid = '" + list3[i] + "'";

                        db.cmd_Execute(strsql2);

                    }

                }
                else
                {
                    string strsql2 = "update adjust set warningstatus =0,warningtype='暂无预警',warningtime='1970-1-1 0:00:00' where prescriptionid = '" + list3[i] + "'";

                    db.cmd_Execute(strsql2);
                }


            }


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");





            string str2 = "";
            string str = "select prescriptionId from adjust where status =1 and  warningstatus = 1 and prescriptionid not in (select pid from audit) and prescriptionId in (select id from prescription as p where p.dotime between '" + strS + "' and  '" + strS2 + "')";
            SqlDataReader sr = db.get_Reader(str);

            while (sr.Read())
            {

                str2 += sr["prescriptionId"].ToString() + ",";

            }
            return str2;
        }
    }

}
