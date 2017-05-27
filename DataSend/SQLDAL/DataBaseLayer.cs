using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.IO;


namespace SQLDAL
{
    public class DataBaseLayer
    {
        //private static dbControl m_objDBcontrol = null;

        public static string strSql =  ConfigurationManager.ConnectionStrings["consql"].ToString(); //ConfigurationManager.ConnectionStrings["SKConnection"].ConnectionString;//数据库连接字符串
        //public static string strSql = "Data Source=118.244.237.123;Initial Catalog=rinfo;user id=sa;password=dalianvideo;MultipleActiveResultSets=true";

        private SqlConnection myConn = null;

        #region 构造函数
        public DataBaseLayer()
        {
            myConn = new SqlConnection(strSql);
        }
        #endregion

        #region 实例化数据库操作对象
        ///// <summary>
        ///// 实例化数据库操作对象
        ///// </summary>
        ///// <param name="connectionString">连接数据库的字符串</param>
        ///// <returns>数据库控制对象</returns>
        //public static dbControl GetDBOpterator(string connectionString)
        //{
        //    try
        //    {
        //        if (m_objDBcontrol == null)
        //        {
        //            strSql = connectionString;
        //            m_objDBcontrol = new dbControl();
        //        }
        //        return m_objDBcontrol;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}
        #endregion

        #region  执行存储过程要返回值的（Enum)
        /// <summary>
        /// 要执行的SQL语句类型
        /// </summary>
        public enum sp_ReturnType
        {
            /// <summary>
            /// 返回值为单个DataTable
            /// </summary>
            DataTable,

            /// <summary>
            /// 受影响行数
            /// </summary>
            AffectedRowsCount

        }
        #endregion

        #region 通过带有参数的Sql语句获取DataReader[推荐使用此方法]， SqlLDataReader类型
        /// <summary>
        /// 通过带有参数的SQL语句获取SqlDataReader对象
        /// </summary>
        /// <param name="strSql">带有参数的SQL语句,如："select * from Sample where id=@id"</param>
        /// <param name="paramsArr">可以是一个参数数组</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader get_Reader(string strSql, params  SqlParameter[] paramArray)
        {
            SqlCommand myCmd = new SqlCommand();
            //添加SqlCommand对象的参数
            foreach (SqlParameter temp in paramArray)
            {
                myCmd.Parameters.Add(temp);
            }

            //利用SqlCommand对象的ExecuteReader()方法获取SqlDataReader;
            try
            {
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;
                ConnectionManage(true);
                return myCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
            }
        }
        #endregion

        #region 通过不带有参数的Sql语句获取DataReader[不推荐使用此方法]， SQLDataReader类型
        /// <summary>
        /// 通过不带有参数的Sql语句获取DataReader[不推荐使用此方法]
        /// </summary>
        /// <param name="strSql">要获取DataReader执行的Sql语句</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader get_Reader(string strSql)
        {
            SqlCommand myCmd = new SqlCommand();
            myCmd.CommandText = strSql;
            myCmd.Connection = myConn;
            try
            {
                ConnectionManage(true);
                return myCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
           {
                myCmd.Dispose();
            }
        }
        #endregion

        #region 通过带有参数的Sql语句获取DataTable[推荐使用此方法]，返回值：DataTable类型
        /// <summary>
        /// 通过带有参数的Sql语句获取DataTable[推荐使用此方法]
        /// </summary>
        /// <param name="strSql">含参数的带有查询功能的Sql语句
        /// </param>
        /// <param name="paramArray"></param>
        /// <returns>DataTable对象</returns>
        public DataTable get_DataTable(string strSql, params SqlParameter[] paramArray)
        {
            DataTable dtTemp = new DataTable();
            SqlCommand myCmd = new SqlCommand();
            SqlDataAdapter myDataAdapter = null;
            try
            {
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;

                //添加SqlCommand对象的参数
                foreach (SqlParameter temp in paramArray)
                {
                    myCmd.Parameters.Add(temp);
                }

                myDataAdapter = new SqlDataAdapter(myCmd);
                ConnectionManage(true);
                myDataAdapter.Fill(dtTemp);
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                ConnectionManage(false);
                myDataAdapter.Dispose();
                dtTemp.Dispose();
                myCmd.Dispose();
            }
        }
        #endregion

        #region 通过Sql语句获取DataTable[不推荐使用此方法],返回值：DataTable类型
        /// <summary>
        ///  通过Sql语句获取DataTable[不推荐使用此方法]
        /// </summary>
        /// <param name="strSql">要获取DataTable执行的Sql语句</param>
        /// <returns>DataTable对象</returns>
        public DataTable get_DataTable(string strSql)
        { 
         
            DataTable dtTemp = new DataTable();
            SqlCommand myCmd = new SqlCommand();
          if (strSql != "")
         {
                SqlDataAdapter myDataAdapter = null;
                try
                {
                    myCmd.Connection = myConn;
                    myCmd.CommandText = strSql;
                    myDataAdapter = new SqlDataAdapter(strSql, myConn);
                    ConnectionManage(true);

                    myDataAdapter.Fill(dtTemp);
                    return dtTemp;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    //myDataAdapter.Dispose();
                    ConnectionManage(false);
                    myCmd.Dispose();
                    //dtTemp.Dispose();

                }
         }
           else
           {
            return dtTemp;
           }

        }
        #endregion
        #region 通过SQL语句获取DATASET
        public DataSet Get_DataSet(string sql_str)
        {
            SqlDataAdapter sdr = null;
            DataSet ds = new DataSet();
            try
            {
                myConn.Open();
                sdr = new SqlDataAdapter(sql_str, myConn);
               
                sdr.Fill(ds, "table_dap");
                myConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                myConn.Close();
                //ex = Microsoft.SqlServer.Server.
                //ErrorLog.Log(exp);
                //====================================
                string strE = "内部错误:" + ex.InnerException.ToString() + "\r\n堆栈：" + ex.StackTrace + "\r " + "Message:" + ex.Message + "\r 来源:" + ex.Source + "sql:" + sql_str;
                Log(strE);

                // Microsoft.SqlServer.Server.ClearError();                
                throw ex;
            }
            finally
            {
                myConn.Close();
            }
        }
        private void Log(string strE)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 用带参数的Sql语句执行具有添加、修改、删除功能的Sql语句[推荐使用此方法],返回值类型：int类型
        /// <summary>
        /// 用带参数的Sql语句执行具有添加、修改、删除功能的Sql语句[推荐使用此方法]
        /// </summary>
        /// <param name="strSql">带有参数的SQL语句,如："update Sample set Column1=@column1 where id=@id"</param>
        /// <param name="paramArray">可以是一个参数数组</param>
        /// <returns>Sql语句影响的行数</returns>
        public int cmd_Execute(string strSql, params SqlParameter[] paramArray)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;

                myCmd.Parameters.Clear();

                //添加SqlCommand对象的参数
                foreach (SqlParameter temp in paramArray)
                {
                    myCmd.Parameters.Add(temp);
                }

                ConnectionManage(true);
                return myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }


        #endregion

        #region 执行具有添加、修改、删除功能的Sql语句[不推荐使用此方法] int类型
        /// <summary>
        /// 执行具有添加、修改、删除功能的Sql语句[不推荐使用此方法]
        /// 如:"update Sample set column1='value' where column2='value'"
        /// 此种方法无法防止SQL注入，除非你手动过滤其非法字符
        /// </summary>
        /// <param name="strSql">要执行的Sql语句</param>
        /// <returns>SQL语句影响的行数</returns>
        public int cmd_Execute(string strSql)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;
                ConnectionManage(true);
                return myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        
        public int cmd_Execute_value(string strSql)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;
                ConnectionManage(true);
              //  return myCmd.ExecuteNonQuery();
                return Int16.Parse(myCmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion

        #region 获取单行单列的值
        /// <summary>
        /// 获取单行单列的值,这种查询适合于用聚合函数查询时的情况
        /// </summary>
        /// <param name="strSql">SQL语句,如select count(*) from TableName</param>
        /// <returns>第一行第一列的值</returns>
        public string cmd_ExecuteScalar(string strSql)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Connection = myConn;
                myCmd.CommandText = strSql;
                ConnectionManage(true);
                return myCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion

        #region  执行返回值不是表的的存储过程
        /// <summary>
        /// 执行 返回值不是表的且无参数的存储过程
        /// </summary>
        /// <param name="str_ProcudureName">存储过程名</param>
        /// <returns>返回该存储过程影响的行数</returns>
        public int sp_Execute(string str_ProcudureName)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.Connection = myConn;
                ConnectionManage(true);
                return myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion
        #region 执行带参数没有返回值的存储过程
        public int sp_Execute_no_return(string str_ProcudureName,int param_value)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.Parameters.Add("@p_num", SqlDbType.Int);
                myCmd.Parameters["@p_num"].Value = param_value;
                myCmd.Connection = myConn;
                ConnectionManage(true);
                return myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion
        #region 执行带参数没有返回值的存储过程
        public int sp_Execute_no_return_two(string str_ProcudureName, string param_value,string control_status)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.Parameters.Add("@p_id", SqlDbType.VarChar);
                myCmd.Parameters["@p_id"].Value = param_value;
                myCmd.Parameters.Add("@control_status", SqlDbType.VarChar);
                myCmd.Parameters["@control_status"].Value = control_status;

                SqlParameter para_out = new SqlParameter("@r_id", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.Connection = myConn;
                ConnectionManage(true);
               myCmd.ExecuteNonQuery();
                return Int16.Parse(myCmd.Parameters["@r_id"].Value.ToString().Trim());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion
        #region 执行返回值为单个数据表的存储过程
        /// <summary>
        /// 执行带有参数的存储过程
        /// 如果该存储过程返回数据表，则返回值为数据表
        /// 否则，返回该存储过程影响的行数
        /// </summary>
        /// <param name="str_ProcudureName">存储过程名称</param>
        /// <param name="paramArray">参数数组</param>
        /// <returns>DataTable对象</returns>
        public Object sp_Execute(string str_ProcudureName, sp_ReturnType returnType, params SqlParameter[] paramArray)
        {
            SqlParameter s = new SqlParameter();
            SqlCommand myCmd = new SqlCommand();
            SqlDataAdapter myDataAdapter;
            try
            {
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter temp in paramArray)
                {
                    myCmd.Parameters.Add(temp);
                }

                ConnectionManage(true);

                myCmd.Connection = myConn;
                //返回值为DataTable
                if (returnType == sp_ReturnType.DataTable)
                {
                    myDataAdapter = new SqlDataAdapter(myCmd);

                    DataTable dtTemp = new DataTable();
                    myDataAdapter.Fill(dtTemp);
                    if (dtTemp != null)
                        return dtTemp;
                    else
                        return null;
                }
                else
                {
                    //返回值为受影响的行数
                    return myCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        #endregion

        #region 执行具有输出参数的存储过程
        /// <summary>
        /// 执行带有输出参数的存储过程
        /// </summary>
        /// <param name="str_ProcudureName">存储过程名称</param>
        /// <param name="outParam">输出参数的名称</param>
        /// <param name="paramArray">参数数组</param>
        /// <returns>Object类型</returns>
        public object sp_Execute(string str_ProcudureName, string outParam, params SqlParameter[] paramArray)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter temp in paramArray)
                {
                    myCmd.Parameters.Add(temp);
                }

                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters[outParam].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }
        }
        public object sp_Execute_ave(string str_ProcudureName, string outParam)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {   ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
             
              
              
               // SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.VarChar, 600);
                SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.Int);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@jz_id"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }      


        }
        public object sp_Execute_ave_1(string str_ProcudureName)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;



                // SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.VarChar, 600);
                SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.Int);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@jz_id"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }


        }   
        //sp_global_medi_warning
        public object sp_global_medi_warning_new(string str_ProcudureName, string outParam,  string pspnum)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;

                //填充参数
              
                myCmd.Parameters.Add("@cf", SqlDbType.VarChar);
                //为参数赋值
            
                myCmd.Parameters["@cf"].Value = pspnum;

                SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@gm_str"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }

        }
        //sp_alarm_soak
        public object sp_alarm_soak(string str_ProcudureName, string employee)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;

                //填充参数

                myCmd.Parameters.Add("@employee", SqlDbType.VarChar);
                //为参数赋值

                myCmd.Parameters["@employee"].Value = employee;

                SqlParameter para_out = new SqlParameter("@alarm", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@alarm"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {              
                myCmd.Dispose(); 
                myConn.Close();
                ConnectionManage(false);
            }

        }
        //返回药品匹配相关消息
        public object sp_global_medi_warning(string str_ProcudureName, string outParam)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para_out = new SqlParameter("@gm_str", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@gm_str"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }


        }   
        public object sp_Execute_drug(string str_ProcudureName, string outParam, string Hospitalid,string pspnum)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;

                //填充参数
                myCmd.Parameters.Add("@hospitalid", SqlDbType.Int);
                myCmd.Parameters.Add("@cf", SqlDbType.VarChar);
                //为参数赋值
                int hid = Convert.ToInt32(Hospitalid);
                myCmd.Parameters["@hospitalid"].Value = hid;
                myCmd.Parameters["@cf"].Value = pspnum;

                SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@jz_id"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }

                    }
        public object sp_Execute_machine(string str_ProcudureName, string outParam, string cf, string machineid)
        {
            SqlCommand myCmd = new SqlCommand();
            try
            {
                ConnectionManage(true);
                myCmd.Connection = myConn;
                myCmd.CommandText = str_ProcudureName;
                myCmd.CommandType = CommandType.StoredProcedure;

                //填充参数
                myCmd.Parameters.Add("@machineid", SqlDbType.Int);
                myCmd.Parameters.Add("@cf", SqlDbType.VarChar);
                //为参数赋值
                int hid = Convert.ToInt32(machineid);
                myCmd.Parameters["@machineid"].Value = hid;
                myCmd.Parameters["@cf"].Value = cf;

                SqlParameter para_out = new SqlParameter("@jz_id", SqlDbType.VarChar, 600);
                para_out.Direction = ParameterDirection.Output;
                myCmd.Parameters.Add(para_out);
                myCmd.ExecuteNonQuery();
                return myCmd.Parameters["@jz_id"].Value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                myCmd.Dispose();
                ConnectionManage(false);
            }

        }
        #endregion

        #region Connection对象处理

        /// <summary>
        /// 关于对Connection对象的处理
        /// </summary>
        /// <param name="IsOpen">True：打开，False:关闭</param>
        private void ConnectionManage(bool IsOpen)
        {
            if (IsOpen == true)
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
            }
            else if (IsOpen == false)
            {
                if (myConn.State != ConnectionState.Closed)
                {
                    myConn.Close();
                }
            }
        }
     
        #endregion




        public string GetMD5(string strPwd)
        {
            string pwd = "";
            //实例化一个md5对象
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(strPwd));
            //翻转生成的MD5码  


            // s.Reverse();
            //通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //只取MD5码的一部分，这样恶意访问者无法知道取的是哪几位
            for (int i = 3; i < s.Length - 1; i++)
            {
                //将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                //进一步对生成的MD5码做一些改造
                pwd = pwd + (s[i] < 198 ? s[i] + 28 : s[i]).ToString("X");
            }
            return pwd;
        }
        //根据煎药单号获取拍照数量
        public int get_image_num_by_pid(string p_id,string p_str)
        {
            string sql = "select count(".TrimEnd() + "id)".TrimStart() + " as mid from " + "tb_node_image"+" where n_id='"+p_id.Trim()+"'"+" and p_str='"+p_str.Trim()+"'";
            write_log_txt("返回照片数量SQL："+sql);
           SqlDataReader sr1= get_Reader(sql);
           string str = "";
          
           try
           {
               while (sr1.Read())
               {

                   str = sr1["mid"].ToString();

               }
               return Int16.Parse(str);
           }
           catch
           {
               return -1;
           }
        }
        //根据煎药单号获取处方状态
        public string  get_pstatus_by_pid(string p_id)
        {
            string sql = "select top 1 curstate as s from " + "prescription " + " where id='" + p_id.Trim() + "'";
            SqlDataReader sr1 = get_Reader(sql);
            string str = "";
            try
            {
                while (sr1.Read())
                {

                    str = sr1["s"].ToString();

                }
                sr1.Close();
                return str;
            }
            catch               
            { 
                sr1.Close();
                return "0";
            }
        }
        //根据煎药单号获取离煎药完成还剩多长时间
        public string get_tisanetime_by_pid(string p_id)
        {
            string sql = "select top 1 DATEDIFF(MINUTE,getdate(),dateadd(MINUTE,ts.tisane_time,t.starttime )) as t from " + "prescription p inner join tisaneinfo t on t.pid=p.id inner join tb_sys_tisane_method ts on p.decscheme =ts.method_id   " + " where p.id='" + p_id.Trim() + "'";
            SqlDataReader sr1 = get_Reader(sql);
            string str = "";
           write_log_txt("包装排错："+sql);
            try
            {
                while (sr1.Read())
                {

                    str = sr1["t"].ToString();

                }
                sr1.Close();
                str = "距煎药完成还有" + str + "分钟。";
                return str;
            }
            catch
            {
                sr1.Close();
                return "0";
            }
        }
        //根据员工编号获取员工JOB
        public string get_JobNum_by_id(string id)
        {
            string sql = "select top 1 JobNum  as t from Employee WHERE ID='" +id.Trim() + "'";
            SqlDataReader sr1 = get_Reader(sql);
            string str = "";
            try
            {
                while (sr1.Read())
                {

                    str = sr1["t"].ToString();

                }
                sr1.Close();
               // str = "距煎药完成还有" + str + "分钟。";
                return str;
            }
            catch
            {
                sr1.Close();
                return "0";
            }
        }
        //根据用户名获取用户权限
        public string get_role_by_userid(int userid)
        {
            string sql = "select top 1 Role as t from Employee  where JobNum='"+userid.ToString().Trim()+"'";
            SqlDataReader sr1 = get_Reader(sql);
            string str = "";
            try
            {
                while (sr1.Read())
                {

                    str = sr1["t"].ToString();

                }
                sr1.Close();               
                return str;
            }
            catch
            {
                sr1.Close();
                return "-1";
            }
        }
        //煎药各流程插入图片
        public string image_insert(string imgbase64, string demo, string str_p, string p_id, string p_status)
        {
         //   if (get_image_num_by_pid(p_id) >=0 && get_image_num_by_pid(p_id) <= 5)
          //  {
            write_log_txt("图片IMAGBASE64:" + imgbase64);

            if (imgbase64.Trim().Length > 100)
            {
                string imgname = null;
                string[] str1 = imgbase64.Split('_');
                string image = "";
                int j = 1; 
                if (get_image_num_by_pid(p_id, str_p) >= 0 && get_image_num_by_pid(p_id, str_p) <= 5)
                    {
                foreach (string i in str1)
                {
                   
                        image = i.Replace(' ', '+');
                        //  write_log_txt("调剂图片接收： " +j.ToString()+":"+ image);
                        Byte[] bimg = Convert.FromBase64String(image);
                        //D:\\项目\\煎药厂\\src\\web\\upload\\

                        String path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "upload\\";
                        imgname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + j.ToString().Trim() + ".png";
                        FileBinaryConvertHelper.Bytes2File(bimg, path + imgname);
                        // write_log_txt(imgbase64.Length.ToString());
                        //插入图片关联表
                        string image_sql = "INSERT INTO tb_node_image(n_id,image_str,syscreated,demo,p_str,p_status,imgbase64) select '" + p_id.Trim() + "','" +
                            imgname.Trim() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + "'" + demo.Trim() + "'," + "'" + str_p.Trim() + "',"
                            + "'" + p_status.Trim() + "','" + "" + "'";
                        //   write_log_txt(str_p+"环节" + ":" + image_sql);
                        //    write_log_txt(str_p + "环节" + ":" + imgbase64);
                        if (cmd_Execute(image_sql) > 0)
                        {
                            write_log_txt(str_p + "环节" + "PDA插入图片成功");
                        }
                        else
                        {
                            write_log_txt(str_p + "环节" + "PDA插入图片失败");
                        }

                        j++;
                    }
                    int str_num = get_image_num_by_pid(p_id, str_p);
                    write_log_txt("{\"code\":\"0\",\"msg\":\"拍照成功\",\"拍照限定数量\":\"5\",\"msg\":\"" + "已拍照" + str_num.ToString().Trim() + "张\"}");
                    return "{\"code\":\"0\",\"msg\":\"拍照成功\",\"拍照限定数量\":\"5\",\"msg\":\"" + "已拍照" + str_num.ToString().Trim() + "张\"}";
                }
                else 
                {
                   return "{\"code\":\"1\",\"msg\":\"拍照失败\",\"拍照限定数量\":\"5\",\"msg\":\"" + "超过拍照数量限制" + "\"}";
                }
            }
            else
            {
               // if (imgbase64 == null || imgbase64.Length == 0 || imgbase64 == "null" || imgbase64.Trim().Length == 0)
                //{
                if (get_image_num_by_pid(p_id, str_p) >= 0 && get_image_num_by_pid(p_id, str_p) <= 5)
                {
                    int str_num = get_image_num_by_pid(p_id, str_p);
                    return "{\"code\":\"0\",\"msg\":\"拍照成功\",\"拍照限定数量\":\"5\",\"msg\":\"" + "已拍照" + str_num.ToString().Trim() + "张\"}";
                }
                else
                {
                    return "{\"code\":\"1\",\"msg\":\"拍照失败\",\"拍照限定数量\":\"5\",\"msg\":\"" + "超过拍照数量限制" + "\"}";
                }
             //   }
            }
        //    }
          //  else
         //   {
          //      return "{\"code\":\"1\",\"msg\":\"拍照失败\",\"拍照限定数量\":\"5\",\"msg\":\"" + "超过拍照数量限制" + "\"}";
         //   }
        } 
        //  Response.Write("{\"code\":\"1\",\"msg\":\"操作失败\"}");
        //返回tb_node_image 最大id
        public int get_tb_node_image(string tb_name,string tb_filed_name)
        {
            string sql_c = "select count(".TrimEnd() + tb_filed_name.Trim() + ")".TrimStart() + " as mid from " + tb_name;
            string sql_m = "select max(".TrimEnd() + tb_filed_name.Trim() + ")".TrimStart() + " as mid from " + tb_name;
           SqlDataReader sr1= get_Reader(sql_c);
           string str = "";
           string str2 = "";
           try
           {
               while (sr1.Read())
               {

                   str = sr1["mid"].ToString();

               }
               if (Int16.Parse(str) == 0)
               {
                   return 1;
               }
               else
               {
                   SqlDataReader sr2 = get_Reader(sql_m);
                   while (sr2.Read())
                   {

                       str2 = sr2["mid"].ToString();

                   }
                   return Int16.Parse(str2);

               }
           }
           catch
           {
               return 1;
           }

        }

        public void write_log_txt(string str)
        {
            try
            {
                // string file_str = @"..\Debug\log\" + "log".Trim() + "_".Trim() + DateTime.Now.ToString("yyyy-MM-dd").Trim() + ".txt";
                string all_path = System.AppDomain.CurrentDomain.BaseDirectory;
                all_path += "\\log";
                all_path += @"\" + "log".Trim() + "_".Trim() + DateTime.Now.ToString("yyyy-MM-dd").Trim() + ".txt";
                StreamWriter sw = File.AppendText(all_path);
                sw.WriteLine(DateTime.Now.ToString() + ":");
                sw.WriteLine(str);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                write_log_txt("写日志文件异常，write_log_txt(string str)!");
            }
        }
    }



}
