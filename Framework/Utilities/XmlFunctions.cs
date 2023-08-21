using Framework.Reporting;
using Framework.UI;
using System;
using System.IO;
using System.Xml;

namespace Framework.Utilities
{
    public class XmlFunctions
     {
        private string filename;
        private string filepath;
        private XmlDocument xmlDoc;
        private Reporter Reporter;

        public XmlFunctions()
        {
            this.Reporter = BaseClass.Reporter;
        }

        public void SetXMLData(string node, string value)
        {
            filename = Constant.CurrentTestName + ".xml";
            filepath = Constant.Projectbasepath + @"\Xmls\" + filename;
            XmlNode root ;
            xmlDoc = new XmlDocument();
            if (!File.Exists(filepath))
            {
                root = xmlDoc.CreateElement("Root");
                xmlDoc.AppendChild(root);
                XmlNode childnode = xmlDoc.CreateElement(node);
                childnode.InnerText = value;
                root.AppendChild(childnode);
            }
            else
            {
                xmlDoc.Load(filepath);
                root = xmlDoc.DocumentElement;
                if (xmlDoc.SelectNodes("/Root/" + node).Count == 0)
                {
                    XmlNode childnode = xmlDoc.CreateNode(XmlNodeType.Element, node, null);
                    childnode.InnerText = value;
                    root.AppendChild(childnode);
                }
                else
                {
                    xmlDoc.SelectSingleNode("/Root/" + node).InnerText = value;
                }
            }            
            xmlDoc.Save(filepath);
        }

        public string GetXMLData(string node)
        {
            filename = Constant.CurrentTestName + ".xml";
            filepath = Constant.Projectbasepath + @"\Xmls\" + filename;

            string xpath = @"//" + node;
            xmlDoc = new XmlDocument();            
            try
            {
                xmlDoc.Load(filepath);
                return xmlDoc.SelectSingleNode(xpath).InnerText;
            }
            catch(Exception e)
            {
                return "";
            }
        }

        public static string GetData(string tagname)
        {
            tagname = string.Format("{0}", tagname);
            return Constant.xmlDocument.GetElementsByTagName(tagname)[Constant.CurrentIteration].InnerText;
        }

        public static int GetIterations()
        {
            int iterationCount = (Constant.xmlDocument.GetElementsByTagName("iteration")).Count;
            return iterationCount;
        }

        public static void LoadXml(string path)
        {
           
            Constant.xmlDocument.Load(path);
            Constant.Iterations = XmlFunctions.GetIterations();
            Constant.CurrentIteration = 0;
        }



    }
}

