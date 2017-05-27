using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;
using System.Data.SqlClient;

namespace ModelInfo
{

    public class HospitalHandler
    {
        public DataBaseLayer db = new DataBaseLayer();
        public int AddHospital(string hname, string hshortname, string hnum, string contacter, string phone,
            string address, string pricetype, string relation_name)
        {

            int end = 0;
            DataBaseLayer db = new DataBaseLayer();
            string tate = "select top 1  hnum   from hospital where hnum = '" + hnum + "' ";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                end = 0;
            }
            else
            {
                /*string tate2 = "select  settler   from hospital where settler = '" + settler + "' ";
                SqlDataReader tatea = db.get_Reader(tate2);
                if (tatea.Read())
                {
                    end = 0;
                }
                else
                {*/
                    //泡药显示   DrugDisplayState 煎药显示   ChineseDisplayState 发药显示  DrugSendDisplayState
                string r_sql = "select top 1 relation_id from tb_hospital_drug_type where relation_name='" + relation_name.Trim()+ "'";
                SqlDataReader r_2 = db.get_Reader(r_sql);  
                string strSql ="";
                if (r_2.Read())
                {
                   
                   strSql= "insert into hospital(hnum,hname,hshortname,contacter,phone,address,pricetype, DrugDisplayState,ChineseDisplayState,DrugSendDisplayState,relation_drug_type_id) ";
                    strSql += "values ('" + hnum + "','" + hname + "','" + hshortname + "','" + contacter + "',";
                    strSql += "'" + phone + "','" + address + "','" + pricetype + "','0','0','0',"+
                        "'"+r_2["relation_id"].ToString().Trim()+"'"+")";
                    end = db.cmd_Execute(strSql);
                    strSql = "";
                }

              



                  
               // }
            }

            return end;


        }

        public DataTable SearchHospital(string hname, string hnum)
        {
            string strSql = "select h.id,hnum,h.hname,h.Hshortname,h.contacter,h.phone,h.address,h.pricetype,h.settler,t.relation_name from hospital h " +
            " left join tb_hospital_drug_type t on h.relation_drug_type_id=t.relation_id where 1=1 ";
            if (hnum != "0" && hnum != "")
            {
                strSql += "and  hnum ='" + hnum + "'";
            }
            if (hname != "0" && hname != "")
            {
                strSql += "and  h.id ='" + hname + "'";
            }

            DataBaseLayer db = new DataBaseLayer();

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }



        public DataTable PrintRecipeInfo()
        {
            string strSql = "SELECT ID, customid, delnum, barcodescan, Hospitalid, Pspnum, decmothed, name,sex,age,phone,address, department," +
              "inpatientarea, ward, sickbed, diagresult, dose, takemethod, takenum, packagenum, decscheme, oncetime, twicetime," +
                "soakwater, soaktime, labelnum, remark, doctor, footnote, getdrugtime, getdrugnum, ordertime, curstate, dotime," +
                "doperson, dtbcompany, dtbaddress, dtbphone, dtbtype, takeway ,(select hname from hospital as h where h.id = p.hospitalid) as hspname FROM prescription as p";

            DataBaseLayer db = new DataBaseLayer();




            DataTable dt = db.get_DataTable(strSql);


            return dt;

        }

        #region 查询医院信息通过ID
        public DataTable findHospitalInfo(int id)
        {
            string strSql = "select a.*,b.relation_name from  hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id where a.id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
        #endregion
        #region 修改医院信息
        public int updateHospitalInfo(int id, string hname, string hshortname, string hnum, 
            string contacter, string phone, string address, string pricetype,string relation_name)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;
            DataBaseLayer db = new DataBaseLayer();
            string tate = "select  hnum   from hospital where hnum = '" + hnum + "' and id != '"+id+"'";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                end = 0;
            }
            else
            {
               /* string tate2 = "select  settler   from hospital where settler = '" + settler + "' and id != '" + id + "'";
                SqlDataReader tatea = db.get_Reader(tate2);
                if (tatea.Read())
                {
                    end = 0;
                }
                else
                {*/

                   string r_sql = "select top 1 relation_id from tb_hospital_drug_type where relation_name='" + relation_name.Trim()+ "'";
                SqlDataReader r_2 = db.get_Reader(r_sql);  
               string r_id="";
                if (r_2.Read())
                {
                    r_id = r_2["relation_id"].ToString().Trim();
                }
                    string sql = "update hospital set hname='" + hname + "',hshortname='" + 
                        hshortname + "',hnum='" + hnum + "',contacter='" + contacter + 
                        "',phone='" + phone + "',address='" + address + "',pricetype='"
                        + pricetype + "'," + "relation_drug_type_id="+"'"+r_id                        
                        +"' where id = " + id + "";
                    end = db.cmd_Execute(sql);
               // }
            }

                return end;
            }
        #endregion
        }
    }
