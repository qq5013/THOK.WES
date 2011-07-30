using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace THOK.WES
{
    /// <summary>
    /// ����UDP�ź�
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
        /// ��ȡ�����ļ�
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
        /// ����UDP
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
