using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient;
using SQLDAL;
using System.Collections;
using System.Data;

namespace ModelInfo
{
    public class EmployeeHandler
    {
        public DataBaseLayer db = new DataBaseLayer();
        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="einfo"></param>
        /// <returns></returns>
        public int AddEmployee(string JobNum, string EName, string Role, string Age, string Sex, string Nation, string Phone, string Address, string Origin, string password,string room)
        {
            String strSql = "";
            int end = 0;
            string EmNumAName = JobNum + "  " + EName;
            DataBaseLayer db = new DataBaseLayer();
            string tate = "select  JobNum  from Employee where JobNum = '" + JobNum + "'";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                strSql = "";
            }
            else
            {
                strSql = "insert into Employee(JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin,pwd,EmNumAName,room) ";
                strSql += "values ('" + JobNum + "','" + EName + "','" + Role + "','" + Sex + "',";
                strSql += "'" + Age + "','" + Phone + "','" + Address + "','" + Nation + "','" + Origin + "','" + password + "','" + EmNumAName + "','"+room+"')";
            }
            if (strSql == "")
            {
                end = 0;
            }
            else
            {
                string[] rolelist = Role.Split(',');
                for (int i = 0; i < rolelist.Length; i++)
                {
                 string  userRole = "insert into tbUserRole(UserId,RoleId)values('" + JobNum + "','" + rolelist[i] + "')";
                    db.cmd_Execute(userRole);
                }
                end = db.cmd_Execute(strSql);
            }
            return end;

        }
      
        public int updateLimits(int id, string limits)
        {
            string strSql = "update limitsauthority set limits='" + limits + "' where id=" + id;
            return db.cmd_Execute(strSql);
        }
        /// <summary>
        /// 员工信息
        /// </summary>
        /// <param name="einfo"></param>
        /// <returns></returns>
        public DataTable SearchEmployee()
        {
            string strSql = "select id,JobNum,EName,Role,Sex,Age,Phone,Address,Nation,Origin from  Employee  ";


            DataBaseLayer db = new DataBaseLayer();
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }

        public DataTable findLimitsauthorityById(int id)
        {
            DataBaseLayer db = new DataBaseLayer();
            string strSql = "select * from limitsauthority where id="+id;

            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }
        public DataTable employeelimits(string Ename)
        {

            //ArrayList list = new ArrayList();
             DataBaseLayer db = new DataBaseLayer();
          // string role ="";
           // string str = "select Role from  Employee where EName ='" + Ename+ "' ";
            // SqlDataReader sdr = db.get_Reader(str);
           // if(sdr.Read()){
          //      while (sdr.Read())
             //   {
           //   list.Add(sdr["Role"].ToString());
           //     }
          //  }

            string strSql = "";
            /**/
            if (Ename != "0")
            {
                strSql = "select * from limitsauthority where  role in (select Role from  Employee where EName ='" + Ename + "')";
            }
            else
            {
               strSql = "select * from limitsauthority";
            }

           //  strSql = "select * from limitsauthority";

          //  for (int i = 0; i < list.Count; i++)
           // {
            //    strSql += "";
            //}
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }

        public SqlDataReader findrolebyname(string namebar)
        {
            string str = "select Role from Employee where JobNum = '" + namebar + "'";
            return db.get_Reader(str);
        }
        //单用户多角色执行下面方法：

        public DataTable findAllRolesInfo( string namebar)
        {
            string str = "select RoleId from tbUserRole where UserId = '" + namebar + "'";

            return db.get_DataTable(str);
            
        
            
           
        }
        public SqlDataReader findEmployeelimits(string role)
        {
            string str = "select * from limitsauthority where role = '" + role + "'";
            return db.get_Reader(str);

        }
       
    }
}