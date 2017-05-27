
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DataSend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string strOldDataName = "";
        string strNewDataName = "";
        private readonly DispatcherTimer oxygenTimer = new DispatcherTimer();
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlHelper.conn = ConfigurationManager.ConnectionStrings["consql"].ToString();
            strOldDataName = ConfigurationManager.AppSettings["oldDataName"].ToString();
            strNewDataName = ConfigurationManager.AppSettings["newDataName"].ToString(); 
            this.oxygenTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
            this.oxygenTimer.Tick += new EventHandler(this.oxygenTimer_Tick);
            oxygenTimer.Start();
        }

        private void oxygenTimer_Tick(object sender, EventArgs e)
        {
            string str = @"use " + strNewDataName + @"  select * from  " + strOldDataName + ".dbo.UserInfo  where   isupload=0 ";
            DataTable dt  = SqlHelper.ExecuteDataset(SqlHelper.conn, CommandType.Text, str).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string strId = dr["id"].ToString();
                string strEpno = dr["EP_NO"].ToString();
                string hid = dr["hospitalID"].ToString();
                string strsql = @"use " + strNewDataName + @" 
 insert into prescription  (
 [customid] ,[delnum] ,[barcodescan],[Hospitalid]
           ,[Pspnum]
           ,[decmothed]
           ,[name]
           ,[sex]
           ,[age]
           ,[phone]
           ,[address]
           ,[department]
           ,[inpatientarea]
           ,[ward]
           ,[sickbed]
           ,[diagresult]
           ,[dose]
           ,[takemethod]
           ,[takenum]
           ,[packagenum]
           ,[decscheme]
           ,[oncetime]
           ,[twicetime]
           ,[soakwater]
           ,[soaktime]
           ,[labelnum]
           ,[remark]
           ,[doctor]
           ,[footnote]
           ,[getdrugtime]
           ,[getdrugnum]
           ,[ordertime]
           ,[curstate]
           ,[dotime]
           ,[doperson]
           ,[dtbcompany]
           ,[dtbaddress]
           ,[dtbphone]
           ,[dtbtype]
           ,[takeway]
           ,[RemarksA]
           ,[RemarksB]
           ,[confirmDrug]
           ,[logisticsstate]
           ,[qtime]
           ,[qname]
           ,[drug_count])
 select top 1  '','','',hospitalID, Ep_no,1, name,case when  sex='男' then 1 else 0 end,age,phone, Adds,
 '','','','','',fushu,'',tieshu,200, bm_method,'','','','',tieshu*fushu,'','','',GETDATE(),'', GETDATE(),'已审核',
 GETDATE(),	'管理员',	0,'',	'',	'',	'',	'',	'',	'',	'',	 GETDATE(),	'',	''
  from  " + strOldDataName + ".dbo.UserInfo  where id=" + strId + " SELECT @@IDENTITY AS Id  ";
                string pid = SqlHelper.ExecuteDataset(SqlHelper.conn, CommandType.Text, strsql).Tables[0].Rows[0][0].ToString();
                string strdrug = @"use " + strNewDataName + @" insert into drug(customid, delnum,Hospitalid,Pspnum,drugnum,drugname,drugdescription,
                drugposition,drugallnum,drugweight,tienum,description, wholesaleprice,retailprice, wholesalecost, retailpricecost, money,fee, pid ) select   0, null, hospitalID,EP_NO,DrugID, DrugName,'', DrugSpec, case when  wholesum='·' then '' else wholesum end ,  DrugSum,0,'',0,0,0,0,0,null," + pid + "  from " + strOldDataName + ".dbo.DrugInfo where  isupload=0  and EP_NO='" + strEpno + "'";
                int i =SqlHelper.ExecuteNonQuery(SqlHelper.conn, CommandType.Text, strdrug);
                if (i > 0)
                {
                    Update(1, strId, strEpno);
                     string strstate = @"use " + strNewDataName + @" INSERT INTO [PrescriptionCheckState]
           ([prescriptionId]
           ,[PartyPer]
           ,[PartyTime]
           ,[checkStatus]
           ,[refusalreason]
           ,[warningstatus]
           ,[tisaneNumber]
           ,[printstatus]
           ,[warningtime]
           ,[warningtype]
           ,[employeeid])
     VALUES
           ('" + pid + "',\'admin\','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1,'',0,'" + pid + "',0,null,null,101)";
                    int x = SqlHelper.ExecuteNonQuery(SqlHelper.conn, CommandType.Text, strstate);
                }
                else
                {
                    Update(-1, strId, strEpno);
                }
            }

        }

        public int Update(int i,string id, string epno)
        {
            string strUpdateSql = @"use " + strNewDataName + @" update " + strOldDataName + ".dbo.UserInfo set isupload=" + i + " where id =" + id;
            string strDrugUpdatesql = @"use " + strNewDataName + @" update " + strOldDataName + ".dbo.DrugInfo set isupload=" + i + " where EP_NO='" + epno + "'";
            int x = SqlHelper.ExecuteNonQuery(SqlHelper.conn, CommandType.Text, strUpdateSql);
            int z = SqlHelper.ExecuteNonQuery(SqlHelper.conn, CommandType.Text, strDrugUpdatesql);
            return x+z;
        }

      
    }
}
