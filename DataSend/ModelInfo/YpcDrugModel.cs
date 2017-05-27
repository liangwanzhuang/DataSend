using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;
namespace ModelInfo
{
    public class YpcDrugModel
    {
        public DataBaseLayer db = new DataBaseLayer();


        #region 模糊查询饮片厂药品信息
        ///// <summary>
        ///// 查询所有调剂信息
        ///// </summary>
        ///// <param name="text">根据药品编号-药品名称-产地-货位号-助记码模糊查询</param>
        public SqlDataReader findYpcDrugInfo(String text)
        {
            string sql = "select da.id,da.Mnemonic ,da.DrugName drugName,da.DrugCode drugNum,da.DrugSpecificat drugSpec,da.DrugName drugDetailedName,da.PositionNum positionNum,da.DrugName drugAlias,da.ProducingArea drugOrigin from DrugAdmin da where 1=1";
            if (text != null && text.Length > 0)
            {
                sql += " and da.DrugName like '%" + text + "%' or da.DrugCode like '%" + text + "%' or da.Mnemonic like '%" + text + "%' or da.ProducingArea like '%" + text + "%' or da.PositionNum like '%" + text + "%'";
                //sql += "and da.DrugName like '%" + text + "%'";
            }
            return db.get_Reader(sql);

        }
        #endregion
    }
}
