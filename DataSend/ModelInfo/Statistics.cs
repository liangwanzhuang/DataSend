using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using SQLDAL;
using System.Collections;
using System.Data;


namespace ModelInfo
{
    public class Statistics
    {
        //大屏显示包装信息
        public string getpackInfo()
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


            string str2 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select DecoctingNum from Packing where Fpactate =0)";
            SqlDataReader sdr2 = db.get_Reader(str2);
            string ct2 = "";
            if (sdr2.Read())
            {
                ct2 = sdr2["ct"].ToString();
            }

            string result = "";
            result = "当日接单数:" + ct + "  " + " 已包装:" + ct2;



            return result;
        }


        /// <summary>
        ///  综合大屏累计数量显示
        /// </summary>
        /// <returns></returns>
        public string getCompositeInfo()
        {
            DataBaseLayer db = new DataBaseLayer();
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间

            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");
            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");

            string str = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "'";
            SqlDataReader sdr = db.get_Reader(str);
            string OrderNum = "";
            if (sdr.Read())
            {
                OrderNum = sdr["ct"].ToString();
            }
            sdr.Close();
            string str1 = "select count(*) as ct from prescription where id in (select pid from bubble where starttime between '" + strS + "' and '" + strS2 + "' )";
            SqlDataReader sdr1 = db.get_Reader(str1);
            string displayNum = "";
            if (sdr1.Read())
            {
                displayNum = sdr1["ct"].ToString();
            }
            sdr1.Close();

            string str2 = "select count(*) as ct from prescription as a right join tisaneinfo as b on b.Pid=a.ID where b.starttime between '" + strS + "' and '" + strS2 + "' ";
            SqlDataReader sdr2 = db.get_Reader(str2);
            string bmNum = "";
            if (sdr2.Read())
            {
                bmNum = sdr2["ct"].ToString();
            }
            sdr2.Close();

            string str3 = "select count(*) as ct from prescription as a right join packing as b on b.DecoctingNum=a.ID where b.starttime between '" + strS + "' and '" + strS2 + "' ";
            SqlDataReader sdr3 = db.get_Reader(str3);
            string packNum = "";
            if (sdr3.Read())
            {
                packNum = sdr3["ct"].ToString();
            }
            sdr3.Close();

            return OrderNum + "," + displayNum + "," + bmNum + "," + packNum;
        }

        public string countall()
        {

            DataBaseLayer db = new DataBaseLayer();

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;//当前时间


            string strS = currentTime.ToString("yyyy/MM/dd 00:00:00");

            string strS2 = currentTime.ToString("yyyy/MM/dd 23:59:59");


            string num;
            string num1 = "";
            string num2 = "";
            string num3 = "";
            string num4 = "";
            string num5 = "";
            string num6 = "";
            string num7 = "";
            string num8 = "";
            string num9 = "";
            string num10 = "";
            string num11 = "";
            string num12 = "";
            string str1 = "select count(*) as num1 from bubble where endDate between '" + strS + "' and  '" + strS2 + "'";
            //
            string str2 = "select count(*) as num2 from tisaneinfo where endDate between '" + strS + "' and  '" + strS2 + "'";
            //包装
            String str3 = "select count(*) as num3 from Packing where pacTime between '" + strS + "' and  '" + strS2 + "'";
            //发货
            string str4 = "select count(*) as num4 from Delivery where sendtime between '" + strS + "' and  '" + strS2 + "'";
            //复核
            string str5 = "select count(*) as num5 from Audit where audittime between '" + strS + "' and  '" + strS2 + "'";
            //调剂
            string str6 = "select count(*) as num6 from adjust  where endDate between '" + strS + "' and  '" + strS2 + "'";
            //已匹配
            //string str7 = "select count(*) as num7 from DrugMatching where  status =1";
            string str7 = "select count(*) as num7 from DrugMatching where pspId IS NULL";
            //打印
            string str8 = "select count(*) as num8 from  printstatus where printstatus  =1";
            //已审核
            string str9 = "select count(*) as num9 from  PrescriptionCheckState where partytime between '" + strS + "' and  '" + strS2 + "'";
            //已录入
            string str10 = "select count(*) as num10 from  Prescription where dotime between '" + strS + "' and  '" + strS2 + "'";
            //未打印
            string str11 = "select count(*) as num11 from  printstatus where printstatus  =0";
            //未匹配
            // string str12 = "select count(*) as num12 from DrugMatching where  status =2";
            //string str12 = "select count(*) as num12 from prescription as p right join DrugMatching dm on p.id=dm.pspId";
            string str12 = "select count(*)as num12 from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.Pspnum=p.Pspnum and p.Hospitalid = d.Hospitalid left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and dm.drugId IS NULL AND d.ID IS NOT NULL  and p.dotime between '" + strS + "' and  '" + strS2 + "'";

            ///  string str12 = "select count(*) from prescription as p left join PrescriptionCheckState pcs on p.id=pcs.prescriptionId left join drug d on d.Pspnum=p.Pspnum and p.Hospitalid = d.Hospitalid left join DrugMatching dm on d.id=dm.drugId and dm.pspId = p.ID where pcs.prescriptionId IS NULL and dm.drugId IS NULL AND d.ID IS NOT NULL and p.id not in (select pid from InvalidPrescription)";
            ///


            SqlDataReader sr1 = db.get_Reader(str1);
            if (sr1.Read())
            {

                num1 = sr1["num1"].ToString();

            }

            SqlDataReader sr2 = db.get_Reader(str2);
            if (sr2.Read())
            {

                num2 = sr2["num2"].ToString();

            }
            //包装
            SqlDataReader sr3 = db.get_Reader(str3);
            if (sr3.Read())
            {
                num3 = sr3["num3"].ToString();
            }
            //发货
            SqlDataReader sr4 = db.get_Reader(str4);
            if (sr4.Read())
            {
                num4 = sr4["num4"].ToString();
            }
            //复核
            SqlDataReader sr5 = db.get_Reader(str5);
            if (sr5.Read())
            {
                num5 = sr5["num5"].ToString();
            }
            //调剂
            SqlDataReader sr6 = db.get_Reader(str6);
            if (sr6.Read())
            {
                num6 = sr6["num6"].ToString();
            }
            //匹配
            SqlDataReader sr7 = db.get_Reader(str7);
            if (sr7.Read())
            {
                num7 = sr7["num7"].ToString();
            }
            //打印
            SqlDataReader sr8 = db.get_Reader(str8);
            if (sr8.Read())
            {
                num8 = sr8["num8"].ToString();
            }
            //审核
            SqlDataReader sr9 = db.get_Reader(str9);
            if (sr9.Read())
            {
                num9 = sr9["num9"].ToString();
            }
            //录入
            SqlDataReader sr10 = db.get_Reader(str10);
            if (sr10.Read())
            {
                num10 = sr10["num10"].ToString();
            }
            //未打印
            SqlDataReader sr11 = db.get_Reader(str11);
            if (sr11.Read())
            {
                num11 = sr11["num11"].ToString();
            }
            //未匹配
            SqlDataReader sr12 = db.get_Reader(str12);
            if (sr12.Read())
            {
                num12 = sr12["num12"].ToString();
            }
            num = num1 + "," + num2 + "," + num3 + "," + num4 + "," + num5 + "," + num6 + "," + num7 + "," + num8 + "," + num9 + "," + num10 + "," + num11 + "," + num12;

            return num;
        }



        //大屏显示泡药信息

        public string getglobalinfo()
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


            string str2 = "select count(*) as ct from prescription where id in (select pid from bubble where starttime between '" + strS + "' and '" + strS2 + "' )";
            SqlDataReader sdr2 = db.get_Reader(str2);
            string ct2 = "";
            if (sdr2.Read())
            {
                ct2 = sdr2["ct"].ToString();
            }
            string result = "";
            result = "当日接单数:" + ct + "  " + " 已泡药:" + ct2;
            return result;
        }




        //大屏显示煎药信息

        public string gettisaneinfo()
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


            string str2 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select pid from tisaneinfo)";
            SqlDataReader sdr2 = db.get_Reader(str2);
            string ct2 = "";
            if (sdr2.Read())
            {
                ct2 = sdr2["ct"].ToString();
            }

            string result = "";
            result = "当日接单数:" + ct + "  " + " 已煎药:" + ct2;



            return result;
        }


        //大屏显示发药信息

        public string getdeliveryinfo()
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
            string str2 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select decoctingnum from delivery where sendstate =0) AND (Hospitalid IN (SELECT   ID FROM Hospital WHERE (DrugSendDisplayState = '0')))";



            SqlDataReader sdr2 = db.get_Reader(str2);
            string ct2 = "";
            if (sdr2.Read())
            {
                ct2 = sdr2["ct"].ToString();
            }

            string str3 = "select count(*) as ct from prescription where dotime between '" + strS + "' and '" + strS2 + "' and id in (select decoctingnum from delivery where sendstate =1)";
            SqlDataReader sdr3 = db.get_Reader(str3);
            string ct3 = "";
            if (sdr3.Read())
            {
                ct3 = sdr3["ct"].ToString();
            }

            string result = "";
            result = "当日接单数:" + ct + "  " + " 待发货:" + ct2 + "" + "已发货:" + ct3;



            return result;
        }


    }
}
