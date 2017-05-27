using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelInfo
{
  public  class C_pda_image
    {
      public C_pda_image()
            {
                //
                //TODO: 在此处添加构造函数逻辑
                //
            }
            string nid;
            string image_str;
            string syscreated;
            string p_str;
            string p_status;

         
            public string Nid
            {
                get { return nid; }
                set { nid = value; }
            }

           
            public string Image_str
            {
                get { return image_str; }
                set { image_str = value; }
            }
            public string Syscreated
            {
                get { return syscreated ; }
                set { syscreated =value; }
            }
            public string P_str
            {
                get { return p_str ; }
                set { p_str = value; }
            }
            public string P_status
            {
                get { return p_status; }
                set { p_status = value; }
            }

    }
}
