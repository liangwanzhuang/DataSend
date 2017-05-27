using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SQLDAL;
using System.Data.SqlClient;
namespace ModelInfo
{
    public class RecheckedModel
    {

        public DataBaseLayer db = new DataBaseLayer();

        #region 添加复核信息
        ///// <summary>
        ///// 更新复核信息
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>

        public int addRechecked(int userid, string wordDate, string barcode, string wordcontent, string tisaneNum, string imgname, string userName)
        {
            string sql_is_tiaoji = "select count(id) as num from adjust where status='0' and enddate is null and prescriptionId = '" + tisaneNum.Trim() + "'";
            SqlDataReader sr_is_tiaoji = db.get_Reader(sql_is_tiaoji);
            if (sr_is_tiaoji.Read())
            {
                string result_num = sr_is_tiaoji["num"].ToString();
                int flag_t = Convert.ToInt32(result_num);
                //状态为开始调剂
                if (flag_t > 0)
                {
                    string u_sql = "update adjust set status='1',enddate=dateadd(MINUTE,-1,GETDATE()) where prescriptionId = '" + tisaneNum.Trim() + "'";
                    db.cmd_Execute(u_sql);
                }
                //调剂完成状态，不做操作

            }


            string sql = "insert into Audit(ReviewPer,AuditTime,barcode,pid,imgname,AuditStatus,employeeId) values('" + userName + "','" + wordDate + "','" + barcode + "','" + tisaneNum + "','" + imgname + "','1','" + userid + "')";
            string sql2 = "update prescription set doperson ='" + userName + "', curstate = '复核'  where id = '" + tisaneNum + "'";
            db.cmd_Execute(sql2);
            return db.cmd_Execute(sql);

        }


        #endregion

        #region 更新复核信息
        ///// <summary>
        ///// 更新调剂信息
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>int对象</returns>

        public int updateRechecked(int id, int status, string endDate)
        {
            string sql = "update rechecked set status='" + status + "',endDate='" + endDate + "' where id=" + id;

            return db.cmd_Execute(sql);

        }


        #endregion

        #region 查询复核信息
        ///// <summary>
        ///// 查询复核信息
        ///// </summary>
        ///// <returns>DataTable对象</returns>
        public DataTable findRecheckedInfo(int userid, string date,string hospital_name)
        {
              string sql = "";
              string flag = "";
              flag = db.get_role_by_userid(userid);
              string sql_1 = "";
              if (hospital_name != "0")
              {
                  sql_1 = "'" + hospital_name.Replace("_", "','") + "'";
              }
              else
              {
                  sql_1 = "select id from hospital";
              }
              if (flag != "0")
              {
                  sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                     + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,(select count(*) from drug as d where d.pid = p.id) as drugnum,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                     + ",r.id as rid,r.AuditStatus rstatus,r.AuditTime rwordDate,r.imgname rimgname  from prescription as p inner join Audit r on p.id=r.pid left join hospital hs on hs.id = p.hospitalid where r.employeeId=" + userid + " and CONVERT(varchar, r.AuditTime, 120) like '%" + date + "%' "+" and hs.id in("+sql_1  +")"+ " order by r.AuditTime desc";
              }
              else
              {
                  sql = "select p.ID,p.Pspnum,p.customid,p.delnum,p.Hospitalid,p.name,p.sex,p.age,p.phone,p.address,p.department,p.inpatientarea,p.ward,p.sickbed,(select hnum from hospital as h where h.id = p.hospitalid) as hnum,(select hname from hospital as h where h.id = p.hospitalid) as hname,"
                    + "p.diagresult,p.dose,p.takenum,p.getdrugtime,p.getdrugnum,p.takemethod,p.decscheme,p.oncetime,p.twicetime,p.packagenum,p.dotime,p.doperson,(select count(*) from drug as d where d.pid = p.id) as drugnum,p.dtbcompany,p.dtbaddress,p.dtbphone,p.dtbtype,p.soakwater,p.soaktime,p.labelnum,p.remark,p.doctor,p.footnote,p.ordertime,p.curstate"
                    + ",r.id as rid,r.AuditStatus rstatus,r.AuditTime rwordDate,r.imgname rimgname  from prescription as p inner join Audit r on p.id=r.pid left join hospital hs on hs.id = p.hospitalid   where " + "  CONVERT(varchar, r.AuditTime, 120) like '%" + date + "%' " + " and hs.id in(" + sql_1 + ")" + " order by r.AuditTime desc";
              }

            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        public DataTable findRecheckedInfoByBarcode(string barcode)
        {
            string sql = "select top 1 * from Audit where barcode='" + barcode + "'";
            db.write_log_txt("调剂：  " + sql);
            DataTable dt = db.get_DataTable(sql);

            return dt;

        }
        #endregion
    }
}
