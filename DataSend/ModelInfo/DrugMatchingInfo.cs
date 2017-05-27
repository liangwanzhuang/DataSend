using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelInfo
{
   public class DrugMatchingInfo
    {
       public int id;
       public int hospitalId;//医院id
       public string hospitalName;//医院名称
       public string hdrugNum;//医院药品编号
       public string ypcdrugNum;//饮片厂药品编号
       public string hdrugName;//医院药品名称
       public string ypcdrugName;//饮片厂药品名称
       public string hdrugOriginAddress;//医院药品产地
       public string ypcdrugOriginAddress;//饮片厂药品产地
       public string hdrugSpecs;//医院药品规格
       public string ypcdrugSpecs;//饮片药品厂规格
       public string hdrugTotal;//医院药品数量
       public string ypcdrugTotal;//饮片厂药品数量
       public string status;//状态
       public string pspNum;//医院处方号
       public string ypcdrugPositionNum;//饮片厂药品货位号
       public string pspId;//处方id
       public string drugId;//药品id
    }
}
