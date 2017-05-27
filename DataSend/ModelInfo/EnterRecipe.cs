using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;

namespace ModelInfo
{
    public class EnterRecipe
    {
        /// <summary>
        /// 添加处方信息
        /// </summary>
        /// <param name="rinfo"></param>
        /// <returns></returns>
        
       public DataBaseLayer db = new DataBaseLayer();
        public string AddRecipe(RecipeInfo rinfo)
        {
            string strResult = "";
              int n = 0;
              string stateSql = "select Pspnum  from prescription where Hospitalid =" + rinfo.nHospitalID + " and Pspnum ='" + rinfo.strPspnum + "'";
            SqlDataReader srd = db.get_Reader(stateSql);
            //string q = srd["Pspnum"].ToString();
            int cf_id = 0;
            if (srd.Read())
            {
                n = 0;
            }
            else
            {
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;//当前时间  
              

                /*
                string strSql = "insert into prescription(delnum,Hospitalid,Pspnum,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
                strSql += "diagresult,dose,takenum,getdrugtime,getdrugnum,decscheme,oncetime,twicetime,packagenum,dotime,doperson,";
                strSql += "dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,decmothed,takeway,takemethod,RemarksA,RemarksB)";
                strSql += " values('" + rinfo.strDelnum + "','" + rinfo.nHospitalID + "','" + rinfo.strPspnum + "',";
                strSql += "'" + rinfo.strName + "','" + rinfo.nSex + "','" + rinfo.nAge + "','" + rinfo.strPhone + "','" + rinfo.strAddress + "',";
                strSql += "'" + rinfo.strDepartment + "','" + rinfo.strInpatientAreaNum + "','" + rinfo.strWard + "','" + rinfo.strSickBed + "',";
                strSql += "'" + rinfo.strDiagResult + "','" + rinfo.strDose + "','" + rinfo.nNum + "','" + rinfo.strDrugGetTime + "','" + rinfo.strDrugGetNum + "',";
                strSql += "'" + rinfo.strScheme + "','" + rinfo.strTimeOne + "','" + rinfo.strTimeTwo + "','" + rinfo.nPackageNum + "','" + currentTime + "',";
                strSql += "'" + rinfo.strDoPerson + "','" + rinfo.strDtbCompany + "','" + rinfo.strDtbAddress + "','" + rinfo.strDtbPhone + "','" + rinfo.strDtbStyle + "',";
                strSql += "'" + rinfo.nSoakWater + "','" + rinfo.nSoakTime + "','" + rinfo.nLabelNum + "','" + rinfo.strRemark + "','" + rinfo.strDoctor + "','" + rinfo.strFootNote + "','" + rinfo.strOrderTime + "','未匹配','" + rinfo.strDecMothed + "','" + rinfo.strTakeWay + "','" + rinfo.strTakeMethod + "','" + rinfo.strRemarksA + "','" + rinfo.strRemarksB + "')";
              */


                string strSql = "insert into prescription(Hospitalid,Pspnum,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,";
                strSql += "diagresult,dose,takenum,getdrugtime,getdrugnum,decscheme,oncetime,twicetime,packagenum,dotime,doperson,";
                strSql += "dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate,decmothed,takeway,takemethod,RemarksA,RemarksB,confirmDrug)";
                strSql += " values('" + rinfo.nHospitalID + "','" + rinfo.strPspnum + "',";
                strSql += "'" + rinfo.strName + "','" + rinfo.nSex + "','" + rinfo.nAge + "','" + rinfo.strPhone + "','" + rinfo.strAddress + "',";
                strSql += "'" + rinfo.strDepartment + "','" + rinfo.strInpatientAreaNum + "','" + rinfo.strWard + "','" + rinfo.strSickBed + "',";
                strSql += "'" + rinfo.strDiagResult + "','" + rinfo.strDose + "','" + rinfo.nNum + "','" + rinfo.strDrugGetTime + "','" + rinfo.strDrugGetNum + "',";
                strSql += "'" + rinfo.strScheme + "','" + rinfo.strTimeOne + "','" + rinfo.strTimeTwo + "','" + rinfo.nPackageNum + "','" + currentTime + "',";
                strSql += "'" + rinfo.strDoPerson + "','" + rinfo.strDtbCompany + "','" + rinfo.strDtbAddress + "','" + rinfo.strDtbPhone + "','" + rinfo.strDtbStyle + "',";
                strSql += "'" + rinfo.nSoakWater + "','" + rinfo.nSoakTime + "','" + rinfo.nLabelNum + "','" + rinfo.strRemark + "','" + rinfo.strDoctor + "','" + rinfo.strFootNote + "','" + rinfo.strOrderTime + "','未匹配','" + rinfo.strDecMothed + "','" + rinfo.strTakeWay + "','" + rinfo.strTakeMethod + "','" + rinfo.strRemarksA + "','" + rinfo.strRemarksB + "',"+"'"+"0"+"'"+")";
                 n = db.cmd_Execute(strSql);

                 //插入处方打印功能
                 //获取插入处方编号
                 string get_cf_id_sql = "select top 1 id from " + "prescription where Hospitalid =" + rinfo.nHospitalID + " and  Pspnum =" + "'" + rinfo.strPspnum.Trim() + "'";
                 //int cf_id = dba.get_sql_int("2", get_cf_id_sql);
                 SqlDataReader srd_c1 = db.get_Reader(get_cf_id_sql);
               
                 while (srd_c1.Read())
                 {
                     cf_id = Int16.Parse(srd_c1["id"].ToString());
                 }
                //获取登录用户id
                 string get_employee_id_sql = "select top 1 id from Employee where EName='" + rinfo.strDoPerson.Trim() + "'";
                 string e_id = "0";
                 SqlDataReader srd_c2 = db.get_Reader(get_employee_id_sql);
                 while (srd_c2.Read())
                 {
                     e_id = srd_c2["id"].ToString();
                 }
                //插入处方打印标识
                 string insert_cf_sql_c = "  INSERT INTO PrescriptionCheckState(prescriptionId,PartyPer,PartyTime,checkStatus" +
        ",refusalreason,warningstatus,tisaneNumber,printstatus,warningtime" +
       " ,warningtype,employeeid) select " + "'" + cf_id.ToString().Trim() + "'," + "'" + rinfo.strDoPerson.Trim() + "'," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" +
       "0" + "','" + "" + "','" + "0" + "','" + cf_id.ToString().Trim() + "','" + "0" + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + "暂无预警" + "','" + e_id.Trim() + "'";
                 db.write_log_txt("插入打印标识SQL:" + insert_cf_sql_c);
                //判断该处方是否已插入打印标识表
                 int flag_print = 0;
                 string flag_p_sql = "select count(id) as count_id from " + "PrescriptionCheckState where prescriptionId=" + "'" + cf_id.ToString().Trim() + "'";
                 //int cf_id = dba.get_sql_int("2", get_cf_id_sql);
                 SqlDataReader srd_c_p_1 = db.get_Reader(flag_p_sql);

                 while (srd_c_p_1.Read())
                 {
                     flag_print = Int16.Parse(srd_c_p_1["count_id"].ToString());
                 }
                 if (flag_print == 0)
                 {
                     db.cmd_Execute(insert_cf_sql_c);
                 }



                 if (n == 1)
                 {
                     string str2 = "select id from prescription where hospitalid ='" + rinfo.nHospitalID + "' and Pspnum='" + rinfo.strPspnum + "'";
                    SqlDataReader srd2 = db.get_Reader(str2);
                    if (srd2.Read())
                    {
                        string pid = srd2["id"].ToString();
                        string str3 = "insert into jfInfo(pid,jiefangman,jiefangtime)values('" + pid + "','" + rinfo.strDoPerson + "','" + rinfo.strDoTime + "')";
                        db.cmd_Execute(str3);
                    }
                 }

            }

         
            if ( n>0 )
            {
                strResult = "true";
            } 
            else
            {
                strResult = "false";
            }
           strResult  += "," + cf_id;
           return strResult;
        }
    }
}
