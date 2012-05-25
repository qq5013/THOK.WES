using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

namespace THOK.RfidConnectControl
{
    public class ConfigurationRead
    {
        // Fields
        public static string m_Path;

        // Methods
        public static string GetPara(string keyLevelOne, string keyLevelTwo)
        {
            string str;
            try
            {
                if (!File.Exists(m_Path))
                {
                    throw new Exception("配置文件不存在！");
                }
                XmlDocument document = new XmlDocument();
                document.Load(m_Path);
                for (XmlNode node = document.DocumentElement.FirstChild; node != null; node = node.NextSibling)
                {
                    if (node.Name.Trim().ToLower().Equals(keyLevelOne.ToLower().Trim()))
                    {
                        for (node = node.FirstChild; node != null; node = node.NextSibling)
                        {
                            if (node.Name.Trim().ToLower().Equals(keyLevelTwo.ToLower().Trim()))
                            {
                                return node.InnerText;
                            }
                        }
                        throw new Exception("二级节点不存在！节点名：" + keyLevelTwo + "\n请检查配置文件。");
                    }
                }
                throw new Exception("一级节点不存在！节点名：" + keyLevelOne + "\n请检查配置文件。");
            }
            catch (Exception exception)
            {
                throw new Exception("读取配置文件错误！错误信息：" + exception.Message);
            }
            return str;
        }
    }
}
