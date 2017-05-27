using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDAL;

namespace ModelInfo
{
    public class AuthorityHandler
    {
        //添加人员
        public bool AddPersonnelinfo(AuthorityInfo einfo)
        {
            string strSql = "create user Personnel  ";
            strSql += "values ('" + einfo.strPersonnel + "')";

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
        

    

