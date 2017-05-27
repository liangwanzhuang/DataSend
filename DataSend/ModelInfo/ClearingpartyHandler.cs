using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;
using System.Data.SqlClient;

namespace ModelInfo
{
    public class ClearingpartyHandler
    {
        public DataBaseLayer db = new DataBaseLayer();
        public int AddClearingparty(string ClearPName, string ConPerson, string Address, string ConPhone,  string Remarks, string GenDecoct)
        {

            /// <summary>
            /// 添加结算方信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>
               int end = 0;
              DataBaseLayer db = new DataBaseLayer();

              string str = "select ClearPName from Clearingparty where ClearPName = '" + ClearPName + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {

                end = 0;
            }
            else
            {
                string str1 = "select ClearPName from Clearingparty where ConPerson = '" + ConPerson + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read()) {
                    end = 0;
                }
                else
                {
                    string strSql = "insert into Clearingparty(ClearPName,ConPerson,ConPhone,Address,Remarks,GenDecoct) ";
                    strSql += "values ('" + ClearPName + "','" + ConPerson + "','" + ConPhone + "',";
                    strSql += "'" + Address + "','" + Remarks + "','" + GenDecoct + "')";
                    end = db.cmd_Execute(strSql);
                }
            }

           

           
                return end;
            

        }
        #region 添加对账信息
        public int AddReconciliation(string Clearing, string ReconciliaT, string ReconciliaPer, string Remarks, string nPId ,string now,string Retime)
        {
            String strSql = "";
            int end = 0;
            DataBaseLayer db = new DataBaseLayer();

            string sum = "";//处方下药品的零售价之和
            string cheng = "";//处方（次数*贴数）之积
            //计算处方下药品的零售价之和
           // string sq3 = "  SELECT   SUM(cast(retailprice as float(2))) AS a   FROM      drug AS d   WHERE   (pid ='" + nPId + "')";

            //计算处方下匹配药品的零售价之和
            string sq3 = "SELECT SUM(CAST(ad.Univalent AS float(2))) AS a FROM drug AS d INNER JOIN "
                + " ypcDrug AS ypc ON ypc.drugName = d.drugname AND ypc.drugNum = d.drugnum INNER JOIN "
                + " DrugAdmin AS ad ON ad.DrugName = ypc.drugDetailedName AND ad.DrugCode = ypc.drugAlias "
                + " WHERE (d.pid = '" + nPId + "')";

            SqlDataReader srd = db.get_Reader(sq3);
            if (srd.Read())
            {
                sum = srd["a"].ToString();
                //计算处方（次数*贴数）之积
                string sq4 = " SELECT   dose * takenum AS m   FROM      prescription AS p WHERE   (ID = '" + nPId + "')";
                SqlDataReader srd1 = db.get_Reader(sq4);
                if (srd1.Read())
                {
                    cheng = srd1["m"].ToString();

                }
            }
                    // Clearing:结算方 CheckNum:对账单号 ReconciliaPer：对账人  ReconciliaT:对账时间  now：生成对账单时间 State：对账单状态 0 未结算 1结算
            strSql = "insert into Reconciliation(Clearing,CheckNum,ReconciliaPer,ReconciliaT,now,State,Remarks,pid,drugMonSum,shengRe) ";
                    strSql += "values ('" + Clearing + "','" + Retime + "','" + ReconciliaPer + "','" + ReconciliaT + "','" + now + "','0','" + Remarks + "','" + nPId + "','" + sum + "','" + cheng + "')";
           
           


            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
            }
            return end;

        }
        #endregion
        #region 添加对账信息
        public int AddReconciliation1(string Clearing, string ReconciliaT, string ReconciliaPer, string Remarks, string nPId, string now, string Retime)
        {
            String strSql = "";
            int end = 0;

            string sum = "";//处方下药品的零售价之和
            string cheng = "";//处方（次数*贴数）之积
              //计算处方下药品的零售价之和
            //   string sq3 = "  SELECT   SUM(cast(retailprice as float(2))) AS a   FROM      drug AS d   WHERE   (pid ='" + nPId + "')";

             //计算处方下匹配药品的零售价之和
            string sq3 = "SELECT SUM(CAST(ad.Univalent AS float(2))) AS a FROM drug AS d INNER JOIN " 
                +" ypcDrug AS ypc ON ypc.drugName = d.drugname AND ypc.drugNum = d.drugnum INNER JOIN " 
                +" DrugAdmin AS ad ON ad.DrugName = ypc.drugDetailedName AND ad.DrugCode = ypc.drugAlias " 
                +" WHERE (d.pid = '" + nPId + "')";
               SqlDataReader srd = db.get_Reader(sq3);
               if (srd.Read())
               {
                   sum = srd["a"].ToString();
                   //计算处方（次数*贴数）之积
                   string sq4 = " SELECT   dose * takenum AS m   FROM      prescription AS p WHERE   (ID = '" + nPId + "')";
                   SqlDataReader srd1 = db.get_Reader(sq4);
                   if (srd1.Read()) {
                       cheng = srd1["m"].ToString();
                   
                   }
               }
               strSql = "select pid from Reconciliation where pid=" + nPId;
            DataTable dt = db.get_DataTable(strSql);
            if (dt.Rows.Count == 0)
            {
                // Clearing:结算方 CheckNum:对账单号 ReconciliaPer：对账人  ReconciliaT:对账时间  now：生成对账单时间 State：对账单状态 0 未结算 1结算 drugMonSum:处方下药品的零售价之和 shengRe:处方（次数*贴数）之积
                strSql = "insert into Reconciliation(Clearing,CheckNum,ReconciliaPer,ReconciliaT,now,State,Remarks,pid,drugMonSum,shengRe) ";
                strSql += "values ('" + Clearing + "','" + Retime + "','" + ReconciliaPer + "','" + ReconciliaT + "','" + now + "','0','" + Remarks + "','" + nPId + "','" + sum + "','" + cheng + "')";
                if (strSql == "")
                {
                    end = 0;
                }
                else
                {
                    end = db.cmd_Execute(strSql);

                    //  string sq3 = "SELECT   CheckNum,   (SELECT   SUM(retailprice) AS Expr1   FROM      drug AS d   WHERE   (pid ='" + nPId + "')) AS b,  (SELECT   dose * takenum AS m"
                    // + "  FROM      prescription AS p WHERE   (ID = '" + nPId + "')) AS a  FROM      Reconciliation AS r";
                }
            }
              
            return end;

        }
        #endregion
         /// <summary>
         /// 结算方信息
         /// </summary>
         /// <param name="einfo"></param>
         /// <returns></returns>
         public DataTable SearchClearingparty()
         {
             string strSql = "select id,ClearPName,ConPerson,ConPhone,Address,PerSetInformation,Remarks from  Clearingparty  ";


             DataBaseLayer db = new DataBaseLayer();
             DataTable dt = db.get_DataTable(strSql);
             return dt;
         }
         public DataTable SearchInfo(string aPid)
         {
             DataBaseLayer db = new DataBaseLayer();
             string strSql = "select r.id , r.Clearing,r.CheckNum,r.ReconciliaPer,r.ReconciliaT,r.now,r.State,r.Remarks,p.dotime,(select distinct hname from hospital as h where h.id = p.hospitalid and h.id in (select hospitalid from prescription where p.id = r.pid )) as hname,p.Pspnum,p.decscheme,p.name,p.dose,p.takenum,p.packagenum, (select count(pid)  from drug as s where  s.pid =p.id and s.pid=r.pid ) as DrugAcount"
             + " from prescription as p join Reconciliation as r on p.id = r.pid where 1=1";
             string[] strRows1Id1 = aPid.Split(',');
             if (strRows1Id1.Length == 1)
             {
                 strSql += " and r.pid = " + strRows1Id1[0] + "";
             }
             else
             {
                 for (int i = 0; i < strRows1Id1.Length; i++)
                 {
                     if (i == 0)
                     {
                         strSql += " and r.pid in(" + strRows1Id1[i] + ",";
                     }
                     else if (i == strRows1Id1.Length - 1)
                     {
                         strSql += strRows1Id1[i] + ")";
                     }
                     else
                     {
                         strSql += strRows1Id1[i] + ",";
                     }
                 }
             }
             
          
             DataTable dt = db.get_DataTable(strSql);
             return dt;
         }
         public DataTable SearchInfoabc(string[] aPid)
         {
             DataBaseLayer db = new DataBaseLayer();
             string strSql = "select r.id , r.Clearing,r.CheckNum,r.ReconciliaPer,r.ReconciliaT,r.now,r.State,r.Remarks,p.dotime,(select distinct hname from hospital as h where h.id = p.hospitalid and h.id in (select hospitalid from prescription where p.id = r.pid )) as hname,p.Pspnum,p.decscheme,p.name,p.dose,p.takenum,p.packagenum, (select count(pid)  from drug as s where  s.pid =p.id and s.pid=r.pid ) as DrugAcount"
             + " from prescription as p join Reconciliation as r on p.id = r.pid where 1=1";

            /* for (int i = 0; i < aPid.Length; i++)
             {
                 strSql += " or r.pid = " + aPid[i] + "";

             }*/
             if (aPid.Length == 1)
             {
                 strSql += " and r.pid = " + aPid[0] + "";
             }
             else
             {
                 for (int i = 0; i < aPid.Length; i++)
                 {
                     if (i == 0)
                     {
                         strSql += " and r.pid in(" + aPid[i] + ",";
                     }
                     else if (i == aPid.Length - 1)
                     {
                         strSql += aPid[i] + ")";
                     }
                     else
                     {
                         strSql += aPid[i] + ",";
                     }
                 }
             }

             DataTable dt = db.get_DataTable(strSql);
             return dt;
         }
         public SqlDataReader findClearingpartyAll()
         {
             string sql = "select * from Hospital";

             return db.get_Reader(sql);
         }
        #region 通过id查询对账人
         public DataTable findInfo(string id)
         {
             string strSql = "select ReconciliaPer from Reconciliation where CheckNum = '" + id + "'";

             DataTable dt = db.get_DataTable(strSql);

             return dt;
         }
        #endregion
         #region 通过查询对账人
         public SqlDataReader findInfo()
         {
             string strSql = "select ReconciliaPer from Reconciliation group by ReconciliaPer ";
             return db.get_Reader(strSql);
         }
         #endregion
         #region 设置对账信息
         public int update(string id, string sPer)
         {

             int end = 0;
             
             string sql = "";
             string str = "select State from Reconciliation where CheckNum = '" + id + "'";
             SqlDataReader sr = db.get_Reader(str);
             if (sr.Read())
             {
                 if (sr["State"].ToString() == "0")
                 {
                     sql = "update Reconciliation set  State = '1',ReconciliaPer='" + sPer + "'where CheckNum ='" + id + "' ";
                 end = db.cmd_Execute(sql);
                 }else{
                     sql = "update Reconciliation set  State = '0',ReconciliaPer='" + sPer + "'where CheckNum ='" + id + "' ";
                     end = db.cmd_Execute(sql);
             
             }
             }
             else
             {
                 end = 0;
             }


             return end;
         }
         #endregion
    }
}

