using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SQLDAL
{
    class ReadXml
    {
        public ReadXml()
        {
            //   string strp = get;
            // strPath="..\xml\XML_db.xml";
        }
        public string[] getReadXml(string xmlfile, string id, string nodename)
        {
            string[] returnvalue = new string[20];
            int i = 0;
            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements("db") where noderecords.Attribute("id").Value == id select noderecords;
            foreach (XElement element in elements)
            {
                returnvalue[i] = element.Element(nodename).Value;
                i++;
            }
            return returnvalue;
        }
        //XML_SQL
        public string[] getReadXml_url(string xmlfile, string id, string nodename)
        {
            string[] returnvalue = new string[20];
            int i = 0;
            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements("service_url") where noderecords.Attribute("id").Value == id select noderecords;
            foreach (XElement element in elements)
            {
                returnvalue[i] = element.Element(nodename).Value;
                i++;
            }
            return returnvalue;
        }

        public void updateXml(string xmlfile, string id, string nodename, string node_value)
        {
            int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);


            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements("db") where noderecords.Attribute("id").Value == id select noderecords;
            foreach (XElement element in elements)
            {
                // returnvalue[i] = element.Element(nodename).Value;
                //element.Attributes[nodename].Value = ip_data;
                //element.SetElementValue(nodename,node_value);
                //element.nodename.SetValue = node_value;
                //element[nodename].InnerText = node_value;
                element.Element(nodename).SetValue(node_value);
                //element.ReplaceNodes(new XElement(nodename, node_value));
                //myXml.Save(xmlfile);

                i++;
            }
            xe.Save(xmlfile);
        }
        public void updateXml_tjt(string xmlfile, string node_root, string id, string nodename, string node_value)
        {
            int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);


            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements(node_root) where noderecords.Attribute("id").Value == id select noderecords;
            foreach (XElement element in elements)
            {
                // returnvalue[i] = element.Element(nodename).Value;
                //element.Attributes[nodename].Value = ip_data;
                //element.SetElementValue(nodename,node_value);
                //element.nodename.SetValue = node_value;
                //element[nodename].InnerText = node_value;
                element.Element(nodename).SetValue(node_value);
                //element.ReplaceNodes(new XElement(nodename, node_value));
                //myXml.Save(xmlfile);

                i++;
            }
            xe.Save(xmlfile);
        }
        //判断XML_FIELD_SET.XML
        // 注意 参数user_id 为user_id和report_name 合并的字符串。
        public string getRead_field_Xml(string xmlfile, string user_id_report, string nodename)
        {
            //  string [] field_xml_str=new string[1000];
            string field_xml_str = "0";
            //  int i = 0;
            XElement xe = XElement.Load(xmlfile);

            //XElement element = from noderecords in xe.Elements("user_id") where noderecords.Attribute("user_id").Value == user_id select noderecords;
            IEnumerable<XElement> elements = from noderecords in xe.Elements("user_id") where noderecords.Attribute("id").Value == user_id_report select noderecords;
            foreach (XElement element in elements)
            {
                field_xml_str = element.Element(nodename).Value;
                // field_xml_str = element.Attributes["user_id"].Value;
                if (field_xml_str.Length == 0) //获取到的值为NULL时做判断。
                {
                    field_xml_str = "0";
                }

                // i++;
            }

            return field_xml_str;
        }


        //修改XML_field_set.xml
        public void update_field_xml(string xmlfile, string user_id, string nodename, string node_value)
        {
            // int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);


            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements("user_id") where noderecords.Attribute("id").Value == user_id select noderecords;
            foreach (XElement element in elements)
            {
                // returnvalue[i] = element.Element(nodename).Value;
                //element.Attributes[nodename].Value = ip_data;
                //element.SetElementValue(nodename,node_value);
                //element.nodename.SetValue = node_value;
                //element[nodename].InnerText = node_value;
                element.Element(nodename).SetValue(node_value);
                //element.ReplaceNodes(new XElement(nodename, node_value));
                //myXml.Save(xmlfile);

                //  i++;
            }
            xe.Save(xmlfile);
        }
        public void insert_field_xml(string xmlfield_path, string user_id_report, string fields_name)
        {
            // int flag_user_is=0;//判断是否有USER_ID值的节点,0表示没有1表示有
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlfield_path);
            XmlNode root = xmldoc.SelectSingleNode("Field_Config");

            XElement xe = XElement.Load(xmlfield_path);

            //   string user_report_name = getRead_field_Xml(xmlfield_path,user_id, "report_name");
            string user_fields_name = getRead_field_Xml(xmlfield_path, user_id_report, "fields_name");
            if (user_fields_name == "0")
            {
                XmlElement user_id_xml = xmldoc.CreateElement("user_id");
                user_id_xml.SetAttribute("id", user_id_report);

                XmlElement fields_name_xml = xmldoc.CreateElement("fields_name");
                //MessageBox.Show(fields_name);
                fields_name_xml.InnerText = fields_name;
                // user_id_xml.AppendChild(report_name_xml);

                root.AppendChild(user_id_xml);
                user_id_xml.AppendChild(fields_name_xml);
                xmldoc.Save(xmlfield_path);
            }

            if (user_fields_name.Length > 0)
            {
                update_field_xml(xmlfield_path, user_id_report, "fields_name", fields_name);
            }

        }

        //删除XML_field_set.xml NODE节点。
        public void delete_field_xml(string xmlfile, string user_name_cn)
        {
            // string node_str;
            // int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlfile);
            XmlNode root = xmldoc.SelectSingleNode("User_Config");
            //  XmlNode node = root.SelectSingleNode("node");

            foreach (XmlNode n in root.ChildNodes)
            {
                //MessageBox.Show(n.Attributes["id"].Value);
                if (n.Attributes["id"].Value == user_name_cn)
                {
                    root.RemoveChild(n);
                }

                /*  node_str="//Limit_Config/node[@id="+user_report_field.Trim()+"]".Trim();
                    MessageBox.Show(node_str);
                   XmlNodeList nl=  xmldoc.SelectNodes(node_str);
                   foreach(XmlNode xn in nl)
                   {    
                       //xmldoc.documentElement.removeChild(xn);
                   
                    }
                  */
            }

            xmldoc.Save(xmlfile);
        }


        public string getRead_user_Xml(string xmlfile, string node_name, string user_report_field, string nodename)
        {
            //  string [] field_xml_str=new string[1000];
            string field_xml_str = "0";
            //  int i = 0;
            XElement xe = XElement.Load(xmlfile);

            //XElement element = from noderecords in xe.Elements("user_id") where noderecords.Attribute("user_id").Value == user_id select noderecords;
            IEnumerable<XElement> elements = from noderecords in xe.Elements(node_name) where noderecords.Attribute("id").Value == user_report_field select noderecords;
            foreach (XElement element in elements)
            {
                field_xml_str = element.Element(nodename).Value;
                // field_xml_str = element.Attributes["user_id"].Value;
                if (field_xml_str.Length == 0) //获取到的值为NULL时做判断。
                {
                    field_xml_str = "0";
                }
                //if (field_xml_str.Length> 0) //获取到的值为NULL时做判断。
                // {
                //     field_xml_str = "1";
                //  }
                // i++;
            }

            return field_xml_str;
        }

        //修改XML_user.xml
        public void update_user_xml(string xmlfile, string node_name, string user_report_field, string nodename, string node_value)
        {
            // int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);


            XElement xe = XElement.Load(xmlfile);
            IEnumerable<XElement> elements = from noderecords in xe.Elements(node_name) where noderecords.Attribute("id").Value == user_report_field select noderecords;
            foreach (XElement element in elements)
            {
                // returnvalue[i] = element.Element(nodename).Value;
                //element.Attributes[nodename].Value = ip_data;
                //element.SetElementValue(nodename,node_value);
                //element.nodename.SetValue = node_value;
                //element[nodename].InnerText = node_value;
                element.Element(nodename).SetValue(node_value);
                //element.ReplaceNodes(new XElement(nodename, node_value));
                //myXml.Save(xmlfile);

                //  i++;
            }
            xe.Save(xmlfile);
        }


        //删除XML_user.xml NODE节点。
        public void delete_user_xml(string xmlfile, string user_name_cn)
        {
            // string node_str;
            // int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlfile);
            XmlNode root = xmldoc.SelectSingleNode("User_Config");
            //  XmlNode node = root.SelectSingleNode("node");

            foreach (XmlNode n in root.ChildNodes)
            {
                //MessageBox.Show(n.Attributes["id"].Value);
                if (n.Attributes["id"].Value == user_name_cn)
                {
                    root.RemoveChild(n);
                }

                /*  node_str="//Limit_Config/node[@id="+user_report_field.Trim()+"]".Trim();
                    MessageBox.Show(node_str);
                   XmlNodeList nl=  xmldoc.SelectNodes(node_str);
                   foreach(XmlNode xn in nl)
                   {    
                       //xmldoc.documentElement.removeChild(xn);
                   
                    }
                  */
            }

            xmldoc.Save(xmlfile);
        }





        //string fields_name 表示产品线等节点名字：如product_line;
        public void insert_user_xml(string xmlfield_path, string node_name, string user_name_cn, string user_name_cn_id_str)
        {
            // int flag_user_is=0;//判断是否有USER_ID值的节点,0表示没有1表示有
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlfield_path);
            XmlNode root = xmldoc.SelectSingleNode("User_Config");

            XElement xe = XElement.Load(xmlfield_path);

            //   string user_report_name = getRead_field_Xml(xmlfield_path,user_id, "report_name");

            string user_fields_name = getRead_user_Xml(xmlfield_path, node_name, user_name_cn, "user_id");
            // MessageBox.Show(user_fields_name);
            if (user_fields_name != "0")
            {

                //   MessageBox.Show("已存在该权限名称，如需修改，请点击权限字段内容修改选项，进行修改。", "提示");
            }
            if (user_fields_name == "0")
            {
                XmlElement all_xml = xmldoc.CreateElement("node");
                all_xml.SetAttribute("id", user_name_cn);

                XmlElement user_xml = xmldoc.CreateElement("user_id");
                //MessageBox.Show(fields_name);
                user_xml.InnerText = user_name_cn_id_str;
                //  XmlElement user_xml = xmldoc.CreateElement("user_id");
                //MessageBox.Show(fields_name);
                // user_xml.InnerText = user_name_cn;
                // user_id_xml.AppendChild(report_name_xml);
                // XmlElement report_xml = xmldoc.CreateElement("report");
                //MessageBox.Show(fields_name);
                // report_xml.InnerText = report_node;
                // XmlElement field_xml = xmldoc.CreateElement("field");
                //MessageBox.Show(fields_name);
                //  field_xml.InnerText =field_node;

                //  XmlElement field_in_xml = xmldoc.CreateElement("field_in");
                //MessageBox.Show(fields_name);
                //  field_in_xml.InnerText =field_in_node;

                // user_id_xml.AppendChild(report_name_xml);
                root.AppendChild(all_xml);
                all_xml.AppendChild(user_xml);
                // all_xml.AppendChild(limit_xml);
                // all_xml.AppendChild(report_xml);
                //  all_xml.AppendChild(field_xml); 
                // all_xml.AppendChild(field_in_xml);
                xmldoc.Save(xmlfield_path);
            }




        }




        public void init_xml(string xmlfile, string root_str)
        {
            // string node_str;
            // int i = 0;
            //初始化XML文档操作类
            //  XmlDocument myXml = new XmlDocument();
            //加载指定的XML文件
            //  myXml.Load(xmlfile);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlfile);
            XmlNode root = xmldoc.SelectSingleNode("User_Config");
            /*     XmlNode node = root.SelectSingleNode("node");
               XmlNodeList list = xmldoc.SelectSingleNode(root_str).ChildNodes;
               foreach (XmlNode n in list)
               {
               
                       root.RemoveChild(n);
               

                   /*  node_str="//Limit_Config/node[@id="+user_report_field.Trim()+"]".Trim();
                       MessageBox.Show(node_str);
                      XmlNodeList nl=  xmldoc.SelectNodes(node_str);
                      foreach(XmlNode xn in nl)
                      {    
                          //xmldoc.documentElement.removeChild(xn);
                   
                       }
                     */

            //  }
            root.RemoveAll();
            xmldoc.Save(xmlfile);
        }

    }

}
