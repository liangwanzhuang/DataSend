using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
    public class TisaneModel
    {
        DataBaseLayer db = new DataBaseLayer();
        public DataTable searchTisaneClass()
        {
            string strSQL = "select id,pspnum,tisaneclassid,pspstate,remark from TisaneClassDst";
            DataTable dt = db.get_DataTable(strSQL);

            return dt;
        }
        public DataTable find_pda_image(string p_id,string p_str)
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql = "select distinct n_id,image_str,syscreated,demo,p_str,p_status,imgbase64 from dbo.tb_node_image where n_id='" + p_id.Trim() + "'" + " and p_str='" + p_str.Trim() + "'";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
        public DataTable find_pda_image_by_pid(string p_id)
        {
            DataBaseLayer db = new DataBaseLayer();
            string sql = "select distinct n_id,image_str,syscreated,p_str,p_status from dbo.tb_node_image where n_id='" + p_id.Trim() + "'";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
    }
}
