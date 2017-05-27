using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace ModelInfo
{
   public class RecipientModel
    {
       public DataBaseLayer db = new DataBaseLayer();
       public int AddRecipient(string ClearPName, string Telephone, string Address, string Remarks)
       {

           string strSql = "insert into Recipient(ClearPName,Telephone,Address,Remarks) ";
           strSql += "values ('" + ClearPName + "','" + Telephone + "','" + Address + "','" + Remarks + "')";
            

            
           return db.cmd_Execute(strSql);

       }
        #region 查询收件人信息
       public SqlDataReader finRecipienallinfo(){
           string sql = "select * from Recipient ";
           return db.get_Reader(sql);
       
       
       }
        #endregion
        #region 通过收件人查询收件人信息
       public DataTable finRecipientInfo(string ClearPName)
       { 
          string sql = "select * from Recipient where 1=1";
           if (ClearPName !="0" && ClearPName!=""){
           sql +="and ClearPName ='"+ClearPName+"'";
           
           }
           return db.get_DataTable(sql);
       
       }
        #endregion
        #region 修改处方信息

       public int  UpdateRecipientInfo(int id, string ClearPName, string Telephone, string Address, string Remarks)
       {
           int end=0;
           string sql = "Update  Recipient set ClearPName ='" + ClearPName + "',Telephone='" + Telephone + "',Address ='" + Address + "', Remarks='" + Remarks+"' where id = "+id+"";
           end = db.cmd_Execute(sql);

           return end;
       
       }
        #endregion
        #region 通过ID查询收件人信息
     public DataTable  finRecipienallinfoByid(int id){
         string sql = "select * from Recipient where id ="+id+"";
         return db.get_DataTable(sql);
     }
     #endregion
        #region 通过ID删除收件人信息
     public bool deleteRPInfoByid(int id) {
         
         string sql = "delete from Recipient where id ="+id+"";
         int end = db.cmd_Execute(sql);
         return true;
     }
        #endregion 
    }
}
