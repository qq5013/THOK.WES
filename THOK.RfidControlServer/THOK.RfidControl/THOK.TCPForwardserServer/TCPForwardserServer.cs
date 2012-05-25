using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace THOK.TCPForwardserServer
{
    public class TCPForwardserServer
    {
        private IDictionary<string, TCPForwardser> m_TCPForwardsers = new Dictionary<string, TCPForwardser>();

        public void Start()
        {
            try
            {
                if (m_TCPForwardsers.Count == 0)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(AppDomain.CurrentDomain.BaseDirectory + "TCPForwardserServer.xml");
                    XmlNodeList nodes = doc.GetElementsByTagName("TCPForwardserServer");

                    if (nodes.Count == 1)
                    {
                        XmlNodeList propertyNodes = nodes[0].SelectNodes("property");

                        foreach (XmlNode propertyNode in propertyNodes)
                        {
                            if (propertyNode.Attributes["name"].Value == "TCPForwardserServerDic")
                            {
                                XmlNodeList KeyNodes = propertyNode.SelectNodes("key");
                                foreach (XmlNode KeyNode in KeyNodes)
                                {
                                    string key = KeyNode.Attributes["name"].Value;
                                    string sourceTcpConnectionString = KeyNode.SelectNodes("SourceTcpConnectionString")[0].InnerText;
                                    string targetTcpConnectionString = KeyNode.SelectNodes("TargetTcpConnectionString")[0].InnerText;
                                    TCPForwardser tCPForwardser = new TCPForwardser(sourceTcpConnectionString, targetTcpConnectionString);
                                    tCPForwardser.Open();
                                    m_TCPForwardsers.Add(key, tCPForwardser);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Stop()
        {
            try
            {
                foreach (string key in m_TCPForwardsers.Keys)
                {
                    m_TCPForwardsers[key].Close();
                }                
            }
            catch (Exception ex)
            {

            }
        }
    }
}
