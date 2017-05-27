using System;
using System.Data.SqlClient;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
    public class HospitalModel
    {
        public DataBaseLayer db = new DataBaseLayer();

        #region 查询所有医院名称
        ///// <summary>
        ///// 查询所有医院名称
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>SqlDataReader对象</returns>
        //根据医院编号获取医院id
        public int get_hospital_id_by_hnum(string hnum)
        {

            string str = "select top 1 id from Hospital where hnum = '" + hnum.Trim() + "'";

            SqlDataReader sr = db.get_Reader(str);
            string result = "0";
            if (sr.Read())
            {
                result = sr["id"].ToString();

            }
            return Int16.Parse(result);
        }
        public SqlDataReader findHospitalAll()
        {
         string sql = "select a.*,b.relation_name as relation_name from Hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id where a.Hname!='饮片库'";

            return db.get_Reader(sql);
        }
        public SqlDataReader findHospitalAll_j()
        {
            string sql = "select a.*,b.relation_name as relation_name from Hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id where a.Hname!='饮片库' order by a.Hname desc";

            return db.get_Reader(sql);
        }
        public SqlDataReader findHospitalAll_m()
        {
            string sql = "select a.*,b.relation_name as relation_name from Hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id";

            return db.get_Reader(sql);
        }
        public SqlDataReader findHospitalAll_cf()
        {
            string sql = "select a.*,b.relation_name as relation_name from Hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id where a.Hname!='饮片库'  order by Hshortname desc ";
            return db.get_Reader(sql);
        }

        public DataTable findHospitalAll_cfTb()
        {
            string sql = "select a.*,b.relation_name as relation_name from Hospital a left join tb_hospital_drug_type b on a.relation_drug_type_id=b.relation_id where a.Hname!='饮片库'";
            return db.get_DataTable(sql);
        }

        public DataTable findNumById(string  id)
        {
            string sql = "select hnum from Hospital where ID =" + id;

            return db.get_DataTable(sql);
        }
        public string getmaxxh(int hospitalId)
        {
            string newid = "";
            string id = "";
            // string sql = string.Format("select top 1 outpatientIndex from prescriptionSubsidiary where dt1 like '%'"+DateTime.Now.ToString("yyyyMMdd")+"'%' order by dt1 desc");
            string sql = "select top 1 outpatientIndex from prescriptionSubsidiary  where datediff(day,dt1,GETDATE())=0 and hid='" + hospitalId + "' order by dt1 desc";
            //string sql = "select max(outpatientIndex) from prescriptionSubsidiary where dt1 like  '%%'";

            SqlDataReader sdr = db.get_Reader(sql);

            while (sdr.Read())
            {
                id = sdr["outpatientIndex"].ToString();
            }

            if (id == "" || id == null)
            {
                newid = "1";
            }
            else
            {
                newid = Convert.ToString(int.Parse(id) + 1);
            }

            return newid;
        }
        public DataTable findNumById(int id)
        {
            string sql = "select hnum from Hospital where ID =" + id;

            return db.get_DataTable(sql);
        }
        public DataTable findNumById_cf(int id)
        {
            string sql = "select hnum from Hospital where ID =" + id +" and Hname!="+"'"+"饮片库".Trim()+"'";

            return db.get_DataTable(sql);
        }
        public SqlDataReader findHospitalnamebyid(string id)
        {
            string sql = "select hname from Hospital where ID =" + id + " and  Hname!='饮片库'";
            return db.get_Reader(sql);
        }
        #endregion


        public SqlDataReader findYpcdrug(string pid)
        {
            string str = "select ROW_NUMBER() OVER(ORDER BY d.id desc) as id,d.drugdescription,d.description,d.DrugAllNum,d.DrugWeight,d.DrugPosition,d.drugname,(select ypcdrugPositionNum  from  DrugMatching as m where  m.pspId=d.pid and m.drugId = d.id) as ypcdrugPositionNum,d.drugnum from drug as d   where  d.pid='" + pid + "'";
            db.write_log_txt(str);
            SqlDataReader sr2 = db.get_Reader(str);
            return sr2;
        }

        public DataTable  findYpcdrugDt(string pid)
        {
            string str = "select ROW_NUMBER() OVER(ORDER BY d.id desc) as id,d.drugdescription,d.description,d.DrugAllNum,d.DrugWeight,d.DrugPosition,d.drugname,(select ypcdrugPositionNum  from  DrugMatching as m where  m.pspId=d.pid and m.drugId = d.id) as ypcdrugPositionNum from drug as d   where  d.pid='" + pid + "'";
            DataTable sr2 = db.get_DataTable(str);
            return sr2;
        }

        //添加医院报警时间

        public int addwarningtime(string hospitalid, string checkwarning, string adjustwarning, string recheckwarning, string bubblewarning, string tisanewarning, string packwarning, string deliverwarning, string type)
        {





            int end=0;
            string hospitalname = "";

            string sql3 = "select * from warning where hospitalid = '" + hospitalid + "' and type='"+type+"'";
            SqlDataReader sr2 = db.get_Reader(sql3);
            if (sr2.Read())
            {
                end = 0;
            }else{
            



            string sql2 = "select top 1 hname from hospital where id ='" + hospitalid + "'";

            SqlDataReader sr = db.get_Reader(sql2);
            if (sr.Read())
            {
              hospitalname =  sr["hname"].ToString();
            }



            string sql = "INSERT INTO [warning](hospitalid,hospitalname,checkwarning,adjustwarning,recheckwarning,bubblewarning,tisanewarning,packwarning,deliverwarning,type) VALUES('" + hospitalid + "','" + hospitalname + "','" + checkwarning + "','" + adjustwarning + "','" + recheckwarning + "','" + bubblewarning + "','" + tisanewarning + "','" + packwarning + "','" + deliverwarning + "','" + type + "')";

            end = db.cmd_Execute(sql);

            }
            return end;

    }

        //查找医院报警时间信息
        public DataTable findwarningtime(string type)
        {

            string sql = "select id,(select hname from hospital where id = w.hospitalid) as hospitalname,checkwarning,adjustwarning,recheckwarning,bubblewarning,tisanewarning,packwarning,deliverwarning,status from warning as w where type='"+type+"'";
            return db.get_DataTable(sql);

        }

        //查找医院屏显信息
        public DataTable findInfo()
        {

            string sql = "select distinct ID,hname,DrugDisplayState, ChineseDisplayState, DrugSendDisplayState from  hospital ";
            return db.get_DataTable(sql);

        }

        //删除报警信息


        public int deletewarninginfo(string id){


            string sql = "delete from warning where id = '"+id+"'";
            return db.cmd_Execute(sql);
        }
  //找到医院报警时间根据id

        public DataTable findwarningtimebyid(int id)
        {
            string str = "select * from warning where id = '" + id + "'";
            return db.get_DataTable(str);
        }
        //更改报警时间

        public int updatewarningtimeinfo(string id, string checkwarning, string adjustwarning, string recheckwarning, string bubblewarning, string tisanewarning, string packwarning, string deliverwarning)
        {
            //sql = "update bubble set bubbleperson = '" + bubbleman + "' where pid = '" + id + "' ";
           // end = db.cmd_Execute(sql);

            string str = "update warning set checkwarning='" + checkwarning + "',adjustwarning='" + adjustwarning + "',recheckwarning='" + recheckwarning + "',bubblewarning='" + bubblewarning + "',tisanewarning='" + tisanewarning + "',packwarning='" + packwarning + "',deliverwarning='" + deliverwarning + "' where  id ='" + id + "'";
            int end = db.cmd_Execute(str);

            return end;
        }
        //更改开启状态byid
        public int updatewarningstatus(string id)
        {


            string str = "select top 1 status from warning where id = '"+id+"'";
            int end = 0;
            SqlDataReader sr = db.get_Reader(str);
            string result = "";
            if (sr.Read())
            {
                result = sr["status"].ToString();

            }

            if (result == "0")
            {
                string str2 = "update warning set status = 1 where id = '" + id + "'";
               end = db.cmd_Execute(str2);
            }
            else
            {
                string str3 = "update warning set status = 0 where id = '" + id + "'";
               end = db.cmd_Execute(str3);
            }

            return end;
        }
        #region 更改泡药开启状态byid
        public int updateDrugDisplayState(string id)
        {


            string str = "select top 1  DrugDisplayState from hospital where id = '" + id + "'";
            int end = 0;
            SqlDataReader sr = db.get_Reader(str);
            string result = "";
            if (sr.Read())
            {
                result = sr["DrugDisplayState"].ToString();

            }

            if (result == "0")
            {
                string str2 = "update hospital set DrugDisplayState = '1' where id = '" + id + "'";
                end = db.cmd_Execute(str2);
            }
            else
            {
                string str3 = "update hospital set DrugDisplayState = '0' where id = '" + id + "'";
                end = db.cmd_Execute(str3);
            }

            return end;
        }
       #endregion
        #region 更改煎药开启状态byid
        public int updateChineseDisplayState(string id)
        {


            string str = "select ChineseDisplayState from hospital where id = '" + id + "'";
            int end = 0;
            SqlDataReader sr = db.get_Reader(str);
            string result = "";
            if (sr.Read())
            {
                result = sr["ChineseDisplayState"].ToString();

            }

            if (result == "0")
            {
                string str2 = "update hospital set ChineseDisplayState = '1' where id = '" + id + "'";
                end = db.cmd_Execute(str2);
            }
            else
            {
                string str3 = "update hospital set ChineseDisplayState = '0' where id = '" + id + "'";
                end = db.cmd_Execute(str3);
            }

            return end;
        }
        #endregion
        #region 更改发药开启状态byid
        public int updateDrugSendDisplayState(string id)
        {


            string str = "select DrugSendDisplayState from hospital where id = '" + id + "'";
            int end = 0;
            SqlDataReader sr = db.get_Reader(str);
            string result = "";
            if (sr.Read())
            {
                result = sr["DrugSendDisplayState"].ToString();

            }

            if (result == "0")
            {
                string str2 = "update hospital set DrugSendDisplayState = '1' where id = '" + id + "'";
                end = db.cmd_Execute(str2);
            }
            else
            {
                string str3 = "update hospital set DrugSendDisplayState = '0' where id = '" + id + "'";
                end = db.cmd_Execute(str3);
            }

            return end;
        }
        #endregion
        public SqlDataReader findhospitalidbyhname(string hospitalname)
        {
            string str = "select id from hospital where hname='"+hospitalname+"'";
             SqlDataReader sr2 = db.get_Reader(str);
             return sr2;
        }
        #region 删除员工信息
        public bool deleteHospitalById(int nPId)
        {
            string strSql = "delete from hospital where id =" + nPId;
            int n = db.cmd_Execute(strSql);
            return true;
        }
        #endregion
        //查找pda图片开关
        public DataTable findPdaImgSwitchInfo()
        {

            string sql = "select * from  pdaImgSwitch ";
            return db.get_DataTable(sql);

        }
        //查找流程控制设置
        public DataTable find_control_set()
        {

            string sql = "select * from  tb_sys_add_setting ";
            return db.get_DataTable(sql);

        }
        //根据ID查找流程控制设置
        public DataTable find_control_set_by_id(int id)
        {

            string sql = "select * from  tb_sys_add_setting where sys_add_id="+"'"+id.ToString().Trim()+"'";
            return db.get_DataTable(sql);

        }

        
        //根据id查找pda图片开关
        public DataTable findPdaImgSwitchById(int id)
        {

            string sql = "select * from  pdaImgSwitch where id="+id;
            return db.get_DataTable(sql);

        }
        //修改pda图片开关
        public int editPdaImgSwitch(string id ,string tiaoji,string fuhe,string paoyao,string jianyao,string baozhuang,string fahuo)
        {

            string sql = "update pdaImgSwitch set tiaoji="+tiaoji+",fuhe="+fuhe+",paoyao="+paoyao+",jianyao="+jianyao+",baozhuang="+baozhuang+",fahuo="+fahuo+" where id=" + id;


            return db.cmd_Execute(sql);

        }
        //修改流程控制设置
        public int edit_add_setting_update(string sys_add_id, string global_medi_is_flag, string decocting_machine_mode_select,
        string decocting_medi_pda_is_code, string decocting_medi_pda_is_hand, string decocting_time_set, string package_once_time,
        string package_machine_nums)
        {

            string sql = "update tb_sys_add_setting set "+
                "global_medi_is_flag=" + "'"+global_medi_is_flag.Trim()+"'" + ",decocting_machine_mode_select=" +"'"+ decocting_machine_mode_select.Trim() +"'"+
                ",decocting_medi_pda_is_code=" + "'"+decocting_medi_pda_is_code.Trim()+"'" + ",decocting_medi_pda_is_hand=" + "'"+decocting_medi_pda_is_hand.Trim()+"'" +
                ",decocting_time_set=" +"'"+ decocting_time_set.Trim()+"'" + ",package_once_time=" + "'"+package_once_time.Trim()+"'" + 
                ",package_machine_nums=" +"'"+ package_machine_nums.Trim()+"'" +
                " where sys_add_id=" +"'"+ sys_add_id.Trim()+"'";


            return db.cmd_Execute(sql);

        }


     
    }
}