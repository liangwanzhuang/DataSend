using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;
namespace ModelInfo
{
    public class DrugModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        #region 根据处方号查询药品信息
        ///// <summary>
        ///// 根据处方号查询药品信息
        ///// </summary>
        ///// <param name="pspnum">处方号</param>
        ///// <returns>DataTable对象</returns>
        public DataTable findDrugByPid(string pid)
        {



            string sql = "select ID,delnum,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where pid='" + pid + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }
        ///// <returns>DataTable对象</returns>
        public DataTable findDrugByPspnum(string pid)
        {



          /*  string sql = "select  ROW_NUMBER() OVER(ORDER BY delnum desc) as ID,delnum,(select hnum from hospital as h where h.id = (select hospitalid from prescription where id = '" + pid + "')) as hnum,(select hname from hospital as h where h.id = (select hospitalid from prescription where id = '" + pid + "')) as hname,"
                + "(select pspnum from prescription where id = d.pid) as Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where pid ='" + pid + "'";
            */
            string sql = "select   ROW_NUMBER() OVER(ORDER BY ID desc)  as ID,(select top 1 hnum from hospital as h where h.id in (select hospitalid from prescription where id = '" + pid + "')) as hnum,(select hname from hospital as h where h.id = (select hospitalid from prescription where id = '" + pid + "')) as hname,"
                + "(select top 1 pspnum from prescription where id = d.pid) as Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where pid ='" + pid + "'";
            db.write_log_txt("药品录入显示：" + sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;
        }
        public DataTable findDrugByPspnum1232(string pid)
        {



            string sql = "select  id,  ROW_NUMBER() OVER(ORDER BY delnum desc) as yy,delnum,(select hnum from hospital as h where h.id = (select hospitalid from prescription where id = '" + pid + "')) as hnum,(select hname from hospital as h where h.id = (select hospitalid from prescription where id = '" + pid + "')) as hname,"
                + "(select pspnum from prescription where id = d.pid) as Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where pid ='" + pid + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }

        ///// <summary>
        ///// 根据处方号查询未匹配药品信息
        ///// </summary>
        ///// <param name="pspnum">处方号</param>
        ///// <returns>DataTable对象</returns>
        public int findNotMatchDrugByPspnum_count(string pid)
        {
            string sql = "select count(d.id) from drug  d " +
                //   left join DrugMatching dm on d.id=dm.drugId and dm.hospitalId=d.hospitalId 
         "where  d.pid ='" + pid + "' and " +

                "d.id not in(      select distinct aaa.id from ( select d1.id from drug d1 where  d1.Hospitalid in( select id from Hospital where relation_drug_type_id ='1' ) and d1.drugnum  in" +
            "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId ='128') union all" +

            "  select d2.id from drug d2 where  d2.Hospitalid in( select id from Hospital where relation_drug_type_id ='2' ) and d2.drugnum  in" +
             "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId =d2.Hospitalid )) aaa)";

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;
            db.write_log_txt("药品匹配录入药品数量显示NEWNEW:" + sql);
           
        }
        public DataTable findNotMatchDrugByPspnum(string pid)
        {
            string sql =  "select d.ID,d.delnum,(select hnum from hospital as h where h.id = (select top 1 hospitalid from prescription where id = '" + pid + "')) as hnum,(select top 1 hname from hospital as h where h.id = (select  top 1 hospitalid from prescription where id = '" + pid + "')) as hname,"
                + "(select top 1 pspnum from prescription where id = d.pid) as Pspnum,d.Drugnum,d.Drugname,d.DrugDescription,d.DrugPosition,d.DrugAllNum,d.DrugWeight,d.TieNum,d.Description,d.WholeSalePrice,d.RetailPrice,d.WholeSaleCost,d.retailpricecost,"
                + "d.money,d.Fee from drug  d " +
                //   left join DrugMatching dm on d.id=dm.drugId and dm.hospitalId=d.hospitalId 
         "where  d.pid ='" + pid + "' and " +

                "d.id not in(      select distinct aaa.id from ( select d1.id from drug d1 where  d1.Hospitalid in( select id from Hospital where relation_drug_type_id ='1' ) and d1.drugnum  in" +
            "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId ='128') union all" +

            "  select d2.id from drug d2 where  d2.Hospitalid in( select id from Hospital where relation_drug_type_id ='2' ) and d2.drugnum  in" +
             "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId =d2.Hospitalid )) aaa)";
           
            DataTable dt = db.get_DataTable(sql);
            db.write_log_txt("药品匹配录入显示NEWNEW:" + sql);
            return dt;
        }
        public DataTable findNotMatchDrugByPspnum_1(string pid ,int ts, int page)
        {
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by q.id desc ) as rownumber from (" + "select d.ID,d.delnum,(select hnum from hospital as h where h.id = (select top 1 hospitalid from prescription where id = '" + pid + "')) as hnum,(select top 1 hname from hospital as h where h.id = (select  top 1 hospitalid from prescription where id = '" + pid + "')) as hname,"
                + "(select top 1 pspnum from prescription where id = d.pid) as Pspnum,d.Drugnum,d.Drugname,d.DrugDescription,d.DrugPosition,d.DrugAllNum,d.DrugWeight,d.TieNum,d.Description,d.WholeSalePrice,d.RetailPrice,d.WholeSaleCost,d.retailpricecost,"
                + "d.money,d.Fee from drug  d "+
         //   left join DrugMatching dm on d.id=dm.drugId and dm.hospitalId=d.hospitalId 
         "where  d.pid ='" + pid + "' and "+

                "d.id not in(      select distinct aaa.id from ( select d1.id from drug d1 where  d1.Hospitalid in( select id from Hospital where relation_drug_type_id ='1' ) and d1.drugnum  in"+
            "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId ='128') union all"+    
              
            "  select d2.id from drug d2 where  d2.Hospitalid in( select id from Hospital where relation_drug_type_id ='2' ) and d2.drugnum  in"+
             "(select dd.hdrugNum  from DrugMatching dd where dd.hospitalId =d2.Hospitalid )) aaa)";
            sql += "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(sql);
            db.write_log_txt("药品匹配录入显示NEWNEW:"+sql);
            return dt;
        }
        #endregion
        #region 删除处方信息
        public bool deleteDrugInfo(int ndId)
        {
            string strSql = "delete from drug where id =" + ndId;
            int n = db.cmd_Execute(strSql);
            return true;
        }
        #endregion
        #region 根据复核处方号查询药品信息
        ///// <summary>
        ///// 根据处方号查询药品信息
        ///// </summary>
        ///// <param name="pspnum">处方号</param>
        ///// <returns>DataTable对象</returns>
        public DataTable findDrugInfo(string pspnum, string Hospitalid)
        {


            string str = "select id from hospital where hnum ='" + Hospitalid+ "'";


            SqlDataReader sdr2 = db.get_Reader(str);
            string hid = "";
            if (sdr2.Read())
            {
                hid = sdr2["id"].ToString();

            }





            string sql = "select  ROW_NUMBER() OVER(ORDER BY id desc) as ID,delnum,(select hnum from hospital as h where h.id = d.hospitalid) as hnum,(select hname from hospital as h where h.id = d.hospitalid) as hname,"
                + "Pspnum,Drugnum,Drugname,DrugDescription,DrugPosition,DrugAllNum,DrugWeight,TieNum,Description,WholeSalePrice,RetailPrice,WholeSaleCost,retailpricecost,"
                + "money,Fee from drug as d where d.Pspnum='" + pspnum + "' and Hospitalid='" + hid + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;
        }

        #endregion
    }
    
}

