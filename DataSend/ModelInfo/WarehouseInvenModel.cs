using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
    public class WarehouseInvenModel
    {
        public DataBaseLayer db = new DataBaseLayer();
        //添加库房盘点信息
        public int AddWarehouseInven(string fromid, string drugnum, string InventoryPer, string ActualCapacity, string InventoryStatus, string StorageCondition, string Rmarkes)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间
            System.DateTime nowtime = new System.DateTime();
            nowtime = System.DateTime.Now;
            string t = nowtime.ToLongTimeString().ToString();

            string date = currentTime.ToString("yyyy/MM/dd");
            string time = t;



            strSql = "insert into WarehouseInven(Warehouse,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,date,remark,productbatch) ";
            strSql += "values ('" + fromid + "','" + InventoryPer + "','" + ActualCapacity + "','" + InventoryStatus + "','" + StorageCondition + "','" + time + "','" + date + "','" + Rmarkes + "','" + drugnum + "')";



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
        #region 添加药房盘点信息
        public int AddMedicineWarehouseInven(string fromid, string drugnum, string InventoryPer, string ActualCapacity, string InventoryStatus, string StorageCondition, string Rmarkes)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间
            System.DateTime nowtime = new System.DateTime();
            nowtime = System.DateTime.Now;
            string t = nowtime.ToLongTimeString().ToString();

            string date = currentTime.ToString("yyyy/MM/dd");
            string time = t;



            strSql = "insert into WarehouseInvenmedical(Warehouse,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,date,remark,productbatch) ";
            strSql += "values ('" + fromid + "','" + InventoryPer + "','" + ActualCapacity + "','" + InventoryStatus + "','" + StorageCondition + "','" + time + "','" + date + "','" + Rmarkes + "','" + drugnum + "')";



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
        //查询库房盘点信息
        public DataTable finWarehouseInvenInfor(string Warehousing, string LSTime, string LETime, string drugname)
        {

            string strSQL = "select id,Warehouse,productbatch,(select drugname from drugadmin where productbatch = w.productbatch) as drugname,(select drugcode from drugadmin where productbatch = w.productbatch) as drugcode,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,CONVERT(varchar(100), CONVERT(datetime, date, 101), 23) AS date,remark,"
                + "isnull((select sum(cast(amount as int))  from storage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse),0) as iamount,isnull((select sum(cast(amount as int))  from storagefrom where  drugadminid = w.productbatch and fromroom= w.warehouse),0) as famount,isnull((select sum(cast(lossnum as int))  from lossiInfor where  productbatch = w.productbatch and warehouse= w.warehouse),0) as lossnum,"
              + "(isnull(cast((select sum(cast(amount as int))  from storage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as int),0)-isnull(cast((select sum(cast(amount as int))  from storagefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as int),0)-isnull((select sum(cast(lossnum as int))  from lossiInfor where  productbatch = w.productbatch and warehouse= w.warehouse),0)) as kucun from WarehouseInven as w where 1=1";

            if (LSTime != "" && LSTime != "0")
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd");
                strSQL += "and date >='" + strS + "'";

            }


            if (LETime != "" && LETime != " 0")
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd");
                strSQL += "and date  <='" + strE + "'";

            }

            if (drugname != "" && drugname != "0")
            {
                strSQL += "and productbatch in ( select productbatch from drugadmin where drugname ='" + drugname + "')";

            }
            if (Warehousing != "" && Warehousing != "0")
            {
                strSQL += "and Warehouse ='" + Warehousing + "'";

            }
            strSQL += "order by id asc";


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        //查询调配单信息库房

        public DataTable finTransferInfor(string LSTime, string LETime)
        {
            string strSQL = "";
            strSQL = "SELECT  id , Amount,fromroom ,StorageTime,(select DrugSpecificat  from DrugAdmin as d where d.ProductBatch = s.drugadminid )as DrugSpecificat "
                 + "FROM  storagefrom as s where 1=1 ";



            if (LSTime != "" && LSTime != "0")
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (LETime != "" && LETime != " 0")
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        //查询挑拨单信息药房

        public DataTable finMedicineTransferInfor(string Type, string LSTime, string LETime)
        {
            string strSQL = "";


            strSQL = " SELECT   id , Amount,storageroom, StorageTime,(select DrugSpecificat  from DrugAdmin as d where d.ProductBatch = s.drugadminid )as DrugSpecificat   "


               + "FROM   medicalstoragefrom AS s where 1=1 ";
            if (LSTime != "" && LSTime != "0")
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (LETime != "" && LETime != " 0")
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #region 查询药房盘点信息
        //查询盘点信息
        public DataTable finMedicineInventoryInfor(string Warehousing, string LSTime, string LETime, string drugname)
        {

            // string strSQL = "select id,Warehouse,productbatch,(select drugname from drugadmin where productbatch = w.productbatch) as drugname,(select drugcode from drugadmin where productbatch = w.productbatch) as drugcode,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,date,remark,"
            //   + "(select sum(cast(amount as int))  from medicalstorage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as iamount,(select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as famount,"
            // + "(isnull(cast((select sum(cast(amount as int))  from medicalstorage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as int),0)-isnull(cast((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as int),0)) as kucun"

            //+ " from WarehouseInvenmedical as w where 1=1 ";


            string strSQL = "select id,Warehouse,productbatch,(select drugname from drugadmin where productbatch = w.productbatch) as drugname,(select drugcode from drugadmin where productbatch = w.productbatch) as drugcode,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,CONVERT(varchar(100), CONVERT(datetime, date, 101), 23) AS date,remark,"
               + "isnull((select sum(cast(amount as int))  from medicalstorage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse),0) as iamount,isnull((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse),0) as famount,isnull((select sum(cast(lossnum as int))  from lossiInfomedical where  productbatch = w.productbatch and warehouse= w.warehouse),0) as lossnum,"
             + "(isnull(cast((select sum(cast(amount as int))  from medicalstorage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as int),0)-isnull(cast((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as int),0)-isnull((select sum(cast(lossnum as int))  from lossiInfomedical where  productbatch = w.productbatch and warehouse= w.warehouse),0)) as kucun,"
             + " (SELECT   COUNT(*) AS Expr1 FROM ypcDrug WHERE (drugAlias=(SELECT DrugCode FROM DrugAdmin AS DrugAdmin_4 WHERE (ProductBatch = w.productbatch))) AND (drugDetailedName=(SELECT DrugName FROM DrugAdmin AS DrugAdmin_5 WHERE (ProductBatch = w.productbatch)))) AS consume"
             + " from WarehouseInvenmedical as w  where 1=1 ";




            if (LSTime != "" && LSTime != "0")
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd");
                strSQL += "and date >='" + strS + "'";

            }


            if (LETime != "" && LETime != " 0")
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd");
                strSQL += "and date  <='" + strE + "'";

            }

            if (drugname != "" && drugname != "0")
            {
                strSQL += "and productbatch in ( select productbatch from drugadmin where drugname ='" + drugname + "')";

            }
            if (Warehousing != "" && Warehousing != "0")
            {
                strSQL += "and Warehouse ='" + Warehousing + "'";

            }

            strSQL += "order by date desc,time desc";


            DataTable dt = db.get_DataTable(strSQL);

            return dt;


        }
        #endregion
        #region 通过id查询库房盘点信息
        public DataTable findWarehouseInvenInfo(int id)
        {
            string strSql = "select * from  WarehouseInven where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable findWarehouseInvenmedicineInfo(int id)
        {
            string strSql = "select * from WarehouseInvenmedical where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }

        #endregion
        #region 编辑库房盘点信息
        public int updateWarehouseInvenInfo(int id, string Warehouse, string InventoryPer, string ActualCapacity, string InventoryStatus, string StorageCondition, string remark)
        {

            int end = 0;

            string sql = "";
            string str = "select Warehouse from WarehouseInven where id = " + id + "  ";
            SqlDataReader sr = db.get_Reader(str);
            if (!sr.Read())
            {
                end = 0;
            }
            else
            {
                sql = "update WarehouseInven set Warehouse='" + Warehouse + "',InventoryPer='" + InventoryPer + "',ActualCapacity='" + ActualCapacity + "',InventoryStatus='" + InventoryStatus + "',StorageCondition='" + StorageCondition + "',remark='" + remark + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion
        #region 编辑药房盘点信息
        public int updateMedicineWarehouseInvenInfo(int id, string Warehouse, string InventoryPer, string ActualCapacity, string InventoryStatus, string StorageCondition, string remark)
        {

            int end = 0;

            string sql = "";
            string str = "select Warehouse from WarehouseInven where id != " + id + " ";
            SqlDataReader sr = db.get_Reader(str);
            if (!sr.Read())
            {
                end = 0;
            }
            else
            {
                sql = "update WarehouseInvenmedical set Warehouse='" + Warehouse + "',InventoryPer='" + InventoryPer + "',ActualCapacity='" + ActualCapacity + "',InventoryStatus='" + InventoryStatus + "',StorageCondition='" + StorageCondition + "',remark='" + remark + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion

    }
}
