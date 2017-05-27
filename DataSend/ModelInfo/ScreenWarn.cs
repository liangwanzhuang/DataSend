using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
    public  class ScreenWarn
    {
        public DataBaseLayer db = new DataBaseLayer();

        public int AddScreenWarn(ScreenWarnModel  model)
        {
            string strSql = @"INSERT INTO ScreenWarn
           ([hospitalID]
           ,[amDosageTime]
           ,[amDosageNumber]
           ,[pmDosageTime]
           ,[pmDosageNumber]
           ,[amTisaneTime]
           ,[amTisaneNumber]
           ,[pmTisaneTime]
           ,[pmTisaneNumber]
           ,[warnDate])
     VALUES(" + model.hospitalID + ",'" + model.amDosageTime + "'," + model.amDosageNumber + ",'" + model.pmDosageTime  +"'," + model.pmDosageNumber + ",'" + model.amTisaneTime + "'," + model.amTisaneNumber + ",'" + model.pmTisaneTime + "'," + model.pmTisaneNumber + ",'" + model.warnDate + "')";
            int n = db.cmd_Execute(strSql);
            return n;
        }

        public int UpdateScreenWarn(ScreenWarnModel model)
        {
            string strSql = @"update  ScreenWarn  set amDosageTime='" +  model.amDosageTime + "',amDosageNumber=" + model.amDosageNumber + ",pmDosageTime='" + model.pmDosageTime + "',pmDosageNumber=" + model.pmDosageNumber + ",amTisaneTime='" + model.amTisaneTime + "',amTisaneNumber=" + model.amTisaneNumber + ",pmTisaneTime='" + model.pmTisaneTime + "',pmTisaneNumber=" + model.pmTisaneNumber + ",warnDate='" + model.warnDate + "'  where hospitalID=" + model.hospitalID + "";
            int n = db.cmd_Execute(strSql);
            return n;
        }

        public DataTable GetScreenWarn(string date,int id)
        {
            string strSql = @"select s.* ,h.Hname  as hospitalName  from Hospital h  left join   ScreenWarn s  on  s.hospitalID = h.ID where warnDate='" + date + "'  and s.hospitalID = " + id;
            DataTable dt= db.get_DataTable(strSql);
            return dt;
        }

        public DataTable GetScreenWarnByHid(string date)
        {
            string strSql = @"select s.*   from   ScreenWarn s  where warnDate='" + date + "'";
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }

        /// <summary>
        /// 配方
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDosageWarning(string date, int num, int hId)
        {
            string strdate = DateTime.Now.ToString("yyyy-MM-dd");
            string strSql = @"select s.*,h.Hnum ,h.Hname  from  prescription as s   inner  join Hospital  h 
on  h.ID=s.Hospitalid and h.DrugSendDisplayState='0'  and s.id not in (
select top " + num + @"  p.id   from prescription p  where  p.curstate in ('已审核','未匹配','已匹配')  and p.dotime  between '" + strdate + " 00:00:00" + "'  and '" + strdate + " " + date + "' and p.Hospitalid=" + hId + @") and   s.curstate in ('已审核','未匹配','已匹配')  and s.dotime>='" + strdate + " 00:00:00" + "' and s.Hospitalid=" + hId;
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }

        /// <summary>
        /// 煎药
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetTisaneWarning(string date, int num, int hId)
        {
            string strdate = DateTime.Now.ToString("yyyy-MM-dd");
            string strSql = @"select s.*,h.Hnum ,h.Hname  from  prescription as s   inner  join Hospital  h 
on  h.ID=s.Hospitalid and h.DrugSendDisplayState='0'  and s.id not in (
select top " + num + @"  p.id   from prescription p  where  p.curstate in ('复核','开始泡药','泡药完成')  and p.dotime  between '" + strdate + " 00:00:00" + "'  and '" + strdate + " " + date + "' and p.Hospitalid=" + hId + @") and   s.curstate in ('复核','开始泡药','泡药完成')  and s.dotime>='" + strdate + " 00:00:00" + "'  and s.Hospitalid=" + hId;
            DataTable dt = db.get_DataTable(strSql);
            return dt;
        }
    }
}
