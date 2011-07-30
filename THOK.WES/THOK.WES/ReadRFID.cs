using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO.Ports;

namespace THOK.WES
{
    /// <summary>
    /// ��ȡRFID
    /// </summary>
    public class ReadRFID
    {
        private string port = string.Empty;
        private SerialPort serialPort = null;
        public ReadRFID()
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

            XmlNodeList nodeList = doc.GetElementsByTagName("RFID");
            foreach (XmlNode node in nodeList[0].ChildNodes)
            {              
                if (node.Attributes["name"].Value.ToString() == "PORT")
                {
                    this.port = node.Attributes["value"].Value;
                }
            }
        }


        /// <summary>
        /// ��ȡRFID
        /// </summary>
        /// <returns>RFID</returns>
        public string GetRFID()
        {
            String strRecv = string.Empty;

            //�򿪴���
            serialPort = new SerialPort();
            serialPort.ReadTimeout = 1000;
            serialPort.PortName = this.port;
            serialPort.BaudRate = 115200;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.Open();
           
            //��ʼ��
            serialPort.Write("ATTRIB NOTAGRPT=ON\n\t");
            serialPort.Write("ATTRIB FIELDSTRENGTH=100,100,100,10\n\t");
            serialPort.Write("ATTRIB IDTRIES=3\n\t");
            serialPort.Write("ATTRIB ANTTRIES=3\n\t");
            serialPort.Write("ATTRIB ANTS=1\n\t");
            serialPort.Write("ATTRIB TAGTYPE=EPCC1G2\n\t");
            serialPort.Write("ATTRIB SESSION=2\n\t");
            serialPort.Write("ATTRIB INITIALQ=0\n\t");
            serialPort.Write("ATTRIB IDREPORT=ON\n\t");
            serialPort.Write("VER\n\t");

            //��ȡRFID
            strRecv = this.RepeatRFID();
            //�ж��Ƿ��ȡ��(���һ�γ�ʼ��������Ϣ����,��ʱ����ȡ����)
            if (strRecv.Length != 25 || strRecv.IndexOf("H") != 0)
            {
               strRecv = this.RepeatRFID();
            }

            serialPort.Close();
            //�����ȡ������Ϣ�����ض��ĸ�ʽ,���ؿ��ַ���
            if (strRecv.Length != 25 || strRecv.IndexOf("H") != 0)
            {
                return "";
            }
            else
            {
                return strRecv;
            }
            
        }

        /// <summary>
        /// ��ȡRFID
        /// </summary>
        /// <returns>RFID</returns>
        private string RepeatRFID()
        {
            String strRecv = string.Empty;
            //���Ͷ�ȡ�ź�
            serialPort.Write("R\n\t");
            DateTime now = DateTime.Now;
            //��ȡһ��ķ�����Ϣ
            do
            {
                char[] btRecv = new char[serialPort.ReadBufferSize];
                int nRead = serialPort.Read(btRecv, 0, serialPort.BytesToRead);
                for (int i = 0; i < nRead; i++)
                {
                    strRecv = strRecv + btRecv[i].ToString();
                }

            } while (((TimeSpan)(DateTime.Now - now)).TotalSeconds < 1);
            strRecv = strRecv.Trim();
            strRecv = strRecv.Substring(strRecv.Length - 32, 25);
            return strRecv;
        }
    }
}
