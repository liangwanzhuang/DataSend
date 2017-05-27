using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace ModelInfo
{
   public class DataTableToJson
    {

       public static string ToJson(DataTable dt)
       {
           StringBuilder jsonBuilder = new StringBuilder();
           //  jsonBuilder.Append("{\"data");
           jsonBuilder.Append(dt.TableName.ToString());
           // jsonBuilder.Append("\":[");
           jsonBuilder.Append("[");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               jsonBuilder.Append("{");
               for (int j = 0; j < dt.Columns.Count; j++)
               {
                   jsonBuilder.Append("\"");
                   jsonBuilder.Append(dt.Columns[j].ColumnName);
                   jsonBuilder.Append("\":\"");
                   jsonBuilder.Append(dt.Rows[i][j].ToString());
                   jsonBuilder.Append("\",");
               }
               jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
               jsonBuilder.Append("},");
           }
           if (dt.Rows.Count > 0) {
               jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
           }
           jsonBuilder.Append("]");
           //  jsonBuilder.Append("}");
           return jsonBuilder.ToString();
       }
       public static string ToJson_by_str(string str)
       {
           StringBuilder jsonBuilder = new StringBuilder();
           //  jsonBuilder.Append("{\"data");
           jsonBuilder.Append("get");
           // jsonBuilder.Append("\":[");
           jsonBuilder.Append("[");
         
               jsonBuilder.Append("{");
              
                   jsonBuilder.Append("\"");
                   jsonBuilder.Append("value");
                   jsonBuilder.Append("\":\"");
                   jsonBuilder.Append(str.Trim());
                   jsonBuilder.Append("\"");
           
            //   jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
               jsonBuilder.Append("}");
          
        
               //jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
         
           jsonBuilder.Append("]");
           //  jsonBuilder.Append("}");
           return jsonBuilder.ToString();
       } 
    }
}
