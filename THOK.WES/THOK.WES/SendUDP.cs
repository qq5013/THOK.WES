using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace THOK.WES
{
    /// <summary>
    /// 发送UDP信号
    /// </summary>
    public class SendUDP
    {

        private string ip = string.Empty;
        private int port = 0;

        public SendUDP()
        {
            this.ReadConfig();
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void ReadConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AFConfig.xml");          
            XmlNodeList nodeList = doc.GetElementsByTagName("UDP");
            foreach (XmlNode node in nodeList[0].ChildNodes)
            {                
                if (node.Attributes["name"].Value.ToString() == "IP")
                {
                    this.ip = node.Attributes["value"].Value.ToString();
                }
                if (node.Attributes["name"].Value.ToString() == "PORT")
                {
                    this.port = Convert.ToInt32(node.Attributes["value"].Value);
                }
            }
        }

        /// <summary>
        /// 发送UDP
        /// </summary>
        public void Send()
        {
            THOK.UDP.Client export = new THOK.UDP.Client(this.ip, this.port);
            THOK.UDP.Util.MessageGenerator generator = new THOK.UDP.Util.MessageGenerator("UpdateLable", "AF");
            generator.AddParameter("UpdateLable", "UpdateLable");
            export.Send(generator.GetMessage());
        }
    }
}
