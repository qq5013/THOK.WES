using System;
using System.Collections.Generic;
using System.IO.Ports;

using System.Text;
using System.Threading;
using Intermec.DataCollection.RFID;
using DBRabbit;
using DBRabbit.Relation;
using System.Globalization;

namespace THOK.RfidControlServer
{
    public class RfidControlAdapter
    {
        private Thread m_Thread = null;

        private BRIReader m_BRIReader = null;
        private string m_BRIConnectionString = "serial://com8";
        private RfidSoftAdapter m_RfidSoftAdapter = null;
        private string m_RfidSoftConnectionString = "serial://com2";

        private int m_EPCTagIDStartPos = 4;
        private int m_EPCTagDataLength = 500;
        private bool m_EPCTagAutoCreateGuid = false;
        private int m_EPCTagGuidSetsDaraStartPos = 20;
        private int m_EPCTagGuidSetsDataLength = 2;
        private string m_EPCTagGuidSets = "HEEFF";        

        public void Start()
        {
            try
            {
                ConfigurationRead.m_Path = AppDomain.CurrentDomain.BaseDirectory + "RfidControlServerConfig.Xml";
                m_BRIConnectionString = ConfigurationRead.GetPara("RfidControlServerConfig", "BRIConnectionString");
                m_RfidSoftConnectionString = ConfigurationRead.GetPara("RfidControlServerConfig", "RfidSoftConnectionString");

                m_EPCTagIDStartPos = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagIDStartPos"));
                m_EPCTagDataLength = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagDataLength"));
                m_EPCTagAutoCreateGuid =Convert.ToBoolean(ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagAutoCreateGuid"));
                m_EPCTagGuidSetsDaraStartPos = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagGuidSetsDaraStartPos"));
                m_EPCTagGuidSetsDataLength = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagGuidSetsDataLength"));
                m_EPCTagGuidSets = ConfigurationRead.GetPara("RfidControlServerConfig", "EPCTagGuidSets");

                if (m_Thread == null)
                {
                    m_Thread = new Thread(new ThreadStart(this.Run));
                    m_Thread.Name = AppDomain.CurrentDomain.ToString();
                    m_Thread.IsBackground = true;
                    m_Thread.SetApartmentState(ApartmentState.STA);
                }
                m_Thread.Start();
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }

        public void Stop()
        {
            try
            {
                if (m_RfidSoftAdapter != null)
                {
                    m_RfidSoftAdapter.Close();
                    m_RfidSoftAdapter = null;
                }

                if (m_BRIReader != null)
                {
                    m_BRIReader.Dispose();
                    m_BRIReader = null;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Run()
        {
            while (m_BRIReader == null || m_RfidSoftAdapter == null)
            {
                Thread.Sleep(5000);
                try
                {
                    m_BRIReader = new BRIReader(null, m_BRIConnectionString);                    
                    m_BRIReader.Attributes.ReadTries = 3;

                    m_RfidSoftAdapter = new RfidSoftAdapter(m_RfidSoftConnectionString);
                    m_RfidSoftAdapter.DataReceived += new RfidSoftAdapter.DataReceivedEvent(m_RfidSoftAdapter_DataReceived);
                    m_RfidSoftAdapter.Open();

                    if (!m_BRIReader.IsConnected || !m_RfidSoftAdapter.m_IsOpen)
                    {
                        this.Stop();
                    }
                }
                catch (Exception ex)
                {
                    this.Stop();
                }
            }

            m_Thread = null;
        }

        private void m_RfidSoftAdapter_DataReceived(RfidSoftAdapter sender, RfidSoftAdapter.DataReceivedEventArgs e)
        {
            try
            {
                string[] strCmd;
                string strtmp = "";
                bool btResult = false;
                switch (e.m_CommandType)
                {
                    case RfidSoftAdapter.CommandType.ReadTagID:
                        if (m_BRIReader.Read(null, string.Format("HEX({0},16) HEX({1},{2})", m_EPCTagIDStartPos, m_EPCTagGuidSetsDaraStartPos, m_EPCTagGuidSetsDataLength), BRIReader.RFIDTagTypes.EPCC1G2) && m_BRIReader.TagCount > 0)
                        {
                            if (m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString.Length == 0x21)
                            {
                                btResult = true;
                                if (m_EPCTagAutoCreateGuid && m_BRIReader.Tags[0].TagFields.FieldArray[1].DataString != m_EPCTagGuidSets)
                                {
                                    btResult = false;
                                    string guid = "H";
                                    byte[] byteGuid = System.Guid.NewGuid().ToByteArray();
                                    foreach (byte byteItem in byteGuid)
                                    {
                                        guid = guid + byteItem.ToString("X2");
                                    }
                                    m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString = guid;
                                    m_BRIReader.Tags[0].TagFields.FieldArray[1].DataString = m_EPCTagGuidSets;
                                    m_BRIReader.Attributes.RFIDTagType = BRIReader.RFIDTagTypes.EPCC1G2;
                                    btResult = m_BRIReader.Update();
                                }
                                if (btResult)
                                {
                                    m_RfidSoftAdapter.Write("\r\n");
                                    m_RfidSoftAdapter.Write(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString + " " + m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString);
                                    m_RfidSoftAdapter.Write("\r\n");
                                    m_RfidSoftAdapter.Write("OK>");
                                    m_RfidSoftAdapter.Write("\r\n");
                                }
                            }
                        }
                        else if (m_BRIReader.Read(null, null, BRIReader.RFIDTagTypes.ISO6BG2) && m_BRIReader.TagCount > 0)
                        {
                            if (m_BRIReader.Tags[0].ToString().Length == 0x10)
                            {
                                m_RfidSoftAdapter.Write("\r\n");
                                m_RfidSoftAdapter.Write("H" + m_BRIReader.Tags[0].ToString() + " H" + m_BRIReader.Tags[0].ToString());
                                m_RfidSoftAdapter.Write("\r\n");
                                m_RfidSoftAdapter.Write("OK>");
                                m_RfidSoftAdapter.Write("\r\n");
                            }
                        }
                        else
                        {
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("NOTAG");
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        break;
                    case RfidSoftAdapter.CommandType.ReadRawData:
                        strCmd = (string[])e.m_Data;
                        if (m_BRIReader.Read(string.Format("HEX({0},{1})={2}", m_EPCTagIDStartPos, (strCmd[2].Length-1)/2, strCmd[2]), string.Format("HEX({0},16)", m_EPCTagIDStartPos), BRIReader.RFIDTagTypes.EPCC1G2) && m_BRIReader.TagCount > 0)
                        {
                            strtmp = Read(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString, Convert.ToInt64(strCmd[0]), Convert.ToInt64(strCmd[1]));
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString + " " + strtmp);
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        else if (m_BRIReader.Read(string.Format("TAGID={0}", strCmd[2]), string.Format("HEX({0},{1})", strCmd[0], strCmd[1]), BRIReader.RFIDTagTypes.ISO6BG2) && m_BRIReader.TagCount > 0)
                        {
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("H" + m_BRIReader.Tags[0].ToString() + " " + m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString);
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        else
                        {
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("NOTAG");
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        break;
                    case RfidSoftAdapter.CommandType.WriteRawData:
                        strCmd = (string[])e.m_Data;
                        if (m_BRIReader.Read(string.Format("HEX({0},{1})={2}",m_EPCTagIDStartPos,(strCmd[3].Length-1)/2,strCmd[3]),string.Format("HEX({0},16)",m_EPCTagIDStartPos), BRIReader.RFIDTagTypes.EPCC1G2) && m_BRIReader.TagCount > 0)
                        {
                            btResult = Write(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString, Convert.ToInt64(strCmd[0]), Convert.ToInt64(strCmd[1]), strCmd[2]);
                            m_RfidSoftAdapter.Write("\r\n");
                            if (btResult)
                            {
                                m_RfidSoftAdapter.Write(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString + " WORK");
                            }
                            else
                            {
                                m_RfidSoftAdapter.Write(m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString + " ERR");
                            }
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        else if (m_BRIReader.Read(string.Format("TAGID={0}", strCmd[3]), string.Format("HEX({0},{1})", strCmd[0], strCmd[1]), BRIReader.RFIDTagTypes.ISO6BG2) && m_BRIReader.TagCount > 0)
                        {
                            m_BRIReader.Tags[0].TagFields.FieldArray[0].DataString = strCmd[2];
                            m_BRIReader.Attributes.RFIDTagType = BRIReader.RFIDTagTypes.ISO6BG2;
                            btResult = m_BRIReader.Update();
                            m_RfidSoftAdapter.Write("\r\n");
                            if (btResult)
                            {
                                m_RfidSoftAdapter.Write("H" + m_BRIReader.Tags[0].ToString() + " WORK");
                            }
                            else
                            {
                                m_RfidSoftAdapter.Write("H" + m_BRIReader.Tags[0].ToString() + " ERR");
                            }
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        else
                        {
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("NOTAG");
                            m_RfidSoftAdapter.Write("\r\n");
                            m_RfidSoftAdapter.Write("OK>");
                            m_RfidSoftAdapter.Write("\r\n");
                        }
                        break;
                    case RfidSoftAdapter.CommandType.WriteWarningInfo:
                        strtmp = m_BRIReader.Execute(string.Format("WRITEGPIO={0}", e.m_Data));
                        m_RfidSoftAdapter.Write("\r\n");
                        m_RfidSoftAdapter.Write(strtmp);
                        m_RfidSoftAdapter.Write("\r\n");
                        break;
                    case RfidSoftAdapter.CommandType.Other:
                        strtmp = m_BRIReader.Execute((string)e.m_Data);
                        m_RfidSoftAdapter.Write("\r\n");
                        m_RfidSoftAdapter.Write(strtmp);
                        m_RfidSoftAdapter.Write("\r\n");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                m_RfidSoftAdapter.Write("\r\n");
                m_RfidSoftAdapter.Write("ERR");
                m_RfidSoftAdapter.Write("\r\n");
                m_RfidSoftAdapter.Write("OK>");
                m_RfidSoftAdapter.Write("\r\n");
            }
        }

        private string Read(string tagID, long startPos, long reqLen)
        {
            try
            {                
                byte[] byteResult;

                using (TransactionScopeManager tm = new TransactionScopeManager())
                {
                    IRelationAccesser relationAccesser = tm["TAGINFODB"].NewRelationAccesser();
                    object obj1 = relationAccesser.DoScalar(string.Format("SELECT TAGDATA FROM TAGINFO WHERE TAGID = '{0}'", tagID));
                    if (obj1 != null && ((byte [])obj1).Length == m_EPCTagDataLength)
                    {
                        byteResult = (byte [])obj1;
                    }
                    else
                    {
                        byteResult = new byte[m_EPCTagDataLength];
                    }
                    tm.Commit();
                }

                string strResult = "H";
                for (long i = startPos; i < startPos + reqLen; i++)
                {
                    strResult = strResult + byteResult[i].ToString("X2");
                }
                
                if (strResult.Length == reqLen * 2 + 1)
                {
                    return strResult;
                }
                else
                {
                    return "ERR";
                }
            }
            catch (Exception ex)
            {
                return "ERR";
            }
        }

        private bool Write(string tagID, long startPos, long reqLen, string data)
        {
            try
            {
                byte[] byteResult;

                using (TransactionScopeManager tm = new TransactionScopeManager(true, System.Data.IsolationLevel.ReadCommitted))
                {
                    IRelationAccesser relationAccesser = tm["TAGINFODB"].NewRelationAccesser();
                    object obj2 = relationAccesser.DoScalar(string.Format("SELECT TAGDATA FROM TAGINFO WHERE TAGID = '{0}'", tagID));

                    if (obj2 != null && ((byte [])obj2).Length == m_EPCTagDataLength)
                    {
                        byteResult = (byte[])obj2;
                        for (long i = startPos; i < startPos + reqLen; i++)
                        {
                            byteResult[i] = byte.Parse(data.Substring((int)(i - startPos) * 2 + 1, 2), NumberStyles.HexNumber);
                        }
                        string strResult = "0x";
                        for (int i = 0; i < byteResult.Length; i++)
                        {
                            strResult = strResult + byteResult[i].ToString("X2");
                        }
                        relationAccesser.DoCommand(string.Format("UPDATE TAGINFO SET TAGDATA = {0} WHERE TAGID = '{1}'", strResult, tagID));
                    }
                    else
                    {
                        byteResult = new byte[m_EPCTagDataLength];
                        for (long i = startPos; i < startPos + reqLen; i++)
                        {
                            byteResult[i] = byte.Parse(data.Substring((int)(i - startPos) * 2 + 1, 2), NumberStyles.HexNumber);
                        }
                        string strResult = "0x";
                        for (int i = 0; i < byteResult.Length; i++)
                        {
                            strResult = strResult + byteResult[i].ToString("X2");
                        }
                        relationAccesser.DoCommand(string.Format("INSERT TAGINFO VALUES('{0}',{1})", tagID, strResult));
                    }

                    tm.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        
    }
}
