using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;
using System.Data.SqlClient;

namespace ModelInfo
{
    public class DeliveryHandler
    {
        public DataBaseLayer db = new DataBaseLayer();
        public int AddDelivery(int userid, string wordDate, string barcode, string tisaneNum, string imgname, string userName,string kd_type,string kd_num,string demo)
        {
         //  string sql = "insert into Delivery(employeeId,SendTime,barcode,DecoctingNum,imgname,Sendstate,Sendpersonnel,kd_type,kd_num,demo) values('" +
            //   userid + "','" + wordDate + "','" + barcode + "','" + tisaneNum + "','" + imgname + "','1" + "','"
            //   + userName + "','" + kd_type + "','" + kd_num + "','" + demo + "')";

            #region 添加发货时判断包装是否完成，未完成，设置包装结束时间：包装开始时间+包装代数*预设时间
            db.sp_Execute_no_return("sp_auto_packing",Convert.ToInt32(tisaneNum));
            #endregion
            string sql = "update  Delivery set employeeId='" + userid + "',SendTime='" + wordDate + "',barcode='" + barcode + "',imgname='" + imgname
                + "',Sendstate='1',Sendpersonnel='"+ userName 
                + "',kd_type='" + kd_type  + "'"
                + ",kd_num='" + kd_num + "'"
                + ",demo='" + demo + "'"
                + " where DecoctingNum='" + tisaneNum + "'";
            string sql2 = "update prescription set doperson ='" + userName + "',curstate = '已发货'  where id = '" + tisaneNum + "'";
          //  Console.WriteLine("sql:" + sql);
          //  Console.ReadLine();
           // Console.WriteLine("sql:" + sql2);
            db.cmd_Execute(sql2);
            //写log文件
            db.write_log_txt("sql:" + sql + "-------" + "sql:" + sql2);
            return db.cmd_Execute(sql);

        }
        #region 通过权限查询人员
        public SqlDataReader findNameAll()
        {
            string sql = "select * from  Employee where Role ='6' or  Role ='0' ";

            return db.get_Reader(sql);
        }

        #endregion
        public int AddDelivery(string id, int DeNum, string Sendpersonnel, string SendTime, string Remarks, string dtbtype, string logisticsnum, string kd_type, string kd_num, string demo)
        {
            /// <summary>
            /// 添加发货信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>

            #region 添加发货时判断包装是否完成，未完成，设置包装结束时间：包装开始时间+包装代数*预设时间
            db.sp_Execute_no_return("sp_auto_packing", DeNum);
            #endregion
            String sql = "";
            int end = 0;
            string per = Sendpersonnel.Substring(6);

            string employeeid = "";
            string str6 = "select id from employee where EmNumAName ='" + Sendpersonnel + "'";
            SqlDataReader sr6 = db.get_Reader(str6);

            if (sr6.Read())
            {

                employeeid = sr6["id"].ToString();

            }

           // string all = "select DecoctingNum from Delivery where DecoctingNum not in (select pid from InvalidPrescription) and DecoctingNum = " + DeNum + "";
           // SqlDataReader allinfo = db.get_Reader(all);
           // if (allinfo.Read())
            //{
                string tate = "select Sendstate from Delivery where DecoctingNum = " + DeNum + "";
                SqlDataReader tate1 = db.get_Reader(tate);
                if (tate1.Read())
                {
                    if (id == DeNum.ToString() && tate1["Sendstate"].ToString() == "1")
                    {
                        sql = "";

                    }
                    else
                    {
                        sql = "Update Delivery set Sendpersonnel='" + per + "',  SendTime ='" + SendTime + "',  Sendstate='1' ,  Remarks='" + Remarks + "' ,employeeid ='" + employeeid + "', logisticsnum ='" + logisticsnum
                             + "',kd_type='" + kd_type + "',"
                + "kd_num='" + kd_num + "',"
                + "demo='" + demo + "'"
                            + " where DecoctingNum='" + DeNum + "'";
                        //写log文件
                        db.write_log_txt("sql:" + sql );
                        if (db.cmd_Execute(sql) == 1)
                        {

                            sql = "update prescription set doperson ='" + per + "',curstate = '已发货' ,dtbtype ='" + dtbtype + "' where id = '" + DeNum + "'";
                        }


                    }
                }
                else
                {
                    sql = "";
                }
           // }
           // else {
              //  sql = "";
            //
           // }

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
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public DataTable SearchDelivery()
        {
            string strSql = "select id ,DecoctingNum,Sendpersonnel,SendTime,Sendstate,Starttime,Remarks,kd_type,kd_num,demo from Delivery";
            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #region 通过id查询煎药条码


        public string  finDecotingBarbyId(int id)
        {  /*  //通过id查询煎药条码
            string strSql = "select    (select    RIGHT(CAST('0' + RTRIM(dose *takenum  ) AS varchar(20)), 2)  as b   from prescription as p where  p.id in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as packNum  ," 
                +"(select    RIGHT(CAST('0' + RTRIM(decscheme) AS varchar(20)), 2)  as b   from prescription as p where  p.id in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as DeScheme, "
                +"(select    RIGHT(CAST('000' + RTRIM( packagenum ) AS varchar(20)), 4)  as b   from prescription as p where  p.id in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as packAcount  ,"
                + "(select    RIGHT(CAST('0' + RTRIM( oncetime) AS varchar(20) ), 2)  as b   from prescription as p where  p.id in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as OTime  ,"
                + "(select    RIGHT(CAST('0' + RTRIM( twicetime ) AS varchar(20)), 2)  as b   from prescription as p where  p.id in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as TTime  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( machineid ) AS varchar(20)), 2)  as b   from tisaneinfo as t where  t.pid in (select DecoctingNum from Delivery as d where   id = " + id + ") ) as hao  ,"

               + " RIGHT(CAST('000000000' + RTRIM(DecoctingNum) AS varchar(20)), 10)  as bNum , DecoctingNum from Delivery as d where   id = " + id;*/
           //通过煎药单号查询煎药条码，下面id获取的是煎药单号
            string strSql = "select    (select    RIGHT(CAST('0' + RTRIM(dose *takenum  ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as packNum  ,"
               + "(select    RIGHT(CAST('0' + RTRIM(decscheme) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as DeScheme, "
               + "(select    RIGHT(CAST('000' + RTRIM( packagenum ) AS varchar(20)), 4)  as b   from prescription as p where  p.id  = " + id + ") as packAcount  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( oncetime) AS varchar(20) ), 2)  as b   from prescription as p where  p.id  = " + id + ")  as OTime  ,"
               + "(select    RIGHT(CAST('0' + RTRIM( twicetime ) AS varchar(20)), 2)  as b   from prescription as p where  p.id  = " + id + ")  as TTime  ,"
              + "(select    RIGHT(CAST('0' + RTRIM( machineid ) AS varchar(20)), 2)  as b   from tisaneinfo as t where  t.pid   = " + id + ") as hao  ,"

              + " RIGHT(CAST('000000000' + RTRIM(DecoctingNum) AS varchar(20)), 10)  as bNum , DecoctingNum from Delivery as d where   DecoctingNum = " + id;
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
      
        #endregion


    }
    }

