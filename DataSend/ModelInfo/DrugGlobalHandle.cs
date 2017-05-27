using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLDAL;

namespace ModelInfo
{
    public class DrugGlobalHandle
    {
        public bool DrugGlobalInfo(DrugGlobalInfoGet drug)
        {

            /// <summary>
            /// 添加泡药信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>
            string strSql = "insert into drugGlobalInfo(num,delNum,hospitalNum,hospitalName,pspNum,drugTakeWay,"+
            "patientName,sex,age,personPhone,address,department,inpatientAreaNum,inpatientRoomNum,inpatientBedNum," +
            "diagnosisResult,drugNum,drinkWay,takeNum,packageNum,drinkMethod,takeMethod,firstTakeTime,"+
            "secondTakeTime,soakPlusWater,soakTime,labelNum,remarkInfo,doctor,doctorFootNote,getDrugTime,"+
            "getDrugNum,orderTime,stateNow,operationTime,operationPerson,distributionCompany,distributionAddress,"+
            "companyPhone,expressType) ";
            strSql += "values ('" + drug.strNum + "','" + drug.strDelNum + "','" + drug.strHospitalNum + "',";
            strSql += "'" + drug.strHospitalName + "','" + drug.strPspNum + "','" + drug.strDrugTakeWay + "',";
            strSql += "'" + drug.strName + "','" + drug.strSex + "','" + drug.strAge + "',";
            strSql += "'" + drug.strPersonPhone + "','" + drug.strAddress + "','" + drug.strDepartment + "',";
            strSql += "'" + drug.strInpatientAreaNum + "','" + drug.strInpatientRoomNum + "','" + drug.strInpatientBedNum + "',";
            strSql += "'" + drug.strDiagnosisResult + "','" + drug.strDrugNum + "','" + drug.strDrinkWay + "',";
            strSql += "'" + drug.strTakeNum + "','" + drug.strPackageNum + "','" + drug.strDrinkMethod + "',";
            strSql += "'" + drug.strTakeMethod + "','" + drug.strFirstTakeTime + "','" + drug.strSecondTakeTime + "',";
            strSql += "'" + drug.strSoakPlusWater + "','" + drug.strSoakTime + "','" + drug.strLabelNum + "',";
            strSql += "'" + drug.strRemarkInfo + "','" + drug.strDoctor + "','" + drug.strDoctorFootNote + "',";
            strSql += "'" + drug.strGetDrugTime + "','" + drug.strGetDrugNum + "','" + drug.strOrderTime + "',";
            strSql += "'" + drug.strStateNow + "','" + drug.strOperationTime + "','" + drug.strOperationPerson + "',";
            strSql += "'" + drug.strDistributionCompany + "','" + drug.strDistributionAddress + "','" + drug.strCompanyPhone + "',";
            strSql += "'" + drug.strExpressType +  "')";

            DataBaseLayer db = new DataBaseLayer();

            int n = db.cmd_Execute(strSql);
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DrugDelete()
        {
            /// <summary>
            /// 删除泡药信息
            /// </summary>
            /// <param name="einfo"></param>
            /// <returns></returns>

            string strSql = "delete from drugGlobalInfo where num = @num";
            DataBaseLayer db = new DataBaseLayer();
            
            int n = db.cmd_Execute(strSql);
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
