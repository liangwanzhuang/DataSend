using System;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;
using System.Collections;
using System.Text;
namespace ModelInfo
{
    public class RecipeModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        public int GetzpInfoCount(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
       {
           /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
           string strSql = " select count(a.id) from (select distinct p.id from prescription as p  inner join tb_node_image i on i.n_id=p.id where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
               + " 1=1 ";




           if (hospitalid != "0")
           {
               strSql += "and hospitalid ='" + hospitalid + "'";
           }

           if (strName != "0")
           {
               strSql += "and name ='" + strName + "'";
           }
           if (strTime != "0")
           {
               //   strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
               strSql += "and p.ordertime>= '" + strTime + "'" + " 00:00:00";
           }
           if (strPspnum != "0")
           {
               strSql += "and Pspnum ='" + strPspnum + "'";
           }
           if (tisaneid != "0")
           {
               strSql += "and ID ='" + tisaneid + "'";
           }
           /*     if (jftime != "")
                {
                    DateTime d = Convert.ToDateTime(jftime);
                    string strS = d.ToString("yyyy/MM/dd 00:00:00");
                    string strE = d.ToString("yyyy/MM/dd 23:59:59");
                    //
                    strSql += "and dotime between '" + strS + "' and '" + strE + "'";
                }
                if (doper != "0")
                {
                    strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
                }

                strSql += "order by p.id desc";
    */
           strSql += ") a ";
           int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSql));

           return count;

       }
       /// <summary>
       /// 处方列表，分页显示
       /// </summary>
       /// <returns></returns>
        public DataTable findzpInfo(int ts, int page, string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
       {
           /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
        strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
        strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
           /* string strSql = "select top " + ts + " *  from " + "(select row_number() over(order by a.id desc ) as idd,a.* from (select distinct p.ID,p.delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p  inner join tb_node_image i on i.n_id=p.id where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
                + " 1=1 ";
            */
           string strSql = "select top " + ts + " *  from " + "(select row_number() over(order by a.id desc ) as idd,a.* from (select distinct p.ID,p.delnum,h.hnum as hnum,h.hname as hname,p.Pspnum,p.decmothed,p.name,p.sex,p.age,p.phone,p.address" +
          " from prescription as p  inner join tb_node_image i on i.n_id=p.id inner join hospital h on h.id=p.Hospitalid where " +
              " 1=1 ";


           if (hospitalid != "0")
           {
               strSql += "and hospitalid ='" + hospitalid + "'";
           }

           if (strName != "0")
           {
               strSql += "and name ='" + strName + "'";
           }
           if (strTime != "0")
           {
               //   strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
               strSql += "and p.ordertime>= '" + strTime + "'" + " 00:00:00";
           }
           if (strPspnum != "0")
           {
               strSql += "and Pspnum ='" + strPspnum + "'";
           }
           if (tisaneid != "0")
           {
               strSql += "and ID ='" + tisaneid + "'";
           }
           /*     if (jftime != "")
                {
                    DateTime d = Convert.ToDateTime(jftime);
                    string strS = d.ToString("yyyy/MM/dd 00:00:00");
                    string strE = d.ToString("yyyy/MM/dd 23:59:59");
                    //
                    strSql += "and dotime between '" + strS + "' and '" + strE + "'";
                }
                if (doper != "0")
                {
                    strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
                }

                strSql += "order by p.id desc";
    */
           strSql += ") a) t  where t.idd>" + (page - 1) * ts + " order by t.idd asc";
           db.write_log_txt("pda:"+strSql );
           DataTable dt = db.get_DataTable(strSql);

           return dt;

       }
  
        #region 根据医院+姓名+订单日期 进行综合查询
        public DataTable hospitalSearch(string hospitalname, string patient, string ordertime, string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string jftime)
        {
            string strSQL = "select (select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.ordertime,p.curstate,p.department,p.department,p.inpatientarea,p.ward,p.sickbed,p.diagresult,p.takemethod,p.takenum,p.packagenum,p.decscheme,oncetime,p.twicetime,p.soakwater,p.getdrugtime,"
            + "(select jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dotime,(select PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer,(select PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,(select SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate,(select ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select starttime from  bubble as b where  b.pid=p.ID ) as bstarttime, "
            + "(select tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,(select machinename from machine where id = (select machineid from tisaneunit where pid = p.id )) as machineid,(select starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,(select Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,( select machinename  from machine   where  mark =1 and unitnum in (select unitnum from machine where id in (select machineid from tisaneunit as t where t.pid =p.id))) as packmachine,(select PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime,(select Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,case p.dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' when 4 then 'EMS' when 0 then '厂内配送' else p.dtbtype end  as dtbtype,( select Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel,( select SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime,( select logisticsnum from Delivery as d  where d.DecoctingNum  =p.ID ) as logisticsnum from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (hospitalname != "0")
            {
                strSQL += "and (select hname from hospital as h where h.id = p.hospitalid)  ='" + hospitalname + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }
            if (ordertime != null && ordertime.Length > 0)
            {
                DateTime d = DateTime.Parse(ordertime);

                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.ordertime between '" + strS + "' and '" + strE + "'";
            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }

            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "order by id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 根据医院+处方号进行综合查询
        public DataTable hospitalchufangSearch(string hospitalname, string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime)
        {
            string strSQL = "select (select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.curstate,p.department,p.department,p.inpatientarea,p.ward,p.sickbed,p.diagresult,p.takemethod,p.takenum,p.packagenum,p.decscheme,oncetime,p.twicetime,p.soakwater,p.getdrugtime,"
            + "(select jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dotime,(select PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer,(select PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,(select SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate,(select ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select starttime from  bubble as b where  b.pid=p.ID ) as bstarttime, "
            + "(select tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,(select machinename from machine where id = (select machineid from tisaneunit where pid = p.id )) as machineid,(select starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,(select Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,( select machinename  from machine   where  mark =1 and unitnum in (select unitnum from machine where id in (select machineid from tisaneunit as t where t.pid =p.id))) as packmachine,(select PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime,(select Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,case p.dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' when 4 then 'EMS' when 0 then '厂内配送' else p.dtbtype end  as dtbtype,( select Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel,( select SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime,( select logisticsnum from Delivery as d  where d.DecoctingNum  =p.ID ) as logisticsnum from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (hospitalname != "0")
            {
                strSQL += "and (select hname from hospital as h where h.id = p.hospitalid)  ='" + hospitalname + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "order by id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 根据姓名+处方号进行综合查询
        public DataTable namechufangSearch(string name, string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime)
        {
            string strSQL = "select (select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.curstate,p.department,p.department,p.inpatientarea,p.ward,p.sickbed,p.diagresult,p.takemethod,p.takenum,p.packagenum,p.decscheme,oncetime,p.twicetime,p.soakwater,p.getdrugtime,"
            + "(select jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dotime,(select PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer,(select PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,(select SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate,(select ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select starttime from  bubble as b where  b.pid=p.ID ) as bstarttime, "
            + "(select tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,(select machinename from machine where id = (select machineid from tisaneunit where pid = p.id )) as machineid,(select starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,(select Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,( select machinename  from machine   where  mark =1 and unitnum in (select unitnum from machine where id in (select machineid from tisaneunit as t where t.pid =p.id))) as packmachine,(select PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime,(select Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,case p.dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' when 4 then 'EMS' when 0 then '厂内配送' else p.dtbtype end as dtbtype,( select Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel,( select SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime,( select logisticsnum from Delivery as d  where d.DecoctingNum  =p.ID ) as logisticsnum from prescription as p where 1=1";
            if (name != "0")
            {
                strSQL += "and p.name ='" + name + "'";
            }


            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "order by id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        public DataTable findPrescriptionById(string pid)
        {
            string sql = "SELECT   ID, customid, delnum, barcodescan, Hospitalid, Pspnum, decmothed, name, sex, age, phone, address, department,"
               + " inpatientarea, ward, sickbed, diagresult, dose, takemethod, takenum, packagenum, decscheme, oncetime, twicetime, "
               + " soakwater, soaktime, labelnum, remark, doctor, footnote, getdrugtime, getdrugnum, ordertime, curstate, dotime, "
               + " doperson, dtbcompany, dtbaddress, dtbphone, dtbtype, takeway, RemarksA, RemarksB, confirmDrug "
               + " FROM prescription WHERE (ID = '" + pid + "')";
            db.write_log_txt("insert_drug_1:" + sql);
            return db.get_DataTable(sql);
        }
        public DataTable findSingleDrugInfo(string pid, string ypid)
        {
            string sql = "select ID,delnum,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where pid = '" + pid + "'and drugnum = '" + ypid + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public SqlDataReader findRecipAlla()
        {
            string sql = "select   case dtbcompany when '' then '无'else dtbcompany end as b from prescription group by dtbcompany";

            return db.get_Reader(sql);
        }
        public SqlDataReader findRecipAll()
        {
            string sql = "select  distinct Pspnum, dtbcompany  from prescription";

            return db.get_Reader(sql);
        }
        public SqlDataReader findRecipAll11()
        {
            string sql = "select distinct dtbcompany from prescription";

            return db.get_Reader(sql);
        }
        #region 获取委托单号的最大值+1
        public int getPrescriptionMaxDelnum()
        {
            string sql = "select max(delnum)+1 as delnumMax from prescription";
            DataTable dt = db.get_DataTable(sql);
            int delnumMax = 0;
            if (dt.Rows.Count > 0)
            {
                delnumMax = Convert.ToInt32(dt.Rows[0]["delnumMax"]);
            }
            if (delnumMax < 10000)
            {
                delnumMax = 10001;
            }
            return delnumMax;
        }
        #endregion
        #region 更新处方状态
        public int updatePrescriptionStatus(int pid, string status)
        {
            // if (status == "0")
            // {

            //string sql = "update prescription set confirmDrug='" + status + "' where ID='" + pid + "'";
            //  string sql = "update prescription set curstate='" + status + "' where ID='" + pid + "'";
            // string sql0 = "select count(id) as cid from  PrescriptionCheckState where prescriptionId ='"+pid.ToString().Trim()+"'";
            //  SqlDataReader sdr2 = db.get_Reader(sql0);
            /*  string flag = "0";
              if (sdr2.Read())
              {
                  flag = sdr2["cid"].ToString().Trim();
              } 
              string sql ="";
              if (flag != "0")
              {
                sql = "update  PrescriptionCheckState  set checkStatus ='" + status.Trim() + "'" + " where prescriptionId='" + pid.ToString() + "'";
              }
              if (flag == "0")
              {
                  String sql1 = "insert into PrescriptionCheckState(prescriptionId,checkStatus,refusalreason,tisaneNumber,PartyTime,PartyPer,employeeid) values('" + pid.ToString() + "','" + "1" + "','" + "" + "','" + pid.ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00'".TrimEnd() + "','" + name + "','" + emid + "')";
                  int result = db.cmd_Execute(sql1);

                  sql = "update  PrescriptionCheckState  set checkStatus ='" + status.Trim() + "'" + " where prescriptionId='" + pid.ToString() + "'";
              }
              */

            //   }
            string sql = "update  PrescriptionCheckState  set checkStatus ='" + status.Trim() + "'" + " where prescriptionId='" + pid.ToString() + "'";
            return db.cmd_Execute(sql);
        }
        public int updatePrescriptionStatus_1(int pid, string status, string name, string emid)
        {
            // if (status == "0")
            // {

            //string sql = "update prescription set confirmDrug='" + status + "' where ID='" + pid + "'";
            //  string sql = "update prescription set curstate='" + status + "' where ID='" + pid + "'";
            string sql0 = "select count(id) as cid from  PrescriptionCheckState where prescriptionId ='" + pid.ToString().Trim() + "'";
            SqlDataReader sdr2 = db.get_Reader(sql0);
            string flag = "0";
            if (sdr2.Read())
            {
                flag = sdr2["cid"].ToString().Trim();
            }
            string sql = "";
            if (flag != "0")
            {
                sql = "update  PrescriptionCheckState  set checkStatus ='" + status.Trim() + "'" + " where prescriptionId='" + pid.ToString() + "'";
            }
            if (flag == "0")
            {
                String sql1 = "insert into PrescriptionCheckState(prescriptionId,checkStatus,refusalreason,tisaneNumber,PartyTime,PartyPer,employeeid) values('" + pid.ToString() + "','" + "1" + "','" + "" + "','" + pid.ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "','" + name + "','" + emid + "')";
                int result = db.cmd_Execute(sql1);

                sql = "update  PrescriptionCheckState  set checkStatus ='" + status.Trim() + "'" + " where prescriptionId='" + pid.ToString() + "'";
            }


            //   }
            return db.cmd_Execute(sql);
        }
        #region 更新处方状态
        public int updatePrescriptionStatus_1(int pid, string status)
        {
            // if (status == "0")
            // {

            //string sql = "update prescription set confirmDrug='" + status + "' where ID='" + pid + "'";
            string sql = "update prescription set curstate='" + status + "' where ID='" + pid + "'";

            string sql_1 = "update  PrescriptionCheckState  set checkStatus ='" + "0" + "'" + " where prescriptionId='" + pid.ToString() + "'";
            db.cmd_Execute(sql_1);
            db.cmd_Execute(sql);
            //   }
            return 1;
        }
        #endregion
        #endregion
        #region 确认药品已录入完成
        public int confirmDrug(int pid)
        {
            string sql = "update prescription set confirmDrug='1' where ID='" + pid + "'";

            return db.cmd_Execute(sql);
        }
        #endregion

        #region 判断处方是否匹配完
        public bool checkPrescriptionIsMath(int pid)
        {
            string sql2 = "SELECT DISTINCT COUNT(*) AS drugNum FROM prescription AS p LEFT OUTER JOIN PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId LEFT OUTER JOIN"
                + " drug AS d ON d.pid =p.id WHERE   (p.ID = " + pid + ")";
            DataTable dt2 = db.get_DataTable(sql2);
            int drugTotal = 0;
            if (dt2.Rows.Count > 0)
            {
                drugTotal = Convert.ToInt32(dt2.Rows[0]["drugNum"].ToString());

            }

            string sql = "SELECT DISTINCT COUNT(*) AS drugNum FROM prescription AS p LEFT OUTER JOIN PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId LEFT OUTER JOIN"
               + " drug AS d ON d.pid =p.id RIGHT OUTER JOIN DrugMatching AS dm ON d.ID = dm.drugId AND dm.pspId = p.ID"
               + " WHERE   (p.ID = " + pid + ")";
            DataTable dt = db.get_DataTable(sql);
            int mathTotal = 1;

            if (dt.Rows.Count > 0)
            {
                mathTotal = Convert.ToInt32(dt.Rows[0]["drugNum"].ToString());

            }
            if (drugTotal == mathTotal)
            {
                return true;
            }


            return false;
        }
        #endregion
        #region 审核处方
        ///// <summary>
        ///// 审核处方
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <param name="check_result)">审核状态,0未审核,1通过,2不通过</param>
        ///// <param name="refusalreason)">不通过原因</param>
        ///// <returns>int对象</returns>
        public int checkPrescription(int pid, int check_result, string refusalreason, string name, string emid)
        {

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间
            string stateSql = "select id,prescriptionId,checkStatus,refusalreason from PrescriptionCheckState where prescriptionId=" + pid;
            SqlDataReader srd = db.get_Reader(stateSql);
            Boolean isCheck = false;
            while (srd.Read())
            {
                isCheck = true;
            }

            if (!isCheck)
            {

                string sql2 = "SELECT MAX(id) + 1 AS Expr1 FROM PrescriptionCheckState";
                DataTable dt2 = db.get_DataTable(sql2);
                string tisaneNumber = dt2.Rows[0]["Expr1"].ToString();

                String sql = "insert into PrescriptionCheckState(prescriptionId,checkStatus,refusalreason,tisaneNumber,PartyTime,PartyPer,employeeid) values('" + pid + "','" + check_result + "','" + refusalreason + "','" + tisaneNumber + "','" + currentTime + "','" + name + "','" + emid + "')";
                int result = db.cmd_Execute(sql);
                if (result > 0 && check_result == 2)
                {
                    checkError(pid);
                }
                return result;
            }
            return 0;

        }
        public int checkError(int pid)
        {
            String sql = "update jfInfo set warningtype='审核异常',warningtime='" + System.DateTime.Now + "' where pid=" + pid;
            return db.cmd_Execute(sql);

        }
        #endregion
        
        public string defaultcheck(string pid)
        {

            RecipeModel rm = new RecipeModel();
            DataTable dt = rm.findspecialdrug2();
            string drugname1 = "";
            string drugname2 = "";
            string type = "0";

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                drugname1 = dt.Rows[i][2].ToString();
                drugname2 = dt.Rows[i][3].ToString();

                SqlDataReader sdr = rm.finddrugnamebypid(pid, drugname1);
                SqlDataReader sdr2 = rm.finddrugnamebypid(pid, drugname2);

                if (sdr.Read() && sdr2.Read())
                {
                    // z = "<span style='color:red'>"+dr["Drugname"].ToString()+"</span>";
                    type = dt.Rows[i][1].ToString();
                    break;
                }




            }

            return type;

        }
        //手动更新处方列表药品信息
        public int updateSingledruginfo(string id, string ip, string drugnum, string drugdescription, string drugposition, string drugweight, string description, string wholesaleprice, string wholesalecost, string moneywithtax, string drugname, string drugallnum, string tienum, string retailprice, string retailcost, string fee)
        {

            //double dWholeSalePrice = Convert.ToDouble(wholesaleprice);
            //double dRetailPrice = Convert.ToDouble(retailprice);

            string str = "update drug set drugnum='" + drugnum + "',drugdescription='" + drugdescription + "',drugposition='" + drugposition + "',drugweight='" + drugweight + "',description='" + description + "',wholesaleprice='" + wholesaleprice + "', drugname='" + drugname + "', drugallnum='" + drugallnum + "', tienum='" + tienum + "',retailprice='" + retailprice + "' where pid='" + ip + "' and drugnum='" + id + "'";
            int end = db.cmd_Execute(str);



            return end;
        }
        
        #region 根据处方id查询调剂已完成
        ///// <summary>
        ///// 查询调剂已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findAdjustFinish(int pid)
        {
            string sql = "select * from adjust where prescriptionId=" + pid + " and status = 1";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        public DataTable findAdjustFinish_scan(int pid)
        {
            string sql = "select * from adjust where prescriptionId=" + pid + " and status in('1','0')";
            db.write_log_txt("调剂2： " + sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询复核已完成
        ///// <summary>
        ///// 根据处方id查询复核已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findRecheckedFinish(int pid)
        {
            string sql = "select * from Audit where pid=" + pid + " and AuditStatus = 1";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询泡药已完成
        ///// <summary>
        ///// 根据处方id查询泡药已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findBubbleFinish(int pid)
        {
            string sql = "select * from bubble where pid=" + pid + " and bubblestatus = 1";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        public DataTable findBubbleFinish_0(int pid)
        {
            string sql = "select * from bubble where pid=" + pid + " and bubblestatus = 0";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询煎药已完成
        ///// <summary>
        ///// 根据处方id查询煎药已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findTisaneFinish(int pid)
        {
            string sql = "select * from tisaneinfo where pid=" + pid + " and tisanestatus = 1";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询包装已完成
        ///// <summary>
        ///// 根据处方id查询包装已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findPackingFinish(int pid)
        {
            string sql = "select * from Packing where DecoctingNum=" + pid + " and Fpactate = 1";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询发货已完成
        ///// <summary>
        ///// 根据处方id查询发货已完成
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findDeliveryFinish(int pid)
        {
            string sql = "select * from Delivery where DecoctingNum=" + pid + " and Sendstate = 0";

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion
        #region 根据处方id查询
        ///// <summary>
        ///// 根据处方id查询
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        ///// <returns>int对象</returns>
        public DataTable findRecipeInfoById(int pid)
        {
            string sql = "select * from prescription where ID=" + pid;

            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        ///// <summary>
        ///// 查询审核过的处方
        ///// </summary>
        ///// <param name="pid)">处方id</param>
        public DataTable findRecipeInfoByCheckId(int pid)
        {
            string sql = "select p.ID from prescription p inner join PrescriptionCheckState pc on p.ID=pc.prescriptionId and pc.checkStatus=1 where p.ID=" + pid;

            db.write_log_txt("调剂1：" + sql);
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        #endregion

        #region 重审处方

        public int AgainCheckPres(int pid, int check_result, string refusalreason)
        {

            String sql = "";
            int end = 0;
            string stateSql = "select  id,prescriptionId,checkStatus,refusalreason from AgainPrescriptionCheckState where prescriptionId=" + pid;
            SqlDataReader srd = db.get_Reader(stateSql);
            if (srd.Read())
            {


                if (check_result == 2)
                {
                    string sql1 = "update adjust  set status =0  where prescriptionId = '" + pid + "'";
                    //"insert into adjust(prescriptionId,status) values ('" + pid + "'," + 0 + ")";

                    if (db.cmd_Execute(sql1) == 1)
                    {
                        string sql2 = "delete *  from Audit  where prescriptionId = '" + pid + "'";
                        if (db.cmd_Execute(sql2) == 1)
                        {
                            sql = "update prescription  set curstate = '未通过复核'  where id = '" + pid + "'";
                            // db.cmd_Execute(sql3);
                        }
                    }
                }
                else
                {
                    sql = "update AgainPrescriptionCheckState set checkStatus = " + check_result + " where prescriptionId = '" + pid + "'";
                }


            }
            else
            {


                if (check_result == 2)
                {
                    string sql1 = "update adjust  set status =0  where prescriptionId = '" + pid + "'";
                    if (db.cmd_Execute(sql1) == 1)
                    {
                        string sql2 = "delete  from Audit  where pid = '" + pid + "'";
                        if (db.cmd_Execute(sql2) == 1)
                        {
                            sql = "update  prescription  set curstate = '未通过复核'  where id = '" + pid + "'";
                            // db.cmd_Execute(sql3);
                        }
                    }

                }
                else
                {

                    sql = "insert into AgainPrescriptionCheckState(prescriptionId,checkStatus,refusalreason) values('" + pid + "','" + check_result + "','" + refusalreason + "')";
                }
                /*  Boolean isCheck = false;
                  while (srd.Read())
                  {
                      isCheck = true;
                  }*/

                //  if (!isCheck)
                //  {


            }


            if (sql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(sql);
            }
            return end; ;

            //  return 0;


        }






        #endregion

        #region 订单列表信息

        public DataTable OrderListInfo(string per, string STime, string ETime, int hospitalId)
        {
            string sql = "select distinct p.ID,p.Pspnum,p.Hospitalid,p.name,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
               + "p.dotime,p.dose,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,(select printstatus from PrescriptionCheckState as x where x.prescriptionId =p.id) as printstatus,"
               + "(select State from Reconciliation as r where r.pid = p.id) as st from prescription as p  where p.id not in (select pid from InvalidPrescription)";
            if (per != "")
            {

                sql += " and p.id in ( select pid from Reconciliation where ReconciliaPer='" + per + "')";

            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                sql += "and p.id in ( select pid from Reconciliation where ReconciliaT >='" + strS + "')";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += "and p.id in ( select pid from Reconciliation where ReconciliaT <='" + strE + "')";

            }
            if (hospitalId != 0)
            {
                sql += "  and p.Hospitalid='" + hospitalId + "'";
            }




            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region 对账列表信息

        public DataTable CheckListInfo(string Clearing, string ClearingS, string STime, string ETime)
        {

            string sql = "SELECT   CheckNum, MAX(id) AS ID, MAX(now) AS now, SUM(CAST(drugMonSum AS float(2))) AS money,   (SELECT   CAST(GenDecoct AS float(2)) AS Expr1"
                    + " FROM      Clearingparty  WHERE   (ClearPName = MAX(Reconciliation.Clearing))) * SUM(CAST(shengRe AS int)) AS GeneraDecoc, "
               + " MAX(ReconciliaPer) AS ReconciliaPer, MAX(ReconciliaT) AS ReconciliaT, MAX(Clearing) AS Clearing, MAX(State) AS State, "
               + " MAX(Remarks) AS Remarks FROM     Reconciliation WHERE   (1 = 1)  ";
            if (Clearing != "" && Clearing != "0")
            {

                sql += " and Clearing='" + Clearing + "'";

            }
            if (ClearingS != "")
            {
                sql += " and State='" + ClearingS + "'";


            }

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                sql += "and now >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += "and now <='" + strE + "'";

            }

            sql += " Group By CheckNum  order by id desc ";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion

        //重审处方

        public int AgainCheckPres(int pid)
        {

            String sql = "";
            int end = 0;
            string stateSql = "select * from bubble where pid=" + pid;
            SqlDataReader srd = db.get_Reader(stateSql);
            if (srd.Read())
            {
                sql = "";
            }
            else
            {
                sql = "update adjust  set status =0  where prescriptionId = " + pid + "";
                if (db.cmd_Execute(sql) == 1)
                {

                    sql = "delete   from Audit  where pid = '" + pid + "'";

                    if (db.cmd_Execute(sql) == 1)
                    {
                        sql = "update prescription  set curstate = '开始调剂'  where id = '" + pid + "'";

                    }
                }
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

            //  return 0;


        }




        #region 查询所有处方名称
        ///// <summary>
        ///// 查询所有处方名称
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <returns>SqlDataReader对象</returns>
        public SqlDataReader findRecipeByHospitalId(int hospitalId)
        {
            string sql = "select ID,Pspnum from prescription where Hospitalid=" + hospitalId;

            return db.get_Reader(sql);
        }
        #endregion

        #region 查询所有处方信息总个数
        ///// <summary>
        ///// 查询所有处方信息总个数
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
        ///// <param name="checkstatus)">审核状态,0未审核,1通过,2不通过</param>
        public int findRecipeTotal(int hospitalId, string Pspnum, string patient, int checkstatus)
        {
            /*   string sql = "select count(*) from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId where pcs.checkStatus = " + checkstatus;

               if (Pspnum != null && Pspnum.Length != 0)
               {
                   sql += " and Pspnum='" + Pspnum + "'";
               }
               if (patient != null && patient.Length != 0)
               {
                   sql += " and name like '%" + patient + "%'";
               }
               return Convert.ToInt32(db.cmd_ExecuteScalar(sql));*/
            return 10;
        }



        ///// <summary>
        ///// 查询处方信息总条数
        ///// </summary>
        ///// <returns>int对象</returns>
        public int findAllRecipeTotal()
        {
            string sql = "select count(*) from prescription";
            return Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            ;
        }
        ///// <summary>
        ///// 查询未匹配处方信息总条数
        ///// </summary>
        ///// <returns>int对象</returns>
        ///// <param name="workContent">工作内容,0全部,1匹配人员,2打印人员</param>
        ///// <param name="date)">日期</param>
        ///// <param name="barCode)">员工条码</param>
        public int findNotMatchRecipeTotal(string workContent, string date, string barCode)
        {
            string sql = "select count(*) from prescription";
            return Convert.ToInt32(db.cmd_ExecuteScalar(sql));
        }
        ///// <summary>
        ///// 查询匹配处方信息总条数
        ///// </summary>
        ///// <returns>int对象</returns>
        ///// <param name="workContent">工作内容,0全部,1匹配人员,2打印人员</param>
        ///// <param name="date)">日期</param>
        ///// <param name="barCode)">员工条码</param>
        public int findMatchRecipeTotal(string workContent, string date, string barCode)
        {
            string sql = "select count(*) from prescription";
            return Convert.ToInt32(db.cmd_ExecuteScalar(sql));
        }
        #endregion

        #region 查询处方信息
        ///// <summary>
        ///// 查询匹配未审核处方信息
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
        ///// <param name="checkstatus)">审核状态,0未审核,1通过,2不通过</param>
        public DataTable findMatchAndNotCheckRecipeInfo(int hospitalId, string Pspnum, string patient)
        {
            /* string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
              + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
              + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId inner join drug d on d.pid = p.id inner join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and p.id not in (select pid from InvalidPrescription)";
             */
            string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
            + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
            + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid = p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where   p.curstate ='未审核' ";
            //pcs.prescriptionId IS NULL"; //and p.id not in (select pid from InvalidPrescription)";
            if (hospitalId != 0)
            {
                sql += " and p.Hospitalid='" + hospitalId + "'";
            }

            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }
            if (patient != null && patient.Length != 0)
            {
                sql += " and p.name like '%" + patient + "%'";
            }

            DataTable dt = db.get_DataTable(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    bool boo = checkPrescriptionIsMath(Convert.ToInt32(dt.Rows[i]["id"].ToString()));
                    if (!boo)
                    {
                        dt.Rows.RemoveAt(i);
                    }


                }

            }
            return dt;

        }

        public DataTable findMatchAndNotCheckRecipeInfo_1(int hospitalId, string Pspnum, string patient)
        {
            /* string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
              + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
              + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId inner join drug d on d.pid = p.id inner join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and p.id not in (select pid from InvalidPrescription)";
             */
            string sql = " select distinct   p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
            + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
            + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid = p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where   p.curstate ='未审核' ";
            //pcs.prescriptionId IS NULL"; //and p.id not in (select pid from InvalidPrescription)";
            if (hospitalId != 0)
            {
                sql += " and p.Hospitalid='" + hospitalId + "'";
            }

            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }
            if (patient != null && patient.Length != 0)
            {
                sql += " and p.name like '%" + patient + "%'";
            }
            sql += "  order by p.id desc";
            DataTable dt = db.get_DataTable(sql);
            /*   if (dt.Rows.Count > 0)
               {
                   for (int i = dt.Rows.Count - 1; i >= 0; i--)
                   {
                       bool boo = checkPrescriptionIsMath(Convert.ToInt32(dt.Rows[i]["id"].ToString()));
                       if (!boo)
                       {
                           dt.Rows.RemoveAt(i);
                       }


                   }

               }*/
            return dt;

        }

        public  int  findMatchAndNotCheckRecipeInfots_1(int hospitalId, string Pspnum, string patient)
        {
            string sql = "select  count(distinct p.id) from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid = p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where   p.curstate ='未审核' ";
          
            if (hospitalId != 0)
            {
                sql += " and p.Hospitalid='" + hospitalId + "'";
            }

            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }
            if (patient != null && patient.Length != 0)
            {
                sql += " and p.name like '%" + patient + "%'";
            }
            int  count  = Convert.ToInt32(db.cmd_ExecuteScalar(sql));

            return count;
        }

        public DataTable findMatchAndNotCheckRecipeInfofy_1(int hospitalId, string Pspnum, string patient,int ts ,int page )
        {

            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select distinct p.ID, p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
            + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
            + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid = p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where   p.curstate ='未审核' ";

            if (hospitalId != 0)
            {
                sql += " and p.Hospitalid='" + hospitalId + "'";
            }

            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }
            if (patient != null && patient.Length != 0)
            {
                sql += " and p.name like '%" + patient + "%'";
            }
            sql += " ) t  ) p where p.rownumber>" + (page - 1) * ts + " order by p.id desc";
            DataTable dt = db.get_DataTable(sql);
          
            return dt;

        }
        ///// <summary>
        ///// 查询匹配,审核处方信息
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
        ///// <param name="checkstatus)">审核状态,0未审核,1通过,2不通过</param>
        public DataTable findMatchAndCheckRecipeInfo()
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
             + " from prescription as p right join DrugMatching dm on p.id=dm.pspId right join PrescriptionCheckState pcs on p.id=pcs.prescriptionId where 1=1 ";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }

        ///// <summary>
        ///// 查询调剂,处方信息
        ///// </summary>
        ///// <param name="status">状态 0未完成,1完成</param>
        ///// <param name="date">日期</param>
        ///// <param name="eName)">员工姓名</param>
        ///// <returns>DataTable对象</returns>



        #endregion

        public DataTable findAdjustRecipeInfo(int status, string date, string eName)
        {
            string sql = "select  ID,Pspnum,delnum,(select top 1 SwapPer from adjust as a where  a.prescriptionId =p.id) as SwapPer ,(select top 1 status from adjust as a where a.prescriptionId =p.id )as status,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime, doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,  RemarksA,RemarksB"
             + ",( select top 1 wordDate from adjust as a where  a.prescriptionId =p.id) as wordDate,(select top 1 endDate from adjust as a where  a.prescriptionId =p.id) as endDate  from prescription as p where   p.id not in (select pid from InvalidPrescription) and  p.id in (select prescriptionId from  adjust as a where 1=1   ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (date != null && date.Length > 0)
            {
                sql += " and  Convert(varchar,a.wordDate ,120)   like '" + date + "%'";


            }

            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }

            sql += ")";
            sql += " order by ID desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public DataTable findAdjustRecipeInfo_c(int status, string date, string eName)
        {
            string sql = "select  ID,Pspnum,delnum,(select top 1 SwapPer from adjust as a where  a.prescriptionId =p.id) as SwapPer ,(select top 1 status from adjust as a where a.prescriptionId =p.id )as status,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime, doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,  RemarksA,RemarksB"
             + ",(select top 1 wordDate from adjust as a where  a.prescriptionId =p.id) as wordDate,(select top 1 endDate from adjust as a where  a.prescriptionId =p.id) as endDate  from prescription as p where    p.id in (select prescriptionId from  adjust as a where 1=1   ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (date != null && date.Length > 0)
            {
                sql += " and  Convert(varchar,a.wordDate ,120)   like '" + date + "%'";


            }

            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }

            sql += ")";
            sql += " order by ID desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }

        public int findAdjustRecipeInfo_cts(int status, string date, string eName)
        {
            string sql = "select count(distinct p.id) from prescription as p  inner join adjust a on a.prescriptionId =p.id  where    p.id in (select prescriptionId from  adjust as a where 1=1   ";

            if (status != 2)
            {
                sql += " and a.status=" + status;
            }
            if (date != null && date.Length > 0)
            {
                sql += " and  Convert(varchar,a.wordDate ,120)   like '" + date + "%'";
            }

            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }

            sql += ")";
           int count = Convert.ToInt32(db.cmd_ExecuteScalar (sql));

           return count;

        }

        public DataTable findAdjustRecipeInfo_cfy(int status, string date, string eName, int ts, int page)
        {
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from (   select  ID,Pspnum,delnum,(select top 1 SwapPer from adjust as a where  a.prescriptionId =p.id) as SwapPer ,(select top 1 status from adjust as a where a.prescriptionId =p.id )as status,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime, doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,  RemarksA,RemarksB"
             + ",(select top 1 wordDate from adjust as a where  a.prescriptionId =p.id) as wordDate,(select top 1 endDate from adjust as a where  a.prescriptionId =p.id) as endDate  from prescription as p where    p.id in (select prescriptionId from  adjust as a where 1=1   ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (date != null && date.Length > 0)
            {
                sql += " and  Convert(varchar,a.wordDate ,120)   like '" + date + "%'";


            }

            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }

            sql += ")";
            sql += ") t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }

        public DataTable findAdjustRecipeInfoDao(int status, string date, string eName)
        {
            string sql = "select  ID,Pspnum,delnum,( top 1 select SwapPer from adjust as a where  a.prescriptionId =p.id) as SwapPer ,(select top 1  case convert(nvarchar(50), status ) when '1' then '调剂完成' when '0' then '开始调剂' else convert(nvarchar(50), status ) end from adjust as a where a.prescriptionId =p.id )as status,p.Hospitalid,p.name,case convert(nvarchar(50), p.sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,case decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end,oncetime,twicetime,packagenum,dotime, doperson,dtbcompany,dtbaddress,dtbphone,case dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' else dtbtype end,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,  RemarksA,RemarksB"
             + ",(select top 1 wordDate from adjust as a where  a.prescriptionId =p.id) as wordDate,(select top 1  endDate from adjust as a where  a.prescriptionId =p.id) as endDate  from prescription as p where   p.id not in (select pid from InvalidPrescription) and  p.id in (select prescriptionId from  adjust as a where 1=1   ";

            if (status != 2)
            {

                sql += " and a.status=" + status;

            }
            if (date != null && date.Length > 0)
            {
                sql += " and  Convert(varchar,a.wordDate ,120)   like '" + date + "%'";


            }

            if (eName != null && eName.Length > 0)
            {
                sql += " and a.SwapPer='" + eName + "'";

            }

            sql += ")";
            sql += " order by ID desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #region 物流信息

        public DataTable LogisticsInfor(string Pspnum, string dtbtype, string hospitalname, string patient, string phone, string curstate, string time, string ftime, string wl)
        {
            string sql = "select p.ID,p.Pspnum,p.name,p.phone,"
             + "p.dtbtype ,d.SendTime as SendTime,p.dtbaddress, p.curstate,"
             + "  p.dtbcompany as a2, p.dtbphone as a3, d.logisticsnum as a4,p.qname as qname,p.qtime as qtime, p.logisticsstate as e from prescription as p ,Delivery as d  where p.curstate='已发货' and d.DecoctingNum = p.id ";

            if (Pspnum != "")
            {

                sql += " and p.Pspnum='" + Pspnum + "'";

            }
            if (patient != "")
            {

                sql += " and p.name='" + patient + "'";

            }
            if (phone != "")
            {

                sql += " and p.phone='" + phone + "'";

            }
            if (dtbtype != "0" && dtbtype != "")
            {
                sql += " and  p.dtbtype  ='" + dtbtype + "'";


            }

            if (hospitalname != "" && hospitalname != "0")
            {
                sql += " and p.hospitalid='" + hospitalname + "'";

            }
            if (curstate != "" && curstate != "0")
            {
                sql += " and p.logisticsstate='" + curstate + "'";

            }
            if (time != "0")
            {
                sql += " and Convert(varchar,getdrugtime  ,120)   like '" + time + "%'";

            }
            if (ftime != "0")
            {
                sql += " and Convert(varchar, d.SendTime  ,120)   like '" + ftime + "%'";


            }

            if (wl != "")
            {

                sql += " and d.logisticsnum='" + wl + "'";

            }
            sql += " order by SendTime desc ";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region 物流信息 2016-03-19 lbf update

        public DataTable LogisticsInfor_dao(string Pspnum, string dtbtype, string hospitalname, string patient, string phone, string curstate, string time, string ftime, string wl)
        {
            string sql = "select p.ID,p.Pspnum,p.name,p.phone,"
             + "p.dtbtype ,d.SendTime as SendTime,p.dtbaddress, p.curstate,"
             + "  p.dtbcompany as a2, p.dtbphone as a3, d.logisticsnum as a4,p.qname as qname,p.qtime as qtime, p.logisticsstate as e from prescription as p ,Delivery as d  where p.curstate='已发货' and d.DecoctingNum = p.id ";

            if (Pspnum != "")
            {

                sql += " and p.Pspnum='" + Pspnum + "'";

            }
            if (patient != "")
            {

                sql += " and p.name='" + patient + "'";

            }
            if (phone != "")
            {

                sql += " and p.phone='" + phone + "'";

            }
            if (dtbtype != "0" && dtbtype != "")
            {
                sql += " and  p.dtbtype  ='" + dtbtype + "'";


            }

            if (hospitalname != "" && hospitalname != "0")
            {
                sql += " and p.hospitalid='" + hospitalname + "'";

            }
            if (curstate != "" && curstate != "0")
            {
                sql += " and p.logisticsstate='" + curstate + "'";

            }
            if (time != "0")
            {
                sql += " and Convert(varchar,getdrugtime  ,120)   like '" + time + "%'";

            }
            if (ftime != "0")
            {
                sql += " and Convert(varchar, d.SendTime  ,120)   like '" + ftime + "%'";


            }

            if (wl != "")
            {

                sql += " and d.logisticsnum='" + wl + "'";

            }
            sql += " order by SendTime desc ";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region

        public DataTable AccountStatementInfo(string Pspnum, string STime, string ETime, int hospitalId)
        {
            string sql = "select p.ID,p.Pspnum,p.dotime,p.decscheme,p.name, (select count(pid)  from drug as s where  s.pid =p.id ) as DrugAcount,takenum,packagenum,"
             + "(select hname from hospital as h where h.id = p.hospitalid) as hname,dose "
             + " from prescription as p where p.id not in (select pid from InvalidPrescription)and p.id not in (select pid from Reconciliation) and p.id in (select PrescriptionId from PrescriptionCheckState) ";

            if (Pspnum != "")
            {

                sql += " and p.Pspnum='" + Pspnum + "'";

            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                sql += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                sql += "and p.dotime <='" + strE + "'";

            }
            if (hospitalId != 0)
            {
                sql += "  and p.Hospitalid='" + hospitalId + "'";
            }


            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        public DataTable findAdjustRecipeInfo(int userid, string date)
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,(select count(*) from drug as d where d.pid = p.id) as drugnum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + ",a.id as aid,a.status astatus,a.wordDate awordDate,a.endDate aendDate  from prescription as p inner join adjust a on p.id=a.prescriptionId left join Employee e on a.employeeId=e.id where a.employeeId=" + userid + " and CONVERT(varchar, a.wordDate, 120) like '%" + date + "%' order by p.ID desc";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public DataTable findAdjustRecipeInfo_tj(int userid, string date, string hospital_name)
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
                sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1  hname from hospital as h where h.id = p.hospitalid) as hname,"
                + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,(select count(*) from drug as d where d.pid = p.id) as drugnum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
                + ",a.id as aid,a.status astatus,a.wordDate awordDate,a.endDate aendDate  from prescription as p inner join adjust a on p.id=a.prescriptionId left join Employee e on a.employeeId=e.id left join hospital hs on hs.id = p.hospitalid  where a.employeeId=" + userid + " and a.worddate>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00'" + 
                " and hs.id in("+sql_1  +")"+ " order by a.wordDate desc";
            }
            else
            {
                sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                  + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,(select count(*) from drug as d where d.pid = p.id) as drugnum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
                  + ",a.id as aid,a.status astatus,a.wordDate awordDate,a.endDate aendDate  from prescription as p inner join adjust a on p.id=a.prescriptionId left join Employee e on a.employeeId=e.id  left join hospital hs on hs.id = p.hospitalid where  a.worddate>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00'".TrimEnd() +
                 " and hs.id in(" + sql_1 + ")" + " order by a.wordDate desc";
            }
            //+ " order by p.id desc";
            db.write_log_txt("调剂环节:" + sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #region 匹配查询
        public DataTable searchInfo(string Employeenum, string STime, string Ksbq, string RecipeStatus, string Psnum, string HostaileName)
        {
            string strSQL = "select distinct p.ID,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {
                strSQL += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                strSQL += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                strSQL += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            strSQL += "order by p.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public int searchInfots_1(string Employeenum, string STime, string Ksbq, string RecipeStatus, string Psnum, string HostaileName, string takemethod)
        {
            string strSQL = "select count(distinct p.ID)  from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {
                strSQL += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                strSQL += " and hospitalId ='" + HostaileName + "'";
            }
            if (takemethod != null && takemethod.Length > 0)
            {

                strSQL += "and p.takemethod='" + takemethod + "'";
            }
            if (Ksbq != "")
            {
                strSQL += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));

            return count;
        }

        public int searchInfots(string Employeenum, string STime, string Ksbq, string RecipeStatus, string Psnum, string HostaileName)
        {
            string strSQL = "select count(distinct p.ID)  from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {
                strSQL += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                strSQL += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                strSQL += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));

            return count;
        }

        public DataTable searchInfofy_1(string Employeenum, string STime, string Ksbq, string RecipeStatus, string Psnum, string HostaileName, int ts, int page, string takemethod)
        {
            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select distinct p.ID,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {
                strSQL += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                strSQL += " and hospitalId ='" + HostaileName + "'";
            }
            if (takemethod != null && takemethod.Length > 0)
            {

                strSQL += "and p.takemethod='" + takemethod + "'";
            }
            if (Ksbq != "")
            {
                strSQL += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            strSQL += " ) t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public DataTable searchInfofy(string Employeenum, string STime, string Ksbq, string RecipeStatus, string Psnum, string HostaileName, int ts, int page)
        {
            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select distinct p.ID,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += " and p.dotime >='" + strS + "'  and p.dotime <='" + strE + "'";
            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {
                strSQL += "and p.Pspnum='" + Psnum + "'";
            }
            if (HostaileName != "" && HostaileName != "0")
            {
                strSQL += " and hospitalId ='" + HostaileName + "'";
            }
            if (Ksbq != "")
            {
                strSQL += " and (p.department like '%" + Ksbq + "%' or p.inpatientarea like '%" + Ksbq + "%')";
            }
            strSQL += " ) t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        public DataTable searchInfoDao(string Employeenum, string STime, string RecipeStatus, string Psnum)
        {
            string strSQL = "select distinct p.ID,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,case convert(nvarchar(50), p.sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), p.sex) end,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,case decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,case dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' else dtbtype end,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and p.doperson='" + Employeenum + "'";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "' and p.dotime  <='" + d.ToString("yyyy/MM/dd 23:59:59") + "'";

            }


            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (Psnum != null && Psnum.Length > 0)
            {

                strSQL += "and p.Pspnum='" + Psnum + "'";
            }


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region //导出数据药品查询（匹配）
        public DataTable findDrugInfobyCondition1(string Employeenum, string STime, string RecipeStatus, string Psnum)
        {
            string strSQL = "select  ROW_NUMBER() OVER(ORDER BY id desc) as ID,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice"
                + " from drug as d where 1=1";
            if (Employeenum != "" && Employeenum.Length > 0)
                strSQL += "and Pspnum in (select Pspnum from prescription where  doperson='" + Employeenum + "')";

            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and  Pspnum in (select Pspnum from prescription where  dotime >='" + strS + "' and dotime  <='" + strE + "' )";

            }

            if (RecipeStatus != null && RecipeStatus.Length > 0)
            {

                strSQL += " and Pspnum in (select Pspnum from prescription where curstate='" + RecipeStatus + "')";
            }
            if (Psnum != null && Psnum.Length > 0)
            {

                strSQL += "and Pspnum='" + Psnum + "'";
            }


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 配送统计信息
        public DataTable finDistributInfo(string hospitalID, string STime, string ETime, string dtbcompany)
        {
            string strSQL = "select distinct p.ID,p.Pspnum,p.name,p.phone,(select Sendpersonnel from Delivery as d where d.DecoctingNum = p.ID and Sendstate ='1')as Sendpersonnel,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.dtbcompany,p.dtbaddress ,Delivery.SendTime from prescription as p  join Delivery on p.ID = Delivery.DecoctingNum where  p.ID in ( select DecoctingNum from Delivery as d where d.DecoctingNum = p.ID and Sendstate ='1')";

            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and Delivery.SendTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and Delivery.SendTime <='" + strE + "'";

            }

            if (dtbcompany != "0")
            {

                strSQL += "and p.dtbcompany ='" + dtbcompany + "'";
            }




            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 配送统计信息数量
        public DataTable finDistributInfoCount(string hospitalID, string STime, string ETime, string dtbcompany)
        {
            if ("无".Equals(dtbcompany))
            {
                dtbcompany = "";
            }
            string strSQL = "";
            if ("0".Equals(dtbcompany))
            {
                strSQL = "select distinct p.dtbcompany as data, COUNT(*) AS count from prescription as p  join Delivery on p.ID = Delivery.DecoctingNum where  p.ID in ( select DecoctingNum from Delivery as d where d.DecoctingNum = p.ID and Sendstate ='1')";

            }
            else
            {
                strSQL = "select distinct CONVERT(varchar, Delivery.SendTime, 111) AS data, COUNT(*) AS count from prescription as p  join Delivery on p.ID = Delivery.DecoctingNum where  p.ID in ( select DecoctingNum from Delivery as d where d.DecoctingNum = p.ID and Sendstate ='1')";
            }


            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and Delivery.SendTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and Delivery.SendTime <='" + strE + "'";

            }

            if (dtbcompany != "0")
            {

                strSQL += "and p.dtbcompany ='" + dtbcompany + "'";
            }
            if ("0".Equals(dtbcompany))
            {
                strSQL += " GROUP BY p.dtbcompany";
            }
            else
            {
                strSQL += " GROUP BY CONVERT(varchar, Delivery.SendTime, 111)";
            }




            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 业务员业绩统计信息
        public DataTable finBusiPerinfo(string Salesman, string STime, string ETime, string StaffId)
        {
            // string strSQL = "Select ID,Date,WorkContent,Sales,Workload from Busperforstatistics where 1=1";
            string strSQL = "select ID,'待定'as Date,'待定'as WorkContent,'待定'as Sales,'待定'as Workload from prescription where 1=1";
            /* if (Salesman != null && Salesman.Length > 0)
             {
                 strSQL += "and Salesman='" + Salesman + "'";
             }
             if (STime != null && STime.Length > 0)
             {
                 DateTime d = Convert.ToDateTime(STime);
                 string strS = d.ToString("yyyy/MM/dd 00:00:00");
                 strSQL += "and p.dotime >='" + strS + "'";

             }


             if (ETime != null && ETime.Length > 0)
             {
                 DateTime d4 = Convert.ToDateTime(ETime);
                 string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                 strSQL += "and p.dotime  <='" + strE + "'";

             }

             if (StaffId != null && StaffId.Length > 0)
             {

                 strSQL += "and p.dtbcompany ='" + StaffId + "'";
             }

             */


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 综合查询信息
        public DataTable ComprehensiveInquiryInfo(string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime)
        {
            string strSQL = "select p.ID,p.Pspnum,p.customid,p.delnum,(select top 1 ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select  top 1  AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select  top 1  bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select  top 1  endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select  top 1 starttime from  bubble as b where  b.pid=p.ID ) as bstarttime,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select  top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1  hname from hospital as h where h.id = p.hospitalid) as hname,"
            + "p.diagresult,(  select  top 1 tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,( select top 1 machinename from machine where id = (select  top 1  machineid from tisaneunit where pid = p.id )) as machineid,(select top 1  starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select top 1  endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,(select top 1  jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB,"
            + "  ( select  top 1 Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel ,( select  top 1 SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime ,(select top 1  Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,(select top 1  PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime ,(select top 1  Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,( select top 1  machinename  from machine   where  mark =1 and unitnum in (select top 1  unitnum from machine where id in (select top 1  machineid from tisaneunit as t where t.pid =p.id))) as packmachine,(select top 1  SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select top 1  endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select top 1  wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate ,(select top 1  PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer ,(select top 1  refusalreason  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as refusalreason,(select top 1  checkStatus  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as checkStatus,(select top 1  PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,'待定' as SignPer,'待定' as SignTime from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += " order by id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public int ComprehensiveInquiryInfots(string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime)
        {
            string strSQL = "select count(*) from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));
            return count;
        }


        public DataTable ComprehensiveInquiryInfofy(string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime, int ts, int page)
        {
            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by q.id desc ) as rownumber  from ( select p.ID,p.Pspnum,p.customid,p.delnum,(select top 1 ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select  top 1  AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select  top 1  bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select  top 1  endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select  top 1 starttime from  bubble as b where  b.pid=p.ID ) as bstarttime,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select  top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1  hname from hospital as h where h.id = p.hospitalid) as hname,"
            + "p.diagresult,(  select  top 1 tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,( select top 1 machinename from machine where id = (select  top 1  machineid from tisaneunit where pid = p.id )) as machineid,(select top 1  starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select top 1  endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,(select top 1  jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB,"
            + "  ( select  top 1 Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel ,( select  top 1 SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime ,(select top 1  Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,(select top 1  PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime ,(select top 1  Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,( select top 1  machinename  from machine   where  mark =1 and unitnum in (select top 1  unitnum from machine where id in (select top 1  machineid from tisaneunit as t where t.pid =p.id))) as packmachine,(select top 1  SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select top 1  endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select top 1  wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate ,(select top 1  PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer ,(select top 1  refusalreason  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as refusalreason,(select top 1  checkStatus  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as checkStatus,(select top 1  PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,'待定' as SignPer,'待定' as SignTime from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != null && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        public DataTable ComprehensiveInquiryInfoDao(string Pspnum, string STime, string ETime, string RecipeStatus, string hospitalID, string tisaneid, string patient, string jftime1)
        {
            /*
              string strSQL = "select p.ID,p.delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
            + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,(select jiefangman from jfInfo as ll where ll.pid = p.id ) as doperson1,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB,"
            + " (select ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select AuditTime from Audit as a where a.pid = p.ID ) as AuditTime,(select bubbleperson from  bubble as b where  b.pid=p.ID ) as bubbleperson,(select endDate from  bubble as b where  b.pid=p.ID ) as bendDate,(select starttime from  bubble as b where  b.pid=p.ID ) as bstarttime,(select tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,(select starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate, ( select Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID ) as Sendpersonnel ,( select SendTime from Delivery as d  where d.DecoctingNum  =p.ID ) as SendTime ,(select Starttime  from Packing as m where m.DecoctingNum=p.ID ) as PacEndTime,(select PacTime  from Packing as m where m.DecoctingNum=p.ID ) as PacTime ,(select Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID ) as Pacpersonnel,(select SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select endDate  from adjust as a where a.prescriptionId=p.ID ) as endDate,(select wordDate  from adjust as a where a.prescriptionId=p.ID ) as wordDate ,(select PartyPer  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyPer ,(select refusalreason  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as refusalreason,(select checkStatus  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as checkStatus,(select PartyTime  from PrescriptionCheckState as x where x.prescriptionId=p.ID ) as PartyTime,'待定' as SignPer,'待定' as SignTime from prescription as p where 1=1";
            */

            string strSQL = "select p.ID,p.delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,p.Pspnum,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,"
            + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,case p.decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else p.decscheme end,p.oncetime,p.twicetime,p.packagenum,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.RemarksA,p.RemarksB,(select top 1 partyper from prescriptioncheckstate where prescriptionid = p.id) as chackman,(select top 1 partytime from prescriptioncheckstate where prescriptionid = p.id) as chacktime,(select checkstatus from prescriptioncheckstate where prescriptionid = p.id) as checkstatus,(select top 1 refusalreason from prescriptioncheckstate where prescriptionid = p.id) as checkreson,(select top 1 SwapPer  from adjust as a where a.prescriptionId=p.ID ) as SwapPer,(select top 1  wordDate  from adjust as a where a.prescriptionId=p.ID) as endDate,(select  top 1 endDate  from adjust as a where a.prescriptionId=p.ID) as wordDate, "
            + "(select top 1  ReviewPer from Audit as a where a.pid = p.ID ) as ReviewPer,(select top 1  AuditTime from Audit as a where a.pid = p.ID) as AuditTime,(select top 1  bubbleperson from  bubble as b where  b.pid=p.ID) as bubbleperson,(select  top 1 starttime from  bubble as b where  b.pid=p.ID) as bendDate,(select top 1 endDate from  bubble as b where  b.pid=p.ID) as bstarttime,(select top 1 tisaneman from tisaneinfo as t  where  t.pid=p.ID) as tisaneman,(select top 1  starttime from tisaneinfo as t  where  t.pid=p.ID) as tstarttime,(select top 1 endDate from tisaneinfo as t  where  t.pid=p.ID) as tendDate,(select top 1  Pacpersonnel  from Packing as m where m.DecoctingNum=p.ID) as Pacpersonnel,(select top 1 PacTime  from Packing as m where m.DecoctingNum=p.ID) as PacTime,(select top 1 Starttime  from Packing as m where m.DecoctingNum=p.ID) as PacEndTime, ( select top 1 Sendpersonnel from Delivery as d  where d.DecoctingNum  =p.ID) as Sendpersonnel ,(select top 1 SendTime from Delivery as d  where d.DecoctingNum  =p.ID) as SendTime ,'待定' as SignPer,'待定' as SignTime from prescription as p where 1=1";
            if (hospitalID != "0" && hospitalID != "")
            {
                strSQL += "and p.Hospitalid='" + hospitalID + "'";
            }
            if (STime != "0" && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != "0" && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }

            if (Pspnum != "0")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "0" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            if (patient != "0" && patient.Length > 0)
            {

                strSQL += "and p.name='" + patient + "'";
            }
            if (jftime1 != "")
            {

                DateTime d = Convert.ToDateTime(jftime1);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }

            if (tisaneid != "0" && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "order by id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        
        public DataTable ComprehensiveInquiryInfo(string Pspnum, string STime, string ETime, string RecipeStatus, string tisaneid)
        {
            string strSQL = "select p.ID,p.Pspnum,p.curstate,p.dotime from prescription as p where 1=1";
            /*  if (hospitalID != "0" && hospitalID != "")
              {
                  strSQL += "and p.Hospitalid='" + hospitalID + "'";
              }*/
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";
            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            /*  if (jftime != "")
              {

                  DateTime d = Convert.ToDateTime(jftime);
                  string strS = d.ToString("yyyy/MM/dd 00:00:00");
                  string strE = d.ToString("yyyy/MM/dd 23:59:59");

                  strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
              }
              */
            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            /*  if (patient != null && patient.Length > 0)
              {

                  strSQL += "and p.name='" + patient + "'";
              }*/
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += " order by p.ID desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public int ComprehensiveInquiryInfots(string Pspnum, string STime, string ETime, string RecipeStatus, string tisaneid)
        {
            string strSQL = "select p.ID,p.Pspnum,p.curstate,p.dotime from prescription as p where 1=1";
            /*  if (hospitalID != "0" && hospitalID != "")
              {
                  strSQL += "and p.Hospitalid='" + hospitalID + "'";
              }*/
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";
            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            /*  if (jftime != "")
              {

                  DateTime d = Convert.ToDateTime(jftime);
                  string strS = d.ToString("yyyy/MM/dd 00:00:00");
                  string strE = d.ToString("yyyy/MM/dd 23:59:59");

                  strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
              }
              */
            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            /*  if (patient != null && patient.Length > 0)
              {

                  strSQL += "and p.name='" + patient + "'";
              }*/
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));
            return count;
        }

        public DataTable ComprehensiveInquiryInfofy(string Pspnum, string STime, string ETime, string RecipeStatus, string tisaneid, int ts, int page)
        {
            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select p.ID,p.Pspnum,p.curstate,p.dotime from prescription as p where 1=1";
            /*  if (hospitalID != "0" && hospitalID != "")
              {
                  strSQL += "and p.Hospitalid='" + hospitalID + "'";
              }*/
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";
            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }
            /*  if (jftime != "")
              {

                  DateTime d = Convert.ToDateTime(jftime);
                  string strS = d.ToString("yyyy/MM/dd 00:00:00");
                  string strE = d.ToString("yyyy/MM/dd 23:59:59");

                  strSQL += "and p.dotime between '" + strS + "' and '" + strE + "'";
              }
              */
            if (Pspnum != "")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }
            /*  if (patient != null && patient.Length > 0)
              {

                  strSQL += "and p.name='" + patient + "'";
              }*/
            if (tisaneid != null && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "  ) t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        public DataTable ComprehensiveInquiryInfoDao_1(string Pspnum, string STime, string ETime, string RecipeStatus, string tisaneid)
        {
            string strSQL = "select p.ID,p.Pspnum,p.curstate,p.dotime from prescription as p where 1=1";

            if (STime != "0" && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != "0" && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }

            if (Pspnum != "0")
            {

                strSQL += "and p.Pspnum ='" + Pspnum + "'";
            }
            if (RecipeStatus != "0" && RecipeStatus != "0")
            {

                strSQL += "and p.curstate='" + RecipeStatus + "'";
            }


            if (tisaneid != "0" && tisaneid.Length > 0)
            {

                strSQL += "and p.ID='" + tisaneid + "'";
            }

            strSQL += "order by p.ID desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 综合预警信息
        public DataTable WarningInfo(int hospitalId, string Pspnum, string STime, string ETime, string HandleStatus, string EarlyWarning)
        {

            // string strSQL = "";
            /*string strSQL = " select ID,delnum,Hospitalid,Pspnum,(select PartyTime from PrescriptionCheckState as x where x.prescriptionId=p.id and checkStatus = 2 ) as AbnormalTM,(select PartyPer from PrescriptionCheckState as x where x.prescriptionId=p.id and checkStatus = 2) as AbnormalPS,(select checkStatus from PrescriptionCheckState as x where x.prescriptionId=p.id and checkStatus = 2) as AbnormalTP,(select refusalreason from PrescriptionCheckState as x where x.prescriptionId=p.id and checkStatus = 2) as AbnormalSt,getdrugtime,getdrugnum,ordertime,curstate as HandleRS,   dotime as HandleTM ,doperson as HandlePS from prescription as p where 1=1";
            if (hospitalId != 0 )
            {
                strSQL += " and p.Hospitalid='" + hospitalId + "'";
            }
            if (Pspnum != "" )
            {
                strSQL += "and p.Pspnum='" + Pspnum + "'";
            }
            if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and p.dotime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and p.dotime <='" + strE + "'";

            }

            if (HandleStatus != "0" && HandleStatus != "")
            {

                strSQL += "and HandleStatus ='" + HandleStatus + "'";
            }
            if (EarlyWarning != "0" && EarlyWarning != "")
            {

                strSQL += "and EarlyWarning='" + EarlyWarning + "'";
            }*/
            string str = "select pid as id, (select top 1 pspnum from prescription where (id =d.pid)) as pspnum ,(select top 1 hname from hospital as h where h.id in (select hospitalid from prescription as mm where mm.id = d.pid )) as hname,(select top 1 getdrugtime from prescription where (id =d.pid)) as getdrugtime,";
            str += "(select top 1 getdrugnum from prescription where (id = d.pid)) as getdrugnum,warningtime as errortime,'审核人员' as errorman,'审核警告' as errortype,'已过了医院设定的审核报警时间' as errordescription,";
            str += "(select top 1 partyper from prescriptioncheckstate where (prescriptionId = d.pid)) as doneperson,(select top 1  partytime from prescriptioncheckstate where (prescriptionId = d.pid)) as donetime,";
            str += "(select top 1  checkstatus from prescriptioncheckstate where (prescriptionId = d.pid)) as doneresult  FROM  jfInfo AS d WHERE warningstatus = 1";

            if (HandleStatus == "1")
            {
                str += "and pid not in (select prescriptionId from prescriptioncheckstate)";
            }
            if (HandleStatus == "2")
            {
                str += "and pid  in (select prescriptionId from prescriptioncheckstate)";
            }

            if (hospitalId != 0)
            {
                str += "and pid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and pid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }

            str += "union all select prescriptionId as id, (select top 1 pspnum from prescription where (id =d.prescriptionId)) as pspnum ,(select top 1  hname from hospital as h where h.id in (select top 1  hospitalid from prescription as mm where mm.id = d.prescriptionId )) as hname,(select top 1 getdrugtime from prescription where (id =d.prescriptionId)) as getdrugtime,";
            str += "(select  top 1 getdrugnum from prescription where (id = d.prescriptionId)) as getdrugnum,warningtime as errortime,'调剂人员' as errorman,'调剂警告' as errortype,'已过了医院设定的调剂报警时间' as errordescription,";
            str += "(select top 1 swapper from adjust where (prescriptionId = d.prescriptionId)) as doneperson,(select top 1 worddate from adjust where (prescriptionId = d.prescriptionId)) as donetime,";
            str += "(select top 1 status from adjust where (prescriptionId = d.prescriptionId)) as doneresult  FROM  prescriptioncheckstate AS d WHERE   checkstatus =1 and warningstatus =1";

            if (HandleStatus == "1")
            {
                str += "and prescriptionid not in (select prescriptionId from adjust)";
            }
            if (HandleStatus == "2")
            {
                str += "and prescriptionid  in (select prescriptionId from adjust)";
            }

            if (hospitalId != 0)
            {
                str += "and prescriptionid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and prescriptionid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }

            str += "union all select prescriptionId as id, (select top 1 pspnum from prescription where (id =d.prescriptionId)) as pspnum ,(select top 1 hname from hospital as h where h.id in (select top 1 hospitalid from prescription as mm where mm.id = d.prescriptionId )) as hname,(select top 1 getdrugtime from prescription where (id =d.prescriptionId)) as getdrugtime,";
            str += "(select top 1 getdrugnum from prescription where (id = d.prescriptionId)) as getdrugnum,warningtime as errortime,'复核人员' as errorman,'复核警告' as errortype,'已过了医院设定的复核报警时间' as errordescription,";
            str += "(select top 1 reviewper from audit where (pid = d.prescriptionId)) as doneperson,(select  top 1 AUDITTIME from audit where (pid = d.prescriptionId)) as donetime,";
            str += "(select top 1 auditstatus from audit where (pid = d.prescriptionId)) as doneresult  FROM  adjust AS d WHERE   status =1 and  warningstatus = 1";


            if (HandleStatus == "1")
            {
                str += "and prescriptionid not in (select pid from audit)";
            }
            if (HandleStatus == "2")
            {
                str += "and prescriptionid  in (select pid from audit)";
            }

            if (hospitalId != 0)
            {
                str += "and prescriptionid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and prescriptionid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }
            str += "union all select pid as id, (select top 1 pspnum from prescription where (id =d.pid)) as pspnum ,(select  top 1 hname from hospital as h where h.id in (select hospitalid from prescription as mm where mm.id = d.pid )) as hname,(select top 1 getdrugtime from prescription where (id =d.pid)) as getdrugtime,";
            str += "(select  top 1 getdrugnum from prescription where (id = d.pid)) as getdrugnum,warningtime as errortime,'泡药人员' as errorman,'泡药警告' as errortype,'已过了医院设定的泡药报警时间' as errordescription,";
            str += "(select top 1 bubbleperson from bubble where (pid = d.pid)) as doneperson,(select top 1 starttime from bubble where (pid = d.pid)) as donetime,";
            str += "(select top 1 bubblestatus from bubble where (pid = d.pid)) as doneresult  FROM  Audit AS d WHERE   (bubblewarningstatus = 1)";


            if (HandleStatus == "1")
            {
                str += "and pid not in (select pid from bubble)";
            }
            if (HandleStatus == "2")
            {
                str += "and pid  in (select pid from bubble)";
            }
            if (hospitalId != 0)
            {
                str += "and pid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and pid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }

            str += "union all select pid as id, (select top 1 pspnum from prescription where (id =d.pid)) as pspnum ,(select top 1 hname from hospital as h where h.id in (select hospitalid from prescription as mm where mm.id = d.pid )) as hname,(select top 1 getdrugtime from prescription where (id =d.pid)) as getdrugtime,";
            str += "(select top 1 getdrugnum from prescription where (id = d.pid)) as getdrugnum,warningtime as errortime,'煎药人员' as errorman,'煎药警告' as errortype,'已过了医院设定的煎药报警时间' as errordescription,";
            str += "(select  top 1 tisaneman from tisaneinfo where (pid = d.pid)) as doneperson,(select top 1 starttime from tisaneinfo where (pid = d.pid)) as donetime,";
            str += "(select top 1 tisanestatus from tisaneinfo where (pid = d.pid)) as doneresult  FROM  bubble AS d WHERE   ((bubblestatus =1) and (warningstatus =1))";


            if (HandleStatus == "1")
            {
                str += "and pid not in (select pid from tisaneinfo)";
            }
            if (HandleStatus == "2")
            {
                str += "and pid  in (select pid from tisaneinfo)";
            }


            if (hospitalId != 0)
            {
                str += "and pid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and pid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }
            str += "union all select pid as id, (select top 1 pspnum from prescription where (id =d.pid)) as pspnum ,(select top 1 hname from hospital as h where h.id in (select hospitalid from prescription as mm where mm.id = d.pid )) as hname,(select top 1 getdrugtime from prescription where (id =d.pid)) as getdrugtime,";
            str += "(select top 1 getdrugnum from prescription where (id = d.pid)) as getdrugnum,warningtime as errortime,'包装人员' as errorman,'包装警告' as errortype,'已过了医院设定的包装报警时间' as errordescription,";
            str += "(select top 1 pacpersonnel from packing where (decoctingnum = d.pid)) as doneperson,(select top 1 Starttime from packing where (decoctingnum = d.pid)) as donetime,";
            str += "(select top 1 fpactate from packing where (decoctingnum = d.pid)) as doneresult  FROM  tisaneinfo AS d WHERE   tisanestatus =1 and warningstatus = 1";


            if (HandleStatus == "1")
            {
                str += "and pid not in (select decoctingnum from packing)";
            }
            if (HandleStatus == "2")
            {
                str += "and pid  in (select decoctingnum from packing)";
            }

            if (hospitalId != 0)
            {
                str += "and pid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and pid in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }
            str += "union all select decoctingnum as id, (select top 1 pspnum from prescription where (id =d.decoctingnum)) as pspnum ,(select top 1 hname from hospital as h where h.id in (select hospitalid from prescription as mm where mm.id = d.decoctingnum )) as hname,(select top 1 getdrugtime from prescription where (id =d.decoctingnum)) as getdrugtime,";
            str += "(select top 1 getdrugnum from prescription where (id = d.decoctingnum)) as getdrugnum,warningtime as errortime,'发货人员' as errorman,'发货警告' as errortype,'已过了医院设定的发货报警时间' as errordescription,";
            str += "(select top 1 sendpersonnel from delivery where (decoctingnum = d.decoctingnum)) as doneperson,(select top 1 sendtime from delivery where (decoctingnum = d.decoctingnum)) as donetime,";
            str += "(select top 1 sendstate from delivery where (decoctingnum = d.decoctingnum)) as doneresult  FROM  packing AS d WHERE   fpactate =1 and warningstatus =1";


            if (HandleStatus == "1")
            {
                str += "and decoctingnum not in (select decoctingnum from delivery)";
            }
            if (HandleStatus == "2")
            {
                str += "and decoctingnum  in (select decoctingnum from delivery)";
            }

            if (hospitalId != 0)
            {
                str += "and decoctingnum in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
            }
            if (Pspnum != "0")
            {
                str += "and decoctingnum in (select id from prescription where pspnum ='" + Pspnum + "' )";
            }
            if (EarlyWarning != "0")
            {
                str += "and warningtype='" + EarlyWarning + "'";
            }
            if (STime != "0")
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and warningtime >='" + strS + "'";
            }
            if (ETime != "0")
            {
                DateTime d = Convert.ToDateTime(ETime);
                string strS = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and warningtime <='" + strS + "'";
            }

            str += " order by errortime desc";

            /*  str += "union all select (10000000*prescriptionid) as id, (select pspnum from prescription where (id =d.prescriptionid)) as pspnum ,(select getdrugtime from prescription where (id =d.prescriptionid)) as getdrugtime,";
               str += "(select getdrugnum from prescription where (id = d.prescriptionid)) as getdrugnum,partytime as errortime,partyper as errorman,'审核异常' as errortype,'审核没有通过的处方' as errordescription,";
               str += "'' as doneperson,'' as donetime,";
               str += "'' as doneresult  FROM  prescriptioncheckstate AS d WHERE   checkstatus =2";
             

             if (HandleStatus == "2")
               {
                   str += "and prescriptionid =-1";
               }

               if (hospitalId != 0)
               {
                   str += "and prescriptionid in (select id from prescription where hospitalid ='" + hospitalId + "' ) ";
               }
               if (Pspnum != "0")
               {
                   str += "and prescriptionid in (select id from prescription where pspnum ='" + Pspnum + "' )";
               }
              /* if (EarlyWarning != "0")
               {
                   str += "and warningtype='" + EarlyWarning + "'";
               }
               if (STime != "0")
               {
                   DateTime d = Convert.ToDateTime(STime);
                   string strS = d.ToString("yyyy/MM/dd  00:00:00");
                   str += "and warningtime >='" + strS + "'";
               }
               if (ETime != "0")
               {
                   DateTime d = Convert.ToDateTime(ETime);
                   string strS = d.ToString("yyyy/MM/dd  23:59:59");
                   str += "and warningtime <='" + strS + "'";
               }
               */








            // SqlDataReader sr = db.get_Reader(str);
            // strSQL = "";


            DataTable dt = db.get_DataTable(str);
            dt.Columns.Add("seq", typeof(int));
            dt.Columns["seq"].SetOrdinal(0);
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {


                j = j + 1;


                dt.Rows[i]["seq"] = j;

            }




            return dt;
        }
        #endregion
        #region
        /// <summary>
        /// 查询复核处方信息
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="AuditPer"></param>
        /// <returns></returns>
        public DataTable searchReviewInfo(string StartTime, string EndTime, string AuditPer, string num)
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间
            /* ArrayList list = new ArrayList();
             ArrayList list1 = new ArrayList();
             ArrayList list2 = new ArrayList();


         
             string sql3 = "";

             sql3 = "select  ID,delnum,(select recheckwarning from warning where hospitalid = p.Hospitalid) as recheckwarning,(select AuditTime  from Audit where pid = p.id ) as AuT,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             sql3 += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             sql3 += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where  p.id  in  (select  prescriptionId from adjust  where status = '2') and  p.id not in  (select  prescriptionId from AgainPrescriptionCheckState)  ";

             SqlDataReader sr3 = db.get_Reader(sql3);//找到调剂完成的但未重审复核的处方信息



             while (sr3.Read())
             {


                 string drugtime = sr3["getdrugtime"].ToString();
                 //  DateTime d2 = Convert.ToDateTime(sr3["getdrugtime"].ToString());
                 list2.Add(drugtime);//获取处方信息里的得到取药时间

                 string recheckwarning = sr3["recheckwarning"].ToString();
                 list1.Add(recheckwarning);//得到该处方号所对应的医院的报警时间

             }
             for (int i = 0; i < list2.Count; i++)
             {


                 string d1 = list1[i].ToString();//把报警时间转变为字符串格式


                 DateTime d2 = Convert.ToDateTime(list2[i].ToString());//获取取药时间


                 string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                 DateTime d3 = Convert.ToDateTime(strY);//把当前时间转变为datetime



                 TimeSpan d4 = d2.Subtract(d3);//取药时间-当前时间

                 int time = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());//差值转换为分钟数
                 int time2 = Convert.ToInt32(d1);//报警时间
                 if (time < time2)
                 {
                     string strsql1 = "update Audit set warningstatus = 1 ";//更新报警状态为1

                     db.cmd_Execute(strsql1);
                 }
                 else {

                     string sql4 = "update Audit set warningstatus = 0 ";

                     db.cmd_Execute(sql4);

                 }


             }*/




            string strSQL = "select  ID,delnum,(select ReviewPer from Audit where pid =p.id ) as ReviewPer,(select imgname  from Audit where pid = p.id ) as imgname,(select AuditStatus from Audit where pid =p.id ) as AuditStatus,(select warningstatus from Audit where pid = p.id) as warningstatus,(select AuditTime  from Audit where pid = p.id ) as AuT,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p  where  id in (select pid from Audit  ) and id not in  (select  prescriptionId from AgainPrescriptionCheckState) and id not in (select pid from InvalidPrescription)";
            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");

                strSQL += "and   p.dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");


                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and   id in ( select pid from Audit as a where  a.ReviewPer ='" + AuditPer + "')";
            }

            if (num != null && num.Length > 0)
            {

                strSQL += "and   id  ='" + num + "'";
            }
            strSQL += " order by ID desc";

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public DataTable searchReviewInfDao(string StartTime, string EndTime, string AuditPer, string num)
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string strSQL = "select  ID,delnum,(select top 1 ReviewPer from Audit where pid =p.id ) as ReviewPer,(select top 1 imgname  from Audit where pid = p.id ) as imgname,(select top 1 case convert(nvarchar(50),AuditStatus) when '1' then '复核' when '0' then '正在复核' else convert(nvarchar(50),AuditStatus) end  from Audit where pid =p.id ) as AuditStatus,(select top 1  AuditTime  from Audit where pid = p.id ) as AuT,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,case convert(nvarchar(50),decmothed) when 1 then '先煎' when 2 then '后下' when 3 then '加糖加蜜' else decmothed end as decscheme,name,case convert(nvarchar(50), sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end as sex,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,case decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end as AuditStatus,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,case dtbtype when '1' then '顺丰' when '2' then '圆通' when '3' then '中通' else dtbtype end as dtbtype,case convert(nvarchar(50),takeway) when 1 then '水煎餐后' else takeway end,RemarksA,RemarksB from prescription as p where "
+ "id in (select pid from Audit)" + " and 1=1"; //"and id not in  (select  prescriptionId from AgainPrescriptionCheckState) and id not in (select pid from InvalidPrescription)";
            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");

                strSQL += " and  p.dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");


                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and   id in ( select pid from Audit as a where  a.ReviewPer ='" + AuditPer + "')";
            }

            if (num != null && num.Length > 0)
            {

                strSQL += "and   id  ='" + num + "'";
            }
            strSQL += " order by ID desc";

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        public int searchReviewInfDaots(string StartTime, string EndTime, string AuditPer, string num)
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string strSQL = "select  count(distinct p.id)  from prescription as p  inner join Audit a on  a.pid = p.id  where  1=1  ";
            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");

                strSQL += " and  p.dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");


                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and   id in ( select pid from Audit as a where  a.ReviewPer ='" + AuditPer + "')";
            }

            if (num != null && num.Length > 0)
            {

                strSQL += "and   id  ='" + num + "'";
            }

            int  count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));

            return count;
        }

        public DataTable searchReviewInfDaofy(string StartTime, string EndTime, string AuditPer, string num, int ts, int page)
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间

            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select  ID,delnum,(select top 1 ReviewPer from Audit where pid =p.id ) as ReviewPer,(select top 1 imgname  from Audit where pid = p.id ) as imgname,(select top 1 case convert(nvarchar(50),AuditStatus) when '1' then '复核' when '0' then '正在复核' else convert(nvarchar(50),AuditStatus) end  from Audit where pid =p.id ) as AuditStatus,(select top 1  AuditTime  from Audit where pid = p.id ) as AuT,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,case convert(nvarchar(50),decmothed) when 1 then '先煎' when 2 then '后下' when 3 then '加糖加蜜' else decmothed end as decmothed,name,case convert(nvarchar(50), sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end as sex,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,case decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end as decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,case dtbtype when '1' then '顺丰' when '2' then '圆通' when '3' then '中通' else dtbtype end as dtbtype,case convert(nvarchar(50),takeway) when 1 then '水煎餐后' else takeway end as takeway,RemarksA,RemarksB from prescription as p where "
+ "id in (select pid from Audit)" + " and 1=1"; //"and id not in  (select  prescriptionId from AgainPrescriptionCheckState) and id not in (select pid from InvalidPrescription)";
            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");

                strSQL += " and  p.dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");


                strSQL += "and p.dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and   id in ( select pid from Audit as a where  a.ReviewPer ='" + AuditPer + "')";
            }

            if (num != null && num.Length > 0)
            {

                strSQL += "and   id  ='" + num + "'";
            }
            strSQL += " ) t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        /// <summary>
        /// 大屏显示复核
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="AuditPer"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable ReviewInfDaoDp()
        {
            string strSQL = "select top 20 p.*,h.Hname, a.reviewper, case AuditStatus when '1' then '复核' when '0' then '正在复核' else ''  end as AuditStatus,convert(varchar,AuditTime ,108) as AuditTime   from prescription as p  inner join Audit a on  a.pid = p.id  inner join Hospital h on p.Hospitalid= h.ID where convert(varchar,a.AuditTime ,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by p.ID desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
         
        }

        #region //导出数据药品查询（复核）
        public DataTable findDrugInfobyCondition(string StartTime, string EndTime, string AuditPer, string num)
        {
            string sql = "select  ROW_NUMBER() OVER(ORDER BY id desc) as ID,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice"
                + " from drug as d where 1=1";
            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");

                sql += "and d.Pspnum in (  select pspnum from  prescription where  dotime  >= ' " + strS + "')";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");


                sql += "and  d.Pspnum in (  select pspnum from  prescription where  dotime <='" + strE + "')";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                sql += "and  d.Pspnum in (  select pspnum from  prescription where  id in ( select pid from Audit as a where  a.ReviewPer ='" + AuditPer + "'))";
            }

            if (num != null && num.Length > 0)
            {

                sql += "and  d.Pspnum in (  select pspnum from  prescription where id  ='" + num + "')";
            }
            // sql += " order by ID desc";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }
        #endregion
        /// <summary>
        /// 查询工作记录处方信息
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="AuditPer"></param>
        /// <returns></returns>
        public DataTable findWorkRQfo(string StartTime, string EndTime, string AuditPer, string PNum)
        {   //and id  in (Select prescriptionId from AgainPrescriptionCheckState  where checkStatus = 1)   限制条件
            DataBaseLayer db = new DataBaseLayer();
            string strSQL = "select distinct ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where  p.curstate='" + "复核".Trim() + "' ";


            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");


                strSQL += " and   dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and  doperson='" + AuditPer + "'";
            }
            if (PNum != null && PNum.Length > 0)
            {
                strSQL += "and Pspnum='" + PNum + "'";
            }

            DataTable dt = db.get_DataTable(strSQL);

            return dt;

        }

        public int findWorkRQfots(string StartTime, string EndTime, string AuditPer, string PNum)
        {   //and id  in (Select prescriptionId from AgainPrescriptionCheckState  where checkStatus = 1)   限制条件
            DataBaseLayer db = new DataBaseLayer();
            string strSQL = "select count(distinct ID)  from prescription as p where  p.curstate='" + "复核".Trim() + "' ";

            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");


                strSQL += " and   dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and  doperson='" + AuditPer + "'";
            }
            if (PNum != null && PNum.Length > 0)
            {
                strSQL += "and Pspnum='" + PNum + "'";
            }

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSQL));

            return count;

        }


        public DataTable findWorkRQfofy(string StartTime, string EndTime, string AuditPer, string PNum, int ts, int page)
        {   //and id  in (Select prescriptionId from AgainPrescriptionCheckState  where checkStatus = 1)   限制条件
            DataBaseLayer db = new DataBaseLayer();
            string strSQL = "select top " + ts + " *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from (  select distinct ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where  p.curstate='" + "复核".Trim() + "' ";


            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");


                strSQL += " and   dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and  doperson='" + AuditPer + "'";
            }
            if (PNum != null && PNum.Length > 0)
            {
                strSQL += "and Pspnum='" + PNum + "'";
            }
            strSQL += " ) t  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;

        }



        public DataTable findWorkRQfoDao(string StartTime, string EndTime, string AuditPer, string PNum)
        {   //and id  in (Select prescriptionId from AgainPrescriptionCheckState  where checkStatus = 1)   限制条件
            DataBaseLayer db = new DataBaseLayer();
            string strSQL = "select distinct ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,case convert(nvarchar(50),decmothed) when 1 then '先煎' when 2 then '后下' when 3 then '加糖加蜜' else decmothed end,name,case convert(nvarchar(50), sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end,age,phone,address,department,inpatientarea,";
            strSQL += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,case decscheme when 1 then '微压（密闭）解表（15min)' when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSQL += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,case dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' else dtbtype end,case convert(nvarchar(50),takeway) when 1 then '水煎餐后' else takeway end,RemarksA,RemarksB from prescription as p where  id not in (select pid from InvalidPrescription)";


            if (StartTime != null && StartTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(StartTime);

                string strS = d.ToString("yyyy/MM/dd  00:00:00");


                strSQL += "and   dotime  >= ' " + strS + "'";
            }
            if (EndTime != null && EndTime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(EndTime);

                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and dotime <='" + strE + "'";

            }
            if (AuditPer != null && AuditPer.Length > 0)
            {

                strSQL += "and  doperson='" + AuditPer + "'";
            }
            if (PNum != null && PNum.Length > 0)
            {
                strSQL += "and Pspnum='" + PNum + "'";
            }

            DataTable dt = db.get_DataTable(strSQL);

            return dt;

        }
        #endregion
        #region
        ///// <summary>
        ///// 查询处方信息
        ///// </summary>
        ///// <param name="page">当前页</param>
        ///// <param name="pageSize)">每页大小</param>
        public DataTable findAllRecipeInfo(int page, int pageSize)
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p ";
            DataTable dt = db.get_DataTable(sql);
            return dt;
        }

        ///// <summary>
        ///// 查询未匹配处方信息
        ///// </summary>
        ///// <param name="workContent">工作内容,0全部,1匹配人员,2打印人员</param>
        ///// <param name="date)">日期</param>
        ///// <param name="barCode)">员工条码</param>
        ///// <returns>SqlDataReader对象</returns>
        ///// <param name="page">当前页</param>
        ///// <param name="pageSize)">每页大小</param>
        public DataTable findNotMatchRecipeInfo(string workContent, string date, string barCode, int page, int pageSize)
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p left join DrugMatching dm on p.id=dm.pspId where dm.pspId IS NULL";



            DataTable dt = db.get_DataTable(sql);
            return dt;
        }
        ///// <summary>
        ///// 查询匹配处方信息
        ///// </summary>
        ///// <param name="workContent">工作内容,0全部,1匹配人员,2打印人员</param>
        ///// <param name="date)">日期</param>
        ///// <param name="barCode)">员工条码</param>
        ///// <returns>SqlDataReader对象</returns>
        ///// <param name="page">当前页</param>
        ///// <param name="pageSize)">每页大小</param>
        public DataTable findMatchRecipeInfo(string workContent, string date, string barCode, int page, int pageSize)
        {
            string sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p right join DrugMatching dm on p.id=dm.pspId";


            DataTable dt = db.get_DataTable(sql);
            return dt;
        }


        ///// <summary>
        ///// 查询未审核和未匹配处方信息
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
        public DataTable findNotCheckAndMatchRecipeInfo(int hospitalId, string Pspnum)
        {
            string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
             + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid=p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and dm.drugId IS NULL AND d.ID IS NOT NULL and p.id not in (select pid from InvalidPrescription) and p.confirmDrug=1";
            if (hospitalId != 0)
            {
                sql += "  and p.Hospitalid='" + hospitalId + "'";
            }
            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }


            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        //修改后的方法
        public int findNotCheckAndMatchRecipeInfo_count_2(int hospitalId, string Pspnum)
        {
            /*  string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
               + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
               + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid=p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and dm.drugId IS NULL AND d.ID IS NOT NULL  and p.confirmDrug=1";
             */
            string sql = "select count(p.id)"
          + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid=p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where p.curstate='未匹配'  and p.confirmDrug=1";
            if (hospitalId != 0)
            {
                sql += "  and p.Hospitalid='" + hospitalId + "'";
            }
            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }

            db.write_log_txt("药品匹配数量：" + sql);
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;
           

        }
        public DataTable findNotCheckAndMatchRecipeInfo_2(int hospitalId, string Pspnum, int ts, int page)
        {
            /*  string sql = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
               + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
               + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid=p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and dm.drugId IS NULL AND d.ID IS NOT NULL  and p.confirmDrug=1";
             */
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by q.id desc ) as rownumber from (" + "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select top 1 hname from hospital as h where h.id = p.hospitalid) as hname,"
          + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
          + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.pid=p.id left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where  p.curstate='未匹配' ";
            if (hospitalId != 0)
            {
                sql += "  and p.Hospitalid='" + hospitalId + "'";
            }
            if (Pspnum != null && Pspnum.Length != 0)
            {
                sql += " and p.Pspnum='" + Pspnum + "'";
            }
            sql += "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            db.write_log_txt("药品匹配：" + sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public DataTable findRecipeInfo()
        {
            string strSql = "select distinct ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select TOP 1 hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where " +//"id not in (select pid from InvalidPrescription) and 此处解决部分处方在处方录入页面不显示问题
                "(curstate='接方' or curstate='未匹配'   or curstate='未审核' or curstate='审核未通过') and " +
              " confirmDrug=0 ";
            strSql += "order   by   ID   desc";

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        /// <summary>
        /// 大屏显示接方
        /// </summary>
        /// <returns></returns>
        public DataTable findRecipeInfoTo20()
        {
            string strSql = "select top 20  p.*,h.Hname, case sex when 1 then '男'  else  '女'  end  as xb from prescription p inner join Hospital h on p.Hospitalid= h.ID  where (curstate='接方' or curstate='未匹配'   or curstate='未审核' or curstate='审核未通过') and  confirmDrug=0 and convert(varchar,p.ordertime ,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by p.ID desc";
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }

        /// <summary>
        /// 处方列表，分页显示
        /// </summary>
        /// <returns></returns>
        public DataTable findRecipeInfo(int ts,int page)
        {
            string strSql = "select top " + ts + " *  from  ( select   distinct ID, row_number() over(order by id desc ) as rownumber,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select TOP 1 hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where " +//"id not in (select pid from InvalidPrescription) and 此处解决部分处方在处方录入页面不显示问题
                "(curstate='接方' or curstate='未匹配'   or curstate='未审核' or curstate='审核未通过') and " +
              " confirmDrug=0 ) p  where rownumber>" + (page - 1) * ts;
            strSql += "  order   by   ID   desc";

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public int GetRecipeInfoCount()
        {
            string strSql = "select count(*) from prescription as p where (curstate='接方' or curstate='未匹配'   or curstate='未审核' or curstate='审核未通过') and " +
              " confirmDrug=0 ";
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSql));
            return count;
        }

       public int findRecipeInfoTs(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            string strSql = "select count(*) from prescription  where 1=1 ";
               // + "id not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";

            if (hospitalid != "0")
            {
                strSql += " and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += " and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += " and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                //   strSql += "and getdrugtime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += " and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += " and ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {
                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                //
                strSql += " and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += " and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "') ";
            }
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSql));
            return count;
        }

        public DataTable findRecipeInfo(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            string strSql = " select distinct ID,row_number() over(order by id desc ) as rownumber, delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where 1=1 ";//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
              //  + "id not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";




            if (hospitalid != "0")
            {
                strSql += " and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += " and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += " and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                //   strSql += "and getdrugtime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += " and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += " and ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {
                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                //
                strSql += " and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += " and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "') ";
            }

            strSql += " order by p.id desc";


            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public DataTable findRecipeInfofy(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime,int ts,int page)
        {
            /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
            string strSql = "select top " + ts + " *  from  ( select distinct ID,row_number() over(order by id desc ) as rownumber, delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
                + "id not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";




            if (hospitalid != "0")
            {
                strSql += "and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                //   strSql += "and getdrugtime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {
                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                //
                strSql += "and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
            }
            strSql += ") p  where rownumber>" + (page-1)*ts + " order by p.id desc";

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public DataTable findRecipeInfofyTjt(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime, int ts, int page)
        {
            string strSql = "select top " + ts + " *  from  ( select  top 100 percent p.ID,row_number() over(order by p.id desc ) as rownumber,s.outpatientIndex, delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea, case when s.ptype=0 then '门诊' when s.ptype=1 then '住院' else '先煎' end as ptype,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p  left join prescriptionSubsidiary  s on p.ID= s.pid  where 1=1 ";
            // + " p.id not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";


            if (hospitalid != "0")
            {
                strSql += " and  p.hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                //   strSql += "and getdrugtime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += "and p.Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and p.ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {
                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");
                //
                strSql += "and p.dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "'" + "order by hname desc" + ")";
            }
            strSql += ") t  where t.rownumber>" + (page - 1) * ts + " order by t.id desc";

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public DataTable findRecipeInfo_1(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
            string strSql = "select distinct p.ID,p.delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p  inner join tb_node_image i on i.n_id=p.id where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
                + " 1=1 ";




            if (hospitalid != "0")
            {
                strSql += "and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                //   strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                strSql += "and p.ordertime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and ID ='" + tisaneid + "'";
            }
            /*     if (jftime != "")
                 {
                     DateTime d = Convert.ToDateTime(jftime);
                     string strS = d.ToString("yyyy/MM/dd 00:00:00");
                     string strE = d.ToString("yyyy/MM/dd 23:59:59");
                     //
                     strSql += "and dotime between '" + strS + "' and '" + strE + "'";
                 }
                 if (doper != "0")
                 {
                     strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
                 }

                 strSql += "order by p.id desc";
     */

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable findRecipeInfo_temp(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime, int ts, int page)
        {
            /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
            string strSql = "select top " + ts + " *  from  (  select *,row_number() over(order by q.pid desc ) as ID from " + "(select top 100 percent p.ID as pid,p.delnum,(select top 1 hnum from hospital as h where h.id = p.hospitalid) as hnum,(select  top 1 hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select top 1 jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription p   where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
                + " 1=1 ";




            if (hospitalid != "0")
            {
                strSql += "and p.hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and p.name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                //   strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                strSql += "and p.ordertime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += "and p.Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and p.ID ='" + tisaneid + "'";
            }


            /*     if (jftime != "")
                 {
                     DateTime d = Convert.ToDateTime(jftime);
                     string strS = d.ToString("yyyy/MM/dd 00:00:00");
                     string strE = d.ToString("yyyy/MM/dd 23:59:59");
                     //
                     strSql += "and dotime between '" + strS + "' and '" + strE + "'";
                 }
                 if (doper != "0")
                 {
                     strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
                 }

                 strSql += "order by p.id desc";
     */
            strSql += "  order by p.id desc) q  ) z where z.ID>" + (page - 1) * ts + " order by z.ID asc";
            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public int findRecipeInfo_count(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/
            string strSql = "select count(p.id) as pages from prescription p   where "//+"id not in (select pid from InvalidPrescription) and" 该注释解决在配方查询中可以查到的处方在接方查询中查不到
                + " 1=1 ";




            if (hospitalid != "0")
            {
                strSql += "and p.hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and p.name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                //   strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";
                strSql += "and p.ordertime>= '" + strTime + "'";
            }
            if (strPspnum != "0")
            {
                strSql += "and p.Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and p.ID ='" + tisaneid + "'";
            }
            /*     if (jftime != "")
                 {
                     DateTime d = Convert.ToDateTime(jftime);
                     string strS = d.ToString("yyyy/MM/dd 00:00:00");
                     string strE = d.ToString("yyyy/MM/dd 23:59:59");
                     //
                     strSql += "and dotime between '" + strS + "' and '" + strE + "'";
                 }
                 if (doper != "0")
                 {
                     strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
                 }

                 strSql += "order by p.id desc";
     */

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSql));

            return count;
        }
        public DataTable findRecipeInfoDao(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            /* string strSql = "select ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,hospitalId,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
             strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
             strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.hospitalid =" + hospitalid + " or name='" + strName + " ' or  getdrugtime='" + strTime + "'or Pspnum='" + strPspnum + "'";*/

            string strSql = "select distinct ID,delnum,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,case convert(nvarchar(50),decmothed) when 1 then '先煎' when 2 then '后下' when 3 then '加糖加蜜' else decmothed end,name,case convert(nvarchar(50), sex)  when 1 then '男' when 2 then '女' else convert(nvarchar(50), sex) end,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,case decscheme when 1 then '微压（密闭）解表（15min)'  when 85 then '先煎后下自定义' when 2 then '微压（密闭）汤药（25min）' when 3 then '微压（密闭）补药（40min）' when 4 then '常压解表（10min，10min）' when 5 then '常压汤药（20min，15min）' when 6 then '常压补药（25min，20min）'when 20 then '先煎解表（10min，10min，10min）'when 21 then '先煎汤药（10min，20min，15min）'when 22 then '先煎补药（10min，25min，20min）' when 36 then '后下解表（10min（3：7），10min）' when 37 then '后下汤药（20min（13：7），15min）' when 38 then '后下补药（25min（18：7），20min）' when 81 then '微压自定义' when 82 then '常压自定义'when 83 then '先煎自定义' when 84 then '后下自定义' else decscheme end,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,(select jiefangman  from jfInfo as j where j.pid=p.id ) as person,dtbcompany,dtbaddress,dtbphone,case dtbtype when 1 then '顺丰' when 2 then '圆通' when 3 then '中通' else dtbtype end,case convert(nvarchar(50),takeway) when 1 then '水煎餐后' else takeway end,RemarksA,RemarksB from prescription as p where id not in (select pid from InvalidPrescription) and id not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";
            if (hospitalid != "0")
            {
                strSql += "and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";

            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSql += "and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')";
            }
            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public DataTable findRecipeInfoDaoTjt_1(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {

            string strSql = @"select top 100 percent s.outpatientIndex as outpatientIndex,p.Pspnum,p.Hospitalid,h.Hname,p.name,p.dose,p.takenum,p.RemarksA     from prescription p  
inner join hospital h on p.Hospitalid = h.ID
left join prescriptionSubsidiary s  on p.id=s.pid  where p.id not in (select pid from InvalidPrescription) and p.id 
not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";
            if (hospitalid != "0")
            {
                strSql += "and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";

            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and p.ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSql += "and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')   ";
            }
           string strend="order by  cast(s.outpatientIndex as int)  asc  ";
           string strSql1 = " and ISNUMERIC(s.outpatientIndex)=1 ";
            string strSql2 = " and ISNUMERIC(s.outpatientIndex)=0 ";
            strSql = "select  * from (" + strSql + strSql1 + strend + ") a  order by  cast(a.outpatientIndex as int) asc";
         //   db.write_log_txt("接方导出"+strSql );
                //"+ " union all " + strSql + strSql2;
            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        public DataTable findRecipeInfoDaoTjt_2(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {

            string strSql = @"select top 100 percent ' '+s.outpatientIndex,p.Pspnum,p.Hospitalid,h.Hname,p.name,p.dose,p.takenum,p.RemarksA     from prescription p  
inner join hospital h on p.Hospitalid = h.ID
left join prescriptionSubsidiary s  on p.id=s.pid  where p.id not in (select pid from InvalidPrescription) and p.id 
not in (select prescriptionId  from PrescriptionCheckState where checkStatus=2) ";
            if (hospitalid != "0")
            {
                strSql += "and hospitalid ='" + hospitalid + "'";
            }

            if (strName != "0")
            {
                strSql += "and name ='" + strName + "'";
            }
            if (strTime != "0")
            {
                strSql += "and Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%'";

            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and p.ID ='" + tisaneid + "'";
            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSql += "and dotime between '" + strS + "' and '" + strE + "'";
            }
            if (doper != "0")
            {
                strSql += "and p.id in (select pid  from jfInfo as j where j.pid=p.id  and  jiefangman ='" + doper + "')   ";
            }
        //    string strend = "order by  cast(s.outpatientIndex as int)  asc  ";
          //  string strSql1 = " and ISNUMERIC(s.outpatientIndex)=1 ";
            string strSql2 = " and ISNUMERIC(s.outpatientIndex)=0 ";
           strSql= strSql + strSql2;
            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #region //导出数据药品查询（接方）
        public DataTable findDrugInfobyCondition(string hospitalid, string strPspnum, string strTime, string strName, string tisaneid, string doper, string jftime)
        {
            string strSql = "select  ROW_NUMBER() OVER(ORDER BY id desc) as ID,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice"
                + " from drug as d where 1=1";
            if (hospitalid != "0")
            {
                strSql += "and Pspnum in (select Pspnum from prescription where hospitalid ='" + hospitalid + "')";
            }

            if (strName != "0")
            {
                strSql += "and Pspnum in (select Pspnum from prescription where name ='" + strName + "')";
            }
            if (strTime != "0")
            {
                strSql += "and Pspnum in (select Pspnum from prescription where Convert(varchar,getdrugtime  ,120)   like '" + strTime + "%')";

            }
            if (strPspnum != "0")
            {
                strSql += "and Pspnum ='" + strPspnum + "'";
            }
            if (tisaneid != "0")
            {
                strSql += "and pid ='" + tisaneid + "'";
            }
            if (doper != "0")
            {
                strSql += "and pid in (select pid  from jfInfo as j where j.pid=pid  and  jiefangman ='" + doper + "')";
            }
            if (jftime != "")
            {

                DateTime d = Convert.ToDateTime(jftime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                string strE = d.ToString("yyyy/MM/dd 23:59:59");

                strSql += "and pid in (select id from prescription where dotime between '" + strS + "' and '" + strE + "')";
            }

            // sql += " order by ID desc";

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        public DataTable findRecipeInfo(int id)
        {
            string strSql = "select ID,delnum,hospitalid,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Pspnum,decmothed,name,sex,age,phone,address,department,inpatientarea,";
            strSql += "ward,sickbed,diagresult,dose,takemethod,takenum,packagenum,decscheme,oncetime,twicetime,soakwater,soaktime,labelnum,remark,";
            strSql += "doctor,footnote,getdrugtime,getdrugnum,ordertime,curstate,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,takeway,RemarksA,RemarksB from prescription as p where p.id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }


        public DataTable findDrugInfo(int id)
        {
            string sql = "select ID,delnum,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where id = '" + id + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }


        public SqlDataReader findPatient(int hospitalId)
        {
            string sql = "select ID,Pspnum from prescription where Hospitalid=" + hospitalId;

            return db.get_Reader(sql);
        }

        ///// <summary>
        ///// 查询处方信息
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
        public DataTable findRecipeInfo(int hospitalId, string Pspnum)
        {
            string sql = "select ID,Pspnum,customid,delnum,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
             + "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate from prescription as p where Hospitalid=" + hospitalId + " and Pspnum=" + Pspnum;


            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region 重新审核处方信息
        public int ReAuditRecipeInfo(int nRecipeId)
        {
            String sql = "";
            int end = 0;
            string str1 = " select * from  adjust  where prescriptionId ='" + nRecipeId + "'";
            SqlDataReader sr1 = db.get_Reader(str1);
            if (sr1.Read())
            {
                sql = "";
            }
            else
            {
                string str = "select prescriptionId from PrescriptionCheckState where   prescriptionId = '" + nRecipeId + "'";
                SqlDataReader sr = db.get_Reader(str);

                if (sr.Read())
                {
                    sql = "delete from PrescriptionCheckState  where prescriptionId =" + nRecipeId;
                    if (db.cmd_Execute(sql) == 1)
                    {
                        sql = "update prescription set curstate = '未审核'  where id = '" + nRecipeId + "'";
                    }
                }
                else
                {
                    sql = "";
                }
            }
            if (sql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(sql);
            }
            return end; ;
        }



        #endregion
        /* #region 作废处方信息
        public bool deleteRecipeInfo(int nRecipeId)
        {
            string strSql = "";
             strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";
             
             if (db.cmd_Execute(strSql) == 1)
             {
                 strSql = "update prescription set curstate = '作废'  where id = '" + nRecipeId + "'";
                 int n = db.cmd_Execute(strSql);
             }
            return true;
        }
        #endregion*/


        #region 配方的作废处方信息
        public int deleteRecipeInfo1(int nRecipeId)
        {

            int end = 0;
            string strSql = "";
            string str = "select * from adjust where   prescriptionId = '" + nRecipeId + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                strSql = "";

            }
            else
            {
                //string str2 = " select  prescriptionId  from PrescriptionCheckState   where    checkStatus =1 and  prescriptionId =" + nRecipeId;
                //SqlDataReader sr2 = db.get_Reader(str2);
                //if (sr2.Read())
                //{

                string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read())
                {
                    strSql = "";
                }
                else
                {
                    strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";

                    if (db.cmd_Execute(strSql) == 1)
                    {
                        strSql = "update prescription set curstate = '作废'  where id = '" + nRecipeId + "'";
                        if (db.cmd_Execute(strSql) == 1)
                        {
                            strSql = "delete from  PrescriptionCheckState where   prescriptionId = '" + nRecipeId + "'";

                        }
                    }
                }
                // }
                //else
                // {
                //    strSql = "";

                //}
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 0)
                {
                    end = 1;
                }
            }
            return end; ;
        }
        public int deletemedicineRecipeInfo1(int nRecipeId)
        {

            int end = 0;
            string strSql = "";
            string str = "select * from medicalstorage where   id = '" + nRecipeId + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (!sr.Read())
            {
                strSql = "";

            }
            else
            {
                //string str2 = " select  prescriptionId  from PrescriptionCheckState   where    checkStatus =1 and  prescriptionId =" + nRecipeId;
                //SqlDataReader sr2 = db.get_Reader(str2);
                //if (sr2.Read())
                //{

                string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read())
                {
                    strSql = "";
                }
                else
                {
                    strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";


                    if (db.cmd_Execute(strSql) == 1)
                    {
                        strSql = "update prescription set curstate = '作废'  where id = '" + str1 + "'";


                        strSql = "update medicalstorage set Amount ='0' where  id = '" + nRecipeId + "'";

                    }
                }
                // }
                //else
                // {
                //    strSql = "";

                //}
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 0)
                {
                    end = 1;
                }
            }
            return end; ;
        }
        public int deleteoutbounddataqueryInfo1(int nRecipeId)
        {

            int end = 0;
            string strSql = "";
            string str = "select * from storagefrom where   id = '" + nRecipeId + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (!sr.Read())
            {
                strSql = "";

            }
            else
            {
                //string str2 = " select  prescriptionId  from PrescriptionCheckState   where    checkStatus =1 and  prescriptionId =" + nRecipeId;
                //SqlDataReader sr2 = db.get_Reader(str2);
                //if (sr2.Read())
                //{

                string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read())
                {
                    strSql = "";
                }
                else
                {
                    strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";

                    if (db.cmd_Execute(strSql) == 1)
                    {
                        strSql = "update prescription set curstate = '作废'  where id = '" + nRecipeId + "'";
                        if (db.cmd_Execute(strSql) == 1)
                        {
                            strSql = "update storagefrom set Amount ='0' where  id = '" + nRecipeId + "'";

                        }
                    }
                }
                // }
                //else
                // {
                //    strSql = "";

                //}
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 0)
                {
                    end = 1;
                }
            }
            return end; ;
        }
        public int deletemedicaloutbounddataqueryInfo1(int nRecipeId)
        {

            int end = 0;
            string strSql = "";
            string str = "select * from medicalstoragefrom where   id = '" + nRecipeId + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (!sr.Read())
            {
                strSql = "";

            }
            else
            {
                //string str2 = " select  prescriptionId  from PrescriptionCheckState   where    checkStatus =1 and  prescriptionId =" + nRecipeId;
                //SqlDataReader sr2 = db.get_Reader(str2);
                //if (sr2.Read())
                //{

                string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read())
                {
                    strSql = "";
                }
                else
                {
                    strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";

                    if (db.cmd_Execute(strSql) == 1)
                    {
                        strSql = "update prescription set curstate = '作废'  where id = '" + nRecipeId + "'";
                        if (db.cmd_Execute(strSql) == 1)
                        {
                            strSql = "update medicalstoragefrom set Amount ='0' where  id = '" + nRecipeId + "'";

                        }
                    }
                }
                // }
                //else
                // {
                //    strSql = "";

                //}
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 0)
                {
                    end = 1;
                }
            }
            return end; ;
        }
        #endregion
        #region 取消作废的处方信息


        public int CloseRecipeInfo(int nRecipeId)
        {
            RecipeModel rm = new RecipeModel();
            SqlDataReader sdr2 = rm.findisneedcheckstatus();
            string isneedcheck = "";
            if (sdr2.Read())
            {
                isneedcheck = sdr2["isneedcheck"].ToString();

            }

            int end = 0;
            string strSql = "";

            string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
            SqlDataReader sr1 = db.get_Reader(str1);
            if (sr1.Read())
            {
                strSql = "delete from  InvalidPrescription where   pid = '" + nRecipeId + "'";

                if (db.cmd_Execute(strSql) == 1)
                {
                    //匹配未审核
                    string str = "select distinct p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                     + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate,p.RemarksA,p.RemarksB"
                      + " from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId inner join drug d on d.Pspnum=p.Pspnum and p.Hospitalid = d.Hospitalid inner join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL  and p.ID = '" + nRecipeId + "'";
                    SqlDataReader sr = db.get_Reader(str);
                    if (sr.Read())
                    {


                        if (isneedcheck == "0")
                        {
                            strSql = "update prescription set curstate = '未审核'  where id = '" + nRecipeId + "'";
                        }
                        else
                        {
                            strSql = "update prescription set curstate = '已审核'  where id = '" + nRecipeId + "'";
                        }
                    }
                    else
                    {
                        strSql = "update prescription set curstate = '未匹配'  where id = '" + nRecipeId + "'";

                    }
                }

            }
            else
            {

                strSql = "";
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
            }
            return end; ;
        }
        #endregion
        #region 综合查询的作废信息
        public int deleteCompInfo1(int nRecipeId)
        {
            int end = 0;
            string strSql = "";
            string str1 = "select * from InvalidPrescription where   pid = '" + nRecipeId + "'";
            SqlDataReader sr1 = db.get_Reader(str1);
            if (sr1.Read())
            {
                strSql = "";
            }
            else
            {
                strSql = "insert  into  InvalidPrescription(pid) values('" + nRecipeId + "')";

                if (db.cmd_Execute(strSql) == 1)
                {
                    strSql = "update prescription set curstate = '作废'  where id = '" + nRecipeId + "'";
                    if (db.cmd_Execute(strSql) == 1)
                    {
                        strSql = "delete from  PrescriptionCheckState where   prescriptionId = '" + nRecipeId + "'";
                        if (db.cmd_Execute(strSql) == 1)
                        {
                            strSql = "delete from  adjust where   prescriptionId = '" + nRecipeId + "'";
                            if (db.cmd_Execute(strSql) == 1)
                            {
                                strSql = "delete from  Audit where   pid = '" + nRecipeId + "'";
                                if (db.cmd_Execute(strSql) == 1)
                                {
                                    strSql = "delete from  bubble where   pid = '" + nRecipeId + "'";
                                    if (db.cmd_Execute(strSql) == 1)
                                    {
                                        strSql = "delete from  tisaneinfo where   pid = '" + nRecipeId + "'";
                                        if (db.cmd_Execute(strSql) == 1)
                                        {
                                            strSql = "delete from  Packing where   DecoctingNum = '" + nRecipeId + "'";
                                            if (db.cmd_Execute(strSql) == 1)
                                            {
                                                strSql = "delete from  Delivery where   DecoctingNum = '" + nRecipeId + "'";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }


                }
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 0)
                {
                    end = 1;
                }
            }
            return end; ;
        }
        #endregion




        //删除处方信息

        public int deleteRecipeInfo(int nRecipeId)
        {
            string strSql = "";
            int n = 0;
            string str1 = "select * from prescription where id = '" + nRecipeId + "'";
            SqlDataReader sdr = db.get_Reader(str1);

            string pspnum = "";
            string hospitalid = "";
            string curstate = "";
            if (sdr.Read())
            {
                pspnum = sdr["pspnum"].ToString();
                hospitalid = sdr["hospitalid"].ToString();
                curstate = sdr["curstate"].ToString();
            }

            RecipeModel rm = new RecipeModel();
            SqlDataReader sdr2 = rm.findisneedcheckstatus();
            string isneedcheck = "";
            if (sdr2.Read())
            {
                isneedcheck = sdr2["isneedcheck"].ToString();
            }
            if (isneedcheck == "0")//需要审核
            {
                if (curstate == "未匹配" || curstate == "接方" || curstate == "未审核")
                {
                    strSql = "";
                    strSql = "delete from prescription where id = '" + nRecipeId + "'";


                    string strSql2 = "delete from jfInfo where pid = '" + nRecipeId + "'";
                    db.cmd_Execute(strSql2);
                    if (db.cmd_Execute(strSql) == 1)
                    {
                        n = 1;
                        string str2 = "delete from drug where pspnum = '" + pspnum + "'  and  hospitalid ='" + hospitalid + "'";
                        db.cmd_Execute(str2);
                    }

                }
                else
                {
                    n = 2;
                }
            }
            else//不需要审核
            {
                if (curstate == "未匹配" || curstate == "接方" || curstate == "未审核" || curstate == "已审核")
                {
                    strSql = "";
                    strSql = "delete from prescription where id = '" + nRecipeId + "'";
                    string strSql2 = "delete from jfInfo where pid = '" + nRecipeId + "'";
                    db.cmd_Execute(strSql2);
                    if (db.cmd_Execute(strSql) == 1)
                    {
                        n = 1;
                        string str2 = "delete from drug where pspnum = '" + pspnum + "'  and  hospitalid ='" + hospitalid + "'";
                        db.cmd_Execute(str2);
                    }

                }
                else
                {
                    n = 2;
                }

            }
            return n;
        }

        //删除药品信息

        public int deleteDrugInfo(int ndrugId)
        {
            string strSql = "";
            int n = 0;
            string strpspnum = "";
            string strhospitalid = "";
            string curstatus = "";
            string str2 = "select * from drug where id = '" + ndrugId + "'";
            SqlDataReader sdr1 = db.get_Reader(str2);
            if (sdr1.Read())
            {
                strpspnum = sdr1["pspnum"].ToString();
                strhospitalid = sdr1["hospitalid"].ToString();
            }

            string str3 = "select * from prescription where pspnum ='" + strpspnum + "' and hospitalid ='" + strhospitalid + "'";
            SqlDataReader sdr2 = db.get_Reader(str3);

            if (sdr2.Read())
            {
                curstatus = sdr2["curstate"].ToString();
            }

            if (curstatus == "未匹配" || curstatus == "接方" || curstatus == "未审核")
            {
                strSql = "delete from drug where id = '" + ndrugId + "'";
                n = db.cmd_Execute(strSql);
            }
            else
            {
                n = 2;
            }
            return n;
        }





        public DataTable findRecipeInfo(int hospitalId, string Pspnum, int Status, string name, string bmid, string date)
        {
            string sqlName = string.Empty;
            if (name != "")
            {
                sqlName = " and name='" + name + "'";
            }
            string sqlbmid = string.Empty;
            if (bmid != "")
            {
                sqlbmid = " and id=" + bmid;
            }
            string sqlDate = string.Empty;
            if (date != "")
            {
                DateTime d = Convert.ToDateTime(date);
                string strS = d.ToString("yyyy-MM-dd 00:00:00");
                string strE = d.ToString("yyyy-MM-dd 23:59:59");
                sqlDate = " and ordertime>='" + strS + "' and ordertime<= '" + strE + "'";
            }

            string sqlHospital = string.Empty;
            if (hospitalId != 0)
            {
                sqlHospital = " and hospitalId ='" + hospitalId + "'";
            }

            string sqlPsP = string.Empty;
            if (Pspnum != "")
            {
                sqlPsP = " and pspNum ='" + Pspnum + "'";
            }

            string sqlStatus = " and printstatus ='" + Status + "'";

            StringBuilder strBul = new StringBuilder();
            strBul.AppendFormat(@"select distinct ID,Pspnum,customid,delnum,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,
diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate from prescription as p  where 1=1 {0}{1}{2}{3}{4} and id in (select prescriptionid from prescriptionCheckState where checkstatus =1 {5}) order by p.id desc ", sqlName, sqlbmid, sqlDate, sqlHospital, sqlPsP, sqlStatus);
            db.write_log_txt("打印处方显示：" + strBul);
            DataTable dt = db.get_DataTable(strBul.ToString());

            return dt;

        }

        public int  findRecipeInfots(int hospitalId, string Pspnum, int Status, string name, string bmid, string date)
        {
            string sqlName = string.Empty;
            if (name != "")
            {
                sqlName = " and name='" + name + "'";
            }
            string sqlbmid = string.Empty;
            if (bmid != "")
            {
                sqlbmid = " and id=" + bmid;
            }
            string sqlDate = string.Empty;
            if (date != "")
            {
                DateTime d = Convert.ToDateTime(date);
                string strS = d.ToString("yyyy-MM-dd 00:00:00");
                string strE = d.ToString("yyyy-MM-dd 23:59:59");
                sqlDate = " and ordertime>='" + strS + "' and ordertime<= '" + strE + "'";
            }

            string sqlHospital = string.Empty;
            if (hospitalId != 0)
            {
                sqlHospital = " and hospitalId ='" + hospitalId + "'";
            }

            string sqlPsP = string.Empty;
            if (Pspnum != "")
            {
                sqlPsP = " and pspNum ='" + Pspnum + "'";
            }

            string sqlStatus = " and printstatus ='" + Status + "'";

            StringBuilder strBul = new StringBuilder();
            strBul.AppendFormat(@"select  count(distinct id)  from prescription as p  where 1=1 {0}{1}{2}{3}{4} and id in (select prescriptionid from prescriptionCheckState where checkstatus =1 {5})  ", sqlName, sqlbmid, sqlDate, sqlHospital, sqlPsP, sqlStatus);
            db.write_log_txt("打印处方显示：" + strBul);
            int count  = Convert.ToInt32(db.cmd_ExecuteScalar(strBul.ToString()));

            return count;

        }

        public DataTable findRecipeInfofy(int hospitalId, string Pspnum, int Status, string name, string bmid, string date,int ts ,int page)
        {
            string sqlName = string.Empty;
            if (name != "")
            {
                sqlName = " and name='" + name + "'";
            }
            string sqlbmid = string.Empty;
            if (bmid != "")
            {
                sqlbmid = " and id=" + bmid;
            }
            string sqlDate = string.Empty;
            if (date != "")
            {
                DateTime d = Convert.ToDateTime(date);
                string strS = d.ToString("yyyy-MM-dd 00:00:00");
                string strE = d.ToString("yyyy-MM-dd 23:59:59");
                sqlDate = " and ordertime>='" + strS + "' and ordertime<= '" + strE + "'";
            }

            string sqlHospital = string.Empty;
            if (hospitalId != 0)
            {
                sqlHospital = " and hospitalId ='" + hospitalId + "'";
            }

            string sqlPsP = string.Empty;
            if (Pspnum != "")
            {
                sqlPsP = " and pspNum ='" + Pspnum + "'";
            }

            string sqlStatus = " and printstatus ='" + Status + "'";

            StringBuilder strBul = new StringBuilder();
            strBul.AppendFormat(@"select top " + ts + @" *  from  (  select *,row_number() over(order by t.id desc ) as rownumber  from ( select distinct ID,Pspnum,customid,delnum,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,
diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate from prescription as p  where 1=1 {0}{1}{2}{3}{4} and id in (select prescriptionid from prescriptionCheckState where checkstatus =1 {5}) ) t  ) p where p.rownumber>" + (page - 1) * ts + " order by p.id desc ", sqlName, sqlbmid, sqlDate, sqlHospital, sqlPsP, sqlStatus);
            db.write_log_txt("打印处方显示：" + strBul);
            DataTable dt = db.get_DataTable(strBul.ToString());

            return dt;

        }

        #region
        public int UpdateRecipeInfo(RecipeInfo ri, int id)
        {

            string str1 = "select * from prescription where id = '" + id + "'";
            SqlDataReader sdr = db.get_Reader(str1);
            int n = 0;
            string pspnum = "";
            string hospitalid = "";
            string curstate = "";
            if (sdr.Read())
            {
                pspnum = sdr["pspnum"].ToString();
                hospitalid = sdr["hospitalid"].ToString();
                curstate = sdr["curstate"].ToString();
            }

            RecipeModel rm = new RecipeModel();
            SqlDataReader sdr3 = rm.findisneedcheckstatus();
            string isneedcheck = "";
            if (sdr3.Read())
            {
                isneedcheck = sdr3["isneedcheck"].ToString();

            }

            if (isneedcheck == "0")
            {
                //if (curstate == "未匹配" || curstate == "接方" || curstate == "未审核" || curstate == "已审核")
                ////if (curstate == "未匹配" || curstate == "接方" || curstate == "未审核")
                //{


                    string str = "select * from prescription where hospitalid ='" + ri.nHospitalID + "' and pspnum ='" + ri.strPspnum + "' and id not in ('" + id + "')";
                    SqlDataReader sdr2 = db.get_Reader(str);
                    if (sdr2.Read())
                    {
                        n = 3;//该处方号已存在
                    }
                    else
                    {
                        string strSql = "Update prescription set hospitalid ='" + ri.nHospitalID + "', pspnum='" + ri.strPspnum + "',decmothed='" + ri.strDecMothed + "',name ='" + ri.strName + "',  sex=" + ri.nSex + ",  age=" + ri.nAge;
                        strSql += ", phone='" + ri.strPhone + "' , address='" + ri.strAddress + "', department='" + ri.strDepartment + "', inpatientarea='" + ri.strInpatientAreaNum + "'";
                        strSql += ",ward='" + ri.strWard + "', sickbed = '" + ri.strSickBed + "', diagresult='" + ri.strDiagResult + "', dose='" + ri.strDose + "', takemethod='" + ri.strTakeMethod + "'";
                        strSql += ", takenum= " + ri.nNum + ", packagenum=" + ri.nPackageNum + ", decscheme='" + ri.strScheme + "', oncetime=" + ri.strTimeOne + ", twicetime=" + ri.strTimeTwo + "";
                        strSql += ", soakwater=" + ri.nSoakWater + ", soaktime=" + ri.nSoakTime + ", labelnum=" + ri.nLabelNum + ", remark='" + ri.strRemark + "' ,doctor ='" + ri.strDoctor + "'";
                        strSql += ", footnote='" + ri.strFootNote + "',getdrugtime='" + ri.strDrugGetTime + "', getdrugnum='" + ri.strDrugGetNum + "', ordertime='" + ri.strOrderTime + "', delnum='" + ri.strDelnum + "'";
                        strSql += ", dtbcompany='" + ri.strDtbCompany + "'";
                        strSql += ", dtbaddress='" + ri.strDtbAddress + "', dtbphone='" + ri.strDtbPhone + "',dtbtype ='" + ri.strDtbStyle + "', takeway='" + ri.strTakeWay + "', RemarksA='" + ri.strRemarksA + "', RemarksB='" + ri.strRemarksB + "' where id =" + id;

                        n = db.cmd_Execute(strSql);//更新成功
                    }
                //}
                //else
                //{
                //    n = 2;//未审核的处方不能修改
                //}
            }
            else
            {

                //if (curstate == "未匹配" || curstate == "接方" || curstate == "未审核" || curstate == "已审核")
                //{


                    string str = "select * from prescription where hospitalid ='" + ri.nHospitalID + "' and pspnum ='" + ri.strPspnum + "' and id not in ('" + id + "')";
                    SqlDataReader sdr2 = db.get_Reader(str);
                    if (sdr2.Read())
                    {
                        n = 3;//该处方号已存在
                    }
                    else
                    {
                        string strSql = "Update prescription set hospitalid ='" + ri.nHospitalID + "', pspnum='" + ri.strPspnum + "',decmothed='" + ri.strDecMothed + "',name ='" + ri.strName + "',  sex=" + ri.nSex + ",  age=" + ri.nAge;
                        strSql += ", phone='" + ri.strPhone + "' , address='" + ri.strAddress + "', department='" + ri.strDepartment + "', inpatientarea='" + ri.strInpatientAreaNum + "'";
                        strSql += ",ward='" + ri.strWard + "', sickbed = '" + ri.strSickBed + "', diagresult='" + ri.strDiagResult + "', dose='" + ri.strDose + "', takemethod='" + ri.strTakeMethod + "'";
                        strSql += ", takenum= " + ri.nNum + ", packagenum=" + ri.nPackageNum + ", decscheme='" + ri.strScheme + "', oncetime=" + ri.strTimeOne + ", twicetime=" + ri.strTimeTwo + "";
                        strSql += ", soakwater=" + ri.nSoakWater + ", soaktime=" + ri.nSoakTime + ", labelnum=" + ri.nLabelNum + ", remark='" + ri.strRemark + "' ,doctor ='" + ri.strDoctor + "'";
                        strSql += ", footnote='" + ri.strFootNote + "',getdrugtime='" + ri.strDrugGetTime + "', getdrugnum='" + ri.strDrugGetNum + "', ordertime='" + ri.strOrderTime + "', delnum='" + ri.strDelnum + "'";
                        strSql += ", dtbcompany='" + ri.strDtbCompany + "'";
                        strSql += ", dtbaddress='" + ri.strDtbAddress + "', dtbphone='" + ri.strDtbPhone + "',dtbtype ='" + ri.strDtbStyle + "', takeway='" + ri.strTakeWay + "', RemarksA='" + ri.strRemarksA + "', RemarksB='" + ri.strRemarksB + "' where id =" + id;

                        n = db.cmd_Execute(strSql);//更新成功
                    }
                //}
                //else
                //{
                //    n = 2;//未审核的处方不能修改
                //}


            }
            return n;
        }
        #endregion


        //调剂报警
        public string adjustwarning()
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql3 = "";
            sql3 = "select ID,Pspnum,customid,delnum,(select adjustwarning from warning where hospitalid = p.Hospitalid) as adjustwarning,(select doingtime from bubble where pid = p.id) as doingtime,(SELECT bubbleperson FROM bubble WHERE pid = p.id) as bp,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
            sql3 += "diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate";
            sql3 += " from prescription as p where id in (select prescriptionId from prescriptionCheckState where checkstatus =1)";

            SqlDataReader sr3 = db.get_Reader(sql3);//审核完成的所有信息

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间

            ArrayList list2 = new ArrayList();
            ArrayList list1 = new ArrayList();
            ArrayList list3 = new ArrayList();
            while (sr3.Read())
            {

                // sr3["bubblewarning"].ToString();
                // sr3["getdrugtime"].ToString();
                string d1 = sr3["adjustwarning"].ToString();//调剂警告时间

                list1.Add(d1);

                string drugtime = sr3["getdrugtime"].ToString();//得到该处方号的取药时间
                //  DateTime d2 = Convert.ToDateTime(sr3["getdrugtime"].ToString());
                list2.Add(drugtime);


                string id = sr3["ID"].ToString();//当前id煎药单号
                list3.Add(id);





            }
            for (int i = 0; i < list2.Count; i++)
            {



                string d1 = list1[i].ToString();//调剂警告时间

                DateTime d2 = Convert.ToDateTime(list2[i].ToString());//取药时间


                string strY = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime d3 = Convert.ToDateTime(strY);//当前时间



                TimeSpan d4 = d2.Subtract(d3);//取药时间- 当前时间



                //取药时间- 当前时间
                int time = Convert.ToInt32(d4.Days.ToString()) * 24 * 60 + Convert.ToInt32(d4.Hours.ToString()) * 60 + Convert.ToInt32(d4.Minutes.ToString());
                //泡药警告时间
                int time2 = Convert.ToInt32(d1);
                if (time < time2)
                {
                    string strsql1 = "update prescriptionCheckstate set warningstatus = 1 where prescriptionid = '" + list3[i] + "'";

                    db.cmd_Execute(strsql1);
                }
            }
            string str2 = "";
            string str = "select prescriptionid from bubble where checkstatus =1 and warningstatus =1";
            SqlDataReader sr = db.get_Reader(str);

            while (sr.Read())
            {
                str2 += sr["prescriptionid"].ToString() + ",";

            }
            return str2;
        }

        //打印数据
        public SqlDataReader print(int id)
        {
            string sql = "select * from prescription where id = '" + id + "' ";
            SqlDataReader sr3 = db.get_Reader(sql);

            return sr3;
        }
        //打印数据的药味
        public SqlDataReader findrug(int id)
        {
            string sql = "select count(pid) as p   from drug where  pid ='" + id + "'";
            SqlDataReader sr3 = db.get_Reader(sql);

            return sr3;
        }
        //打印数据的调剂重量
        public SqlDataReader findrug2(int id)
        {


            string sql = "select sum(drugweight) as pp   from drug where pid ='" + id + "'";
            SqlDataReader sr3 = db.get_Reader(sql);


            return sr3;
        }
        //打印数据的药品名称，说明
        public SqlDataReader findrug3(int id)
        {


            string sql = "select   *   from drug where pid ='" + id + "'";
            SqlDataReader sr3 = db.get_Reader(sql);


            return sr3;
        }
        //打印数据的货位
        public SqlDataReader findrug4(int id)
        {


            string sql = "select   ypcdrugPositionNum   from DrugMatching where pspId ='" + id + "'";
            SqlDataReader sr3 = db.get_Reader(sql);


            return sr3;
        }
        //更新打印状态

        public int printstatus(string id)
        {

            string sql = "update prescriptioncheckstate set printstatus = 1 where prescriptionid = '" + id + "'";

            int end = db.cmd_Execute(sql);


            return end;
        }
        //更新药品信息
        public int updatedruginfo(string id, string drugnum, string drugdescription, string drugposition, string drugweight, string description, string wholesaleprice, string wholesalecost, string moneywithtax, string drugname, string drugallnum, string tienum, string retailprice, string retailcost, string fee)
        {
            double dWholeSalePrice = 0;
            if (!string.IsNullOrEmpty(wholesaleprice))
            {
                dWholeSalePrice = Convert.ToDouble(wholesaleprice);
            }

            double dRetailPrice = 0;
            if (!string.IsNullOrEmpty(retailprice))
            {
                dRetailPrice = Convert.ToDouble(retailprice);
            }

            int end = 0;
            string strpspnum = "";
            string strhospitalid = "";
            string curstatus = "";
            string str2 = "select * from drug where id = '" + id + "'";
            SqlDataReader sdr1 = db.get_Reader(str2);
            if (sdr1.Read())
            {
                strpspnum = sdr1["pspnum"].ToString();
                strhospitalid = sdr1["hospitalid"].ToString();
            }

            string str3 = "select * from prescription where pspnum ='" + strpspnum + "' and hospitalid ='" + strhospitalid + "'";
            SqlDataReader sdr2 = db.get_Reader(str3);

            if (sdr2.Read())
            {
                curstatus = sdr2["curstate"].ToString();
            }

            RecipeModel rm = new RecipeModel();
            SqlDataReader sdr3 = rm.findisneedcheckstatus();
            string isneedcheck = "";
            if (sdr3.Read())
            {
                isneedcheck = sdr3["isneedcheck"].ToString();

            }

            if (isneedcheck == "0")
            {

                //if (curstatus == "未匹配" || curstatus == "接方" || curstatus == "未审核")
                //{

                    string str = "update drug set drugnum='" + drugnum + "',drugdescription='" + drugdescription + "',drugposition='" + drugposition + "',drugweight='" + drugweight + "',description='" + description + "',wholesaleprice='" + dWholeSalePrice + "', drugname='" + drugname + "', drugallnum='" + drugallnum + "', tienum='" + tienum + "',retailprice='" + dRetailPrice + "' where id='" + id + "'";
                    end = db.cmd_Execute(str);
                //}
                //else
                //{
                //    end = 2;
                //}
            }
            else
            {
                //if (curstatus == "未匹配" || curstatus == "接方" || curstatus == "未审核" || curstatus == "已审核")
                //{

                    string str = "update drug set drugnum='" + drugnum + "',drugdescription='" + drugdescription + "',drugposition='" + drugposition + "',drugweight='" + drugweight + "',description='" + description + "',wholesaleprice='" + dWholeSalePrice + "', drugname='" + drugname + "', drugallnum='" + drugallnum + "', tienum='" + tienum + "',retailprice='" + dRetailPrice + "' where id='" + id + "'";
                    end = db.cmd_Execute(str);
                //}
                //else
                //{
                //    end = 2;
                //}

            }
            return end;
        }


        public int AddQualitycheck(string qualitytime, string qualityman, string tisaneid, string pspweight, string actualweight, string deviation,
          string docase, string taste, string actualtaste, string matchman, string checkman, string remark, string ischeck, string tienum)
        {
            double b = Convert.ToDouble(deviation);
            double a = System.Math.Abs(b);
            double c = a / Convert.ToDouble(pspweight);

            double aa = Math.Round(c, 2);
            string deviationpercent = aa.ToString();
            int end = 0;
            //int da= Convert.ToInt64(tisaneid);
            string str = "select * from prescription where id =" + tisaneid + "";
            SqlDataReader sdr2 = db.get_Reader(str);
            if (sdr2.Read())
            {
                string strSql = "insert into qualitycheck(qualitytime,qualityman,tisaneid,pspweight,actualweight,deviation,deviationpercent,";
                strSql += "docase,taste,actualtaste,ischeck,matchman,checkman,tie,remark) ";
                strSql += "values('" + qualitytime + "','" + qualityman + "','" + tisaneid + "','" + pspweight + "',";
                strSql += "'" + actualweight + "','" + deviation + "','" + deviationpercent + "','" + docase + "',";
                strSql += "'" + taste + "','" + actualtaste + "','" + ischeck + "','" + matchman + "','" + checkman + "',";
                strSql += "'" + tienum + "','" + remark + "')";

                end = db.cmd_Execute(strSql);
            }
            else
            {
                end = 2;
            }
            return end;

        }



        //查询质检信息
        public DataTable findqualitycheckinfo(string pspstatus, string startTime, string endTime, string qualityman)
        {
            string str = "select *   from qualitycheck where 1=1";
            if (pspstatus != "0")
            {
                str += "and ischeck ='" + pspstatus + "'";
            }
            if (startTime != "0")
            {

                DateTime d = Convert.ToDateTime(startTime);
                string strE = d.ToString("yyyy/MM/dd  00:00:00");
                str += "and qualitytime >='" + strE + "'";
            }
            if (endTime != "0")
            {
                DateTime d = Convert.ToDateTime(endTime);
                string strE = d.ToString("yyyy/MM/dd  23:59:59");
                str += "and qualitytime <='" + strE + "'";
            }

            if (qualityman != "0")
            {
                str += "and qualityman ='" + qualityman + "'";
            }


            DataTable dt = db.get_DataTable(str);

            return dt;
        }



        //修改质检信息
        public int updatequalitycheckinfo()
        {
            return 0;
        }


        //找到质检信息通过id
        public DataTable findqualitycheckinfobyid(int id)
        {
            string str = "select * from qualitycheck where id ='" + id + "'";
            DataTable dt = db.get_DataTable(str);
            return dt;
        }





        //修改质检信息
        public int UpdateQualitycheck(string id, string qualitytime, string qualityman, string tisaneid, string pspweight, string actualweight,
           string docase, string taste, string actualtaste, string matchman, string checkman, string remark, string ischeck, string tienum)
        {
            double m = Convert.ToDouble(actualweight) - Convert.ToDouble(pspweight);
            string deviation = m.ToString();
            double b = Convert.ToDouble(deviation);
            double a = System.Math.Abs(b);
            double c = a / Convert.ToDouble(pspweight);
            string deviationpercent = c.ToString();

            int end = 0;
            string str = "select * from prescription where id ='" + tisaneid + "'";
            SqlDataReader sdr2 = db.get_Reader(str);
            if (sdr2.Read())
            {
                string strSql = "update qualitycheck set qualitytime ='" + qualitytime + "',qualityman='" + qualityman + "',tisaneid='" + tisaneid + "',pspweight='" + pspweight + "',actualweight='" + actualweight + "',deviation='" + deviation + "',deviationpercent='" + deviationpercent + "',";
                strSql += "docase='" + docase + "',taste='" + taste + "',actualtaste='" + actualtaste + "',ischeck='" + ischeck + "',matchman='" + matchman + "',checkman='" + checkman + "',tie='" + tienum + "',remark='" + remark + "' where id ='" + id + "'";

                end = db.cmd_Execute(strSql);
            }
            else
            {
                end = 2;
            }
            return end;

        }


        //调配条码号
        public string getadjustbarcode(int id)
        {

            //通过煎药单号查询煎药条码，下面id获取的是煎药单号
            string strSql = "select    (select    RIGHT(CAST('0' + RTRIM(dose *takenum  ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as packNum  ,"
               + "(select    RIGHT(CAST('0' + RTRIM(decscheme) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as DeScheme, "
               + "(select    RIGHT(CAST('000' + RTRIM( packagenum ) AS varchar(20)), 4)  as b   from prescription as p where  p.id  = " + id + ") as packAcount  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( oncetime) AS varchar(20) ), 2)  as b   from prescription as p where  p.id  = " + id + ")  as OTime  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( twicetime ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as TTime  ,"
                // + "(select    RIGHT(CAST('0' + RTRIM( machineid ) AS varchar(20)), 2)  as b   from tisaneinfo as t where  t.pid   = " + id + ") as hao  ,"

              + " RIGHT(CAST('000000000' + RTRIM(id) AS varchar(20)), 10)  as bNum from prescription where id ='" + id + "'";
            //包装袋数: packNum;  煎药方案: DeScheme;  煎药单号: bNum;   包装量:  packAcount ;一煎时间:OTime; 二煎时间:TTime;  煎药单分配的机组号: hao
            DataTable dt = db.get_DataTable(strSql);

            //string a = dt.Rows[0]["packNum"].ToString();
            //string b = dt.Rows[0]["DeScheme"].ToString();
            // string c = dt.Rows[0]["bNum"].ToString();
            // string y = dt.Rows[0]["hao"].ToString();
            // string d = dt.Rows[0]["packAcount"].ToString();
            //  string r = dt.Rows[0]["OTime"].ToString();
            //  string m = dt.Rows[0]["TTime"].ToString();
            //  string k = dt.Rows[0]["Warehouse"].ToString();


            return dt.Rows[0]["packNum"].ToString() + dt.Rows[0]["DeScheme"].ToString() + dt.Rows[0]["bNum"].ToString() + dt.Rows[0]["packAcount"].ToString() + dt.Rows[0]["OTime"].ToString() + dt.Rows[0]["TTime"].ToString() + "00";
        }




        //煎药条码号
        public string gettisanebarcode(int id)
        {

            //通过煎药单号查询煎药条码，下面id获取的是煎药单号
            string strSql = "select    (select    RIGHT(CAST('0' + RTRIM(dose *takenum  ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as packNum  ,"
               + "(select    RIGHT(CAST('0' + RTRIM(decscheme) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as DeScheme, "
               + "(select    RIGHT(CAST('000' + RTRIM( packagenum ) AS varchar(20)), 4)  as b   from prescription as p where  p.id  = " + id + ") as packAcount  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( oncetime) AS varchar(20) ), 2)  as b   from prescription as p where  p.id  = " + id + ")  as OTime  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( twicetime ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as TTime  ,"
                + "(select    RIGHT(CAST('0' + RTRIM((select unitnum from machine where id = t.machineid)) AS varchar(20)), 2)  as b   from tisaneunit as t where  t.pid   = " + id + ") as hao  ,"

              + " RIGHT(CAST('000000000' + RTRIM(id) AS varchar(20)), 10)  as bNum from prescription where id ='" + id + "'";
            //包装袋数: packNum;  煎药方案: DeScheme;  煎药单号: bNum;   包装量:  packAcount ;一煎时间:OTime; 二煎时间:TTime;  煎药单分配的机组号: hao
            DataTable dt = db.get_DataTable(strSql);

            //string a = dt.Rows[0]["packNum"].ToString();
            //string b = dt.Rows[0]["DeScheme"].ToString();
            // string c = dt.Rows[0]["bNum"].ToString();
            // string y = dt.Rows[0]["hao"].ToString();
            // string d = dt.Rows[0]["packAcount"].ToString();
            //  string r = dt.Rows[0]["OTime"].ToString();
            //  string m = dt.Rows[0]["TTime"].ToString();
            //  string k = dt.Rows[0]["Warehouse"].ToString();


            return dt.Rows[0]["packNum"].ToString() + dt.Rows[0]["DeScheme"].ToString() + dt.Rows[0]["bNum"].ToString() + dt.Rows[0]["packAcount"].ToString() + dt.Rows[0]["OTime"].ToString() + dt.Rows[0]["TTime"].ToString() + dt.Rows[0]["hao"].ToString();
        }

        public SqlDataReader findspecialdrug(string drugname)
        {

            string sql = "select * from specialdrug2 where drugname ='" + drugname + "'";

            SqlDataReader sdr = db.get_Reader(sql);
            return sdr;

        }


        public DataTable findspecialdrug2()
        {

            string sql = "select * from specialdrugname";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }

        public SqlDataReader finddrugnamebypid(string pid, string specialname)
        {

            string sql = "select * from drug where pid='" + pid + "' and drugname ='" + specialname + "'";

            SqlDataReader sdr = db.get_Reader(sql);
            return sdr;

        }



        public int isneedcheck(string id)
        {
            string str = "";
            int result = 0;
            if (id == "1")
            {
                str = "update isneedcheck set isneedcheck =0";
            }

            else
            {
                str = "update isneedcheck set isneedcheck =1";//不需要审核
            }
            result = db.cmd_Execute(str);

            return result;
        }

        public int isneedadjust(string id)
        {
            string str = "";
            int result = 0;
            if (id == "1")
            {
                str = "update isneedcheck set isneedadjust =0";
            }
            else
            {
                str = "update isneedcheck set isneedadjust =1";//不需要调剂
            }
            result = db.cmd_Execute(str);

            return result;
        }

        public int isneedreview(string id)
        {
            string str = "";
            int result = 0;
            if (id == "1")
            {
                str = "update isneedcheck set isneedreview =0";
            }

            else
            {
                str = "update isneedcheck set isneedreview =1";//不需要复核
            }
            result = db.cmd_Execute(str);

            return result;
        }




        public DataTable findmatchingdonepid()
        {

            string sql = "select pspid from drugmatching group by pspid";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
        public SqlDataReader findisneedcheckstatus()
        {

            string sql = "select isneedcheck from isneedcheck";

            SqlDataReader sdr = db.get_Reader(sql);
            return sdr;

        }
        //是否经过调剂
        public SqlDataReader findisneedadjuststatus()
        {

            string sql = "select isneedadjust from isneedcheck";

            SqlDataReader sdr = db.get_Reader(sql);
            return sdr;

        }
        //是否经过复核
        public SqlDataReader findisneedreviewstatus()
        {

            string sql = "select isneedreview from isneedcheck";

            SqlDataReader sdr = db.get_Reader(sql);
            return sdr;

        }

        public int changeprintstatus(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set printstatus='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus3(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strName='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }



        public int changeprintstatus4(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set nSex='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus5(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set nAge='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus6(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strSickBed='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }
        public int changeprintstatus7(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strHospitalName='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus8(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strPspnum='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus9(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strScheme='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus10(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strInpatientAreaNum='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus11(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strWard='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }


        public int changeprintstatus12(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strDepartment='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus13(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strDose='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus14(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set nNum='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus15(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set nPackageNum='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus16(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strDrugGetTime='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }
        public int changeprintstatus17(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strOrderTime='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }
        public int changeprintstatus18(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strTakeMethod='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }
        public int changeprintstatus19(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strTakeWay='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus20(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strDTCompany='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus21(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strRemark='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus22(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strRemarkA='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus23(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strRemarkB='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }

        public int changeprintstatus24(string id, string type)
        {
            int result = 0;
            string sql = "update deployment set strProductionDate='" + id + "' where bartype ='" + type + "'";

            result = db.cmd_Execute(sql);
            return result;

        }


        public string getstatus(string type)
        {
            string result = "";
            string sql = "select * from deployment where bartype ='" + type + "'";
            SqlDataReader sdr = db.get_Reader(sql);
            if (sdr.Read())
            {

                result = sdr["printstatus"].ToString() + "," + sdr["strName"].ToString() + "," + sdr["nSex"].ToString() + "," + sdr["nAge"].ToString() + "," + sdr["strSickBed"].ToString() + "," + sdr["strHospitalName"].ToString() + "," + sdr["strPspnum"].ToString() + "," + sdr["strScheme"].ToString()
                     + "," + sdr["strInpatientAreaNum"].ToString() + "," + sdr["strWard"].ToString() + "," + sdr["strDepartment"].ToString() + "," + sdr["strDose"].ToString() + "," + sdr["nNum"].ToString() + "," + sdr["nPackageNum"].ToString() + "," + sdr["strDrugGetTime"].ToString() + "," + sdr["strOrderTime"].ToString()
                      + "," + sdr["strTakeMethod"].ToString() + "," + sdr["strTakeWay"].ToString() + "," + sdr["strDTCompany"].ToString() + "," + sdr["strRemark"].ToString() + "," + sdr["strRemarkA"].ToString() + "," + sdr["strRemarkB"].ToString() + "," + sdr["strProductionDate"].ToString();

            }
            return result;
        }

        #region 获取各个环节预警信息

        public string getRetentionWarning(int flag)
        {
            string sql = "";
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            //调剂预警
            if (flag == 1)
            {
                /*sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN PrescriptionCheckState AS a ON p.ID = a.prescriptionId INNER JOIN"
                + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.PartyTime, "
                + " GETDATE()) > w.adjustwarning LEFT OUTER JOIN adjust AS ad ON p.ID = ad.prescriptionId"
                + " WHERE   (CONVERT(varchar, a.PartyTime, 120) LIKE '%" + nowDate + "%') AND (a.checkStatus = 1) AND (ad.prescriptionId IS NULL)"
                + " ORDER BY DATEDIFF(minute, a.PartyTime, GETDATE()) DESC";*/
                sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN PrescriptionCheckState AS a ON p.ID = a.prescriptionId INNER JOIN"
               + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 LEFT OUTER JOIN adjust AS ad ON p.ID = ad.prescriptionId"
               + " WHERE   (a.checkStatus = 1) and p.dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' "
               + " ORDER BY DATEDIFF(minute, a.PartyTime, GETDATE()) DESC";
            }
            else
                if (flag == 2)
                {//复核预警
                    sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN adjust AS a ON p.ID = a.prescriptionId INNER JOIN"
                    + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.endDate, "
                    + " GETDATE()) > w.recheckwarning LEFT OUTER JOIN Audit AS au ON au.pid = p.id"
                    + " WHERE   (a.status = 1) AND (CONVERT(varchar, a.wordDate, 120) LIKE '%" + nowDate + "%') AND (au.pid IS NULL)"
                    + " ORDER BY DATEDIFF(minute, a.endDate, GETDATE()) DESC";

                }

                else
                    if (flag == 3)
                    {//泡药预警
                        /*     sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN Audit AS a ON p.ID = a.pid INNER JOIN"
                             + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.AuditTime, "
                             + " GETDATE()) > w.bubblewarning LEFT OUTER JOIN bubble AS b ON b.pid = p.ID"
                             + " WHERE   (CONVERT(varchar, a.AuditTime, 120) LIKE '%" + nowDate + "%') AND (a.AuditStatus = 1) AND (b.pid IS NULL)"
                             + " ORDER BY DATEDIFF(minute, a.AuditTime, GETDATE()) DESC";
                         */
                        sql = "SELECT p.ID FROM prescription AS p INNER JOIN bubble AS b ON p.ID = b.pid AND DATEDIFF(minute, b.starttime, GETDATE()) >= p.soaktime WHERE   (b.bubblestatus = 0) AND (CONVERT(varchar, p.dotime, 120) LIKE '%" + nowDate + "%') ORDER BY p.ID DESC";
                        // db.write_log_txt("泡药预警SQL:----------");
                   //     db.write_log_txt("泡药预警SQL:" + sql);
                        //   db.write_log_txt("泡药预警SQL:--------");
                    }
                    else
                        if (flag == 4)
                        {//煎药预警
                            sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN bubble AS a ON p.ID = a.pid INNER JOIN"
                            + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.endDate, "
                            + " GETDATE()) > w.tisanewarning LEFT OUTER JOIN tisaneinfo AS t ON t.pid = p.ID"
                            + " WHERE   (CONVERT(varchar, a.starttime, 120) LIKE '%" + nowDate + "%') AND (a.bubblestatus = 1) AND (t.pid IS NULL)"
                            + " ORDER BY DATEDIFF(minute, a.endDate, GETDATE()) DESC";
                            db.write_log_txt("煎药预警：" + sql);
                        }
                        else
                            if (flag == 5)
                            {//包装预警
                                sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN tisaneinfo AS a ON p.ID = a.pid INNER JOIN"
                                + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.endDate, "
                                + " GETDATE()) > w.packwarning LEFT OUTER JOIN Packing AS pk ON pk.DecoctingNum = p.ID"
                                + " WHERE   (CONVERT(varchar, a.starttime, 120) LIKE '%" + nowDate + "%') AND (a.tisanestatus = 1) AND (pk.DecoctingNum IS NULL)"
                                + " ORDER BY DATEDIFF(minute, a.starttime, GETDATE()) DESC";

                            }
                            else
                                if (flag == 6)
                                {//发货预警
                                    sql += "SELECT   p.ID FROM      prescription AS p INNER JOIN Packing AS a ON p.ID = a.DecoctingNum INNER JOIN"
                                    + " warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.PacTime, "
                                    + " GETDATE()) > w.deliverwarning LEFT OUTER JOIN Delivery AS d ON d.DecoctingNum = p.ID"
                                    + " WHERE   (CONVERT(varchar, a.Starttime, 120) LIKE '%" + nowDate + "%') AND (a.Fpactate = 1) AND (d.DecoctingNum IS NULL)"
                                    + " ORDER BY DATEDIFF(minute, a.Starttime, GETDATE()) DESC";
                                    db.write_log_txt("发货预警：" + sql);
                                }
            /*    else
                if (flag == 7)
                {
                    sql += "SELECT p.ID FROM prescription AS p INNER JOIN Delivery AS a ON p.ID = a.DecoctingNum INNER JOIN"
                    +" warning AS w ON w.hospitalid = p.Hospitalid AND w.type = 1 AND w.status = 1 AND DATEDIFF(minute, a.SendTime, "
                    + " GETDATE()) > w.deliverwarning WHERE (CONVERT(varchar, a.SendTime, 120) LIKE '%" + nowDate + "%') AND (a.Sendstate = 1)";

                }*/
            string str = "";
            if (sql.Length > 0)
            {
                db.write_log_txt("预警:" + sql);
                SqlDataReader sr = db.get_Reader(sql);

                while (sr.Read())
                {

                    str += sr["ID"].ToString() + ",";

                }

            }



            return str;

        }
        #endregion

        public DataTable getprintstatus()
        {
            string str = "select * from deployment";

            DataTable sdr = db.get_DataTable(str);

            return sdr;
        }

        public SqlDataReader getprintstatusbytype(string type)
        {
            string str = "select * from deployment where bartype ='" + type + "'";

            SqlDataReader sdr = db.get_Reader(str);

            return sdr;
        }

        /// <summary>
        /// 是否自动打印调配单和调配单条形码
        /// </summary>
        /// <returns></returns>
        public string autoPrint()
        {
            string str = "select printstatus  from deployment where bartype in (1,2)  order by id  asc ";
            DataTable dt = db.get_DataTable(str);
            return dt.Rows[0][0].ToString() + "," + dt.Rows[1][0].ToString();
        }

        public DataTable findLogisticsKeyById(int id)
        {
            string sql = "select * from logisticsKey where id=" + id;
            DataTable sdr = db.get_DataTable(sql);

            return sdr;
        }

        public int updateLogisticsKey(int id, string key)
        {
            int result = 0;
            string sql = "update logisticsKey set logisticsKey='" + key + "' where id ='" + id + "'";

            result = db.cmd_Execute(sql);
            return result;
        }
        public DataTable findPrintOnOffById(int id)
        {
            string sql = "select * from printOnOff where id=" + id;
            DataTable sdr = db.get_DataTable(sql);

            return sdr;
        }

        public int updatePrintOnOff(int id, string onOff)
        {
            int result = 0;
            string sql = "update printOnOff set onOff='" + onOff + "' where id ='" + id + "'";

            result = db.cmd_Execute(sql);
            return result;
        }
        public DataTable findprintstatusbytype(string type)
        {
            string str = "select * from deployment where bartype ='" + type + "'";
            return db.get_DataTable(str);
        }
        //根据泡药开关开启状态结束泡药
        public string update_soak_status_by_onoff(string pid)
        {
            string flag_on_off = "";
            int result = 0;
            string sql = "select top 1 global_medi_is_flag from  dbo.tb_sys_add_setting where  sys_add_id='1'";
            SqlDataReader sdr = db.get_Reader(sql);
            if (sdr.Read())
            {
                flag_on_off = sdr["global_medi_is_flag"].ToString().Trim();
            }
            if (flag_on_off == "0") //强制泡药开关关闭状态
            {
                string s1 = "update prescription set curstate ='泡药完成' where id='" + pid + "' and curstate='开始泡药'";
                string s2 = " update bubble set bubblestatus ='1',endDate =GETDATE()  where pid='" + pid + "'";
                db.write_log_txt("强制泡药开关关闭，煎药环节修改泡药状态：" + s1 + "___" + s2);
                result = db.cmd_Execute(s1);
                result += db.cmd_Execute(s2);
                if (result >= 2)
                {
                    result = 1;
                }
            }

            return result.ToString().Trim();
        }
        //对煎药单号状态的判断，如果煎药单号还处于泡药完成状态，则将其置为“开始煎药状态”
        public string update_pnum_status(string pid, string status_b, string status_a)
        {
            string flag_on_off = "";
            int result = 0;
            string sql = "select top 1 curstate from prescription  where  id='" + pid + "'";
            SqlDataReader sdr = db.get_Reader(sql);
            if (sdr.Read())
            {
                flag_on_off = sdr["curstate"].ToString().Trim();
            }
            if (flag_on_off == status_b) //强制泡药开关关闭状态
            {
                string s1 = "update prescription set curstate ='" + status_a + "'" + "where id='" + pid + "' and curstate='" + status_b + "'";

                result = db.cmd_Execute(s1);

                if (result >= 1)
                {
                    result = 1;
                }
            }

            return result.ToString().Trim();
        }
        #region 解放查询中处方插入药品后确认
        public int update_pnum_config(string pspnum)
        {            
            int result = 0;
            string s1 = "update prescription set confirmDrug='1' where pspnum='" + pspnum.Trim() + "'";
                result = db.cmd_Execute(s1);                   
            return result;
        }
        #endregion
    }

}