using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQLDAL
{
    public class SqlComm
    {
        public SqlComm()
        { }

        

        /// <summary>
        /// 通用查询数据业务方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">查询的列名</param>
        /// <param name="condition">查询的条件，直接跟条件，不需要带where</param>
        /// <returns></returns>
        public DataTable getDataByCondition(string tableName, string columns, string condition)
        {
            SqlParameter[] pars = new SqlParameter[]{
             new SqlParameter("@tableName",tableName),
             new SqlParameter("@columns",columns),
             new SqlParameter("@condition",condition)       
          };
            return DbHelperSQL.ExcuteSelectReturnDataTable("Sp_getDataByCondition", CommandType.StoredProcedure, pars);
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="columns">列名</param>
        /// <param name="condition">条件,不需要带where</param>
        /// <param name="pagesize">每页显示条数</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pk">主键</param>
        /// <returns>DataTable</returns>
        public DataTable getdatabyPageIndex(string tablename, string columns, string condition, int pagesize, int pageindex, string pk, out int totalcount, string ordercolumn, string isasc)
        {
            string order = "";
            if (ordercolumn == null)
            {
                order = pk;
            }

            string asc = "";
            if (isasc == null)
            {
                isasc = "desc";
            }

            SqlParameter[] pars = new SqlParameter[]{
         new SqlParameter("@tablename",tablename),
         new SqlParameter("@columns",columns),
         new SqlParameter("@condition",condition),
         new SqlParameter("@pagesize",pagesize),
         new SqlParameter("@pageindex",pageindex),
         new SqlParameter("@pk",pk),
         new SqlParameter("@total",SqlDbType.Int),
         new SqlParameter("@orderculumn",ordercolumn),
         new SqlParameter("@isasc",isasc)
       };
            pars[6].Direction = ParameterDirection.Output;
            DataTable dt = DbHelperSQL.ExcuteSelectReturnDataTable("sp_getdatabyPageIndex", CommandType.StoredProcedure, pars);
            totalcount = int.Parse(pars[6].Value.ToString());
            return dt;
        }
        /// <summary>
        /// 通用更改表内容
        /// </summary>
        /// <param name="table">表名称</param>
        /// <param name="columns">设置列，如：set columns1='zhangsan',columns2='lisi'</param>
        /// <param name="condition">条件 需要加Where</param>
        /// <returns></returns>
        public int UpdateTableByCondition(string table, string columns, string condition)
        {
            SqlParameter[] pars = new SqlParameter[]{
            new SqlParameter("@table",table),
            new SqlParameter("@columns",columns), 
            new SqlParameter("@condition",condition)
          };

            return DbHelperSQL.ExcuteCommandReturnInt("sp_UpdateTableByCondition", CommandType.StoredProcedure, pars);
        }

        /// <summary>
        /// 通用添加数据业务
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int InsertTable(string table, string columns, string values)
        {
            SqlParameter[] pars = new SqlParameter[]{
            new SqlParameter("@table",table),
            new SqlParameter("@columns",columns), 
            new SqlParameter("@values",values)
          };
            return DbHelperSQL.ExcuteCommandReturnInt("sp_InsertTable", CommandType.StoredProcedure, pars);
        }

        /// <summary>
        /// 通用删除数据业务
        /// </summary>
        /// <param name="table"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int DeleteByCondition(string table, string condition)
        {
            SqlParameter[] pars = new SqlParameter[]{
            new SqlParameter("@table",table),          
            new SqlParameter("@condition",condition)
          };
            return DbHelperSQL.ExcuteCommandReturnInt("sp_DeleteByCondition", CommandType.StoredProcedure, pars);
        }



    }
}
