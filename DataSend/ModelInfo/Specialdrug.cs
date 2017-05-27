using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDAL;
using System.Collections;
using System.Data;


namespace ModelInfo
{
    class Specialdrug
    {

        DataBaseLayer db = new DataBaseLayer();
        public DataTable findspecialdrug()
        {
           
            string sql = "select * from specialdrugname";

            DataTable dt = db.get_DataTable(sql);
            return dt;

        }

    }
}
