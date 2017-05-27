using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace ModelInfo
{
   public  class DeliveryModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        #region 查询所有发货信息
        ///// <summary>
        ///// 查询所有发货信息
        ///// </summary>
        ///// <param name="">id</param>
        ///// <returns>SqlDataReader对象</returns>
        public SqlDataReader findDeliveryAll()
        {
            string sql = "select * from Delivery ";

            return db.get_Reader(sql);
        }
        public DataTable findNumById(int id)
        {
            string sql = "select DecoctingNum  from Delivery  where ID =" + id;

            return db.get_DataTable(sql);
        }
        #endregion
        #region

        public DataTable findDeliveryInfo(int userid, string date,string hospital_name)
        {
            string sql = "";
            string flag = "";
            flag = db.get_role_by_userid(userid);
            string sql_1 = "";
            if (hospital_name != "0")
            {
                sql_1 = "'" + hospital_name.Replace("_", "','") + "'";
            }
            else
            {
                sql_1 = "select id from hospital";
            }
            if (flag != "0")
            {
                sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                      + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                      + ",d.Sendstate dstatus,d.SendTime dSendTime   from prescription as p inner join Delivery d on p.id=d.DecoctingNum left join hospital hs on hs.id = p.hospitalid where d.employeeId=" + userid + " and CONVERT(varchar, d.SendTime, 120) like '%" + date + "%' " + " and hs.id in(" + sql_1 + ")" + " order by d.SendTime desc";
            }
            else
            {
                sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                       + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                       + ",d.Sendstate dstatus,d.SendTime dSendTime  from prescription as p inner join Delivery d on p.id=d.DecoctingNum left join hospital hs on hs.id = p.hospitalid where " + "  CONVERT(varchar, d.SendTime, 120) like '%" + date + "%' " + " and hs.id in(" + sql_1 + ")" + " order by d.SendTime desc";
            }
            db.write_log_txt("pda发货列表： "+sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;


        }
        /// <summary>
        /// 查询发货信息
        /// </summary>
        /// <param > Sendstate,  SendTime, Sendpersonnel</param>
        /// <returns>dt</returns>

        public DataTable findDeliveryInfo(string   Sendstate, string  SendTime, string  Sendpersonnel, string Hospitalid, string GetDrugTime)
        {
           
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string sql = "select d.DecoctingNum ,  d.ID,d.Sendpersonnel ,d.warningstatus,d.SendTime,d.Sendstate,d.Remarks,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,";
            sql += "p.Pspnum,p.name,(select machinename from machine where id = (select top 1 machineid from tisaneinfo where pid = p.id )) as machineid,p.dose,p.takenum,p.packagenum,p.dtbaddress,p.getdrugtime,p.getdrugnum";
            sql += " from prescription as p left join Delivery as d on p.id =d.DecoctingNum  left join packing pa   on p.id =pa.DecoctingNum  where 1=1 ";
            if (Sendstate !="")
            {
             sql += " and d.Sendstate ='" + Sendstate + "'";
            }
           

            if (SendTime != "0")
            {
                sql += "and Convert(varchar,SendTime ,120)   like '" + SendTime + "%'";

            }
            if (Sendpersonnel !="0")
            {

                sql += "and  Sendpersonnel='" + Sendpersonnel + "'";
            }
            if (Hospitalid != "0")
            {
                sql += "and  p.hospitalid='" + Hospitalid + "'";
            }
            if (GetDrugTime != "0")
            {
                sql += "and Convert(varchar,p.getdrugtime ,120)   like '" + GetDrugTime + "%'";
            }
            sql += " order by d.DecoctingNum desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }

        /// <summary>
        /// 查询发货信息
        /// </summary>
        /// <param > Sendstate,  SendTime, Sendpersonnel</param>
        /// <returns>dt</returns>

        public int findDeliveryInfots(string Sendstate, string SendTime, string Sendpersonnel, string Hospitalid, string GetDrugTime)
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string sql = "select count(*) from prescription as p left join Delivery as d on p.id =d.DecoctingNum  left join packing pa   on p.id =pa.DecoctingNum  where 1=1 ";
            if (Sendstate != "")
            {
                sql += " and d.Sendstate ='" + Sendstate + "'";
            }


            if (SendTime != "0")
            {
                sql += "and Convert(varchar,SendTime ,120)   like '" + SendTime + "%'";

            }
            if (Sendpersonnel != "0")
            {

                sql += "and  Sendpersonnel='" + Sendpersonnel + "'";
            }
            if (Hospitalid != "0")
            {
                sql += "and  p.hospitalid='" + Hospitalid + "'";
            }
            if (GetDrugTime != "0")
            {
                sql += "and Convert(varchar,p.getdrugtime ,120)   like '" + GetDrugTime + "%'";
            }
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;

        }

        /// <summary>
        /// 查询发货信息
        /// </summary>
        /// <param > Sendstate,  SendTime, Sendpersonnel</param>
        /// <returns>dt</returns>

        public DataTable findDeliveryInfofy(string Sendstate, string SendTime, string Sendpersonnel, string Hospitalid, string GetDrugTime, int ts, int page)
        {

            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select d.DecoctingNum ,  d.ID,d.Sendpersonnel ,d.warningstatus,d.SendTime,d.Sendstate,d.Remarks,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,";
            sql += "p.Pspnum,p.name,(select machinename from machine where id = (select top 1 machineid from tisaneinfo where pid = p.id )) as machineid,p.dose,p.takenum,p.packagenum,p.dtbaddress,p.getdrugtime,p.getdrugnum";
            sql += " from prescription as p left join Delivery as d on p.id =d.DecoctingNum  left join packing pa   on p.id =pa.DecoctingNum  where 1=1 ";
            if (Sendstate != "")
            {
                sql += " and d.Sendstate ='" + Sendstate + "'";
            }


            if (SendTime != "0")
            {
                sql += "and Convert(varchar,SendTime ,120)   like '" + SendTime + "%'";

            }
            if (Sendpersonnel != "0")
            {

                sql += "and  Sendpersonnel='" + Sendpersonnel + "'";
            }
            if (Hospitalid != "0")
            {
                sql += "and  p.hospitalid='" + Hospitalid + "'";
            }
            if (GetDrugTime != "0")
            {
                sql += "and Convert(varchar,p.getdrugtime ,120)   like '" + GetDrugTime + "%'";
            }
            sql += ") t  ) p where p.rownumber>" + (page - 1) * ts ;
            sql += " order by p.DecoctingNum desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }


        public DataTable findDeliveryInfoDao(string Sendstate, string SendTime, string Sendpersonnel)
        {



            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间
            
            //string sql = "select d.DecoctingNum ,  d.ID,d.Sendpersonnel ,d.warningstatus,d.SendTime,d.Sendstate,d.Starttime,d.Remarks, p.customid,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,";
            // sql += "p.diagresult,p.Pspnum,(select machinename from machine where id = (select machineid from tisaneinfo where pid = p.id )) as machineid,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.RemarksA,p.RemarksB,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate";
            // sql += " from prescription as p join Delivery as d on p.id =d.DecoctingNum   where d.DecoctingNum not in (select pid from InvalidPrescription) ";
            string sql = "select d.DecoctingNum ,  d.ID,d.Sendpersonnel ,d.SendTime,case d.Sendstate when '0' then '待发货' when '1' then '已发货' else d.Sendstate end,d.Remarks,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,";
            sql += "p.Pspnum,p.name,(select machinename from machine where id = (select machineid from tisaneinfo where pid = p.id )) as machineid,p.dose,p.takenum,p.packagenum,p.dtbaddress,p.getdrugtime,p.getdrugnum";
            sql += " from prescription as p left join Delivery as d on p.id =d.DecoctingNum   where 1=1 ";
            if (Sendstate != "")
            {
                sql += " and d.Sendstate ='" + Sendstate + "'";
            }


            if (SendTime != "0")
            {
                sql += "and Convert(varchar,SendTime ,120)   like '" + SendTime + "%'";

            }
            if (Sendpersonnel != "0")
            {

                sql += "and  Sendpersonnel='" + Sendpersonnel + "'";
            }
            sql += " order by d.DecoctingNum desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region
        /// <summary>
        /// 编辑时获取发货信息
        /// </summary>
        /// <param >id</param>
        /// <returns>dt</returns>
        public DataTable findDeliveryInfo(int id)
        {
            string strSql = "select DecoctingNum,Sendpersonnel,SendTime,Sendstate,Starttime,Remarks,kd_type,kd_num,demo from Delivery where DecoctingNum = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #region 更新发货信息
        public int updateDelivery(int DecoctingNum, string Sendpersonnel, string SendTime, string Sendstate, string Starttime, string Remarks, string kd_type, string kd_num, string demo)
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
                sql = "update Delivery set  DecoctingNum = '" + a + "',Sendpersonnel='" + Sendpersonnel + "',SendTime='" + SendTime +
                    "',Sendstate='" + Sendstate + "',Starttime='" + Starttime + "',Remarks='" + Remarks 
                       + "',kd_type='" + kd_type + "',"
                + "kd_num='" + kd_num + "',"
                + "demo='" + demo + "'"
                    +" where id = '" + DecoctingNum + "'";
                end = db.cmd_Execute(sql);
            }
            else
            {
                end = 0;
            }


            return end;
        }
        #endregion
        #endregion
        #region 删除发货信息
        public int  deleteDeliveryInfo(int nDId)
        {
            string strSql = "delete from Delivery where id =" + nDId;
            int n = db.cmd_Execute(strSql);
            return  n;
        }

        #endregion
        #region 发货大屏显示信息
        public DataTable MedicineDisplayInfo()
        {


            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");

            DataBaseLayer db = new DataBaseLayer();

            string sql = "select d.DecoctingNum ,  d.ID,(select PacTime from Packing as i where i.DecoctingNum = p.id ) as StartTime,d.Sendpersonnel ,d.warningstatus,d.SendTime,d.Sendstate,d.Starttime,d.Remarks, p.customid,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid ) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,";
            sql += "p.diagresult,p.Pspnum,(select machinename from machine where id = (select machineid from tisaneinfo where pid = p.id )) as machineid,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.RemarksA,p.RemarksB,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate";
            sql += " from prescription as p join Delivery as d on p.id =d.DecoctingNum   where d.DecoctingNum not in (select pid from InvalidPrescription) and d.Sendstate ='0' and p.hospitalid in (select id from hospital where DrugSendDisplayState='0') and p.dotime between '" + strS + "' and '" + strS2 + "' order by d.DecoctingNum desc";
        

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        //发货警告
        public string deliverywarning()
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql3 = "";
            sql3 = "select ID,Pspnum,customid,delnum,(select deliverwarning from warning where hospitalid = p.Hospitalid) as deliverwarning,(select doingtime from bubble where pid = p.id) as doingtime,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql3 += "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate";
            sql3 += " from prescription as p where id in (select DecoctingNum from Packing where Fpactate = 2)";

            SqlDataReader sr3 = db.get_Reader(sql3);//包装完成的所有信息

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间

            ArrayList list2 = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list3 = new ArrayList();
            while (sr3.Read())
            {

                // sr3["bubblewarning"].ToString();
                // sr3["getdrugtime"].ToString();
                string d1 = sr3["deliverwarning"].ToString();//发货警告时间

                list1.Add(d1);

                string drugtime = sr3["getdrugtime"].ToString();//得到该处方号的取药时间
                //  DateTime d2 = Convert.ToDateTime(sr3["getdrugtime"].ToString());
                list2.Add(drugtime);


                string id = sr3["ID"].ToString();//当前id煎药单号
                list3.Add(id);





            }
            for (int i = 0; i < list2.Count; i++)
            {



                string d1 = list1[i].ToString();//发货警告时间

                DateTime d2 = Convert.ToDateTime(list2[i].ToString());//取药时间


                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d3 = Convert.ToDateTime(strY);//当前时间



                TimeSpan d4 = d2.Subtract(d3);//取药时间- 当前时间



                //取药时间- 当前时间
                int time = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());
                //发货警告时间
                int time2 = Convert.ToInt32(d1);
                if (time < time2)
                {
                    string strsql1 = "update packing set warningstatus = 1 where DecoctingNum = '" + list3[i] + "'";

                    db.cmd_Execute(strsql1);
                }


            }

            string str2 = "";
            string str = "select DecoctingNum from packing where Fpactate =2 and warningstatus = 1";
            SqlDataReader sr = db.get_Reader(str);

            while (sr.Read())
            {

                str2 += sr["DecoctingNum"].ToString() + ",";

            }
            return str2;
        }
      


    }
}
