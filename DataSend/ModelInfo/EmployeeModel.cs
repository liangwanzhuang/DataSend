using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
    public class EmployeeModel
    {
        public DataBaseLayer db = new DataBaseLayer();
        #region 通过员工姓名查询员工编号
        public DataTable findNumByEName(String  Ename)
        {
            string sql = "select JobNum from Employee where id =" + Ename;

            return db.get_DataTable(sql);
        }
        #endregion
        #region 查询所有员工信息
        ///// <summary>
        ///// 查询所有员工信息
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>SqlDataReader对象</returns>
        public SqlDataReader  findEmployeeAll()
        {
            string sql = "select * from Employee";

            return db.get_Reader(sql);
        }

        public DataTable findNumById(int id)
        {
            string sql = "select JobNum from Employee where ID =" + id;

            return db.get_DataTable(sql);
        }

        #endregion
        #region 根据id查询员工信息
        ///// <summary>
        ///// 根据id查询员工信息
        ///// </summary>
        ///// <param name="id">员工id</param>
        ///// <returns>SqlDataReader对象</returns>
        public DataTable findEmployeeById(String id)
        {
            string sql = "select ID,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,WorkTime,WorkContent,pwd from Employee where id='" + id + "'";

            return db.get_DataTable(sql);
        }
        public DataTable findEmployeeByJobNum(String JobNum)
        {
            string sql = "select ID,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,WorkTime,WorkContent,pwd from Employee where JobNum='" + JobNum + "'";

            return db.get_DataTable(sql);
        }
    

        #endregion
        #region
        /// <summary>
        /// 查询员工信息
        /// </summary>
        /// <param > EName,  JobNum</param>
        /// <returns>dt</returns>

        public DataTable findEmployeeInfo(string EName, string JobNum, string Role)
        {
            string sql = "select distinct ID,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,pwd,room from  Employee where  1=1 ";
            if (EName != "0")
            {
                sql += "and  EName like'%" + EName + "%'";
            }
            if (JobNum != "0")
            {
                sql += "and  JobNum like'%" + JobNum + "%'";
            }
            if (Role != "")
            {
                sql += "and  Role like'%" + Role + "%'";
            }
               // sql += " and id ='" + EName + "'";

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public int findEmployeeInfo_c(string EName, string JobNum, string Role)
        {
            string sql = " select count(id)  from  Employee where  1=1 ";
            if (EName != "0")
            {
                sql += "and  EName like'%" + EName + "%'";
            }
            if (JobNum != "0")
            {
                sql += "and  JobNum like'%" + JobNum + "%'";
            }
            if (Role != "")
            {
                sql += "and  Role like'%" + Role + "%'";
            }
            // sql += " and id ='" + EName + "'";

            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;
          

        }
        public DataTable findEmployeeInfo_p(string EName, string JobNum, string Role, int ts, int page)
        {
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by q.id desc ) as rownumber from (select distinct ID,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,pwd,room from  Employee where  1=1 ";
            if (EName != "0")
            {
                sql += "and  EName like'%" + EName + "%'";
            }
            if (JobNum != "0")
            {
                sql += "and  JobNum like'%" + JobNum + "%'";
            }
            if (Role != "")
            {
                sql += "and  Role like'%" + Role + "%'";
            }
            sql += "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            // sql += " and id ='" + EName + "'";
            //+ "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
        #region
        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param >id</param>
        /// <returns>dt</returns>
        public DataTable findEmployeeInfo(int id)
        {
            string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,pwd,room from  Employee where id = " + id;

            DataTable dt = db.get_DataTable(strSql);

            return dt;
        }
      
        public int updateEmployeeInfo1(int id, string JobNum, string EName, string Role, string Age, string Sex, string Nation, string Phone, string Address, string Origin, string password,string room)
        {
            //string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee where id = " + id;

            int end = 0;
            
            string sql = "";
            string EmNumAName = JobNum + "  " + EName;
            string str = "select JobNum from Employee where id != " + id + " and JobNum = '" + JobNum + "'";
            SqlDataReader sr = db.get_Reader(str);
            if (sr.Read())
            {
                end = 0;
            }
            else
            {
                sql = "update Employee set JobNum='" + JobNum + "',EName='" + EName + "',Role='" + Role + "',Sex='" + Sex + "',Age='" + Age + "',Phone='" + Phone + "',Address='" + Address + "',Nation='" + Nation + "',Origin='" + Origin + "',pwd='" + password + "',EmNumAName='" + EmNumAName +"',room='" + room +"' where id = " + id + "";
                end = db.cmd_Execute(sql);
            }


            return end;
        }
        #endregion




        public SqlDataReader findusernamebyjobnum(string jobnum)
        {
            string str= "select * from Employee where JobNum ='"+jobnum+"'";

            SqlDataReader sdr = db.get_Reader(str);

            return sdr;
        }




        #region 删除员工信息
        public bool deleteEmployeeInfo(int nPId)
        {
            string strSql = "delete from Employee where id =" + nPId;
            int n = db.cmd_Execute(strSql);
            return true;
        }
        #endregion
    }
}