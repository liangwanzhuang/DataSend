using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
    public class WarehousepharmacyModel
    {
        public DataBaseLayer db = new DataBaseLayer();
        #region 添加库房药房信息
        public int AddWarehousepharmacy(string WName, string WareNum, string Type)
        {
            String strSql = "";
            int end = 0;
            DataBaseLayer db = new DataBaseLayer();
            string tate = "select  WareNum  from Warehousepharmacy where WareNum = '" + WareNum + "'";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                strSql = "";
            }
            else
            {
                strSql = "insert into Warehousepharmacy(WName,WareNum,Type) ";
                strSql += "values ('" + WName + "','" + WareNum + "','" + Type + "')";
            }


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
        #region 通过类型查询库房药房信息
        public DataTable findWarehousepharmacyModelInfo(string Type)
        {
            string sql = "select * from  Warehousepharmacy where  1=1";
            if (Type != "0" && Type != "")
            {
                sql += "and  Type ='" + Type + "'";
            }

            // sql += " and id ='" + EName + "'";



            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region 删除库房药房信息
        public bool deleteWarehousepharmacyInfo(int nPId)
        {
            string strSql = "delete from Warehousepharmacy where id =" + nPId;
            int n = db.cmd_Execute(strSql);
            return true;
        }
        #endregion
        #region 通过ID查询药房仓库信息
        public DataTable findWarehousepharmacy(int id)
        {
            string strSql = "select * from  Warehousepharmacy where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        #region 编辑仓库药房信息
        public int UpdateWarehousepharmacyInfo(int id, string WName, string WareNum, string Type)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = "select WareNum from Warehousepharmacy where id != " + id + " and WareNum = '" + WareNum + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {

                sql = "update Warehousepharmacy set WName='" + WName + "',WareNum='" + WareNum + "',Type='" + Type + "' where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion
        #region 查询库房名称
        public SqlDataReader findWarehouseInfo()
        {
            string sql = "select WName  from Warehousepharmacy  where Type ='库房'";
            return db.get_Reader(sql);
        }
        public SqlDataReader findWarehouseInfodrug()
        {
            string sql = "select WName  from Warehousepharmacy  where Type ='药房'";
            return db.get_Reader(sql);
        }

        public SqlDataReader findWarehouseALLInfo()
        {
            string sql = "select WName  from Warehousepharmacy  ";
            return db.get_Reader(sql);
        }
        #endregion

        //找到所有的库房药方信息
        public SqlDataReader findallWarehouse()
        {
            string sql = "select * from  Warehousepharmacy";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;

        }
        //找到所有的药房信息
        public SqlDataReader findpartWarehouse()
        {
            string sql = "select * from  Warehousepharmacy where type ='药房'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;

        }


        //找到所有的库房信息
        public SqlDataReader findpart2Warehouse()
        {
            string sql = "select * from  Warehousepharmacy where type ='库房'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;

        }
        //找到所有药品的批次
        public SqlDataReader findgrudnum()
        {

            string sql = "select * from  DrugAdmin";

            SqlDataReader sdr = db.get_Reader(sql);



            return sdr;
        }
        //通过产品批次找到所有药品信息
        public SqlDataReader findgrudinfobydrugnum(string drugnum)
        {

            string sql = "select * from  DrugAdmin where id = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }
        //通过产品批次找到所有药品信息
        public SqlDataReader findgrudinfobydrugnumsecond(string drugnum)
        {

            string sql = "select * from  DrugAdmin where productbatch = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }




        public SqlDataReader findgrudinfobydrugnumthrid(string drugnum)
        {

            string sql = "select * from  DrugAdmin where drugcode = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }


        //通过产品批次找到所有药品信息
        public SqlDataReader findgrudinfobyproductbatch(string drugnum)
        {

            string sql = "select * from  DrugAdmin where productbatch = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }


        //通过产品批次在库房入库表里查找所有药品信息
        public SqlDataReader findgrudinfobydrugnumfromstorage(string drugnum)
        {

            string sql = "select * from  storage where id =(select max(id) from storage where  drugadminid =  (SELECT id from drugadmin where productbatch ='" + drugnum + "'))";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }
        //通过产品批次在药房入库表里查找所有药品信息
        public SqlDataReader findgrudinfobydrugnumfromstoragedrug(string drugnum)
        {

            string sql = "select * from  medicalstorage where id =(select max(id) from medicalstorage where drugadminid =  (SELECT id from drugadmin where productbatch ='" + drugnum + "'))";

            SqlDataReader sdr = db.get_Reader(sql);

            return sdr;
        }



        //添加临时表(库房管理的药品)
        public int adddtempruginfo(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark)
        {


            int result = 0;
            string sql2 = "select * from  tempdrugtoroom where drugadminid = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempdrugtoroom(drugadminid,num,productdate,validedate,quality,permitNo,remark) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }



        //添加临时表(库房管理的药品)
        public int adddtempruginfo(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark, string fromid)
        {
            int result = 0;
            string sql2 = "select * from  tempdrugtoroom where drugadminid = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempdrugtoroom(drugadminid,num,productdate,validedate,quality,permitNo,remark,fromid) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "','" + fromid + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }
        //添加临时表(库房管理的药品) change
        public int adddtempruginfochange(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark, string fromid)
        {
            int result = 0;
            string sql2 = "select * from  tempdrugtoroomchange where drugadminid = '" + drugnum + "' and fromid ='" + fromid + "'";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempdrugtoroomchange(drugadminid,num,productdate,validedate,quality,permitNo,remark,fromid) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "','" + fromid + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }


        //添加临时表（药品管理的药品）
        public int adddtemMedicinepruginfo(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark)
        {
            int result = 0;
            string sql2 = "select * from  tempMedicinedrugtoroom where drugadminid = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempMedicinedrugtoroom(drugadminid,num,productdate,validedate,quality,permitNo,remark) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }


        //添加临时表（药品管理的药品）
        public int adddtemMedicinepruginfo(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark, string fromid)
        {
            int result = 0;
            string sql2 = "select * from  tempMedicinedrugtoroom where drugadminid = '" + drugnum + "'";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempMedicinedrugtoroom(drugadminid,num,productdate,validedate,quality,permitNo,remark,fromid) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "','" + fromid + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }


        //添加临时表（药品管理的药品）
        public int adddtemMedicinepruginfochange(string drugnum, string num, string quality, string productdate, string validdate, string permitno, string remark, string fromid)
        {
            int result = 0;
            string sql2 = "select * from  tempMedicinedrugtoroomchange where drugadminid = '" + drugnum + "' and fromid ='" + fromid + "' ";

            SqlDataReader sdr = db.get_Reader(sql2);
            if (sdr.Read())
            {
                result = 0;
            }
            else
            {
                string sql = "insert tempMedicinedrugtoroomchange(drugadminid,num,productdate,validedate,quality,permitNo,remark,fromid) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "','" + fromid + "') ";

                result = db.cmd_Execute(sql);
            }
            return result;
        }
        //删除调拨表（库房药品）
        public int deletedinfo(string id)
        {
            int result = 0;


            string sql = "delete from tempdrugtoroomchange where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }
        //删除调拨表（药房药品）
        public int deletemedicinedinfo(string id)
        {
            int result = 0;


            string sql = "delete from tempMedicinedrugtoroomchange where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }

        //删除临时表（库房药品）
        public int deletedtempruginfo(string id)
        {
            int result = 0;


            string sql = "delete from tempdrugtoroom where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }
        //删除报损信息表
        public int deletedlossiInforinfo(string id)
        {
            int result = 0;


            string sql = "delete from LossiInfor where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }
        public int deletedmedicinelossiInforinfo(string id)
        {
            int result = 0;


            string sql = "delete from lossiInfomedical where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }
        //删除临时表（药房药品）
        public int deletedtemMedicinepruginfo(string id)
        {
            int result = 0;


            string sql = "delete from tempMedicinedrugtoroom where id = '" + id + "' ";

            result = db.cmd_Execute(sql);

            return result;
        }


        //添加入库表
        public int adddrugintoroominfo(string strRowIds, string Warehouse1, string intoman, string OSingle1, string OSTime1)
        {
            int result = 0;


            // string sql = "insert tempdrugtoroom(drugadminid,num,productdate,validedate,quality,permitNo,remark) values('" + drugnum + "','" + num + "','" + productdate + "','" + validdate + "','" + quality + "','" + permitno + "','" + remark + "') ";

            // result = db.cmd_Execute(sql);

            return result;
        }


    }
}
