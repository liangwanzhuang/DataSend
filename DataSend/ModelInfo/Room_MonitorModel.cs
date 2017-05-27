using SQLDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ModelInfo
{
  public   class Room_MonitorModel
    {
      public DataBaseLayer db = new DataBaseLayer();
      public SqlDataReader find_jy_device(string mark)
      {
          string sql = "select distinct roomnum from machine where mark ='"+mark.Trim()+"'";
          return db.get_Reader(sql);
      }
     
      //根据指定煎药室和包装室和每页显示数量获取分页数据
     /* public DataTable get_devices_all_by_param(string jy_room, string bz_room, string pages,string p_rows)
      {
          string sql = "select top "+p_rows+" r.* from "+"(select top 100 percent row_number() over(order by m.m_id asc ) as rid,"+
              "m.machinename as 设备名称, m.status as 设备工作状态, m.usingstatus as 启用状态 from machine m "+
"where m.mark in('0','1') "+
"order by m.m_id asc) r "+
"where r.rid>("+pages.Trim()+"-1)*"+p_rows.Trim();
          return db.get_DataTable(sql);
      }*/
      public DataTable get_devices_all_by_param(string jy_room, string pages, string p_rows)
      {
          string sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by m.m_id asc ) as 序号," +
              "m.machinename as 设备名称, m.status as 设备工作状态, m.usingstatus as 启用状态  pr.Pspnum as 处方号,SUBSTRING(isnull(pr.name,''),1,1)+'**' as 患者姓名," +
              "pr.curstate as 处方状态,ti.starttime as 开始时间,isnull(ti.tisaneman,'') as 操作人 from machine m left join prescription pr "+
              "on m.pid=pr.id left join tisaneinfo ti on ti.pid =m.pid "+
              "where m.mark in('0','1') " +" and m.roomnum='"+jy_room.Trim()+"'"+
              "order by m.m_id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
          return db.get_DataTable(sql);
      }
      public DataSet get_devices_all_by_param_ds(string jy_room, string pages, string p_rows)
      {
          string sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by m.m_id asc ) as 序号," +
              "m.machinename as 设备名称, m.status as 设备工作状态, m.usingstatus as 启用状态,  pr.Pspnum as 处方号,SUBSTRING(isnull(pr.name,''),1,1)+'**' as 患者姓名," +
              "(case when pr.curstate='开始煎药' then '正在煎药' when pr.curstate='开始包装' then '正在包装' else pr.curstate end) as 处方状态,"+
              "ti.starttime as 开始时间,(case when pr.curstate='开始煎药' or pr.curstate='煎药完成' then ti.endtime when pr.curstate='开始包装' or pr.curstate='包装完成' then pa.pactime else pa.pactime end ) as 结束时间, isnull(ti.tisaneman,'') as 操作人"
              +" from machine m left join prescription pr " +
              "on m.pid=pr.id left join tisaneinfo ti on ti.pid =m.pid left join packing pa on pa.DecoctingNum=pr.id" +
              " where m.mark in('0','1') " + " and m.roomnum='" + jy_room.Trim() + "'" +
              "order by m.m_id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
          return db.Get_DataSet(sql);      
      }
      //根据指定煎药室和包装室和每页显示数量获取页数
     /* public int get_pages_by_param(string jy_room, string bz_room, string pages, string p_rows)
      { 
          int p=0;
          string sql = "select COUNT(id)/" + p_rows.Trim() + " as pages from machine where mark in('0','1') and roomnum in('"+jy_room.Trim()+
          "','"+bz_room.Trim()+"'"+")";
          SqlDataReader sr = db.get_Reader(sql);         
          if (sr.Read())
          {
              p = Int32.Parse(sr["pages"].ToString());
          }
          sr.Close();
          return p;
      }
      */
      //返回设备监控页数
      public int get_pages_by_param(string jy_room,string p_rows)
      {
          int p = 0;
          string sql = "select COUNT(id) "  + " as pages from machine where "+"  roomnum ='"+
            jy_room.Trim()+ "'";
          SqlDataReader sr = db.get_Reader(sql);
          if (sr.Read())
          {
              p = Int32.Parse(sr["pages"].ToString());
          }
          sr.Close();
//test
          int r = 1;
          if (p % Int32.Parse(p_rows) == 0)
          {
              r = p / Int32.Parse(p_rows);
          }
          if (p % Int32.Parse(p_rows) != 0)
          {
              r = p / Int32.Parse(p_rows) + 1;
          }
          if (p == 0)
          {
              r = 1;
          }
          return r;
        //  return p+1;
      }
      #region
      //桐君堂二楼大屏接方、调剂、复核环节
      public DataSet get_room_larger_monitor_all_by_param_ds(string room_2, string pages, string p_rows)
      {
          string sql = "";
          if (room_2 == "已接方")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号, "+
              "h.Hshortname  as 处方来源,ps.outpatientIndex as 处方序号, p.Pspnum as 处方号,SUBSTRING(isnull(p.name,''),1,1)+'**' as 患者姓名,p.dose as 贴数,p.takenum as 次数, " +
              "(case when p.curstate='已审核' then '已接方' else '已接方' end) as 当前状态, p.dotime as 接方时间 from " +
" prescription p left join Hospital h on p.Hospitalid=h.id  left join prescriptionSubsidiary ps on p.id=ps.pid" +
" where p.curstate in('已审核','开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and p.dotime >='" +
 System.DateTime.Now.ToString("yyyy-MM-dd")+" 00:00:00" + "'" + " order by p.id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
           //   db.write_log_txt("二楼大屏接方显示SQL:" + sql);
          }
          if (room_2 == "正在调剂")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,  h.Hshortname  as 处方来源," +
             " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(isnull(p.name,''),1,1)+'**'  as 患者姓名,p.dose as 贴数,p.takenum as 次数," +
             "(case when p.curstate='已审核' then '未调剂' when p.curstate='开始调剂' then '正在调剂' else '已调剂' end) as 当前状态,isnull(a.SwapPer,'') as 调剂人员," +
             "a.wordDate as 开始时间  from prescription p left join Hospital h on p.Hospitalid"+
              "=h.id left join adjust a on a.prescriptionId  =p.id left join prescriptionSubsidiary ps on p.id=ps.pid where p.curstate in('已审核','开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') " +
             " and p.dotime >='" +
 System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'"+
              " order by p.id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();      
           //   db.write_log_txt("二楼大屏调剂显示SQL:" + sql);
          }
          if (room_2 == "正在复核")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,  h.Hshortname  as 处方来源," +
            " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(isnull(p.name,''),1,1)+'**'  as 患者姓名,p.dose as 贴数,p.takenum as 次数," +
            "(case when p.curstate='开始调剂' or p.curstate ='调剂完成' then '未复核'  else '已复核'  end) as 当前状态,isnull(a.ReviewPer,'') as 复核人员," +
            "a.AuditTime as 复核时间  from prescription p left join Hospital h on p.Hospitalid" +
             "=h.id left join Audit  a on a.pid  =p.id left join prescriptionSubsidiary ps on p.id=ps.pid  where p.curstate in('开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') " +
            " and p.dotime >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" + 
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim(); 
           //   db.write_log_txt("二楼大屏复核显示SQL:" + sql);
            
          }
           if (room_2 == "调剂预警")
              {
                  sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号," +
                  "p.Hshortname as 处方来源,p.p_no as 处方序号,p.psp_name AS 处方号,SUBSTRING(isnull(n.name,''),1,1)+'**'  AS 患者姓名,'未调剂' AS 当前状态,p.p_dose AS 贴数,n.dotime AS 当前时间" +
                  " FROM tb_led_warning p left join prescription n on n.pspnum=p.psp_name  where p.mark='tj' " +
                " and p.p_Date >='" +
               System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
                  " order by p.id asc) r " +
                 "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              }
          #region 分页实例SQL
          /*string sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by m.m_id asc ) as 序号," +
              "m.machinename as 设备名称, m.status as 设备工作状态, m.usingstatus as 启用状态,  pr.Pspnum as 处方号,pr.name as 患者姓名," +
              "pr.curstate as 处方状态,ti.starttime as 开始时间,ti.tisaneman as 操作人 from machine m left join prescription pr " +
              "on m.pid=pr.id left join tisaneinfo ti on ti.pid =m.pid " +
              "where m.mark in('0','1') " + " and m.roomnum='" + room_2.Trim() + "'" +
              "order by m.m_id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();*/
          #endregion
          return db.Get_DataSet(sql);
      }
      //返回桐君堂二楼大屏接方、调剂、复核环节页数
      public int get_room_2_pages_by_param(string room_2, string p_rows )
      {
          int p = 0;
          string sql = "";
          if (room_2 == "已接方")
          {
              sql = "select COUNT(id) "  + " as pages from prescription where curstate in('已审核','开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏接方显示页数SQL:"+sql);
          }
          if (room_2 == "正在调剂")
          {
              sql = "select COUNT(id)"+ " as pages from prescription where curstate in('已审核','开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏调剂显示页数SQL:" + sql);
          }
          if (room_2 == "正在复核")
          {
              sql = "select COUNT(id) " + " as pages from prescription where curstate in('开始调剂','调剂完成','复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
          }
          
          SqlDataReader sr = db.get_Reader(sql);
          if (sr.Read())
          {
              p = Int32.Parse(sr["pages"].ToString());
          }
          sr.Close();
          int r = 1;
          if (p % Int32.Parse(p_rows) == 0)
          {
              r = p / Int32.Parse(p_rows);
          }
          if (p % Int32.Parse(p_rows) != 0)
          {
              r = p / Int32.Parse(p_rows) + 1;
          }
          if (p == 0)
          {
              r = 1;
          }
          return r;
          //return p + 1;
      }
      #endregion 
      //返回桐君堂一楼大屏接方、调剂、复核环节页数
      #region 
      public DataSet get_room_larger_monito_1_r_all_by_param_ds(string room_1, string pages, string p_rows)
      {
          string sql = "";
          if (room_1== "正在泡药")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号, " +
              "h.Hshortname  as 处方来源,ps.outpatientIndex as 处方序号, p.Pspnum as 处方号,SUBSTRING(p.name,1,1)+'**'  as 患者姓名," +
              "(case when p.curstate='开始泡药' then '正在泡药' when p.curstate='复核' then '未泡药' else '已泡药' end) as 泡药状态, b.bubbleperson as 泡药人员,b.starttime as 开始时间  from " +
" prescription p left join Hospital h on p.Hospitalid=h.id " +
" left join bubble b on p.id=b.pid left join prescriptionSubsidiary ps on p.id=ps.pid where p.curstate in('复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and p.dotime >='" +
 System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" + " order by p.id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏接方显示SQL:" + sql);
          }
          if (room_1 == "泡药完成")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号, " +
              "h.Hshortname  as 处方来源,(select roomnum from machine where m_id=t.machineid) 煎药室,p.name as 患者姓名,p.Pspnum as 处方号," +
              "b.bubbleperson as 泡药员,b.EndDate as 泡药完成时间  from " +
" prescription p left join Hospital h on p.Hospitalid=h.id " +
" left join bubble b on p.id=b.pid  left join prescriptionSubsidiary ps on p.id=ps.pid left join tisaneinfo t on p.id=t.pid where p.curstate in('开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and p.dotime >='" +
 System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" + " order by p.id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏接方显示SQL:" + sql);
          }
          if (room_1 == "正在煎药")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,h.Hshortname  as 处方来源," +
             " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(p.name,1,1)+'**'  as 患者姓名,t.tisaneman as 煎药人员,t.starttime as 开始时间," +// t.endtime as 结束时间," +
             "(case when p.curstate='开始泡药' or p.curstate='泡药完成'  then '未煎药' when p.curstate='开始煎药' then '正在煎药' else '已煎药' end) as 煎药状态" +
             " from prescription p left join Hospital h on p.Hospitalid" +
              "=h.id left join tisaneinfo t on t.pid=p.id inner join tb_sys_tisane_method ts on p.decscheme =ts.method_id left join prescriptionSubsidiary ps on p.id=ps.pid  where p.curstate in('开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') " +
             " and p.dotime >='" +
 System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
              "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏调剂显示SQL:" + sql);
          }
          if (room_1 == "正在包装")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,  h.Hshortname  as 处方来源," +
            " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(p.name,1,1)+'**'  as 患者姓名," +
            "(case when p.curstate='开始煎药' or p.curstate ='煎药完成' then '未包装' when  p.curstate ='开始包装' then  '未包装' else '已包装'  end) as 当前状态," +
            "e.EName as 包装人员,pk.starttime as 开始时间 " +
            "  from prescription p left join Hospital h on p.Hospitalid" +
             "=h.id left join Packing pk on pk.DecoctingNum  =p.id left join Employee e on e.id=pk.employeeId left join machine m on m.pid=pk.DecoctingNum left join prescriptionSubsidiary ps on p.id=ps.pid where p.curstate in('开始煎药','煎药完成','开始包装','包装完成','已发货') " +
            " and p.dotime >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏复核显示SQL:" + sql);
          }
          if (room_1 == "正在发货")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,  h.Hshortname  as 处方来源," +
            " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(p.name,1,1)+'**'  as 患者姓名," +
            "(case when p.curstate='开始包装' or p.curstate ='包装完成' then '未发货'  else '已发货'  end) as 当前状态,isnull(d.Sendpersonnel,'') as 发货人员," +
            "d.SendTime as 发货时间  from prescription p left join Hospital h on p.Hospitalid" +
             "=h.id inner join Delivery d on d.DecoctingNum =p.id left join prescriptionSubsidiary ps on p.id=ps.pid where p.curstate in('开始包装','包装完成','已发货') " +
            " and p.dotime >='" +
          // " and d.SendTime >='" +
           
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏复核显示SQL:" + sql);
          }

          if (room_1 == "处方状态")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,  h.Hshortname  as 处方来源," +
            " ps.outpatientIndex as 处方序号,p.Pspnum as 处方号,SUBSTRING(p.name,1,1)+'**'  as 患者姓名," +
           // "(case when p.curstate='开始包装' or p.curstate ='包装完成' then '未发货'  else '已发货'  end) as 当前状态,isnull(d.Sendpersonnel,'') as 发货人员," +
            "p.curstate as 处方状态  from prescription p left join Hospital h on p.Hospitalid" +
             "=h.id  left join prescriptionSubsidiary ps on p.id=ps.pid where " +
            " p.dotime >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
              //   db.write_log_txt("二楼大屏复核显示SQL:" + sql);
          }
          if (room_1 == "调剂预警")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号,"+
              "p.Hshortname as 处方来源,p.p_no as 处方序号,p.psp_name AS 处方号,SUBSTRING(p.p_name,1,1)+'**' AS 患者姓名,'未调剂' AS 当前状态,p.p_dose AS 贴数,n.dotime AS 接方时间" +
  " FROM tb_led_warning p left join prescription n on n.pspnum=p.psp_name  where p.mark='tj' " +
            " and p.p_Date >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
          }
          if (room_1 == "煎药预警")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号," +
              "p.Hshortname as 处方来源,p.p_no as 处方序号,p.psp_name AS 煎药单号,SUBSTRING(p.p_name,1,1)+'**' AS 患者姓名,'未煎药' AS 当前状态,p.p_dose AS 贴数,n.dotime AS 接方时间" +
  " FROM tb_led_warning p left join prescription n on n.pspnum=p.psp_name where p.mark='jy' " +
            " and p.p_Date >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.id asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
          }
          if (room_1 == "温度监控")
          {
              sql = "select top " + p_rows + " r.* from " + "(select top 100 percent row_number() over(order by p.id asc ) as 序号," +
              "h.Hshortname as 处方来源,n.pspnum AS 煎药单号,p.machinename AS 设备名称,p.unitnum AS  机组名称,p.status AS 状态,p.equipmenttype AS 设备类型" +
  " FROM machine p inner join prescription n on p.pid=n.id inner join Hospital h on h.ID =n.Hospitalid  where p.mark in('0','1') and usingstatus ='启用'" +
            " and n.dotime >='" +
System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " order by p.unitnum asc) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim();
          }
          return db.Get_DataSet(sql);
      }
      public DataTable get_room_larger_monito_1_r_all_by_param_dt(string room_1, string pages, string p_rows)
      {
          string sql = "";         
          if (room_1 == "温度监控")
          {
              sql = "select  tt.id as id ,convert(varchar(60),datepart(hh,tt.time))+':'+replicate('0',2-len(convert(varchar(60),datepart(mi,tt.time))))+convert(varchar(60),datepart(mi,tt.time)) " +
 "as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
  "p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id "+
  " inner join prescription p on p.ID=tt.p_id  where tt.p_id in"
         //   " where  p.dotime >='" +System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
            
                  
                  +"(select top " + p_rows + " r.p_id as p_id from " + "(select top 100 percent row_number() over(order by a.p_id asc ) as 序号," +
              " a.p_id  as p_id from ( select distinct p.id as p_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
  " inner join prescription p on p.ID=tt.p_id " +
            " where  p.dotime >='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'" +
              " ) a) r " +
             "where r.序号>(" + pages.Trim() + "-1)*" + p_rows.Trim() +")  order by tt.id asc";
          }


          return db.get_DataTable(sql);
      }
      public DataTable get_room_larger_monito_1_r_all_by_param_dt_panel(string room_1, string pages, string p_rows,string pspnum)
      {
          string sql = "";
          if (room_1 == "温度监控")
          {
              sql = "select top 1  tt.id as id , convert(varchar(60),datepart(hh,tt.time))+':'+replicate('0',2-len(convert(varchar(60),datepart(mi,tt.time))))+convert(varchar(60),datepart(mi,tt.time)) " +
  "as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
  "p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
  " inner join prescription p on p.ID=tt.p_id  where p.pspnum='"+pspnum +"'  order by tt.id desc";
                 
          }


          return db.get_DataTable(sql);
      }
      public int get_room_3_pages_by_param(string room_3, string p_rows )
      {
          int p = 0;
          string sql = "";
          if (room_3 == "正在泡药")
          {
              sql = "select COUNT(id) "  + " as pages from prescription where curstate in('复核','开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏接方显示页数SQL:"+sql);
          }
          if (room_3 == "泡药完成")
          {
              sql = "select COUNT(id) " + " as pages from prescription where curstate in('泡药完成','开始煎药') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
              //    db.write_log_txt("二楼大屏接方显示页数SQL:"+sql);
          }
          if (room_3 == "正在煎药")
          {
              sql = "select COUNT(id)"+ " as pages from prescription where curstate in('开始泡药','泡药完成','开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏调剂显示页数SQL:" + sql);
          }
          if (room_3 == "正在包装")
          {
              sql = "select COUNT(id) " + " as pages from prescription where curstate in('开始煎药','煎药完成','开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
          }
            if (room_3 == "正在发货")
          {
              sql = "select COUNT(id) " + " as pages from prescription where curstate in('开始包装','包装完成','已发货') and dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
          //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
          }
            if (room_3 == "处方状态")
            {
                sql = "select COUNT(id) " + " as pages from prescription where dotime>='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "'";
                //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
            }
            if (room_3 == "调剂预警" )
            {
                sql = "select COUNT(id) " + " as pages from tb_led_warning where mark='tj'";
                //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
            }
            if (room_3 == "煎药预警")
            {
                sql = "select COUNT(id) " + " as pages from tb_led_warning where mark='jy'";
                //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
            }
            if (room_3 == "温度监控")
            {
              //  sql = "select COUNT(*) as pages from machine where  usingstatus ='启用' and mark in('0','1')";
                sql = "  select count(a.p_id) as pages from (select distinct tt.p_id as p_id from tb_temp tt inner join machine m on m.id=tt.machine_id "+
              "  inner join prescription p on p.ID=tt.p_id  where  p.dotime >='" + System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "') a";
                //    db.write_log_txt("二楼大屏复核显示页数SQL:" + sql);
            }
          SqlDataReader sr = db.get_Reader(sql);
          if (sr.Read())
          {
              p = Int32.Parse(sr["pages"].ToString());
          }
          sr.Close();
          int r = 1;
          if (p % Int32.Parse(p_rows) == 0)
          {
              r = p / Int32.Parse(p_rows);
          }
          if (p % Int32.Parse(p_rows) != 0)
          {
              r = p / Int32.Parse(p_rows) + 1;
          }
          if (p == 0)
          {
              r = 1;
          }
          return r;
          //return p + 1;
      }
      #endregion 
      #region 根据预警消息返回预警记录数
      public string get_yj_count(string yj)
      {
          string sql = "";
         if (yj== "调剂预警" )
            {
                sql = "select COUNT(id) " + " as pages from tb_led_warning where mark='tj'";
            }
         if (yj == "煎药预警")
         {
             sql = "select COUNT(id) " + " as pages from tb_led_warning where mark='jy'";
         }
         SqlDataReader srj = db.get_Reader(sql);
         string statej = "";
         if (srj.Read())
         {
             statej = srj["pages"].ToString();
         }
         srj.Close();
         return statej;
      }
      #endregion 
      #region 根据员工编号取员工真实姓名
      public string get_username_by_num(string num)
      {
          string strj = "select top 1 EName from Employee where id ='" + num.Trim() + "'";
          SqlDataReader srj = db.get_Reader(strj);
          string statej = "";
          if (srj.Read())
          {
              statej = srj["EName"].ToString();
          }
          srj.Close();
          return statej;
      }
        #endregion

      #region 根据处方号获取煎药机名称
      public string get_machinename_by_psp(string psp)
      {
          string strj = "select top 1 m.machinename as machinename from machine m inner join prescription p on p.ID=m.pid where p.pspnum ='" + psp.Trim() + "'";
          SqlDataReader srj = db.get_Reader(strj);
          string statej = "";
          if (srj.Read())
          {
              statej = srj["machinename"].ToString();
          }
          srj.Close();
          return statej;
      }
      #endregion

      #region 单个处方查找温度曲线
      public DataTable get_p_dt_panel(string pspnum)
      {
          DataTable dtt = new DataTable();
          DataTable dt = new DataTable();
          string sql = "";
          //判断是否是当天处方
          int flag = 1;
          string flag_sql = "select count(tt.id) from tb_temp tt inner join machine m on m.id=tt.machine_id " +
  " inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  ";
          flag = Convert.ToInt32(db.cmd_ExecuteScalar(flag_sql));
          if (flag > 0)
          {
           //   sql = "select top 1  tt.id as id , convert(varchar(60),datepart(hh,tt.time))+':'+convert(varchar(60),datepart(mi,tt.time)) " +
  //"as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
 // "p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
//  " inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  order by tt.id desc";
            //  dt = db.get_DataTable(sql);

  sql = "select top 1 tt.id as id , convert(varchar(60),datepart(hh,tt.time))+':'+replicate('0',2-len(convert(varchar(60),datepart(mi,tt.time))))+convert(varchar(60),datepart(mi,tt.time)) " +
"as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
"p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
" inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  order by tt.id desc";
  dt = db.get_DataTable(sql);
          }
              //去历史记录表里查询
          else
          {
              string sql1 = " select top 1 t.temp_json  from tb_temp_back t inner join prescription p on p.ID=t.p_id where p.Pspnum ='" + pspnum + "'";
              SqlDataReader sdr = db.get_Reader(sql1);
              string json = "";
              while (sdr.Read())
              {
                  json = sdr["temp_json"].ToString();
              }
              sdr.Close();
             // dt = JsonHelper.JsonToDataTable(json);
              dtt = JsonHelper.JsonToDataTable(json);             
      //    dtt = JsonHelper.JsonToDataTable(json);             
       /*   string idd= dtt.Compute("max(id)", "").ToString();
          dtt.DefaultView.Sort = "id desc";
          dtt.AcceptChanges();
              DataRow[] dr = dtt.Select("id='"+idd+"'");
              dt=dtt.Clone();//克隆DATATABLE结构
          foreach (DataRow row in dr)  // 将查询的结果添加到dt中； 
            {
             dt.Rows.Add(row.ItemArray);
            //      dt.Rows.Add(row);
             //     dt.ImportRow(row);
            } 
        * */

              DataRow[] dr = dtt.Select("psp_num='" + pspnum + "'");


              //  dr1 = dtt.Rows.Find(dtt.Rows.Count - 1);
              //    int num = int.Parse(dr[dtt.Rows.Count-1].Value.ToString());
              //  DataRow delRow = shiZhongGuanLiBEO1.SHEBEI_JIEDIAN.Rows.Find(num);
              dt = dtt.Clone();//克隆DATATABLE结构
              //    dt.Rows.Add(dr1);
              int i = dr.Count() - 1;
              int j = 0;
              foreach (DataRow row in dr)  // 将查询的结果添加到dt中； 
              {
                  if (j == i)
                  {
                      dt.Rows.Add(row.ItemArray);
                  }
                  //      dt.Rows.Add(row);
                  //     dt.ImportRow(row);
                  j++;
              } 
          }

          return dt ;
      }

      public DataTable get_p_by_param_dt(string pspnum)
      {
          string sql = "";
          //判断是否是当天处方
          int flag = 1;
          DataTable dt = new DataTable();
          string flag_sql = "select count(tt.id) from tb_temp tt inner join machine m on m.id=tt.machine_id " +
  " inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  ";
          flag = Convert.ToInt32(db.cmd_ExecuteScalar(flag_sql));
          if (flag > 0)
          {
           //   sql = "select  tt.id as id , convert(varchar(60),datepart(hh,tt.time))+':'+convert(varchar(60),datepart(mi,tt.time)) " +
  //"as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
 // "p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
 // " inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  order by tt.id asc";
      //        dt=db.get_DataTable(sql);
              sql = "select  tt.id as id , convert(varchar(60),datepart(hh,tt.time))+':'+replicate('0',2-len(convert(varchar(60),datepart(mi,tt.time))))+convert(varchar(60),datepart(mi,tt.time)) " +
  "as time,m.equipmenttype as machine_type,m.machineroom as machine_room,m.unitnum as unit_num," +
  "p.Pspnum as psp_num,tt.temp as temp,m.machinename as machine_name,tt.p_id as p_id,m.id as machine_id from tb_temp tt inner join machine m on m.id=tt.machine_id " +
  " inner join prescription p on p.ID=tt.p_id  where p.pspnum='" + pspnum + "'  order by tt.id asc";
              dt = db.get_DataTable(sql);
          }
          else
          {
              string sql1 = " select top 1 t.temp_json  from tb_temp_back t inner join prescription p on p.ID=t.p_id where p.Pspnum ='" +pspnum+ "'";
             SqlDataReader sdr= db.get_Reader(sql1);  
              string json = "";
              while(sdr.Read())
              {
                  json = sdr["temp_json"].ToString();
              }
              sdr.Close();
             // dt = JsonHelper.JsonToDataTable(json);
              dt = JsonHelper.JsonToDataTable(json);
          }

          return dt;
      }
        #endregion
    }
}
