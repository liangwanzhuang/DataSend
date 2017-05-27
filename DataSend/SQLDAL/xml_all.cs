using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDAL
{
  public   class xml_all
    {
        public string[] getXml_node(string node_no_id, string node_name, string xml_str)
        {
            //string d_path = Directory.GetCurrentDirectory();
            //string all_path = Application.StartupPath;//获取XML文件的当前路径，注意要把XML文件拷贝到DEBUG路径下面.
            string all_path = System.AppDomain.CurrentDomain.BaseDirectory;
            all_path += "\\xml";
            all_path += @"\" + xml_str.Trim();
            // string all_path=d_path+"\\xml\\"+ @"XML_db.xml";
            ReadXml readxml = new ReadXml();
            //根据XML文件中每个节点名称获取每个节点的内容,"1"代表节点ID,考虑到以后XML文件的扩展,所以使用STRING数组,具体获取方法见ReadXml.cs.
            string[] return_node = new string[20];
            return_node = readxml.getReadXml(all_path, node_no_id, node_name);
            return return_node;

        }
        //针对XML_SQL
        public string[] getXml_node_url(string node_no_id, string node_name, string xml_str)
        {
            //string d_path = Directory.GetCurrentDirectory();
            //string all_path = Application.StartupPath;//获取XML文件的当前路径，注意要把XML文件拷贝到DEBUG路径下面.
            string all_path = System.AppDomain.CurrentDomain.BaseDirectory;
            all_path += "\\xml";
            all_path += @"\" + xml_str.Trim();
            // string all_path=d_path+"\\xml\\"+ @"XML_db.xml";
            ReadXml readxml = new ReadXml();
            //根据XML文件中每个节点名称获取每个节点的内容,"1"代表节点ID,考虑到以后XML文件的扩展,所以使用STRING数组,具体获取方法见ReadXml.cs.
            string[] return_node = new string[20];
            return_node = readxml.getReadXml_url(all_path, node_no_id, node_name);
            return return_node;

        }
        public void updateXml_node(string node_no_id, string node_name, string node_value)
        {
            // string all_path = Application.StartupPath;//获取XML文件的当前路径，注意要把XML文件拷贝到DEBUG路径下面.
            string all_path = System.AppDomain.CurrentDomain.BaseDirectory;
            all_path += "\\xml";
            all_path += @"\" + "XML_Server.xml";
            // string all_path=d_path+"\\xml\\"+ @"XML_db.xml";
            ReadXml updatexml = new ReadXml();
            updatexml.updateXml(all_path, node_no_id, node_name, node_value);
        }
        public void updateXml_node_tjt(string node_no_id,string node_root, string node_name, string node_value)
        {
            // string all_path = Application.StartupPath;//获取XML文件的当前路径，注意要把XML文件拷贝到DEBUG路径下面.
            string all_path = System.AppDomain.CurrentDomain.BaseDirectory;
            all_path += "\\xml";
            all_path += @"\" + "XML_Server.xml";
            // string all_path=d_path+"\\xml\\"+ @"XML_db.xml";
            ReadXml updatexml = new ReadXml();
            updatexml.updateXml_tjt(all_path,node_root, node_no_id, node_name, node_value);
        }
       
       
    
    }
}
