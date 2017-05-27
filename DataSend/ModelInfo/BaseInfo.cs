using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
    public class BaseInfo
    {
        public BaseInfo()
        {


        }

        /// <summary>
        /// 开始包装 插入命令表
        /// </summary>
        /// <param name="now">时间</param>
        /// <param name="db">DB层</param>
        /// <param name="bmNum">煎药单号</param>
        public static void Insert_PackCmd(DateTime now, DataBaseLayer db, string bmNum)
        {
            //开始包装指令
            string strtime = now.ToString("yyyy-MM-dd HH:mm:ss");//
            string sql12 = "select macaddress from machine where mark=1 and unitnum = (select  unitnum from machine where id = (select top 1  machineid from tisaneunit where pid ='" + bmNum + "'))";
            SqlDataReader sr12 = db.get_Reader(sql12);
            string mac = "";

            if (sr12.Read())
            {
                mac = sr12["macaddress"].ToString();
            }

            string sql10 = "select *, RIGHT(CAST('000000000' + RTRIM(id) AS varchar(20)), 10)  as bNum from prescription where id = '" + bmNum + "'";
            SqlDataReader sr10 = db.get_Reader(sql10);
            // var strzero = "0000000000";
            // string tid = strzero.substring(0, 10 - Convert.ToInt32(DecoctingNum).length) + DecoctingNum;
            //  String str = String.format("%04d", youNumber);      
            string content = "";
            if (sr10.Read())
            {
                string sql = "select package_machine_nums from tb_sys_add_setting";
                SqlDataReader pack = db.get_Reader(sql);
                int PackNum = 2;
                if (pack.Read())
                {
                    PackNum = Convert.ToInt32(pack["package_machine_nums"].ToString());
                }
                content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString()) + PackNum).ToString().PadLeft(2, '0') + bmNum.PadLeft(10, '0') + sr10["packagenum"].ToString().PadLeft(4, '0');

            }
            string sql11 = "insert into cmdtable(cmd,bmip,time) values('" + content + "','" + mac + "','" + strtime + "');";
            db.cmd_Execute(sql11);
        }
        //开始包装指令 重写
        public static void Insert_PackCmd_override(string bmNum)
        {
            DataBaseLayer db = new DataBaseLayer();
            //开始包装指令
            string strtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//
            string sql12 = "select macaddress from machine where mark=1 and unitnum = (select top 1 unitnum from machine where id =(select top 1  machineid from tisaneunit where pid ='" + bmNum + "'))";
            db.write_log_txt("重发包装指令：" + sql12);
            SqlDataReader sr12 = db.get_Reader(sql12);
            string mac = "";

            if (sr12.Read())
            {
                mac = sr12["macaddress"].ToString();
            }

            string sql10 = "select *, RIGHT(CAST('000000000' + RTRIM(id) AS varchar(20)), 10)  as bNum from prescription where id = '" + bmNum + "'";
            SqlDataReader sr10 = db.get_Reader(sql10);

            string content = "";
            if (sr10.Read())
            {
                string sql = "select package_machine_nums from tb_sys_add_setting";
                SqlDataReader pack = db.get_Reader(sql);
                int PackNum = 2;
                if (pack.Read())
                {
                    PackNum = Convert.ToInt32(pack["package_machine_nums"].ToString());
                }
                content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString()) + PackNum).ToString().PadLeft(2, '0') + bmNum.PadLeft(10, '0') + sr10["packagenum"].ToString().PadLeft(4, '0');
            }
            sr10.Close();
            string sql11 = "insert into cmdtable(cmd,bmip,time) values('" + content + "','" + mac + "','" + strtime + "');";
            db.cmd_Execute(sql11);
        }

        /// <summary>
        /// 开始煎药 插入命令表
        /// </summary>
        /// <param name="now">开始煎药时间</param>
        /// <param name="db">db层</param>
        /// <param name="bmNum">煎药单号</param>
        public static void Insert_TisaneCmd(DateTime now, DataBaseLayer db, string bmNum)
        {
            //开始煎药指令

            string sql12 = "select macaddress from machine where id = (select top 1 machineid from tisaneunit where pid ='" + bmNum + "' order by id asc)";
            SqlDataReader sr12 = db.get_Reader(sql12);
            string mac = "";

            if (sr12.Read())
            {
                mac = sr12["macaddress"].ToString();
            }

            string sql10 = "select * from prescription where id = '" + bmNum + "'";
            SqlDataReader sr10 = db.get_Reader(sql10);
            string content = "";
            if (sr10.Read())
            {
                content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString())).ToString().PadLeft(2, '0') + sr10["decscheme"].ToString().PadLeft(2, '0') + bmNum.PadLeft(10, '0') + sr10["oncetime"].ToString().PadLeft(2, '0') + sr10["twicetime"].ToString().PadLeft(2, '0');

            }
            string sql11 = "insert into cmdtable(cmd,bmip,time) values('" + content + "','" + mac + "','" + now + "')";
            sr10.Close();
            db.write_log_txt("煎药指令：" + sql11);
            db.cmd_Execute(sql11);
        }

        //重写煎药指令
        public static void Insert_TisaneCmd_orerride(string bmNum)
        {
            //开始煎药指令
            DataBaseLayer db = new DataBaseLayer();

            string sql12 = "select macaddress from machine where id = (select top 1 machineid from tisaneunit where pid ='" + bmNum + "' order by id asc)";
            SqlDataReader sr12 = db.get_Reader(sql12);
            string mac = "";

            if (sr12.Read())
            {
                mac = sr12["macaddress"].ToString();
            }

            string sql10 = "select * from prescription where id = '" + bmNum + "'";
            SqlDataReader sr10 = db.get_Reader(sql10);
            string content = "";
            if (sr10.Read())
            {
                //添加30分钟煎药方案
              /*  string str_m = "03";
                string str_w = "00";
                if (sr10["decscheme"].ToString().PadLeft(2, '0') == "02")
                {
                    str_m = "81";
                    str_w = "30";
                }
                else
                {
                    str_m = sr10["decscheme"].ToString().PadLeft(2, '0');
                    str_w = sr10["oncetime"].ToString().PadLeft(2, '0');
                }
               * */
               content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString())).ToString().PadLeft(2, '0') + sr10["decscheme"].ToString().PadLeft(2, '0') + bmNum.PadLeft(10, '0') + sr10["oncetime"].ToString().PadLeft(2, '0') + sr10["twicetime"].ToString().PadLeft(2, '0');
               // content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString())).ToString().PadLeft(2, '0') + str_m + bmNum.PadLeft(10, '0') + str_w + sr10["twicetime"].ToString().PadLeft(2, '0');

            }
            string sql11 = "insert into cmdtable(cmd,bmip,time) values('" + content + "','" + mac + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            sr10.Close();
            db.write_log_txt("煎药指令："+sql11 );
            db.cmd_Execute(sql11);
        }
        //重写煎药指令
        public static void Insert_TisaneCmd_orerride_pda(string bmNum,string mahineid)
        {
            //开始煎药指令
            DataBaseLayer db = new DataBaseLayer();

            string sql12 = "select top 1 macaddress from machine where id = mahineid";
            SqlDataReader sr12 = db.get_Reader(sql12);
            string mac = "";

            if (sr12.Read())
            {
                mac = sr12["macaddress"].ToString();
            }

            string sql10 = "select * from prescription where id = '" + bmNum + "'";
            SqlDataReader sr10 = db.get_Reader(sql10);
            string content = "";
            if (sr10.Read())
            {
                //添加30分钟煎药方案
                /*  string str_m = "03";
                  string str_w = "00";
                  if (sr10["decscheme"].ToString().PadLeft(2, '0') == "02")
                  {
                      str_m = "81";
                      str_w = "30";
                  }
                  else
                  {
                      str_m = sr10["decscheme"].ToString().PadLeft(2, '0');
                      str_w = sr10["oncetime"].ToString().PadLeft(2, '0');
                  }
                 * */
                content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString())).ToString().PadLeft(2, '0') + sr10["decscheme"].ToString().PadLeft(2, '0') + bmNum.PadLeft(10, '0') + sr10["oncetime"].ToString().PadLeft(2, '0') + sr10["twicetime"].ToString().PadLeft(2, '0');
                // content = (Convert.ToInt32(sr10["dose"].ToString()) * Convert.ToInt32(sr10["takenum"].ToString())).ToString().PadLeft(2, '0') + str_m + bmNum.PadLeft(10, '0') + str_w + sr10["twicetime"].ToString().PadLeft(2, '0');

            }
            string sql11 = "insert into cmdtable(cmd,bmip,time) values('" + content + "','" + mac + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            sr10.Close();
            db.write_log_txt("煎药指令：" + sql11);
            db.cmd_Execute(sql11);
        }

        /// <summary>
        /// 获取煎药登记表信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static DataTable GetTisanesRegReport(string ids)
        {
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                //开始包装指令
                string sql = @" select distinct ID,Pspnum,customid,delnum,Hospitalid,name,sex,age,phone,address,department,inpatientarea,ward,sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,
diagresult,dose,takenum,getdrugtime,getdrugnum,takemethod,decscheme,oncetime,twicetime,packagenum,dotime,doperson,dtbcompany,dtbaddress,dtbphone,dtbtype,soakwater,soaktime,labelnum,remark,doctor,footnote,ordertime,curstate from prescription as p  where 1=1 and id in (" + ids + ") order by id asc";
                DataTable table = db.get_DataTable(sql);
                return table;
            }
            catch
            {
                return null;
            }
        }

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
                case 85:
                    return "先煎后下自定义";
                default:
                    break;
            }
            return "NULL";
        }

        /// <summary>
        /// 根据快递编号获取快递名称
        /// </summary>
        /// <param name="Num">快递编号</param>
        /// <returns>快递名称</returns>
        public static string GetExpressageType(int Num)
        {
            switch (Num)
            {
                case 0:
                    return "厂内配送";
                case 1:
                    return "顺丰";
                case 2:
                    return "中通";
                case 3:
                    return "圆通";
                case 4:
                    return "EMS";
                default:
                    break;
            }
            return "NULL";
        }

        /// <summary>
        /// 根据编号获取服用方法
        /// </summary>
        /// <param name="Num">服用方法编号</param>
        /// <returns>服用方法</returns>
        public static string GetDirections(int Num)
        {
            switch (Num)
            {
                case 0:
                    return "无";
                case 1:
                    return "水煎餐后";
                default:
                    break;
            }
            return "NULL";
        }

        /// <summary>
        /// 根据煎药方式编号获取煎药方式名称
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string GetTisanesWay(int Num)
        {
            switch (Num)
            {
                case 1:
                    return "先煎";
                case 2:
                    return "后下";
                case 3:
                    return "加糖加蜜";
                default:
                    break;
            }
            return "NULL";
        }

        /// <summary>
        /// 根据编号获取性别
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string GetGender(int Num)
        {
            switch (Num)
            {
                case 1:
                    return "男";
                case 2:
                    return "女";
                default:
                    break;
            }
            return "NULL";
        }

    }
}
