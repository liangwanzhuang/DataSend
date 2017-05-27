using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SQLDAL;


namespace ModelInfo
{
   public class UserInfo
    {
        public DataBaseLayer db = new DataBaseLayer();

        public String username;
        public String UserName
        {
            set { username = value; }//这里是给私有属性name赋值 
            get { return username; }//这里取出私有属性name的值 
        }

        public String password;
        public String Password
        {
            set { password = value; }//这里是给私有属性name赋值 
            get { return password; }//这里取出私有属性name的值 
        }

        public int UserId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }
        public SqlDataReader login(String usernamebar)
        {
            String sql = "select * from Employee where JobNum = '" + usernamebar + "'";
            return db.get_Reader(sql);

        }

        public int register(String name, String password, String email, String phone)
        {
            String sql = "INSERT INTO [User](name,pwd,email,phone) VALUES('" + name + "','" + password + "','" + email + "','" + phone + "')";

            return db.cmd_Execute(sql);

        }
    }
}
