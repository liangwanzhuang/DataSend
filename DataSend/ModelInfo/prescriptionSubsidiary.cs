using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;
using System.Data;

namespace ModelInfo
{
   public  class prescriptionSubsidiary
    {
       public DataBaseLayer db = new DataBaseLayer();

       public int AddprescriptionSubsidiary(prescriptionSubsidiaryModel model)
       {
          string strSql = @" INSERT INTO prescriptionSubsidiary
           (pid
           ,[unitPrice]
           ,[totalPrice]
           ,[insuranceNumber]
           ,[outpatientNumber]
           ,[outpatientIndex]
           ,[patientIndex]
           ,ptype)
     VALUES
           ('" + model.pid + "','" + model.unitPrice + "','" + model.totalPrice  + "','" + model.insuranceNumber + "','" + model.outpatientNumber + "','" + model.outpatientIndex + "','" + model.patientIndex + "'," + model.ptype + ")";
          int i = db.cmd_Execute(strSql);
          return i;
       }

       public int  UpdateprescriptionSubsidiary(prescriptionSubsidiaryModel model)
       {
           string strSql = @"UPDATE [prescriptionSubsidiary]
   SET [unitPrice]='" + model.unitPrice + "',[totalPrice]='" + model.totalPrice + "',[insuranceNumber] = '" + model.insuranceNumber + "',[outpatientNumber] ='" + model.outpatientNumber + "',[outpatientIndex] = '" + model.outpatientIndex + "',[patientIndex] = '" + model.patientIndex + "',ptype=" + model.ptype + " where pid=" + model.pid;
           int i = db.cmd_Execute(strSql);
           return i;
       }

       public DataTable GetPsTableById(int pid)
       {
           string strSql = @"select * from  prescriptionSubsidiary where pid=" + pid;
           DataTable dt = db.get_DataTable(strSql);
           return dt;
       }

    }
}
