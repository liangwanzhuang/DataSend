using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;
namespace ModelInfo
{
   public class DrugMatchingModel
    {
       public DataBaseLayer db = new DataBaseLayer();

        #region 添加药品匹配信息
        ///// <summary>
        ///// 添加药品匹配信息
        ///// </summary>
        ///// <param name="drugMatchingInfo)">药品匹配信息/param>
        ///// <returns>int对象</returns>
       public int insertDrugMatching(DrugMatchingInfo drugMatchingInfo)
        {
            String sql = "insert into DrugMatching(hospitalId,hospitalName,hdrugNum,ypcdrugNum,hdrugName,ypcdrugName,"
                +"hdrugOriginAddress,ypcdrugOriginAddress,hdrugSpecs,ypcdrugSpecs,hdrugTotal,ypcdrugTotal,pspNum,"
                + "ypcdrugPositionNum,pspId,drugId) values('" + drugMatchingInfo.hospitalId + "','" + drugMatchingInfo.hospitalName + "','" + drugMatchingInfo.hdrugNum
                + "','" + drugMatchingInfo.ypcdrugNum + "','" + drugMatchingInfo.hdrugName + "','" + drugMatchingInfo.ypcdrugName
                + "','" + drugMatchingInfo.hdrugOriginAddress + "','" + drugMatchingInfo.ypcdrugOriginAddress + "','" + drugMatchingInfo.hdrugSpecs
                + "','" + drugMatchingInfo.ypcdrugSpecs + "','" + drugMatchingInfo.hdrugTotal + "','" + drugMatchingInfo.ypcdrugTotal
                + "','" + drugMatchingInfo.pspNum + "','" + drugMatchingInfo.ypcdrugPositionNum + "','" + drugMatchingInfo.pspId + "','" + drugMatchingInfo.drugId
                + "')";

            return db.cmd_Execute(sql); 

        }
        #endregion


        ///// <summary>
        ///// 查询未审核和未匹配处方药品信息
        ///// </summary>
        ///// <param name="hospitalId">医院id</param>
        ///// <param name="Pspnum">处方号</param>
        ///// <returns>SqlDataReader对象</returns>
       public DataTable findNotCheckAndMatchRecipeDrugInfo(string drugnum)
        {
           /* string sql = "SELECT DISTINCT p.ID AS pspId, p.Pspnum AS pspNum,(SELECT   Hname FROM Hospital AS h WHERE   (ID = p.Hospitalid)) AS hospitalName, p.Hospitalid AS hospitalId, d.ID AS drugId, d.drugnum AS hdrugNum, "
               + " d.drugname AS hdrugName, d.drugallnum AS hdrugTotal FROM      prescription AS p LEFT OUTER JOIN PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId RIGHT OUTER JOIN "
               + " (SELECT   ID,pid, customid, delnum, Hospitalid, Pspnum, drugnum, drugname, drugdescription, drugposition, drugallnum, drugweight, tienum, description, wholesaleprice, retailprice, wholesalecost, retailpricecost, money, "
               + " fee FROM drug WHERE   (drugnum = '" + drugnum + "')) AS d ON d.pid = p.id LEFT OUTER JOIN DrugMatching AS dm ON d.ID = dm.drugId AND dm.pspId = p.ID "
               + " WHERE   (pcs.prescriptionId IS NULL) AND (dm.drugId IS NULL) AND (d.ID IS NOT NULL) AND (p.ID IS NOT NULL) AND (p.Hospitalid IS NOT NULL)";
           */
            string sql = "SELECT DISTINCT p.ID AS pspId, p.Pspnum AS pspNum,(SELECT  top 1  Hname FROM Hospital AS h WHERE   (ID = p.Hospitalid)) AS hospitalName, p.Hospitalid AS hospitalId, d.ID AS drugId, d.drugnum AS hdrugNum, "
               + " d.drugname AS hdrugName, d.drugallnum AS hdrugTotal FROM      prescription AS p LEFT OUTER JOIN PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId RIGHT OUTER JOIN "
               + " (SELECT   ID,pid, customid, delnum, Hospitalid, Pspnum, drugnum, drugname, drugdescription, drugposition, drugallnum, drugweight, tienum, description, wholesaleprice, retailprice, wholesalecost, retailpricecost, money, "
               + " fee FROM drug WHERE   (drugnum = '" + drugnum + "')) AS d ON d.pid = p.id LEFT OUTER JOIN DrugMatching AS dm ON d.ID = dm.drugId AND dm.pspId = p.ID "
               + " WHERE p.curstate='未匹配'   AND (dm.drugId IS NULL) AND (d.ID IS NOT NULL) AND (p.ID IS NOT NULL) AND (p.Hospitalid IS NOT NULL)";
             DataTable dt = db.get_DataTable(sql);
             db.write_log_txt("药品匹配显示new："+sql);
            return dt;
        }

       ///// <summary>
       ///// 根据药品编号,医院id查询已匹配的信息
       ///// </summary>

       public DataTable findNotMatchDrugInfo(string drugnum, string hospitalId)
       {
           //根据药品编号查询已匹配的信息
           string sql = "SELECT TOP (1) id, hospitalId, hospitalName, hdrugNum, ypcdrugNum, hdrugName, ypcdrugName, hdrugOriginAddress, "
                +"ypcdrugOriginAddress, hdrugSpecs, ypcdrugSpecs, hdrugTotal, ypcdrugTotal, status, pspNum, ypcdrugPositionNum, pspId, drugId, printstatus, warningstatus "
                + " FROM DrugMatching WHERE (hdrugNum = '" + drugnum + "') AND (hospitalId = '" + hospitalId + "')";

           DataTable dt = db.get_DataTable(sql);

           return dt;
       }
       ///// <summary>
       ///// 查询未审核和未匹配处方药品信息进行匹配
       ///// </summary>
       ///// <param name="hospitalId">医院id</param>
       ///// <param name="Pspnum">处方号</param>
       ///// <returns>SqlDataReader对象</returns>
       public int findNotCheckAndMatchRecipeDrugInfoToMatch()
       {
           RecipeModel rm = new RecipeModel();
           int count = 0;
           //未审核,未匹配的药品和处方
           string sql = "SELECT DISTINCT p.ID AS pspId, p.Pspnum AS pspNum, (SELECT top 1 Hname FROM Hospital AS h "
                    +" WHERE (ID = p.Hospitalid)) AS hospitalName, p.Hospitalid AS hospitalId, d.ID AS drugId, d.drugnum AS hdrugNum,"
                +"d.drugname AS hdrugName, d.drugallnum AS hdrugTotal FROM prescription AS p LEFT OUTER JOIN "
                +"PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId RIGHT OUTER JOIN "
                +"drug AS d ON d.pid = p.id LEFT OUTER JOIN "
                +"DrugMatching AS dm ON d.ID = dm.drugId AND d.drugnum = dm.hdrugNum AND dm.hospitalId = d.Hospitalid "
                + "WHERE ((p.curstate ='未匹配')) AND (p.ID IS NOT NULL) AND (p.Hospitalid IS NOT NULL) AND (dm.drugId IS NULL)";
           /*string sql = "SELECT DISTINCT p.ID AS pspId, p.Pspnum AS pspNum, (SELECT Hname FROM Hospital AS h "
                    +" WHERE (ID = p.Hospitalid)) AS hospitalName, p.Hospitalid AS hospitalId, d.ID AS drugId, d.drugnum AS hdrugNum,"
                +"d.drugname AS hdrugName, d.drugallnum AS hdrugTotal FROM prescription AS p LEFT OUTER JOIN "
                +"PrescriptionCheckState AS pcs ON p.ID = pcs.prescriptionId RIGHT OUTER JOIN "
                +"drug AS d ON d.Pspnum = p.Pspnum AND p.Hospitalid = d.Hospitalid LEFT OUTER JOIN "
                +"DrugMatching AS dm ON d.ID = dm.drugId AND d.drugnum = dm.hdrugNum AND dm.hospitalId = d.Hospitalid "
                +"WHERE (pcs.prescriptionId IS NULL) AND (p.ID IS NOT NULL) AND (p.Hospitalid IS NOT NULL) AND (dm.drugId IS NULL)";*/
           DataTable dt = db.get_DataTable(sql);
 
           if (dt.Rows.Count > 0)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {

                   DrugMatchingInfo drugMatchingInfo = new DrugMatchingInfo();
                   DataTable dtable = findNotMatchDrugInfo(dt.Rows[i]["hdrugNum"].ToString(), dt.Rows[i]["hospitalId"].ToString());
                   string ypcdrugNum = "";
                   string ypcdrugName = "";
                   string hdrugOriginAddress = "";
                   string ypcdrugOriginAddress = "";
                   string hdrugSpecs = "";
                   string ypcdrugSpecs = "";
                   string ypcdrugTotal = "";
                   string ypcdrugPositionNum = "";
                   DrugAdminModel wr = new DrugAdminModel();
                   if (dtable.Rows.Count > 0)
                   {
                       ypcdrugNum = dtable.Rows[0]["ypcdrugNum"].ToString();
                       ypcdrugName = dtable.Rows[0]["ypcdrugName"].ToString();
                       hdrugOriginAddress = dtable.Rows[0]["hdrugOriginAddress"].ToString();
                       ypcdrugOriginAddress = dtable.Rows[0]["ypcdrugOriginAddress"].ToString();
                       hdrugSpecs = dtable.Rows[0]["hdrugSpecs"].ToString();
                       ypcdrugSpecs = dtable.Rows[0]["ypcdrugSpecs"].ToString();
                       ypcdrugTotal = dtable.Rows[0]["ypcdrugTotal"].ToString();
                       ypcdrugPositionNum = dtable.Rows[0]["ypcdrugPositionNum"].ToString();

                       drugMatchingInfo.hospitalId = Convert.ToInt32(dt.Rows[i]["hospitalId"].ToString());
                       drugMatchingInfo.hospitalName = dt.Rows[i]["hospitalName"].ToString();
                       drugMatchingInfo.hdrugNum = dt.Rows[i]["hdrugNum"].ToString();
                       drugMatchingInfo.ypcdrugNum = ypcdrugNum;
                       drugMatchingInfo.hdrugName = dt.Rows[i]["hdrugName"].ToString();
                       drugMatchingInfo.ypcdrugName = ypcdrugName;
                       drugMatchingInfo.hdrugOriginAddress = hdrugOriginAddress;
                       drugMatchingInfo.ypcdrugOriginAddress = ypcdrugOriginAddress;
                       drugMatchingInfo.hdrugSpecs = hdrugSpecs;
                       drugMatchingInfo.ypcdrugSpecs = ypcdrugSpecs;
                       drugMatchingInfo.hdrugTotal = dt.Rows[i]["hdrugTotal"].ToString();
                       drugMatchingInfo.ypcdrugTotal = ypcdrugTotal;
                       drugMatchingInfo.pspNum = dt.Rows[i]["pspNum"].ToString();
                       drugMatchingInfo.ypcdrugPositionNum = ypcdrugPositionNum;
                       drugMatchingInfo.pspId = dt.Rows[i]["pspId"].ToString();
                       drugMatchingInfo.drugId = dt.Rows[i]["drugId"].ToString();
                       count += insertDrugMatching(drugMatchingInfo);
                       // rm.updatePrescriptionStatus(Convert.ToInt32(drugMatchingInfo.pspId), "未审核");


                       wr.Adddrugmatchinginfo(dt.Rows[i]["hospitalId"].ToString(), dt.Rows[i]["hdrugName"].ToString(), dt.Rows[i]["hdrugNum"].ToString(), ypcdrugName, ypcdrugNum);

                       bool boo = rm.checkPrescriptionIsMath(Convert.ToInt32(drugMatchingInfo.pspId));
                       if (boo)
                       {


                           SqlDataReader sdr2 = rm.findisneedcheckstatus();
                           string isneedcheck = "";
                           if (sdr2.Read())
                           {
                               isneedcheck = sdr2["isneedcheck"].ToString();

                           }
                           DrugMatchingModel dmm = new DrugMatchingModel();
                           if (isneedcheck == "0")
                           {
                               dmm.update_p_status_by_pid(drugMatchingInfo.pspId.ToString(), "未审核");
                               rm.updatePrescriptionStatus(Convert.ToInt32(drugMatchingInfo.pspId), "0");
                           }
                           if (isneedcheck == "1")
                           {
                               string reasonText = "";
                               string name = "";
                               string employid = "";

                               int num = rm.checkPrescription(Convert.ToInt32(drugMatchingInfo.pspId), 1, reasonText, name, employid);
                               dmm.update_p_status_by_pid(drugMatchingInfo.pspId.ToString(), "已审核");
                               rm.updatePrescriptionStatus(Convert.ToInt32(drugMatchingInfo.pspId), "1");
                           }


                       }/**/

                   }
               }
           }
           return count;
       }

       public int findNotCheckAndMatchRecipeDrugInfoToMatch_1(string pid)
       {
           int end = 0;

          // DataTable dt = db.get_DataTable(sql);
         //return dt;
           string s111 = "select top 1 Hospitalid from prescription where id='" + pid.Trim() + "'";
           SqlDataReader sr11 = db.get_Reader(s111);
           string hid = "";
           if (sr11.Read())
           {
               hid = sr11["Hospitalid"].ToString();
           }

           string s1111 = "select top 1 Hname from Hospital where id='" + hid.Trim() + "'";
           SqlDataReader sr111 = db.get_Reader(s1111);
           string hname = "";
           if (sr111.Read())
           {
               hname = sr111["Hname"].ToString();
           }


           string s_drug = "select distinct drugname,drugnum from drug where pid='"+pid+"'";
           DataTable dt = db.get_DataTable(s_drug);
         
           //插入匹配
           string drugstr="";
           string drugnum = "";
           foreach (DataRow dr in dt.Rows)
           {
               drugstr = dr["drugname"].ToString().Trim();
               drugnum = dr["drugnum"].ToString().Trim();


               //获取药品管理是否存在

               string strSql_a = "select count(id) as cid from DrugAdmin where DrugCode = '" + drugnum.Trim() + "' and DrugName='" + drugstr.Trim() + "'";
               //    end = db.cmd_Execute(strSql);
               SqlDataReader sdr21 = db.get_Reader(strSql_a);
               string fid = "";
               if (sdr21.Read())
               {
                   fid = sdr21["cid"].ToString();
               }

               if (fid != "0")
               {
                   //获取药品管理DRUGADMIN  ypcnum  ypcname
                   string sy_drug = "select top 1 drugcode,drugname from drugadmin where drugcode ='" + drugnum.Trim() + "' and DrugName='" + drugstr.Trim() + "'";
           DataTable dty = db.get_DataTable(sy_drug);
         
           //插入匹配
           string ypcstr="";
           string ypcnum = "";
           foreach (DataRow dr1 in dty.Rows)
           {
               ypcstr = dr1["drugname"].ToString().Trim();
               ypcnum = dr1["drugcode"].ToString().Trim();
           }

                   //获取医院类型 1 饮片厂匹配，2医院匹配
                   string sql = " select top 1 relation_drug_type_id  from Hospital  where ID='" + hid.ToString().Trim() + "'";
                   db.write_log_txt("dai1:" + sql);
                   SqlDataReader sr1 = db.get_Reader(sql);
                   string str = "";
                   while (sr1.Read())
                   {
                       str = sr1["relation_drug_type_id"].ToString();
                   }
                   //获取饮片库ID 
                   string yp_h_id = "";
                   string sql_h = " select top 1 id  from Hospital  where Hname='" + "饮片库".Trim() + "'";
                   db.write_log_txt("dai2:" + sql_h);
                   SqlDataReader sr1_h = db.get_Reader(sql_h);
                   while (sr1_h.Read())
                   {
                       yp_h_id = sr1_h["id"].ToString();
                   }

                   string strSql_b = "";
                   if (str == "1") //饮片库匹配
                   {
                       strSql_b = "select count(id) as mid from DrugMatching where hdrugName = '" + drugstr + "' and hospitalId='" + yp_h_id + "' and hdrugNum='" + drugnum + "'";
                   }
                   if (str == "2") //医院匹配匹配
                   {
                       strSql_b = "select count(id) as mid from DrugMatching where hdrugName = '" + drugstr + "' and hospitalId='" + hid + "' and hdrugNum='" + drugnum + "'";
                   }
                    db.write_log_txt("插入匹配检查匹配列表：" + strSql_b);
                   //    end = db.cmd_Execute(strSql);
                   SqlDataReader sdr22 = db.get_Reader(strSql_b);
                   string mid = "";
                   if (sdr22.Read())
                   {
                       mid = sdr22["mid"].ToString();
                   }
                   if (mid == "0")
                   {
                       if (str == "2") //医院匹配匹配
                       {
                           string in_m = "insert into DrugMatching(hospitalName,hospitalId,hdrugNum,hdrugName,ypcdrugNum,ypcdrugName,ypcdrugPositionNum)" +
                              "select '" + hname + "', '" + hid + "','" + drugnum + "', '" + drugstr + "', '" + ypcnum + "','" + ypcstr + "'," + "''";
                           db.write_log_txt("医院药品匹配插入："+in_m);
                           end += db.cmd_Execute(in_m);
                       }
                       if (str == "1") //饮片库匹配
                       {
                           string in_m = "insert into DrugMatching(hospitalName,hospitalId,hdrugNum,hdrugName,ypcdrugNum,ypcdrugName,ypcdrugPositionNum)" +
                            "select '" + "饮片库" + "', '" + yp_h_id + "','" + drugnum + "', '" + drugstr + "', '" + ypcnum + "','" + ypcstr + "'," + "''";
                           db.write_log_txt("饮片医院药品匹配插入：" + in_m);
                           end += db.cmd_Execute(in_m);
                       }
                   }
               }
           }
           return end;
       }
       //判断处方是否录入药品
       public string get_p_status_drug_by_pid(string p_id)
       {
           string str = "";
           string sql = "select COUNT(id) as cid from drug where pid ='"+p_id.Trim()+"'";        
           DataTable dt = db.get_DataTable(sql);
           if (dt.Rows.Count > 0)
           {
            str=   dt.Rows[0]["cid"].ToString();
           }
           return str;
       }
       //修改处方状态
       public void update_p_status_by_pid(string p_id,string curstate)
       {
           string str = "";
           string sql = "update prescription set curstate='"+ curstate+"'"+" where id='"  + p_id.Trim() + "'";
           DataTable dt = db.get_DataTable(sql);
           db.cmd_Execute(sql);          
       }
    }
}
