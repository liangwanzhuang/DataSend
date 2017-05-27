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
   public class StorageManageModel
    {

        public DataBaseLayer db = new DataBaseLayer();
        /// <summary>
        /// 添加\库房入库单信息
        /// </summary>
        /// <param name="einfo"></param>
        /// <returns></returns>
        public int AddStorage(string strRowIds, string Warehouse1, string intoman, string OSingle1, string OSTime1, System.DateTime currentTime, string Num)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
          //  System.DateTime currentTime = new System.DateTime();
          //  currentTime = System.DateTime.Now;//获取当前时间
            //单号
          //   string now =  currentTime.ToString("yyyyMMddhhmmss");
           //  string Num = "RK" + now;//单号
      
           /*  string str12 = "select * from Storage where drugadminid = (select drugadminid from tempdrugtoroom where id ='" + strRowIds + "')";
             SqlDataReader sdr12 = db.get_Reader(str12);
             if (sdr12.Read())
             {
                 end = 0;
             }else{
            * */

                 string str1 = "select * from tempdrugtoroom where id ='" + strRowIds + "'";
                 SqlDataReader sdr1 = db.get_Reader(str1);
                 string num1 = "";//数量
                 string productdate = "";//生产日期
                 string validedate = "";//有效期
                 string quality = "";//质量
                 string permitNo = "";//批准文号
                 string remark = "";//备注
                 string dgid = "";//药品id
                 string drugname = "";//药品名
                 if(sdr1.Read())
                 {

                     dgid = sdr1["drugadminid"].ToString();
                     num1 = sdr1["num"].ToString();
                     productdate = sdr1["productdate"].ToString();

                     validedate = sdr1["validedate"].ToString();
                     quality = sdr1["quality"].ToString();
                     permitNo = sdr1["permitNo"].ToString();
                     remark = sdr1["remark"].ToString();
                     

                 }
                 string dgid2 = "";
                 string str2 = "select * from drugadmin where productbatch ='" + dgid + "'";
                 SqlDataReader sdr122 = db.get_Reader(str2);
                 if (sdr122.Read())
                 {
                     dgid2 = sdr122["id"].ToString();
                     drugname = sdr122["DrugName"].ToString();

                 }


                 strSql = "insert into Storage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime,drugname) ";
                 strSql += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "','" + drugname + "')";
           // }


            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(strSql);
                if (end == 1)
                {
                    string str123 = "delete from tempdrugtoroom where id ='" + strRowIds + "'";
                    db.cmd_Execute(str123);
                }
            }
           
            // }
           
            return end;

        }


        /// <summary>
        /// 添加\库房入库单信息
        /// </summary>
        /// <param name="einfo"></param>
        /// <returns></returns>
        public int AddfromStorage(string strRowIds, string Warehouse1, string intoman, string OSingle1, string OSTime1, System.DateTime currentTime, string Num)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
           // System.DateTime currentTime = new System.DateTime();
          //  currentTime = System.DateTime.Now;//获取当前时间
            //单号
         //   string now = currentTime.ToString("yyyyMMddhhmmss");
          //  string Num = "DB" + now;//单号

          /*  string str12 = "select * from storagefrom where drugadminid = (select drugadminid from tempdrugtoroom where id ='" + strRowIds + "')";
            SqlDataReader sdr12 = db.get_Reader(str12);
            if (sdr12.Read())
            {
                end = 0;
            }
            else
            {
            */
                string str1 = "select * from tempdrugtoroomchange where id ='" + strRowIds + "'";
                SqlDataReader sdr1 = db.get_Reader(str1);
                string num1 = "";//数量
                string productdate = "";//生产日期
                string validedate = "";//有效期
                string quality = "";//质量
                string permitNo = "";//批准文号
                string remark = "";//备注
                string dgid = "";//药品批次
                string dgid2 = "";//
                string drugname = "";//药品名称


                string fromroom = "";//从哪个仓库调出
                if (sdr1.Read())
                {

                    dgid = sdr1["drugadminid"].ToString();

                    string str2 = "select * from drugadmin where productbatch ='" + dgid + "'";
                    SqlDataReader sdr122 = db.get_Reader(str2);
                    if (sdr122.Read())
                    {
                        dgid2 = sdr122["id"].ToString();
                        drugname = sdr122["DrugName"].ToString();


                    }




                    num1 = sdr1["num"].ToString();
                    productdate = sdr1["productdate"].ToString();

                    validedate = sdr1["validedate"].ToString();
                    quality = sdr1["quality"].ToString();
                    permitNo = sdr1["permitNo"].ToString();
                    remark = sdr1["remark"].ToString();
                    fromroom = sdr1["fromid"].ToString();

                }



                strSql = "insert into storagefrom(storageroom,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime,fromroom,drugname) ";
                strSql += "values ('" + Warehouse1 + "','" + dgid + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "','" + fromroom + "','" + drugname + "')";
                // }


                string str1234 = "select * from warehousepharmacy where wname ='" + Warehouse1 + "'";
                SqlDataReader sdr123 = db.get_Reader(str1234);
                string type = "";
                if(sdr123.Read()){
                   type = sdr123["Type"].ToString();

                }
                if (type == "库房")
                {
                    string strSql1 = "";
                    strSql1 = "insert into Storage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime,drugname) ";
                    strSql1 += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "','" + drugname + "')";
                    db.cmd_Execute(strSql1);
                }

                if (type == "药房")
                {
                    string strSql1 = "";
                    strSql1 = "insert into medicalStorage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime,drugname) ";
                    strSql1 += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "','" + drugname + "')";
                    db.cmd_Execute(strSql1);
                }



                if (strSql == "")
                {
                    end = 0;
                }
                else
                {
                    end = db.cmd_Execute(strSql);
                    if (end == 1)
                    {
                        string str123 = "delete from tempdrugtoroomchange where id ='" + strRowIds + "'";
                        db.cmd_Execute(str123);
                    }
                }

          //  }

            return end;

        }


       //添加药房入库单信息
        public int AddMedicineStorage(string strRowIds, string Warehouse1, string intoman, string OSingle1, string OSTime1)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//获取当前时间
            //单号
            string now = currentTime.ToString("yyyyMMddhhmmss");
            string Num = "RK" + now;//单号

            /*string str12 = "select * from Storage where drugadminid = (select drugadminid from tempMedicinedrugtoroom where id ='" + strRowIds + "')";
            SqlDataReader sdr12 = db.get_Reader(str12);
            if (sdr12.Read())
            {
                end = 0;
            }
            else
            {*/

                string str1 = "select * from tempMedicinedrugtoroom where id ='" + strRowIds + "'";
                SqlDataReader sdr1 = db.get_Reader(str1);
                string num1 = "";//数量
                string productdate = "";//生产日期
                string validedate = "";//有效期
                string quality = "";//质量
                string permitNo = "";//批准文号
                string remark = "";//备注
                string dgid = "";//药品productbatch
                string dgid2 = "";//药品id
                if (sdr1.Read())
                {

                    dgid = sdr1["drugadminid"].ToString();
                    string str2 = "select * from drugadmin where productbatch = '" + dgid + "'";
                    SqlDataReader sdr2 = db.get_Reader(str2);
                    if (sdr2.Read())
                    {
                    dgid2 = sdr2["id"].ToString();
                    }
                    

                    num1 = sdr1["num"].ToString();
                    productdate = sdr1["productdate"].ToString();
                    validedate = sdr1["validedate"].ToString();
                    quality = sdr1["quality"].ToString();
                    permitNo = sdr1["permitNo"].ToString();
                    remark = sdr1["remark"].ToString();


                }



                strSql = "insert into medicalStorage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime) ";
                strSql += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "')";
                // }


                if (strSql == "")
                {
                    end = 0;
                }
                else
                {
                    end = db.cmd_Execute(strSql);
                    if (end == 1)
                    {
                        string str123 = "delete from tempMedicinedrugtoroom where id ='" + strRowIds + "'";
                        db.cmd_Execute(str123);
                    }
                }

            

            return end;

        }




        /// <summary>
        /// 添加\库房入库单信息
        /// </summary>
        /// <param name="einfo"></param>
        /// <returns></returns>
        public int AddfrommedicalStorage(string strRowIds, string Warehouse1, string intoman, string OSingle1, string OSTime1, System.DateTime currentTime, string Num)
        {
            DataBaseLayer db = new DataBaseLayer();
            String strSql = "";
            int end = 0;
           // System.DateTime currentTime = new System.DateTime();
          //  currentTime = System.DateTime.Now;//获取当前时间
            //单号
           // string now = currentTime.ToString("yyyyMMddhhmmss");
           // string Num = "DB" + now;//单号

           

                string str1 = "select * from tempMedicinedrugtoroomchange where id ='" + strRowIds + "'";
                SqlDataReader sdr1 = db.get_Reader(str1);
                string num1 = "";//数量
                string productdate = "";//生产日期
                string validedate = "";//有效期
                string quality = "";//质量
                string permitNo = "";//批准文号
                string remark = "";//备注
                string dgid = "";//药品id
                string fromroom = "";//
                string dgid2 = "";
                if (sdr1.Read())
                {

                    dgid = sdr1["drugadminid"].ToString();

                    string str2 = "select * from drugadmin where productbatch = '" + dgid + "'";
                    SqlDataReader sdr2 = db.get_Reader(str2);
                    if (sdr2.Read())
                    {
                        dgid2 = sdr2["id"].ToString();
                    }


                    num1 = sdr1["num"].ToString();
                    productdate = sdr1["productdate"].ToString();

                    validedate = sdr1["validedate"].ToString();
                    quality = sdr1["quality"].ToString();
                    permitNo = sdr1["permitNo"].ToString();
                    remark = sdr1["remark"].ToString();
                    fromroom = sdr1["fromid"].ToString();

                }



                strSql = "insert into medicalstoragefrom(storageroom,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime,fromroom) ";
                strSql += "values ('" + Warehouse1 + "','" + dgid + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "','" + fromroom + "')";
                // }






                string str1234 = "select * from warehousepharmacy where wname ='" + Warehouse1 + "'";
                SqlDataReader sdr123 = db.get_Reader(str1234);
                string type = "";
                if (sdr123.Read())
                {
                    type = sdr123["Type"].ToString();

                }
                if (type == "库房")
                {
                    string strSql1 = "";
                    strSql1 = "insert into Storage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime) ";
                    strSql1 += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "')";
                    db.cmd_Execute(strSql1);
                }

                if (type == "药房")
                {
                    string strSql1 = "";
                    strSql1 = "insert into medicalStorage(Warehouse,drugadminid,Num, OSingle, OSTime,Warehousing,Amount,ProDate,ExpiryDate,Quality,LicenseNum,Rmarkes,StorageTime) ";
                    strSql1 += "values ('" + Warehouse1 + "','" + dgid2 + "','" + Num + "','" + OSingle1 + "','" + OSTime1 + "','" + intoman + "','" + num1 + "','" + productdate + "','" + validedate + "','" + quality + "','" + permitNo + "','" + remark + "','" + currentTime + "')";
                    db.cmd_Execute(strSql1);
                }








                if (strSql == "")
                {
                    end = 0;
                }
                else
                {
                    end = db.cmd_Execute(strSql);
                    if (end == 1)
                    {
                        string str123 = "delete from tempMedicinedrugtoroomchange where id ='" + strRowIds + "'";
                        db.cmd_Execute(str123);
                    }
              

            }

            return end;

        }


        public DataTable searchStorageInfo1()
        {
            string strSQL = "select id,fromid, drugadminid as ProductBatch,(select drugcode from drugadmin where productbatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where productbatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where productbatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where productbatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where productbatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where productbatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where productbatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempMedicinedrugtoroom as t";


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        public DataTable searchStorageInfo1change()
        {
            string strSQL = "select id,fromid, drugadminid as ProductBatch,(select drugcode from drugadmin where productbatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where productbatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where productbatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where productbatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where productbatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where productbatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where productbatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempMedicinedrugtoroomchange as t order by id desc";


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        public DataTable searchStorageInfo3()
        {
            string strSQL = "select id,fromid, drugadminid as ProductBatch,(select drugcode from drugadmin where productbatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where productbatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where productbatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where productbatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where productbatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where productbatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where productbatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempdrugtoroom as t order by id desc";
            

            /*if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }*/



            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        //查询药房房调出单列表信息
        public DataTable findmedicalStorageListInfo1(string LSTime, string LETime, string Warehousing, string DrugName)
        {

            string strSQL = "select max(id) as id,max(storageroom) as Warehouse ,Num,max(OSingle) as OSingle ,max(OSTime) as OSTime,max(Warehousing) as Warehousing,max(StorageTime) as StorageTime"
           + " from medicalStoragefrom as s where 1=1";


            //string strSQL = "select id,drugadminid as ProductBatch,storageroom as Warehouse,Warehousing,OSingle,OSTime,Num,(select DrugCode from drugadmin where ProductBatch = s.drugadminid) as DrugCode,(select DrugName from drugadmin where ProductBatch = s.drugadminid) as DrugName,StorageTime,(select Univalent from drugadmin where ProductBatch = s.drugadminid) as Univalent,(select PurUnits from drugadmin where ProductBatch = s.drugadminid) as PurUnits,Amount,(cast((select Univalent from drugadmin where ProductBatch = s.drugadminid) as int)*cast (Amount as int)) as Money"
            // + " from medicalStoragefrom as s where 1=1 ";

            if (LSTime != null && LSTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";
            }


            if (LETime != null && LETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }
            if (Warehousing != null && Warehousing.Length > 0)
            {

                strSQL += "and Warehousing  ='" + Warehousing + "'";

            }
            if (DrugName != null && DrugName.Length > 0)
            {

                //strSQL += "and DrugName  ='" + DrugName + "'";


                strSQL += "and s.drugadminid in (select productBatch from drugadmin where drugname ='" + DrugName + "')";

            }

            strSQL += "group by Num order by id desc";

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        #region 查询库房药品信息
        public DataTable searchStorageInfo()
        {
            string strSQL = "select id,fromid,(select ProductBatch from drugadmin where ProductBatch = t.drugadminid) as ProductBatch,(select drugcode from drugadmin where ProductBatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where ProductBatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where ProductBatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where ProductBatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where ProductBatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where ProductBatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where ProductBatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempdrugtoroom as t";
           


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        #endregion
        #region 查询库房药品信息
        public DataTable searchStorageInfochange()
        {
            string strSQL = "select id,fromid,(select ProductBatch from drugadmin where ProductBatch = t.drugadminid) as ProductBatch,(select drugcode from drugadmin where ProductBatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where ProductBatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where ProductBatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where ProductBatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where ProductBatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where ProductBatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where ProductBatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempdrugtoroomchange as t order by id desc";



            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

        #endregion


        public DataTable searchStorageInfosecond()
        {
            string strSQL = "select id,(select ProductBatch from drugadmin where id = t.drugadminid) as ProductBatch,(select drugcode from drugadmin where id = t.drugadminid) as DrugCode,(select drugname from drugadmin where id = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where id = t.drugadminid) as Univalent, (select PurUnits from drugadmin where id = t.drugadminid) as PurUnits, (select Producer from drugadmin where id = t.drugadminid) as Producer,(select ProducingArea from drugadmin where id = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where id = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempdrugtoroom as t";

            /*if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }*/



            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        #region 查询药房药品信息
        public DataTable searchMedicineStorageInfosecond()
        {
            string strSQL = "select id,(select ProductBatch from drugadmin where ProductBatch = t.drugadminid) as ProductBatch,(select drugcode from drugadmin where ProductBatch = t.drugadminid) as DrugCode,(select drugname from drugadmin where ProductBatch = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where ProductBatch = t.drugadminid) as Univalent, (select PurUnits from drugadmin where ProductBatch = t.drugadminid) as PurUnits, (select Producer from drugadmin where ProductBatch = t.drugadminid) as Producer,(select ProducingArea from drugadmin where ProductBatch = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where ProductBatch = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempMedicinedrugtoroom as t";

            /*if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }*/



            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion




        #region 查询药房药品信息
        public DataTable searchMedicineStorageInfo()
        {
            string strSQL = "select id,(select ProductBatch from drugadmin where id = t.drugadminid) as ProductBatch,(select drugcode from drugadmin where id = t.drugadminid) as DrugCode,(select drugname from drugadmin where id = t.drugadminid) as DrugName,num as Amount,(select Univalent from drugadmin where id = t.drugadminid) as Univalent, (select PurUnits from drugadmin where id = t.drugadminid) as PurUnits, (select Producer from drugadmin where id = t.drugadminid) as Producer,(select ProducingArea from drugadmin where id = t.drugadminid) as ProducingArea,productdate as ProDate,validedate as ExpiryDate,quality as Quality,permitNo as LicenseNum,remark as Remarkes,(cast((select Univalent from drugadmin where id = t.drugadminid) as int)*cast (num as int)) as money"
             + " from tempMedicinedrugtoroom as t";

            /*if (STime != null && STime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(STime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (ETime != null && ETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(ETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }*/



            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion
        #region 查询库房入库单列表信息
        public DataTable findStorageListInfo(string LSTime, string LETime ,string Warehousing,string DrugName)
        {
            string strSQL = "select max(id) as id,max(Warehouse) as Warehouse ,Num,max(OSingle) as OSingle ,max(OSTime) as OSTime,max(Warehousing) as Warehousing,max(StorageTime) as StorageTime"
             + " from Storage as s where 1=1";

            if (LSTime != null && LSTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (LETime != null && LETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }
            if (Warehousing != null && Warehousing.Length > 0)
            {

                strSQL += "and Warehousing  ='" + Warehousing + "'";

            }
            if (DrugName != null && DrugName.Length > 0)
            {

                //strSQL += "and DrugName  ='" + DrugName + "'";


                strSQL += "and s.drugadminid in (select id from drugadmin where drugname ='" + DrugName + "')";

            }

            strSQL += "group by Num order by id desc";


            /*string strSQL = "select id,(select ProductBatch from drugadmin where id = s.drugadminid) as ProductBatch,Warehouse,Warehousing,OSingle,Quality,LicenseNum,ProDate,ExpiryDate,OSTime,Num,(select DrugCode from drugadmin where id = s.drugadminid) as DrugCode,(select DrugName from drugadmin where id = s.drugadminid) as DrugName,StorageTime,(select Univalent from drugadmin where id = s.drugadminid) as Univalent,(select PurUnits from drugadmin where id = s.drugadminid) as PurUnits,Amount,(cast((select Univalent from drugadmin where id = s.drugadminid) as int)*cast (Amount as int)) as Money"
             + " from Storage as s where Warehouse in (select WName  from Warehousepharmacy  where Type ='库房') ";

            if (LSTime != null && LSTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (LETime != null && LETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }
            if (Warehousing != null && Warehousing.Length > 0)
            {

                strSQL += "and Warehousing  ='" + Warehousing + "'";

            }
            if (DrugName != null && DrugName.Length > 0)
            {

                //strSQL += "and DrugName  ='" + DrugName + "'";


                strSQL += "and s.drugadminid in (select id from drugadmin where drugname ='" + DrugName + "')";

            }*/

        

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }

       //通过入库单号找到药品信息
        public DataTable findstoragedruginfobyopertatenum(string operatenum)
        {


            string str = "select id,(select ProductBatch from drugadmin where id = s.drugadminid) as ProductBatch,LicenseNum,ProDate,ExpiryDate,(select DrugCode from drugadmin where id = s.drugadminid) as DrugCode,(select DrugName from drugadmin where id = s.drugadminid) as DrugName,";
            str += "(select Univalent from drugadmin where id = s.drugadminid) as Univalent,(select PurUnits from drugadmin where id = s.drugadminid) as PurUnits,Amount,Quality,(SELECT curstate from prescription where(ID in (SELECT pid from InvalidPrescription WHERE(id = s.id))))as curstate,";
            str += "(cast((select Univalent from drugadmin where id = s.drugadminid) as int)*cast (Amount as int)) as Money from storage as s  where num = '" + operatenum + "' order by id desc";
            DataTable dt = db.get_DataTable(str);

            return dt;

        }

        //通过出库单号找到药品信息
        public DataTable findstoragefromdruginfobyopertatenum(string operatenum)
        {


            string str = "select id,s.drugadminid as ProductBatch,fromroom,LicenseNum,ProDate,ExpiryDate,(select DrugCode from drugadmin where productbatch = s.drugadminid) as DrugCode,(select DrugName from drugadmin where productbatch = s.drugadminid) as DrugName,";
            str += "(select Univalent from drugadmin where productbatch = s.drugadminid) as Univalent,(select PurUnits from drugadmin where productbatch = s.drugadminid) as PurUnits,Amount,Quality,";
            str += "(cast((select Univalent from drugadmin where productbatch = s.drugadminid) as int)*cast (Amount as int)) as Money from storagefrom as s  where num = '" + operatenum + "' order by id desc";
            DataTable dt = db.get_DataTable(str);

            return dt;

        }


        //通过药房出库单号找到药品信息
        public DataTable findmedicalstoragefromdruginfobyopertatenum(string operatenum)
        {


            string str = "select id,s.drugadminid as ProductBatch,fromroom,LicenseNum,ProDate,ExpiryDate,(select DrugCode from drugadmin where productbatch = s.drugadminid) as DrugCode,(select DrugName from drugadmin where productbatch = s.drugadminid) as DrugName,";
            str += "(select Univalent from drugadmin where productbatch = s.drugadminid) as Univalent,(select PurUnits from drugadmin where productbatch = s.drugadminid) as PurUnits,Amount,Quality,";
            str += "(cast((select Univalent from drugadmin where productbatch = s.drugadminid) as int)*cast (Amount as int)) as Money from medicalstoragefrom as s  where num = '" + operatenum + "' order by id desc";
            DataTable dt = db.get_DataTable(str);

            return dt;

        }

        //通过药房入库单号找到药品信息
        public DataTable findmedicalstoragedruginfobyopertatenum(string operatenum)
        {


            string str = "select id,(select ProductBatch from drugadmin where id = s.drugadminid) as ProductBatch,LicenseNum,ProDate,ExpiryDate,(select DrugCode from drugadmin where id = s.drugadminid) as DrugCode,(select DrugName from drugadmin where id = s.drugadminid) as DrugName,";
            str += "(select Univalent from drugadmin where id = s.drugadminid) as Univalent,(select PurUnits from drugadmin where id = s.drugadminid) as PurUnits,Amount,Quality,";
            str += "(cast((select Univalent from drugadmin where id = s.drugadminid) as int)*cast (Amount as int)) as Money from medicalstorage as s  where num = '" + operatenum + "' order by id desc";
            DataTable dt = db.get_DataTable(str);

            return dt;

        }


        #endregion
        //查询库房调出单列表信息
        public DataTable findStorageListInfo1(string LSTime, string LETime, string Warehousing, string DrugName)
        {
           // string strSQL = "select id,drugadminid as ProductBatch,storageroom as Warehouse,fromroom,Warehousing,OSingle,OSTime,Num,(select DrugCode from drugadmin where ProductBatch = s.drugadminid) as DrugCode,(select DrugName from drugadmin where ProductBatch = s.drugadminid) as DrugName,StorageTime,(select Univalent from drugadmin where ProductBatch = s.drugadminid) as Univalent,(select PurUnits from drugadmin where ProductBatch = s.drugadminid) as PurUnits,Amount,(cast((select Univalent from drugadmin where ProductBatch = s.drugadminid) as int)*cast (Amount as int)) as Money"
            // + " from Storagefrom as s where 1=1 ";
            string strSQL = "select max(id) as id,max(storageroom) as Warehouse ,Num,max(OSingle) as OSingle ,max(OSTime) as OSTime,max(Warehousing) as Warehousing,max(StorageTime) as StorageTime"
             + " from Storagefrom as s where 1=1";
            //string strSQL = "select max(id) as id,max(Warehouse) as Warehouse ,Num,max(OSingle) as OSingle ,max(OSTime) as OSTime,max(Warehousing) as Warehousing,max(StorageTime) as StorageTime"
            // + " from Storage as s where 1=1";




            if (LSTime != null && LSTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";
            }


            if (LETime != null && LETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }
            if (Warehousing != null && Warehousing.Length > 0)
            {

                strSQL += "and Warehousing  ='" + Warehousing + "'";

            }
            if (DrugName != null && DrugName.Length > 0)
            {

                //strSQL += "and DrugName  ='" + DrugName + "'";


                strSQL += "and s.drugadminid in (select productBatch from drugadmin where drugname ='" + DrugName + "')";

            }

            strSQL += "group by Num order by id desc";

            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }


        #region 查询药房入库单列表信息
        public DataTable findMedicineStorageListInfo(string LSTime, string LETime, string Warehousing, string DrugName)
        {
          //  string strSQL = "select id,(select ProductBatch from drugadmin where id = s.drugadminid) as ProductBatch,Warehouse,Warehousing,OSingle,Quality,LicenseNum,ProDate,ExpiryDate,OSTime,Num,(select DrugCode from drugadmin where id = s.drugadminid) as DrugCode,(select DrugName from drugadmin where id = s.drugadminid) as DrugName,StorageTime,(select Univalent from drugadmin where id = s.drugadminid) as Univalent,(select PurUnits from drugadmin where id = s.drugadminid) as PurUnits,Amount,(cast((select Univalent from drugadmin where id = s.drugadminid) as int)*cast (Amount as int)) as Money"
          //   + " from medicalStorage as s where 1=1";
            string strSQL = "select max(id) as id,max(Warehouse) as Warehouse ,Num,max(OSingle) as OSingle ,max(OSTime) as OSTime,max(Warehousing) as Warehousing,max(StorageTime) as StorageTime"
            + " from medicalStorage as s where 1=1";
        

            if (LSTime != null && LSTime.Length > 0)
            {
                DateTime d = Convert.ToDateTime(LSTime);
                string strS = d.ToString("yyyy/MM/dd 00:00:00");
                strSQL += "and StorageTime >='" + strS + "'";

            }


            if (LETime != null && LETime.Length > 0)
            {
                DateTime d4 = Convert.ToDateTime(LETime);
                string strE = d4.ToString("yyyy/MM/dd 23:59:59");
                strSQL += "and StorageTime  <='" + strE + "'";

            }
            if (Warehousing != null && Warehousing.Length > 0)
            {

                strSQL += "and Warehousing  ='" + Warehousing + "'";

            }
            if (DrugName != null && DrugName.Length > 0)
            {

                //strSQL += "and DrugName  ='" + DrugName + "'";


                strSQL += "and s.drugadminid in (select id from drugadmin where drugname ='" + DrugName + "')";

            }
            strSQL += "group by Num order by id desc";


            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        #endregion


      


        #region 通过ID查询入库单信息

        public DataTable findStorageListInfo(int id)
        {
            string strSql = "select * from  medicalStorage where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable findStorageInfo(int id)
        {
            string strSql = "select * from  Storage where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        #region 通过ID查询库房入库单药品部分信息

        public DataTable ftempdrugtoroomInfo(int id)
        {
            string strSql = "select * from  tempdrugtoroom where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable LossiInforInfo(int id)
        {
            string strSql = "select * from  LossiInfor where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable MedicineLossiInforInfo(int id)
        {
            string strSql = "select * from  lossiInfomedical where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable fdrugtoroomInfo(int id)
        {
            string strSql = "select * from  tempdrugtoroomchange where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable fmedicinedrugtoroomInfo(int id)
        {
            string strSql = "select * from  tempMedicinedrugtoroomchange where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable fqureydrugtoroomInfo(int id)
        {
            string strSql = "select * from  Storagefrom where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        public DataTable fmedicinequreydrugtoroomInfo(int id)
        {
            string strSql = "select * from  medicalStoragefrom where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        #region 通过ID查询药房入库单药品部分信息

        public DataTable ftempMedicinedrugtoroomInfo(int id)
        {
            string strSql = "select * from  tempMedicinedrugtoroom where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        #region 编辑库房入库单信息


        public int StorageListInfo(int id, string Warehouse, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum, string OSingle, string OSTime, string Warehousing)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = " ";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {


                sql = "update Storage set Warehouse='" + Warehouse + "',Amount='" + Amount + "',Rmarkes='" + Rmarkes + "',ProDate='" + ProDate + "',ExpiryDate='" + ExpiryDate + "',Quality='" + Quality + "',LicenseNum='" + LicenseNum + "',OSingle='" + OSingle + "',OSTime='" + OSTime + "',Warehousing='" + Warehousing + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion
        #region 编辑药房入库单信息


        public int MedicineStorageListInfo(int id, string Warehouse, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum, string OSingle, string OSTime, string Warehousing)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = " ";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {


                sql = "update medicalStorage set Warehouse='" + Warehouse + "',Amount='" + Amount + "',Rmarkes='" + Rmarkes + "',ProDate='" + ProDate + "',ExpiryDate='" + ExpiryDate + "',Quality='" + Quality + "',LicenseNum='" + LicenseNum + "',OSingle='" + OSingle + "',OSTime='" + OSTime + "',Warehousing='" + Warehousing + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        public int StoragequeryInfo(int id, string Warehouse, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum, string OSingle, string OSTime, string Warehousing)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = " ";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {


                sql = "update Storage set Warehouse='" + Warehouse + "',Amount='" + Amount + "',Rmarkes='" + Rmarkes + "',ProDate='" + ProDate + "',ExpiryDate='" + ExpiryDate + "',Quality='" + Quality + "',LicenseNum='" + LicenseNum + "',OSingle='" + OSingle + "',OSTime='" + OSTime + "',Warehousing='" + Warehousing + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        public int outboundqueryInfo(int id, string storageroom, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum, string OSingle, string OSTime, string Warehousing)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = " ";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {


                sql = "update storagefrom set storageroom='" + storageroom + "',Amount='" + Amount + "',Rmarkes='" + Rmarkes + "',ProDate='" + ProDate + "',ExpiryDate='" + ExpiryDate + "',Quality='" + Quality + "',LicenseNum='" + LicenseNum + "',OSingle='" + OSingle + "',OSTime='" + OSTime + "',Warehousing='" + Warehousing + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        public int medicineoutboundqueryInfo(int id, string storageroom, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum, string OSingle, string OSTime, string Warehousing)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = " ";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {


                sql = "update medicalStoragefrom set storageroom='" + storageroom + "',Amount='" + Amount + "',Rmarkes='" + Rmarkes + "',ProDate='" + ProDate + "',ExpiryDate='" + ExpiryDate + "',Quality='" + Quality + "',LicenseNum='" + LicenseNum + "',OSingle='" + OSingle + "',OSTime='" + OSTime + "',Warehousing='" + Warehousing + "'where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion
    public DataTable findStorage(string medicaltype,string medicalname){
        string sql = "select * from DrugAdmin where 1=1";
        if(medicaltype !="0"){
            sql += "and DrugType='" + medicaltype + "'";
        }
        if (medicalname != "0")
        {
            sql += "and DrugName='" + medicalname + "'";
        }
        DataTable dt = db.get_DataTable(sql);
        return dt;

    }

    #region 编辑库房药品信息


    public int StorageDrugInfo(int id,  string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality,  string LicenseNum)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update tempdrugtoroom set num='" + Amount + "',remark='" + Rmarkes + "',productdate='" + ProDate + "',validedate='" + ExpiryDate + "',quality='" + Quality + "',permitNo='" + LicenseNum + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    public int LossiInforInfo(int id, string Type, string Per, string Reason, string lossnum, string Rmarkes)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update LossiInfor set Type='" + Type + "',Per='" + Per + "',Reason='" + Reason + "',lossnum='" + lossnum + "',remark='" + Rmarkes + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    public int MedicineLossiInforInfo(int id, string Type, string Per, string Reason, string lossnum, string Rmarkes)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update lossiInfomedical set Type='" + Type + "',Per='" + Per + "',Reason='" + Reason + "',lossnum='" + lossnum + "',remark='" + Rmarkes + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    public int StorageInfo(int id, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update tempdrugtoroomchange set num='" + Amount + "',remark='" + Rmarkes + "',productdate='" + ProDate + "',validedate='" + ExpiryDate + "',quality='" + Quality + "',permitNo='" + LicenseNum + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    public int StoragemedicineInfo(int id, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update tempMedicinedrugtoroomchange set num='" + Amount + "',remark='" + Rmarkes + "',productdate='" + ProDate + "',validedate='" + ExpiryDate + "',quality='" + Quality + "',permitNo='" + LicenseNum + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    public int StoragequreyInfo(int id, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update Storagefrom set num='" + Amount + "',remark='" + Rmarkes + "',productdate='" + ProDate + "',validedate='" + ExpiryDate + "',quality='" + Quality + "',permitNo='" + LicenseNum + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }

    #endregion
    #region 编辑药房药品信息


    public int MedicineStorageDrugInfo(int id, string Amount, string Rmarkes, string ProDate, string ExpiryDate, string Quality, string LicenseNum)
    {
        //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

        int end = 0;

        string sql = "";
        string str = " ";
        SqlDataReader sr = db.get_Reader(str);
        if (sr.Read())
        {
            end = 0;
        }
        else
        {


            sql = "update tempMedicinedrugtoroom set num='" + Amount + "',remark='" + Rmarkes + "',productdate='" + ProDate + "',validedate='" + ExpiryDate + "',quality='" + Quality + "',permitNo='" + LicenseNum + "'where id = " + id + "";
            end = db.cmd_Execute(sql);
        }


        return end;
    }
    #endregion
    
    public DataTable findStoragebyfromid(string fromid)
    {
        string sql = "select * from storage where 1=1";
        if (fromid != "0")
        {
            sql += "and Warehouse='" + fromid + "'";
        }
       
        DataTable dt = db.get_DataTable(sql);

        return dt;

    }
       //找到匹配完的信息
    public DataTable finddrugmatchdone(string medicalname, string hospitalname,string  hospitaldrugname)
    {
       // string sql = "select id,(select Hname from hospital where id = y.hospitalid) as hospitalname,(select Hnum from hospital where id = y.hospitalid) as hospitalnum,drugName,drugNum,drugDetailedName,(select max(positionnum) from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as positionNum,drugAlias,
        //(select top 1 drugspecificat from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as drugspecificat,(select top 1 producingarea from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as producingarea  from ypcdrug as y where 1=1";
        //string sql = "select row_number() over (order by da.drugNum) as id,da.drugAlias  as y_id,  da.* from " + 
      //  string sql = "select da.drugAlias as id,  da.* from " + 
    //     "(select distinct a.hospitalName as hospitalname,h.Hnum as hospitalnum,a.hdrugNum as drugNum,a.hdrugName as drugName,a.ypcdrugNum as drugAlias,a.ypcdrugName as drugDetailedName,a.ypcdrugPositionNum as positionNum,d.DrugSpecificat  as drugspecificat,"+
//"d.ProducingArea as producingarea  from DrugMatching a inner join Hospital h on h.ID=a.hospitalId   left join DrugAdmin  d on d.DrugCode =a.ypcdrugNum ) da "+
//"  where 1=1 and da.drugAlias is not null ";
       // string sql = 
         //     "select distinct a.hospitalName as hospitalname,h.Hnum as hospitalnum,a.hdrugNum as drugNum,a.hdrugName as drugName,a.ypcdrugNum as drugAlias,a.ypcdrugName as drugDetailedName,a.ypcdrugPositionNum as positionNum,d.DrugSpecificat  as drugspecificat," +
    // "d.ProducingArea as producingarea  from DrugMatching a inner join Hospital h on h.ID=a.hospitalId   left join DrugAdmin  d on d.DrugCode =a.ypcdrugNum " +
   //  "  where 1=1 and da.drugAlias is not null ";
        string sql = "select y.id as id,h.Hname as hospitalname,y.hospitalId as hospitalnum,y.drugNum as drugNum,y.drugName as drugName,y.drugAlias as drugAlias,y.drugDetailedName as drugDetailedName,(select top 1 positionnum from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as positionNum,(select top 1 producingarea from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as producingarea,(select top 1 drugspecificat from drugadmin where drugname = y.drugDetailedName and drugcode = y.drugAlias) as drugspecificat from ypcDrug y left join Hospital h on h.ID=y.hospitalId " 
       +" where 1=1 ";
        if (medicalname != "0")
        {
            sql += "and y.drugDetailedName ='" + medicalname + "'";
        }
        if (hospitalname != "0")
        {
            sql += "and y.hospitalId = '" + hospitalname.Trim() + "'";
        }
        if (hospitaldrugname != "0")
        {
            sql += "and y.drugName ='" + hospitaldrugname + "'";
        }
        DataTable dt = db.get_DataTable(sql);

        return dt;

    }
       //匹配列表数据显示 20160330  lbf update
    public DataTable finddrugmatchdone_1(string medicalname, string hospitalname, string hospitaldrugname)
    {
       
        string sql = "select ROW_NUMBER() over(order by  a.id asc) as id, a.hospitalname,a.hospitalnum,a.drugNum,a.drugName,a.drugAlias,a.drugDetailedName,a.positionNum,a.producingarea,a.drugspecificat"+
" from  (select distinct  m.hospitalname,m.hospitalnum,m.drugNum,m.drugName,m.drugAlias,m.drugDetailedName, m.positionNum,m.producingarea,d.drugspecificat,d.id from (select distinct id,hospitalName,hospitalId AS hospitalnum,hdrugNum as drugNum,hdrugName as drugName,ypcdrugNum as drugAlias,"+
"ypcdrugName as drugDetailedName,ypcdrugPositionNum as positionNum,ypcdrugOriginAddress as producingarea from DrugMatching) m inner  join DrugAdmin d"+
" on m.drugAlias =d.DrugCode ) a  "
       + " where 1=1 ";
        if (medicalname != "0")
        {
            sql += "and a.drugDetailedName ='" + medicalname + "'";
        }
        if (hospitalname != "0")
        {
            sql += "and a.hospitalnum = '" + hospitalname.Trim() + "'";
        }
        if (hospitaldrugname != "0")
        {
            sql += "and a.drugName ='" + hospitaldrugname + "'";
        }
        DataTable dt = db.get_DataTable(sql);

        return dt;

    }

    //通过库房找到库房里的产品批次
    public string findproductbatchbyfromid(string fromid)
    {
        string sql1 = "select productBatch from drugadmin where id in (select drugadminid from storage where warehouse ='" + fromid + "')";

        DataTable dt = db.get_DataTable(sql1);
        string result = "";

        int count = dt.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            result += dt.Rows[i][0].ToString() + ",";

        }
        return result;

    }

    //通过药房找到药房里的产品批次
    public string findproductbatchbyfromiddrug(string fromid)
    {
        string sql1 = "select productBatch from drugadmin where id in (select drugadminid from medicalstorage where warehouse ='" + fromid + "')";

        DataTable dt = db.get_DataTable(sql1);
        string result = "";

        int count = dt.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            result += dt.Rows[i][0].ToString() + ",";

        }
        return result;

    }

       //库存统计

       public DataTable inventory(string drugname,string room)
    {


      /*  string sql2 = "  SELECT   max(id) as id,drugname,(select positionNum from drugadmin where drugname = s.drugname) as positionnum ,(select drugcode from drugadmin where drugname = s.drugname) as drugcode,Warehouse, SUM(CAST(Amount AS int)) AS inamount,";
         sql2 += " (SELECT   SUM(CAST(Amount AS int)) AS Expr1 FROM      storagefrom AS f where 1=1";
                    

                    if (drugname != "0")
                    {
                        sql2 += "and f.drugname ='" + drugname + "'";
                    }

                    if (room != "0")
                    {
                        sql2 += "and f.fromroom ='" + room + "'";
                    }
           

                    sql2+=" GROUP BY fromroom, drugname";
                    sql2 += " HAVING   (drugname = s.drugname) AND (fromroom = s.Warehouse)) AS famount FROM  Storage AS s where 1=1";
                   

                    if (drugname != "0")
                    {
                        sql2 += "and s.drugname ='"+drugname+"'";
                    }

           if (room != "0")
           {
               sql2 += "and s.warehouse ='"+room+"'";
           }
           
           
           sql2 +="GROUP BY Warehouse, drugname";

        //cast(cast(amount as int) -cast((select amount from storagefrom where drugadminid = s.drugadminid )as int)) as kcamount,(select positionnum from drugadmin where id = s.drugadminid ) as positionnum
      //  string sql = "select id, '100' as actualamount ,'0' as chazhi,(select productbatch from drugadmin where id =s.drugadminid) as productbatch,(select drugname from drugadmin where id = s.drugadminid ) as drugname,(select positionnum from drugadmin where id = s.drugadminid ) as positionnum,(select drugcode from drugadmin where id = s.drugadminid ) as drugcode,amount,(select amount from storagefrom where drugadminid = (select productbatch from drugadmin where id =s.drugadminid) ) as famount from storage as s where 1=1";
       //    if(drugname !="0"){
      //         sql += "and drugadminid in (select id from drugadmin where drugname = '" + drugname + "')";
      //     }

       //    if (room != "0")
        //   {
        //       sql += "and warehouse ='"+room+"'";
         //  }
       * */
    /*    string strSQL = "select id,Warehouse,productbatch,(select positionNum from drugadmin where productbatch = w.productbatch) as positionnum,(select drugname from drugadmin where productbatch = w.productbatch) as drugname,(select drugcode from drugadmin where productbatch = w.productbatch) as drugcode,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,date,remark,"
               + "(select sum(cast(amount as int))  from storage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as iamount,(select sum(cast(amount as int))  from storagefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as famount,"
             + "(isnull(cast((select sum(cast(amount as int))  from storage where  drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as int),0)-isnull(cast((select sum(cast(amount as int))  from storagefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as int),0)) as kucun"

            + " from WarehouseInven as w where  1=1 ";


        if (drugname != "" && drugname != "0")
        {
            strSQL += "and productbatch in ( select productbatch from drugadmin where drugname ='" + drugname + "')";

        }
        if (room != "" && room != "0")
        {
            strSQL += "and warehouse = '"+room+"'";

        }*/





        string strSQL = "select max(id) as id,max(Warehouse) as warehouse,(select productbatch from drugadmin where id = max(s.drugadminid)) as productbatch,(select positionNum from drugadmin where id = max(s.drugadminid)) as positionnum,(select drugname from drugadmin where id = max(s.drugadminid)) as drugname,(select drugcode from drugadmin where id = max(s.drugadminid)) as drugcode,isnull((select w.ActualCapacity from warehouseInven as w where id = (select max(id) from warehouseInven where warehouse = s.warehouse and productbatch =(select productbatch from drugadmin where id = s.drugadminid) )),0) as ActualCapacity,"
                + "sum(cast(amount as int)) as iamount,isnull((select sum(cast(amount as int))  from storagefrom where  drugadminid = (select productbatch from drugadmin where id = max(s.drugadminid)) and fromroom= s.warehouse),0) as famount,isnull((select sum(cast(lossnum as int))  from lossiinfor where  productbatch = (select productbatch from drugadmin where id = max(s.drugadminid)) and warehouse= s.warehouse),0) as lossnum,"
              + "(isnull(cast(sum(cast(amount as int)) as int),0)-isnull(cast((select sum(cast(amount as int))  from storagefrom where  drugadminid = (select productbatch from drugadmin where id = max(s.drugadminid)) and fromroom= s.warehouse) as int),0)- isnull((select sum(cast(lossnum as int))  from lossiinfor where  productbatch = (select productbatch from drugadmin where id = max(s.drugadminid)) and warehouse= s.warehouse),0)) as kucun"

             + " from storage as s where  1=1 ";


             
                    if (drugname != "0")
                    {
                        strSQL += "and s.drugname ='" + drugname + "'";
                    }

                    if (room != "0")
                    {
                        strSQL += "and s.Warehouse ='" + room + "'";
                    }



                    strSQL += "group by drugadminid,warehouse";







      DataTable dt = db.get_DataTable(strSQL);


        return dt;

    }
       //药房库存统计drug

       public DataTable druginventory(string drugname, string room)
       {

           //cast(cast(amount as int) -cast((select amount from storagefrom where drugadminid = s.drugadminid )as int)) as kcamount,(select positionnum from drugadmin where id = s.drugadminid ) as positionnum
         //  string strSQL = "select id,Warehouse,productbatch,(select positionNum from drugadmin where productbatch = w.productbatch) as positionnum,(select drugname from drugadmin where productbatch = w.productbatch) as drugname,(select drugcode from drugadmin where productbatch = w.productbatch) as drugcode,InventoryPer,ActualCapacity,InventoryStatus,StorageCondition,time,date,remark,"
          //     + "(select sum(cast(amount as int))  from medicalstorage where drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as iamount,(select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as famount,"
       //      + "(isnull(cast((select sum(cast(amount as int))  from medicalstorage where  drugadminid = (select id from drugadmin where productbatch = w.productbatch) and warehouse = w.warehouse) as int),0)-isnull(cast((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = w.productbatch and fromroom= w.warehouse) as int),0)) as kucun"

       //     + " from WarehouseInvenmedical as w where  1=1 ";


           string strSQL = "select max(id) as id,max(Warehouse) as warehouse,(select productbatch from drugadmin where id = max(s.drugadminid)) as productbatch,(select positionNum from drugadmin where id = max(s.drugadminid)) as positionnum,(select drugname from drugadmin where id = max(s.drugadminid)) as drugname,(select drugcode from drugadmin where id = max(s.drugadminid)) as drugcode,isnull((select w.ActualCapacity from warehouseInvenmedical as w where id = (select max(id) from warehouseInvenmedical where warehouse = s.warehouse and productbatch =(select productbatch from drugadmin where id = s.drugadminid) )),0) as ActualCapacity,"
              + "sum(cast(amount as int)) as iamount,isnull((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = (select productbatch from drugadmin where id = max(s.drugadminid)) and fromroom= s.warehouse),0) as famount,isnull((select sum(cast(lossnum as int))  from lossiinfomedical where  productbatch = (select productbatch from drugadmin where id = max(s.drugadminid)) and warehouse= s.warehouse),0) as lossnum,"
            + "(isnull(cast(sum(cast(amount as int)) as int),0)-isnull(cast((select sum(cast(amount as int))  from medicalstoragefrom where  drugadminid = (select productbatch from drugadmin where id = max(s.drugadminid)) and fromroom= s.warehouse) as int),0)- isnull((select sum(cast(lossnum as int))  from lossiinfomedical where  productbatch = (select productbatch from drugadmin where id = max(s.drugadminid)) and warehouse= s.warehouse),0)) as kucun,"
            +"(SELECT COUNT(*) AS Expr1 FROM ypcDrug WHERE (drugAlias=(SELECT DrugCode FROM DrugAdmin AS DrugAdmin_8 WHERE (id = MAX(s.drugadminid)))) AND (drugDetailedName=(SELECT DrugName FROM DrugAdmin AS DrugAdmin_9 WHERE (id = MAX(s.drugadminid))))) AS consume "
           + " from medicalstorage as s where  1=1 ";


           if (drugname != "" && drugname != "0")
           {
               strSQL += "and drugname in ( select drugname from drugadmin where drugname ='" + drugname + "')";

           }
           if (room != "" && room != "0")
           {
               strSQL += "and s.warehouse = '" + room + "'";

           }

           strSQL += "group by drugadminid,warehouse order by id desc";
           DataTable dt = db.get_DataTable(strSQL);


           return dt;

       }
       #region 入库作废信息
       public int deleteStorageInfor(int nRecipeId)
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
           }
           return end; ;
       }
       public int deleteStoragequeryInfor(int nRecipeId)
       {

           int end = 0;
           string strSql = "";
           string str = "select * from Storage where  id = '" + nRecipeId + "'";
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
                       
                       
                           strSql = "update storage set Amount ='0' where  id = '" + nRecipeId + "'";
                      
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
           }
           return end; ;
       }
       #endregion



       //模糊查询药品名
       public string getdrugname(String text)
       {

           if (text == "")
           {
               text = "1000000";
           }
         //  string sql = "select DrugName from Drugadmin where  DrugName like '%" + text + "%'";

           string sql = "SELECT DrugName FROM DrugAdmin WHERE (DrugName LIKE '%" + text + "%') GROUP BY DrugName";
           

           DataTable dt = db.get_DataTable(sql);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }

           return result;

       }


       //模糊查询药品名
       public string getdrugname(String text,string fromid)
       {

           if (text == "")
           {
               text = "1000000";
           }
           string sql = "select distinct DrugName from Drugadmin where id in (select drugadminid from storage where warehouse = '" + fromid + "')  and  DrugName like '%" + text + "%'";


           DataTable dt = db.get_DataTable(sql);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }

           return result;

       }



       //模糊查询药品名
       public string getdrugnamemedical(String text, string fromid)
       {

           if (text == "")
           {
               text = "1000000";
           }
           string sql = "select distinct DrugName from Drugadmin where id in (select drugadminid from medicalstorage where warehouse = '" + fromid + "')  and  DrugName like '%" + text + "%'";


           DataTable dt = db.get_DataTable(sql);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }

           return result;

       }


       public string getproductbatchbydrugnamemedical(string drugname,string fromid)
       {
           string sql1 = "select productBatch from drugadmin where DrugName = '" + drugname + "' and id in (select drugadminid from medicalstorage where warehouse ='" + fromid + "')";

           DataTable dt = db.get_DataTable(sql1);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }
           return result;
       }


       public string getproductbatchbydrugnamekufang(string drugname, string fromid)
       {
           string sql1 = "select productBatch from drugadmin where DrugName = '" + drugname + "' and id in (select drugadminid from storage where warehouse ='" + fromid + "')";

           DataTable dt = db.get_DataTable(sql1);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }
           return result;
       }



       public string getproductbatchbydrugname(string drugname)
       {
           string sql1 = "select productBatch from drugadmin where DrugName = '" + drugname + "'";

           DataTable dt = db.get_DataTable(sql1);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }
           return result;
       }

       //得到药品编号
       public string getdrugnumbydrugname(string drugname)
       {
           string sql1 = "select drugcode from drugadmin where DrugName = '" + drugname + "'";

           DataTable dt = db.get_DataTable(sql1);
           string result = "";

           int count = dt.Rows.Count;
           for (int i = 0; i < count; i++)
           {
               result += dt.Rows[i][0].ToString() + ",";

           }
           return result;
       }


       public int ischeckoutnum(string num,string fromid,string drugnum)
       {

           string sql = "select * from drugadmin where productbatch = '" + drugnum + "'";
           SqlDataReader sdr = db.get_Reader(sql);
           string drugadminid = "";
           if(sdr.Read()){
               drugadminid = sdr["id"].ToString();

           }
           string innum = "0";
           string sql1 = "select  SUM(CAST(Amount AS int)) as innum from storage as s where warehouse = '" + fromid + "' and drugadminid='" + drugadminid + "'";
           SqlDataReader sdr1 = db.get_Reader(sql1);
           if (sdr1.Read())
           {

               innum = sdr1["innum"].ToString();

               if (innum == "")
               {
                   innum = "0";
               }


           }

           string outnum = "0";
           string sql2 = "select  SUM(CAST(Amount AS int)) as outnum from storagefrom as s where storageroom = '" + fromid + "' and drugadminid='" + drugnum + "'";
           SqlDataReader sdr2 = db.get_Reader(sql2);
           if (sdr2.Read())
           {

               outnum = sdr2["outnum"].ToString();

               if (outnum == "")
               {
                   outnum = "0";
               }
           }



           string lossnum = "0";
           string sql3 = "select  SUM(CAST(lossnum AS int)) as lossnum from LossiInfor as l where warehouse = '" + fromid + "' and productbatch='" + drugnum + "'";
           SqlDataReader sdr3 = db.get_Reader(sql3);
           if (sdr3.Read())
           {

               lossnum = sdr3["lossnum"].ToString();

               if (lossnum == "")
               {
                   lossnum = "0";
               }
           }

           int a = Convert.ToInt32(innum);
           int remainingnum = Convert.ToInt32(innum) - Convert.ToInt32(outnum) - Convert.ToInt32(lossnum);

           int currentnum = Convert.ToInt32(num);

           if (remainingnum >= currentnum)
           {
               return -100;
           }
           else
           {
               return remainingnum;
           }
 
       }



       //
       public int ischeckoutnummedical(string num, string fromid, string drugnum)
       {

           string sql = "select * from drugadmin where productbatch = '" + drugnum + "'";
           SqlDataReader sdr = db.get_Reader(sql);
           string drugadminid = "";
           if (sdr.Read())
           {
               drugadminid = sdr["id"].ToString();

           }
           string innum = "0";
           string sql1 = "select  SUM(CAST(Amount AS int)) as innum from medicalstorage as s where warehouse = '" + fromid + "' and drugadminid='" + drugadminid + "'";
           SqlDataReader sdr1 = db.get_Reader(sql1);
           if (sdr1.Read())
           {

               innum = sdr1["innum"].ToString();

               if (innum == "")
               {
                   innum = "0";
               }


           }

           string outnum = "0";
           string sql2 = "select  SUM(CAST(Amount AS int)) as outnum from medicalstoragefrom as s where fromroom = '" + fromid + "' and drugadminid='" + drugnum + "'";
           SqlDataReader sdr2 = db.get_Reader(sql2);
           if (sdr2.Read())
           {

               outnum = sdr2["outnum"].ToString();

               if (outnum == "")
               {
                   outnum = "0";
               }
           }


           string lossnum = "0";
           string sql3 = "select  SUM(CAST(lossnum AS int)) as lossnum from lossiInfomedical as l where warehouse = '" + fromid + "' and productbatch='" + drugnum + "'";
           SqlDataReader sdr3 = db.get_Reader(sql3);
           if (sdr3.Read())
           {

               lossnum = sdr3["lossnum"].ToString();

               if (lossnum == "")
               {
                   lossnum = "0";
               }
           }

           int a = Convert.ToInt32(innum);
           int remainingnum = Convert.ToInt32(innum) - Convert.ToInt32(outnum) - Convert.ToInt32(lossnum);

           int currentnum = Convert.ToInt32(num);

           if (remainingnum >= currentnum)
           {
               return -100;
           }
           else
           {
               return remainingnum;
           }

           //int a = Convert.ToInt32(innum);
           //int remainingnum = Convert.ToInt32(innum) - Convert.ToInt32(outnum) - Convert.ToInt32(lossnum);

           //int currentnum = Convert.ToInt32(num);

           //if (remainingnum >= currentnum)
           //{
           //    return -100;
           //}
           //else
           //{
           //    return remainingnum;
           //}

       }


       //计算库房指定批次的库存量
       public int remainingnum(string fromid, string drugnum)
       {

           string sql = "select * from drugadmin where productbatch = '" + drugnum + "'";
           SqlDataReader sdr = db.get_Reader(sql);
           string drugadminid = "";
           if (sdr.Read())
           {
               drugadminid = sdr["id"].ToString();

           }
           string innum = "0";
           string sql1 = "select  SUM(CAST(Amount AS int)) as innum from storage as s where warehouse = '" + fromid + "' and drugadminid='" + drugadminid + "'";
           SqlDataReader sdr1 = db.get_Reader(sql1);
           if (sdr1.Read())
           {
              
               innum = sdr1["innum"].ToString();

               if (innum == "")
               {
                   innum = "0";
               }
              

           }
          
           string outnum = "0";
           string sql2 = "select  SUM(CAST(Amount AS int)) as outnum from storagefrom as s where fromroom = '" + fromid + "' and drugadminid='" + drugnum + "'";
           SqlDataReader sdr2 = db.get_Reader(sql2);
           if (sdr2.Read())
           {
               
               outnum = sdr2["outnum"].ToString();

               if (outnum=="")
               {
                   outnum = "0";
               }
           }



           string lossnum = "0";
           string sql3 = "select  SUM(CAST(lossnum AS int)) as lossnum from LossiInfor as l where warehouse = '" + fromid + "' and productbatch='" + drugnum + "'";
           SqlDataReader sdr3 = db.get_Reader(sql3);
           if (sdr3.Read())
           {

               lossnum = sdr3["lossnum"].ToString();

               if (lossnum == "")
               {
                   lossnum = "0";
               }
           }

           int a = Convert.ToInt32(innum);
           int remainingnum = Convert.ToInt32(innum) - Convert.ToInt32(outnum) - Convert.ToInt32(lossnum);

           return remainingnum;
       }

       //
       //计算药房指定批次的库存量
       public int remainingnummedical(string fromid, string drugnum)
       {

           string sql = "select * from drugadmin where productbatch = '" + drugnum + "'";
           SqlDataReader sdr = db.get_Reader(sql);
           string drugadminid = "";
           if (sdr.Read())
           {
               drugadminid = sdr["id"].ToString();

           }
           string innum = "0";
           string sql1 = "select  SUM(CAST(Amount AS int)) as innum from medicalstorage as s where warehouse = '" + fromid + "' and drugadminid='" + drugadminid + "'";
           SqlDataReader sdr1 = db.get_Reader(sql1);
           if (sdr1.Read())
           {

               innum = sdr1["innum"].ToString();

               if (innum == "")
               {
                   innum = "0";
               }


           }

           string outnum = "0";
           string sql2 = "select  SUM(CAST(Amount AS int)) as outnum from medicalstoragefrom as s where fromroom = '" + fromid + "' and drugadminid='" + drugnum + "'";
           SqlDataReader sdr2 = db.get_Reader(sql2);
           if (sdr2.Read())
           {

               outnum = sdr2["outnum"].ToString();

               if (outnum == "")
               {
                   outnum = "0";
               }
           }


           string lossnum = "0";
           string sql3 = "select  SUM(CAST(lossnum AS int)) as lossnum from lossiInfomedical as l where warehouse = '" + fromid + "' and productbatch='" + drugnum + "'";
           SqlDataReader sdr3 = db.get_Reader(sql3);
           if (sdr3.Read())
           {

               lossnum = sdr3["lossnum"].ToString();

               if (lossnum == "")
               {
                   lossnum = "0";
               }
           }


           int a = Convert.ToInt32(innum);
           int remainingnum = Convert.ToInt32(innum) - Convert.ToInt32(outnum) - Convert.ToInt32(lossnum);

           return remainingnum;



       }

    }
}
