using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
    public class ClearingpartyModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        /// <summary>
        /// 根据煎药方案编号获取煎药方案名称
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string GetTisanesScheme(int Num)
        {
            switch (Num)
            {
                case 1:
                    return "微压（密闭）解表（15min）";
                case 2:
                    return "微压（密闭）汤药（25min）";
                case 3:
                    return "微压（密闭）补药（40min）";
                case 4:
                    return "常压解表（10min，10min）";
                case 5:
                    return "常压汤药（20min，15min）";
                case 6:
                    return "常压补药（25min，20min）";
                case 20:
                    return "先煎解表（10min，10min，10min）";
                case 21:
                    return "先煎汤药（10min，20min，15min）";
                case 22:
                    return "先煎补药（10min，25min，20min）";
                case 36:
                    return "后下解表（10min（3：7），10min）";
                case 37:
                    return "后下汤药（20min（13：7），15min）";
                case 38:
                    return "后下补药（25min（18：7），20min）";
                case 81:
                    return "微压自定义";
                case 82:
                    return "常压自定义";
                case 83:
                    return "先煎自定义";
                case 84:
                    return "后下自定义";
                default:
                    break;
            }
            return "NULL";
        }
        #region 查询所有结算方信息
        ///// <summary>
        ///// 查询所有结算方信息
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>SqlDataReader对象</returns>
        public SqlDataReader findClearingpartyAll()
        {
            string sql = "select * from Clearingparty";

            return db.get_Reader(sql);
        }

        public DataTable findNumById(int id)
        {
            string sql = "select ClearPName from Clearingparty where ID =" + id;

            return db.get_DataTable(sql);
        }
        #endregion
        #region
        /// <summary>
        /// 查询结算方信息
        /// </summary>
        /// <param > ClearPName</param>
        /// <returns>dt</returns>

        public DataTable findClearingpartyInfo(int ClearPName)
        {
            string sql = "select id ,ClearPName,ConPerson,ConPhone,Address,PerSetInformation,Remarks ,GenDecoct from  Clearingparty where 1=1";

            if (ClearPName!=0)
            {
           sql+="and id = " + ClearPName+"";
          
          }

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
       #region 删除包装信息
        public bool deleteClearingpartyInfo(int nCId)
        {
            string strSql = "delete from Clearingparty where id =" + nCId;
            int n = db.cmd_Execute(strSql);
            return true;
        }
        #endregion
        #region 修改结算方信息
        public int updateClearingpartyInfo(int id, string ClearPName, string ConPerson, string ConPhone, string Address,  string Remarks, string GenDecoct)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;

            string sql = "";
            string str = "select ConPerson from Clearingparty where id != " + id + " and ConPerson = '" + ConPerson + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {
                string str1 = "select ClearPName from Clearingparty where id != " + id + " and ClearPName = '" + ClearPName + "'";
                SqlDataReader sr1 = db.get_Reader(str1);
                if (sr1.Read()) {

                    end = 0;
                }
                else
                {
                    sql = "update Clearingparty set ClearPName='" + ClearPName + "',ConPerson='" + ConPerson + "',ConPhone='" + ConPhone + "',Address='" + Address + "',Remarks='" + Remarks + "',GenDecoct='" + GenDecoct + "' where id = " + id + "";
                    end = db.cmd_Execute(sql);
                }
            }


            return end;
        }
        #endregion 
    }
    }
