using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelInfo
{
    public class DrugInfo
    {
        public int nCustomId = 0;//客户
        public string strDelNum = "";//委托单号
        public int nHospitalID = 0; //医院ID
        public int nHospitalNum = 0;// 医院编号
        public string strHospitalName = "";// 医院名称
        public string strPspnum = "";  //电子处方号
        public string strDrugNum = "";//药品编号
        public string strDrugName = "";//药品名称
        public string strDrugDsp = "";// 药品描述
        public string strDrugPosition = "";//药品位置
        public int nAllNum = 0;//药品总数量
        public double dWeight = 0.0;//重量
        public int nTieNum = 0;
        public string strDsp = "";//说明
        public double dWholeSalePrice = 0.0; //批发价格
        public double dRetailPrice = 0.0;// 零售价格
        public double dWholeSaleCost = 0.0; //批发费用
        public double dRetailCost = 0.0;
        public double dMoneyWithTax = 0.0;
        public double dFee = 0.0;
        public string pid = "";
    }
}
