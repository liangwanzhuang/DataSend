using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;

namespace ModelInfo
{
    public class EnterDrug
    {
        public bool AddDrug(DrugInfo dinfo)
        {
             DataBaseLayer db = new DataBaseLayer();


              int n = 0;
              //string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 1 and prescriptionId  =  (select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
              string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 0 and prescriptionId  =  (select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
              db.write_log_txt("insert drug_2:"+stateSql);
              SqlDataReader srd = db.get_Reader(stateSql);
            //string q = srd["Pspnum"].ToString();
            if (srd.Read())
            {
                n = 0;
            }
            else
            {
                string stateSql1 = "select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "'";
                SqlDataReader srd1 = db.get_Reader(stateSql1);
                string pid = "";
                if (srd1.Read())
                {
                    pid = srd1["id"].ToString();
                }

                string str = "select * from drug where pid='" + pid + "' and drugnum ='" + dinfo.strDrugNum + "' and drugname ='" + dinfo.strDrugName + "'";
                SqlDataReader srd4 = db.get_Reader(str);

                if (srd4.Read())
                {
                    n=-1;
                }
                else 
                {
                    string strSql = "insert into drug(customid,Hospitalid,Pspnum,drugnum,drugname,drugdescription,";
                    strSql += "drugposition,drugallnum,drugweight,tienum,description,wholesaleprice,retailprice,pid) ";
                    strSql += "values(" + dinfo.nCustomId + "," + dinfo.nHospitalNum + ",'" + dinfo.strPspnum + "',";
                    strSql += "'" + dinfo.strDrugNum + "','" + dinfo.strDrugName + "','" + dinfo.strDrugDsp + "','" + dinfo.strDrugPosition + "',";
                    strSql += "" + dinfo.nAllNum + "," + dinfo.dWeight + "," + dinfo.nTieNum + ",'" + dinfo.strDsp + "'," + dinfo.dWholeSalePrice + ",";
                    strSql += "" + dinfo.dRetailPrice + ",'" + pid + "')";

                    db.write_log_txt("药品录入："+strSql);
                    n = db.cmd_Execute(strSql);
                }
            }
            if ( n > 0)
            {
                return true;
            }
            else if (n == 0)
            {
              //  Response.Write("<script>alert('录入药品重复');window.parent.loginview();</script>");
                return true;
            }
            else
            {
                return true;
            }

        }
        public bool AddDrug_1(DrugInfo dinfo)
        {
            DataBaseLayer db = new DataBaseLayer();


            int n = 0;
            //string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 1 and prescriptionId  =  (select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
            string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 0 and prescriptionId  in(select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
            db.write_log_txt("insert drug_2:" + stateSql);
            SqlDataReader srd = db.get_Reader(stateSql);
            //string q = srd["Pspnum"].ToString();
         //   if (srd.Read())
         //   {
           //     n = 0;
          //  }
          //  else
          //  {
                string stateSql1 = "select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "'";
                SqlDataReader srd1 = db.get_Reader(stateSql1);
                string pid = "";
                if (srd1.Read())
                {
                    pid = srd1["id"].ToString();
                }

                string str = "select * from drug where pid='" + pid + "' and drugnum ='" + dinfo.strDrugNum + "' and drugname ='" + dinfo.strDrugName + "'";
                SqlDataReader srd4 = db.get_Reader(str);

                if (srd4.Read())
                {
                    n = -1;
                }
                else
                {
                    string strSql = "insert into drug(customid,Hospitalid,Pspnum,drugnum,drugname,drugdescription,";
                    strSql += "drugposition,drugallnum,drugweight,tienum,description,pid) ";
                    strSql += "values(" + dinfo.nCustomId + "," + dinfo.nHospitalNum + ",'" + dinfo.strPspnum + "',";
                    strSql += "'" + dinfo.strDrugNum + "','" + dinfo.strDrugName + "','" + dinfo.strDrugDsp + "','" + dinfo.strDrugPosition + "',";
                    strSql += "" + dinfo.nAllNum + "," + dinfo.dWeight + "," + dinfo.nTieNum + ",'" + dinfo.strDsp + "'," +"'" + pid + "')";

                    db.write_log_txt("药品录入：" + strSql);
                    n = db.cmd_Execute(strSql);
                }
          //  }
            if (n > 0)
            {
                return true;
            }
            else if (n == 0)
            {
                //  Response.Write("<script>alert('录入药品重复');window.parent.loginview();</script>");
                return true;
            }
            else
            {
                return true;
            }

        }
        public bool AddDrug_1_new(DrugInfo dinfo)
        {
            DataBaseLayer db = new DataBaseLayer();


            int n = 0;
            //string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 1 and prescriptionId  =  (select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
            string stateSql = "select prescriptionId  from PrescriptionCheckState where checkStatus = 0 and prescriptionId  in(select  id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "')";
            db.write_log_txt("insert drug_2:" + stateSql);
            SqlDataReader srd = db.get_Reader(stateSql);
            //string q = srd["Pspnum"].ToString();
            //   if (srd.Read())
            //   {
            //     n = 0;
            //  }
            //  else
            //  {
            /*
            string stateSql1 = "select top 1 id  from prescription as p where p.Pspnum = '" + dinfo.strPspnum + "' and p.hospitalid = '" + dinfo.nHospitalNum + "'";
            SqlDataReader srd1 = db.get_Reader(stateSql1);
            string pid = "";
            if (srd1.Read())
            {
                pid = srd1["id"].ToString();
            }
            */
            string str = "select * from drug where pid='" + dinfo.pid + "' and drugnum ='" + dinfo.strDrugNum + "' and drugname ='" + dinfo.strDrugName + "'";
            SqlDataReader srd4 = db.get_Reader(str);

            if (srd4.Read())
            {
                n = -1;
            }
            else
            {
                string strSql = "insert into drug(customid,Hospitalid,Pspnum,drugnum,drugname,drugdescription,";
                strSql += "drugposition,drugallnum,drugweight,tienum,description,pid) ";
                strSql += "values(" + dinfo.nCustomId + "," + dinfo.nHospitalNum + ",'" + dinfo.strPspnum + "',";
                strSql += "'" + dinfo.strDrugNum + "','" + dinfo.strDrugName + "','" + dinfo.strDrugDsp + "','" + dinfo.strDrugPosition + "',";
                strSql += "" + dinfo.nAllNum + "," + dinfo.dWeight + "," + dinfo.nTieNum + ",'" + dinfo.strDsp + "'," + "'" + dinfo.pid + "')";

                db.write_log_txt("药品录入：" + strSql);
                n = db.cmd_Execute(strSql);
            }
            //  }
            if (n > 0)
            {
                return true;
            }
            else if (n == 0)
            {
                //  Response.Write("<script>alert('录入药品重复');window.parent.loginview();</script>");
                return true;
            }
            else
            {
                return true;
            }

        }
    }
}
