using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;

 namespace ModelInfo
{
     public class LossiModel
     {
         public DataBaseLayer db = new DataBaseLayer();
         #region 查询仓库编号
         public SqlDataReader findWarehouseNum(string Warehouse)
         {

             string sql = "select WareNum from  Warehousepharmacy where WName = '" + Warehouse + "'";

             SqlDataReader sdr = db.get_Reader(sql);

             return sdr;
         }
         public SqlDataReader findDeugName(string Warehouse)
         {
             SqlDataReader end = null;
             string sql = "select DrugName from  DrugAdmin where id in (select drugadminid  from  Storage  where Warehouse= '" + Warehouse + "')";
             SqlDataReader sdr = db.get_Reader(sql);
             if (sdr.Read()) {
                 end = sdr;
             } else {
                 string sql1 = "select DrugName from  DrugAdmin where id in (select drugadminid  from  medicalstorage  where Warehouse= '" + Warehouse + "')";
                 SqlDataReader sdr1 = db.get_Reader(sql1);
                 if (sdr1.Read()) {
                     end = sdr1;
                 } else {
                     end = null;
                 }
             }
             return end ;
         }
         public SqlDataReader findDeugNameInfo()
         {

             string sql = "select DrugName from  DrugAdmin  ";
             SqlDataReader sdr = db.get_Reader(sql);
             
             return sdr;
         }
        #endregion
         #region 查询药品编号
         public SqlDataReader findDeugNameNum(string DeugName)
         {

             string sql = "select DrugCode from  DrugAdmin where DrugName = '" + DeugName + "'";

             SqlDataReader sdr = db.get_Reader(sql);

             return sdr;
         }

         #endregion
         #region 添加报损信息库房

         public int LossiInforAdd(string fromid, string drugnum, string losstype, string Per, string Reason, string Rmarkes, string lossnum)
         {
             DataBaseLayer db = new DataBaseLayer();
             String strSql = "";
             int end = 0;
             System.DateTime currentTime = new System.DateTime();
             currentTime = System.DateTime.Now;//获取当前时间

            
              strSql = "insert into LossiInfor(Warehouse,Type,productbatch,Per,Reason,remark,time,lossnum) ";
              strSql += "values ('" + fromid + "','" + losstype + "','" + drugnum + "','" + Per + "','" + Reason + "','" + Rmarkes + "','" + currentTime + "','" + lossnum + "')";
              end = db.cmd_Execute(strSql);


             return end;

         }
         #endregion
         #region 添加报损信息药房

         public int GetMedicineLossiInfo(string fromid, string drugnum, string losstype, string Per, string Reason, string Rmarkes, string lossnum)
         {

             DataBaseLayer db = new DataBaseLayer();
             String strSql = "";
             int end = 0;
             System.DateTime currentTime = new System.DateTime();
             currentTime = System.DateTime.Now;//获取当前时间


             strSql = "insert into LossiInfomedical(Warehouse,Type,productbatch,Per,Reason,remark,time,lossnum) ";
             strSql += "values ('" + fromid + "','" + losstype + "','" + drugnum + "','" + Per + "','" + Reason + "','" + Rmarkes + "','" + currentTime + "','" + lossnum + "')";
             end = db.cmd_Execute(strSql);



             return end;

         }
         #endregion
         #region 查询报损信息库房
         public DataTable finLossiInfor(string Type)
         {
             string strSQL = "select id,Warehouse,productbatch,(select drugname from drugadmin where productbatch = l.productbatch) as dname,(select drugcode from drugadmin where productbatch = l.productbatch) as dcode,Type,lossnum,Per,time,Reason,remark   from LossiInfor as l where 1=1 ";

             if (Type != "0" && Type!="")
             {

                 strSQL += "and Type  ='" + Type + "'";

             }
             strSQL += "order by time desc";
             
             DataTable dt = db.get_DataTable(strSQL);

             return dt;
         }
         #endregion
         #region 查询报损信息药房
         public DataTable finMedicineLossiInfor(string Type)
         {
           //  string strSQL = "select *,(select drugname from drugadmin where productbatch = l.productbatch) as dname,(select drugcode from drugadmin where productbatch = l.productbatch) as dcode   from LossiInfomedical  as l where 1=1 ";

             string strSQL = "select id,Warehouse,productbatch,(select drugname from drugadmin where productbatch = l.productbatch) as dname,(select drugcode from drugadmin where productbatch = l.productbatch) as dcode,Type,lossnum,Per,time,Reason,remark   from lossiinfomedical as l where 1=1 ";
             strSQL += "order by time desc";


             if (Type != "0" && Type != "")
             {

                 strSQL += "and Type  ='" + Type + "'";

             }

             DataTable dt = db.get_DataTable(strSQL);

             return dt;
         }
         #endregion
     }
}
