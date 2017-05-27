using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
    public class meRoomModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        public DataTable findRecipeInfo()
        {
            string sql = "select  '调剂' AS wordcontent,convert(varchar, wordDate, 111) as wordDate,sum(cast(workload as int)) AS workload from adjust as a left join employee as e on a.employeeId=e.id where 1=1 GROUP BY CONVERT(varchar, a.wordDate, 111)";




            DataTable dt = db.get_DataTable(sql);
            return dt;

        }
        public DataTable findmeRoomInfo()
        {
            string sql = "select * from MedicineRoom ";

            return db.get_DataTable(sql);
        }
        public int AddmeRoom(string meRoomNum, string meRoomName, string Remarks)
        {
            /// <summary>
            /// 添加煎药室信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>

            String sql = "";
            int end = 0;

            string tate = "select * from MedicineRoom where meRoomNum = '" + meRoomNum + "' or meRoomName ='"+meRoomName+"'";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                sql = "";
            }
            else
            {

                sql = "insert into MedicineRoom(meRoomNum,meRoomName,Remarks) values('" + meRoomNum + "','" + meRoomName + "','" + Remarks + "')";
     
            }
            
            if (sql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(sql);
            }
            return end; ;

        }

        public SqlDataReader findmachineroom()
        {
            string sql = "select * from MedicineRoom ";

            return db.get_Reader(sql);
        }
        public int addmachineinfo(string m_id,string meRoomName, string unitnum, string machinetype, string machinenum, string macaddresss, string status, string openstatus, string healthystatus, string disinfectionstatus, string checkman, string checktime, string equipmenttype)
        {
            int end=0;
            SqlDataReader sdr = findmachineroombyid(meRoomName);
            string roomname = "";
            if(sdr.Read()){
               roomname= sdr["meRoomName"].ToString();

            }


            string str = "select * from machine where machinename ='"+machinenum+"'";
            SqlDataReader sdr3 = db.get_Reader(str);

            if (sdr3.Read())
            {
                end = 3;//机器名存在不能添加
            }
            else
            {



                //一个机组只能有一台包装机
                if (machinetype == "1")
                {
                    string sql2 = "select * from machine where roomnum ='" + roomname + "' and unitnum ='" + unitnum + "' and mark =1";
                    SqlDataReader sdr2 = db.get_Reader(sql2);
                    if (sdr2.Read())
                    {
                        end = 2;//该机组已经有了包装机
                    }
                    else
                    {
                        string sql = "";

                        sql = "insert into machine(m_id,machinename,mark,unitnum,roomnum,macaddress,status,usingstatus,healthstatus,disinfectionstatus,checkman,checktime,equipmenttype) values('" + 
                           m_id.Trim()+"',"+"'"+machinenum + "','" + machinetype + "','" + unitnum + "','" + roomname + "','" + macaddresss + "','" + status + "','" + openstatus + "','" + healthystatus + "','" + disinfectionstatus + "','" + checkman + "','" + checktime + "','" + equipmenttype + "')";

                        end = db.cmd_Execute(sql);
                    }
                }
                else
                {

                    string sql = "";

                    sql = "insert into machine(m_id,machinename,mark,unitnum,roomnum,macaddress,status,usingstatus,healthstatus,disinfectionstatus,checkman,checktime,equipmenttype) values('" +m_id.Trim()+"',"+"'"+machinenum + "','" + machinetype + "','" + unitnum + "','" + roomname + "','" + macaddresss + "','" + status + "','" + openstatus + "','" + healthystatus + "','" + disinfectionstatus + "','" + checkman + "','" + checktime + "','" + equipmenttype + "')";

                    end = db.cmd_Execute(sql);
                }

            }
            return end;
        }
        
        public int updatemachineinfo(string m_id,string meRoomName, string unitnum, string machinetype, string machinenum, string macaddresss, string idnum, string status, string openstatus, string disinfectionstatus, string healthystatus, string checkman, string checktime, string equipmenttype)
        {
            int end = 0;
         



            SqlDataReader sdr = findmachineroombyid(meRoomName);
            string roomname = "";
            if (sdr.Read())
            {
                roomname = sdr["meRoomName"].ToString();

            }

           // //把该id 机器对应的信息置空
           // string str3 = "update machine set machinename=null,mark=null,unitnum=null,roomnum=null,macaddress=null,status =null,usingstatus =null,healthstatus =null,disinfectionstatus=null,checkman=null,checktime=null,equipmenttype=null where id ='" + idnum + "'";

          //  db.cmd_Execute(str3);


            
           // string str = "select * from machine where machinename ='"+machinenum+"'";
          //  SqlDataReader sdr3 = db.get_Reader(str);

           // if (sdr3.Read())
          //  {
           //     end = 3;//机器名存在不能添加
          //  }
           // else
           // {


                //一个机组只能有一台包装机
                string sql = "";

                sql = "update machine set machinename='" + machinenum + "',mark='" + machinetype + "',unitnum='" + unitnum + "',roomnum='" + roomname + "',macaddress='" + macaddresss + "',status ='" + status + "',usingstatus ='" + openstatus + "',healthstatus ='" + healthystatus + "',disinfectionstatus='" + disinfectionstatus + "',checkman='" + checkman + "',checktime='" + checktime + "',equipmenttype='" + equipmenttype +"',"+"m_id='"+m_id.Trim()  + "' where id ='" + idnum + "'";


                end = db.cmd_Execute(sql);
           // }

            return end;
        }
        //修改煎药室信息
        public int updatemeRoom(int id,string meRoomName, string meRoomNum, string Remarks) {

            int end = 0;
            
            string sql = "";
            string tate = "select * from MedicineRoom where meRoomNum = '" + meRoomNum + "' or meRoomName ='" + meRoomName + "' and id !=" + id + "";
            SqlDataReader tate1 = db.get_Reader(tate);
            if (tate1.Read())
            {
                sql = "";
            }
            else
            {
                sql = "update MedicineRoom set meRoomName='" + meRoomName + "',meRoomNum='" + meRoomNum + "',Remarks='" + Remarks + "' where id ='" + id + "'";


                

            }

            if (sql == "")
            {
                end = 0;
            }
            else
            {
                end = db.cmd_Execute(sql);
            }
            return end; ;

        
        
        }
        #region  查询煎药机组编号
        public SqlDataReader findunitnumbymachine()
        {
            string sql = "select distinct unitnum  from machine where mark = 0  ";

            return db.get_Reader(sql);
        }
        #endregion
        #region  查询煎药室
        public SqlDataReader findroomnumbymachine()
        {
            string sql = "SELECT DISTINCT id, meRoomName FROM      MedicineRoom  ";

            return db.get_Reader(sql);
        }
        #endregion
        #region  查询包装机组编号
        public SqlDataReader findunitnumbymachine1()
        {
            string sql = "select distinct unitnum  from machine where mark = 1  ";

            return db.get_Reader(sql);
        }
        #endregion
        #region 通过煎药机组编号查询煎药机组监控信息
        public DataTable DecoctingMonitoring(string unitnum,string roomnum)
        {
           // string sql = "select id, (select meRoomNum from MedicineRoom  as m where m.meRoomName = a.roomnum) as meRoomNum, unitnum,machinename,  roomnum,usingstatus,status,healthstatus,disinfectionstatus ,'待定' as CurrentTemp from machine as a where mark = 0";
            string sql = "select id,unitnum,machinename,roomnum,usingstatus,status,healthstatus,disinfectionstatus ,'待定' as CurrentTemp from machine as a where mark = 0";
            db.write_log_txt("大屏显示相关SQL"+sql);
            if (unitnum != "0" && unitnum != "")
            {
                sql += "and unitnum ='" + unitnum + "'";
            }
            if (roomnum != "0" && roomnum != "")
            {
                sql += "and roomnum ='" + roomnum + "'";
            }
            return db.get_DataTable(sql);
        }
        #endregion
        #region 通过包装机组编号查询包装机组监控信息
        public DataTable PackingMonitoring(string unitnum, string roomnum)
        {
           // string sql = "select id, (select meRoomNum from MedicineRoom  as m where m.meRoomName = a.roomnum) as meRoomNum, unitnum,machinename,  roomnum,usingstatus,status,healthstatus,disinfectionstatus from machine as a where mark = 1";
            string sql = "select id,unitnum,machinename,roomnum,usingstatus,status,healthstatus,disinfectionstatus from machine as a where mark = 1";

            if (unitnum != "0" && unitnum != "")
            {
                sql += "and unitnum ='" + unitnum + "'";
            }
            if (roomnum != "0" && roomnum != "")
            {
                sql += "and roomnum ='" + roomnum + "'";
            }
            return db.get_DataTable(sql);
        }
        #endregion

        public DataTable findallmachineinfo(string typeofmachine)
        {
            string sql = "select * from machine where 1=1";
            if (typeofmachine !="3")
            {
            sql+="and mark ='" + typeofmachine + "'";
            }
            return db.get_DataTable(sql);
        }

        public int findallmachineinfots(string typeofmachine,string machine_name)
        {
            string sql = "select count(*) from machine where 1=1";
            if (typeofmachine != "3")
            {
                sql += "and mark ='" + typeofmachine + "' and machinename like '%"+machine_name +"%'";
            }
            int count = Convert.ToInt32(db.cmd_ExecuteScalar(sql));
            return count;
        }

        public DataTable findallmachineinfofy(string typeofmachine,string machine_name, int ts, int page)
        {
            string sql = "select top " + ts + " *  from  (  select *,row_number() over(order by q.id desc ) as rownumber  from ( select * from machine where 1=1 ";
            if (typeofmachine != "3")
            {
                sql += "and mark ='" + typeofmachine + "'";
            }
            string temp_sql = "";
            if (machine_name.Trim().Length>0 )
            {
                temp_sql = " and machinename like '%" + machine_name + "%'";
            }
            sql +=temp_sql + "  ) q  ) z where z.rownumber>" + (page - 1) * ts + " order by z.id desc";
            return db.get_DataTable(sql);
        }

        public int deletemachineinfo(string id)
        {
            string sql = "";
            int end = 0;
            sql = "delete from machine where id ='"+id+"'";

            end = db.cmd_Execute(sql);

            return end;
        }
        //更改开启状态byid
        public int updatewarningstatus(string id)
        {


            string str = "select usingstatus from machine where id = '" + id + "'";
            int end = 0;
            SqlDataReader sr = db.get_Reader(str);
            string usingstatus = "";
            if (sr.Read())
            {
                usingstatus = sr["usingstatus"].ToString();

            }

            if (usingstatus == "启用")
            {
                string str2 = "update machine set usingstatus = '停用' where id = '" + id + "'";
                end = db.cmd_Execute(str2);
            }
            else
            {
                string str3 = "update machine set usingstatus = '启用' where id = '" + id + "'";
                end = db.cmd_Execute(str3);
            }

            return end;
        }
        public int deleteMeRoomInfo(int id)
        {
            string sql = "";
            int end = 0;
            sql = "delete from MedicineRoom where id ='" + id + "'";

            end = db.cmd_Execute(sql);

            return end;
        }

        public SqlDataReader findmachineroombyid(string id)
        {
            string sql = "select * from MedicineRoom where id='"+id+"'";

            return db.get_Reader(sql);
        }
        public DataTable findmachineroombyid1(int id)
        {
            string sql = "select * from MedicineRoom where id='" + id + "'";

            return db.get_DataTable(sql);
        }
        public SqlDataReader findmachineroomidbymachineroom(string roomname)
        {
            string sql = "select * from MedicineRoom where meRoomName='" + roomname + "'";

            return db.get_Reader(sql);
        }



        public DataTable findmachienInfobyid(int id)
        {
            string sql = "select * from machine where id ='"+id+"' ";

            return db.get_DataTable(sql);
        }
        public DataTable findMachineByMarkAndUnitnum(string mark, string unitnum)
        {
            string sql = "select * from machine where mark='" + mark + "' and unitnum='" + unitnum + "'";
            return db.get_DataTable(sql);
        }
      

    }

}
