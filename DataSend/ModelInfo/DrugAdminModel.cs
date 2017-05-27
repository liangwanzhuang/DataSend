using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
   public class DrugAdminModel
    {
       public DataBaseLayer db = new DataBaseLayer();
       #region 根据编号查询
       public DataTable findDrugAdinByDrugCode(string drugCode)
       {
           string sql = "select * from DrugAdmin where DrugCode='" + drugCode + "'";
           DataTable dt = db.get_DataTable(sql);

           return dt;
       }
           

       #endregion
       #region 添加药品信息
       public int AddDrug(string DrugType, string DrugCode, string PurUnits, string DrugName, string DrugSpecificat, string PositionNum, string Univalent, string Mnemonic, string Rmarkes, string Producer, string ProducingArea,  string UpperLimit, string LowerLimit, string Rmarkes2, string Rmarkes3)
       {
           DataBaseLayer db = new DataBaseLayer();
           String strSql = "";
           int end = 0;
           System.DateTime currentTime = new System.DateTime();
           currentTime = System.DateTime.Now;//获取当前时间
          
           //产品批次
           string ProductBatch = "";
           string now1 = currentTime.ToString("yyyyMMdd");//1当前日期

           string str = "select StorageTime from DrugAdmin where StorageTime='" + now1 + "'";//2查询到数据库存储的当前时间
           SqlDataReader sr = db.get_Reader(str);
           if (sr.Read())
           {
               string result = sr["StorageTime"].ToString();
              
               int m = 0;

               if (now1 == result)
               {
                   string str1 = "select   max(CAST(ProductBatch AS int)) as kk  from DrugAdmin  WHERE  StorageTime= '" + now1 + "'";//2查询到数据库存储的当前时间
                   SqlDataReader s1r = db.get_Reader(str1);
                   string result1 = "";
                   if (s1r.Read())
                   {
                       result1 = s1r["kk"].ToString();
                   }
                   string DeNum = result1.Substring(8);
                   int hh = Convert.ToInt32(DeNum);
                   int sum1 = ++hh;

                   ProductBatch = now1 + sum1;

               }
               else
               {
                   int sum = ++m;
                   ProductBatch = now1 + sum.ToString();

               }

           }
           else
           {
               int ss = 0;
               int sum2 = ++ss;
               ProductBatch = now1 + sum2.ToString();

           }

           /* string tate = "";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                strSql = "";
            }
            else
            {*/
           strSql = "insert into DrugAdmin(ProductBatch, DrugType, DrugCode, PurUnits, DrugName,DrugSpecificat, PositionNum,  Univalent, Mnemonic, Rmarkes,  Producer, ProducingArea,  StorageTime,UpperLimit,LowerLimit,Rmarkes2,Rmarkes3) ";
           strSql += "values ('" + ProductBatch + "','" + DrugType + "','" + DrugCode + "','" + PurUnits + "',";
           strSql += "'" + DrugName + "','" + DrugSpecificat + "','" + PositionNum + "','" + Univalent + "','" + Mnemonic + "','" + Rmarkes + "','" + Producer + "','" + ProducingArea + "','" + now1 + "','" + UpperLimit + "','" + LowerLimit + "','" + Rmarkes2 + "','" + Rmarkes3 + "')";
           // }


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
       //添加匹配列表信息
       public int Adddrugmatchinginfo(string hospitalname, string DrugName12, string DrugCode1, string ypcdrugname, string ypcdrugcode)
       {
           int end = 0;
           string strSql = "select top 1 id from Hospital where id = '" + hospitalname + "' ";
           //    end = db.cmd_Execute(strSql);
           SqlDataReader sdr2 = db.get_Reader(strSql);
           string hid = "";
           if (sdr2.Read())
           {
               hid = sdr2["id"].ToString();
           }
           string strSqlh = "select top 1 Hname from Hospital where id = '" + hospitalname + "' ";
           //    end = db.cmd_Execute(strSql);
           SqlDataReader sdr2h = db.get_Reader(strSqlh);
           string hn = "";
           if (sdr2h.Read())
           {
               hn = sdr2h["Hname"].ToString();
           }

           string strSql_a = "select count(id) as cid from DrugAdmin where DrugCode = '" + ypcdrugcode + "' ";
           //    end = db.cmd_Execute(strSql);
           SqlDataReader sdr21 = db.get_Reader(strSql_a);
           string fid = "";
           if (sdr21.Read())
           {
               fid = sdr21["cid"].ToString();
           }
           if (fid == "0")
           { 
              string in_admin = "insert into DrugAdmin(ProductBatch, DrugType, DrugCode, PurUnits, DrugName,DrugSpecificat, PositionNum,  Univalent, Mnemonic, Rmarkes,  Producer, ProducingArea,  StorageTime,UpperLimit,LowerLimit,Rmarkes2,Rmarkes3)"+ 
                 "select '', '中药饮片','"+ ypcdrugcode+"', 'kg', '"+ypcdrugname+"','', '','','', '','','','','','','',''";
              end = db.cmd_Execute(in_admin);
           }
           //插入匹配

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

          string strSql_b ="";
           if (str == "1") //饮片库匹配
           {
               strSql_b = "select count(id) as mid from DrugMatching where hdrugNum = '" + DrugCode1 + "' and hospitalId='" + yp_h_id + "'";
           }
           if (str == "2") //医院匹配匹配
           {
               strSql_b = "select count(id) as mid from DrugMatching where hdrugNum = '" + DrugCode1 + "' and hospitalId='" + hid + "'";
           }
           db.write_log_txt("插入匹配："+strSql_b );
           //    end = db.cmd_Execute(strSql);
           SqlDataReader sdr22 = db.get_Reader(strSql_b);
           string mid = "";
           if (sdr22.Read())
           {
               mid = sdr22["mid"].ToString();
           }
           if (mid== "0")
           {


               string in_m="";
               if (str == "2") //医院匹配匹配
               {
                   in_m = "insert into DrugMatching(hospitalName,hospitalId,hdrugNum,hdrugName,ypcdrugNum,ypcdrugName,ypcdrugPositionNum)" +
                      "select '" + hn + "', '" + hid + "','" + DrugCode1 + "', '" + DrugName12 + "', '" + ypcdrugcode + "','" + ypcdrugname + "'," + "''";
               }
               if (str == "1") //医院匹配匹配
               {
                   in_m = "insert into DrugMatching(hospitalName,hospitalId,hdrugNum,hdrugName,ypcdrugNum,ypcdrugName,ypcdrugPositionNum)" +
                      "select '" + "饮片库" + "', '" + yp_h_id + "','" + DrugCode1 + "', '" + DrugName12 + "', '" + ypcdrugcode + "','" + ypcdrugname + "'," + "''";
               }
               end += db.cmd_Execute(in_m);
           }
          
        
           return end;
       }

       //修改匹配列表信息
       public int updatedrugmatchinginfo(string hospitalname, string DrugName12, string DrugCode1, string ypcdrugname, string ypcdrugcode, string positionnum ,string id )
       {
           int end = 0;
           string strSql = "";

           string str = "";

           str = "select * from ypcdrug where hospitalid ='" + hospitalname + "' and drugNum='" + DrugCode1 + "' and id != '" + id + "'";
           SqlDataReader sdr2  = db.get_Reader(str);
           if (sdr2.Read())
           {

           }
           else
           {
               strSql = "update ypcdrug set drugName='" + DrugName12 + "',drugNum='" + DrugCode1 + "',drugDetailedName ='" + ypcdrugname + "', positionNum ='" + positionnum + "',drugAlias='" + ypcdrugcode + "',hospitalid='" + hospitalname + "' where id = '" + id + "' ";
               end = db.cmd_Execute(strSql);
           }



           return end;
       }
       //修改匹配列表信息
       public int updatedrugmatchinginfo_1(string hospitalname, string DrugName12, string DrugCode1, string ypcdrugname, string ypcdrugcode, string positionnum, string id)
       {
           int end = 0;
           string strSql = "select top 1 id from Hospital where Hname = '" + hospitalname + "' ";
           //    end = db.cmd_Execute(strSql);
          SqlDataReader sdr2 = db.get_Reader(strSql);
             string hid = "";            
             if (sdr2.Read())
             {
                 hid = sdr2["id"].ToString();
             }

             string sql = "update  DrugMatching set hospitalName='" + hospitalname + "'," + "hospitalId='" + hid + "'," + "hdrugNum ='" + DrugCode1 + "'," + "hdrugName ='" + DrugName12 + "'," +
            "ypcdrugName ='" + ypcdrugname + "'," + "ypcdrugNum ='" + ypcdrugcode + "'," + "ypcdrugPositionNum='" + positionnum + "'" + " where hdrugNum ='" + DrugCode1 + "' and " + "hospitalId ='" + hid + "' and " + "hdrugName='" + DrugName12 + "'";
             end = db.cmd_Execute(sql);
           return end;
       }
       public int GetDrugInfoCount()
       {
           string strSql = "select count(*) from DrugAdmin";
           int count = Convert.ToInt32(db.cmd_ExecuteScalar(strSql));
           return count;
       }
       /// <summary>
       /// 处方列表，分页显示
       /// </summary>
       /// <returns></returns>
       public DataTable findInfo(int ts, int page,string medicaltype,string medicalname)
       {
           string strSql = "select top " + ts + " *  from  ( select  row_number() over(order by id desc ) as rownumber,* from DrugAdmin) a where 1=1";
           if (medicaltype != "0")
           {
               strSql += "and DrugType='" + medicaltype + "'";
           }
           if (medicalname != "0")
           {
               strSql += "and DrugName='" + medicalname + "'";
           }

          strSql+=" and a.rownumber> ('" + page + "' - 1) * '" + ts + "'  order   by   rownumber   asc";
           DataTable dt = db.get_DataTable(strSql);

           return dt;
       }
        #region 查询所有信息通过id
       public DataTable findDrugAdminInfo(int id)
       {
           string strSql = "select * from  DrugAdmin where id = " + id;

           DataTable dt = db.get_DataTable(strSql);

           return dt;
       }

        #endregion

       #region 查询所有匹配表信息通过id
       public DataTable findDrugmatchingInfo(int id)
       {
           string strSql = "select * from  ypcdrug where id = " + id;

           DataTable dt = db.get_DataTable(strSql);

           return dt;
       }

        #endregion
       #region 查询所有匹配表信息通过id new 20160330 lbf update
       public DataTable findDrugmatchingInfo_1(string h_n,string h_d_n,string h_d_d)
       {
           string strSql = "select top 1 * from  DrugMatching where hospitalId = " + "'" + h_n.Trim() + "' and hdrugNum='" + h_d_n.Trim() + "' and  hdrugName='"+h_d_d.Trim()+"'";

           DataTable dt = db.get_DataTable(strSql);

           return dt;
       }

       #endregion
        #region 修改药品信息

       public int UpdateDrugAdminInfo(int id, string DrugType, string DrugCode, string PurUnits, string DrugName, string DrugSpecificat, string PositionNum, string Univalent, string Mnemonic, string Rmarkes, string Producer, string ProducingArea, string UpperLimit, string LowerLimit, string Rmarkes2, string Rmarkes3)
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


               sql = "update DrugAdmin set DrugType='" + DrugType + "',DrugCode='" + DrugCode + "',Mnemonic='" + Mnemonic + "',PurUnits='" + PurUnits + "',DrugName='" + DrugName + "',DrugSpecificat='" + DrugSpecificat + "',PositionNum='" + PositionNum + "' ,Univalent='" + Univalent + "',UpperLimit='" + UpperLimit + "',Rmarkes='" + Rmarkes + "',Producer='" + Producer + "',ProducingArea='" + ProducingArea + "',LowerLimit='" + LowerLimit + "',Rmarkes2='" + Rmarkes2 + "',Rmarkes3='" + Rmarkes3 + "'where id = " + id + "";
               end = db.cmd_Execute(sql);
           }


           return end;
       }
        #endregion 
       #region 删除药品信息
       public bool deleteDrugAdminInfo(int nPId)
       {
           string strSql = "delete from DrugAdmin where id =" + nPId;
           int n = db.cmd_Execute(strSql);
           return true;
       }
       #endregion


       #region 删除匹配列表药品信息
       public int deleteDrugmatchingInfo(int nPId)
       {
             string str = "select hospitalid ,drugnum from ypcDrug where id = '" + nPId + "'";
             SqlDataReader sdr2 = db.get_Reader(str);

             string hid = "";
             string hdrugnum = "";
             if (sdr2.Read())
             {

                 hid = sdr2["hospitalid"].ToString();
                 hdrugnum = sdr2["drugnum"].ToString();
             }

            string str2 = "delete from drugmatching where hospitalid ='" + hid + "' and hdrugnum ='" + hdrugnum + "'";

            db.cmd_Execute(str2);

           string strSql = "delete from ypcdrug where id =" + nPId;
           int n = db.cmd_Execute(strSql);


         



           return n;
       }
       #endregion
       #region 删除匹配列表药品信息
       public int deleteDrugmatchingInfo_1(string nPId)
       {
          /* string str = "select hospitalid ,drugnum from ypcDrug where id = '" + nPId + "'";
           SqlDataReader sdr2 = db.get_Reader(str);

           string hid = "";
           string hdrugnum = "";
           if (sdr2.Read())
           {

               hid = sdr2["hospitalid"].ToString();
               hdrugnum = sdr2["drugnum"].ToString();
           }
           */
           string str2 = "delete from drugmatching where  hdrugnum ='" + nPId + "'";

        int n=  db.cmd_Execute(str2);






           return n;
       }
       #endregion
    }
}
