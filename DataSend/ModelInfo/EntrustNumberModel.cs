using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SQLDAL;
namespace ModelInfo
{
    public class EntrustNumberModel
    {

        public DataBaseLayer db = new DataBaseLayer();

        #region 获取医院的委托单号
        ///// <summary>
        ///// 更新复核信息
        ///// </summary>
        ///// <param name="hid">医院id</param>
        ///// <returns>int对象</returns>

        public int getEntrustNumber(int hid)
        {
            string sql = "select id,entrustNumber,updateDate from entrustNumber where Hospitalid=" + hid;
            string nowDate = DateTime.Now.ToString("yyyyMM");
            DataTable dt = db.get_DataTable(sql);
            if (dt.Rows.Count > 0)
            {
                string updateDate = dt.Rows[0]["updateDate"].ToString();
                string id = dt.Rows[0]["id"].ToString();
                int entrustNumber = Convert.ToInt32(dt.Rows[0]["entrustNumber"].ToString());
                if (!updateDate.Equals(nowDate))
                {
                    entrustNumber++;
                    string updateSql = "update entrustNumber set entrustNumber=" + entrustNumber + ",updateDate='" + nowDate + "' where id=" + id;
                    db.cmd_Execute(updateSql);
                   
                }
                return entrustNumber;
                
            }
            else
            {
                string sqlStr = "insert into entrustNumber(Hospitalid,entrustNumber,updateDate) values('" + hid + "','1','" + nowDate + "')";
                db.cmd_Execute(sqlStr);
            }


            return 1;
        }


        #endregion

    }
}
