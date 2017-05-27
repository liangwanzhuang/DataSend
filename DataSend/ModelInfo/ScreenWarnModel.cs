using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelInfo
{
  public   class ScreenWarnModel
    {
          public int hospitalID=0;
          public string amDosageTime="";
          public int  amDosageNumber=0;
          public string pmDosageTime="";
          public int pmDosageNumber =0;
          public string amTisaneTime="";
          public int amTisaneNumber = 0;
          public string pmTisaneTime="";
          public int pmTisaneNumber = 0;
          public DateTime warnDate = DateTime.Now;
    }
}
