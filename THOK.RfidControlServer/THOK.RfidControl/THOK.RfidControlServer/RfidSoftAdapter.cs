using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO.Ports;
using THOK.TCP;

namespace THOK.RfidControlServer
{
    public class RfidSoftAdapter
    {
        #region 事件及命令        
        
        public class DataReceivedEventArgs
        {
            public CommandType m_CommandType = CommandType.ReadTagID;
            public object m_Data = null;
        }

        public enum CommandType
        {
            ReadTagID,
            ReadRawData,
            WriteRawData,
            WriteWarningInfo,
            Other
        }

        public delegate void DataReceivedEvent(RfidSoftAdapter sender,DataReceivedEventArgs e);
        public event DataReceivedEvent DataReceived = null;

        #endregion

        public bool m_IsOpen = false;
        private bool m_IsSerialPort = false;
        private SerialPort m_SerialPort = null;
        private string m_ComName = "";
        private int m_BaudRate = 0;
        private Parity m_Parity = Parity.None;
        private int m_DataBits = 0;
        private StopBits m_StopBits = StopBits.None;

        private bool m_IsSocketServer = false;
        private Server m_SocketServer = null;
        private string m_ServerIp = "";
        private int m_Port = 0;
        private string m_RemoteSoftClientIp = "";
        private string m_RemoteControlSoftClientIp = "";

        private string m_Sources = "";
        private string m_ReadSTX = "@{";
        private string m_ReadEND = "}@";
        public bool m_IsConnectEnable = false;

        private string m_ReadErr = "";
        private string m_Reading = "";
        private string m_ReadRuning = "";
        private string m_ReadClose = "";

        private int m_ReadBeforeSleepTime = 0;
        private int m_WriteBeforeSleepTime = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfidSoftConnectionString"></param>
        public RfidSoftAdapter(string rfidSoftConnectionString)
        {
            ConfigurationRead.m_Path = AppDomain.CurrentDomain.BaseDirectory + "RfidControlServerConfig.Xml";
            m_IsConnectEnable = Convert.ToBoolean(ConfigurationRead.GetPara("RfidControlServerConfig", "IsConnectEnable"));
            m_ReadErr = ConfigurationRead.GetPara("RfidControlServerConfig", "ReadErr");
            m_Reading = ConfigurationRead.GetPara("RfidControlServerConfig", "Reading");
            m_ReadRuning = ConfigurationRead.GetPara("RfidControlServerConfig", "ReadRuning");
            m_ReadClose = ConfigurationRead.GetPara("RfidControlServerConfig", "ReadClose");
            m_ReadBeforeSleepTime = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "ReadBeforeSleepTime"));
            m_WriteBeforeSleepTime = Convert.ToInt32(ConfigurationRead.GetPara("RfidControlServerConfig", "WriteBeforeSleepTime"));

            string[] param = rfidSoftConnectionString.Split(new string[] { "://", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (param[0].ToUpper() == "TCP" && param.Length == 4)
            {
                this.ctor(param[1], Convert.ToInt32(param[2]), param[3]);
            }
            else if (param[0].ToUpper() == "SERIAL" && param.Length == 2)
            {
                this.ctor(param[1], 115200, Parity.None, 8, StopBits.One);
            }
        }

        private void ctor(string serverIp, int port, string remoteServerIp)
        {
            this.m_IsSocketServer = true;
            this.m_IsSerialPort = false;
            this.m_ServerIp = serverIp;
            this.m_Port = port;
            this.m_RemoteSoftClientIp = remoteServerIp;
        }

        private void ctor(string comName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.m_IsSocketServer = false;
            this.m_IsSerialPort = true;
            this.m_ComName = comName;
            this.m_BaudRate = baudRate;
            this.m_Parity = parity;
            this.m_DataBits = dataBits;
            this.m_StopBits = stopBits;
        }

        public bool Open()
        {
            bool returns = false;

            if (m_IsSocketServer)
            {
                returns = SocketServerStart();
            }
            else if (m_IsSerialPort)
            {
                returns = OpenSerialPortCom();
            }
            m_IsOpen = returns;
            return returns;
        }

        public bool Close()
        {
            bool returns = false;

            if (m_IsSocketServer)
            {
                returns = SocketServerStop();
            }
            else if (m_IsSerialPort)
            {
                returns = CloseSerialPortCom();
            }
            PlaySound.SayStop();
            return returns;
        }
        
        private bool SocketServerStart()
        {
            try
            {
                m_SocketServer = new Server();
                m_SocketServer.OnReceive += new ReceiveEventHandler(m_SocketServer_OnReceive);
                m_SocketServer.OnConnect += new SocketEventHandler(m_SocketServer_OnConnect);
                m_SocketServer.OnDisconnect += new SocketEventHandler(m_SocketServer_OnDisconnect);
                m_SocketServer.StartListen(m_ServerIp, m_Port);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SocketServerStop()
        {
            try
            {
                if (m_SocketServer != null)
                {
                    m_SocketServer.StopListen();                    
                }                
                m_SocketServer = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void m_SocketServer_OnConnect(object sender, SocketEventArgs e)
        {
            if (m_SocketServer != null && m_SocketServer.OnlineCount > 0 && (this.m_RemoteSoftClientIp == "" || e.RemoteAddress == this.m_RemoteSoftClientIp || e.RemoteAddress.Split(":"[0])[0] == this.m_RemoteSoftClientIp.Split(":"[0])[0]))
            {
                this.m_RemoteSoftClientIp = e.RemoteAddress;
                this.Write("\r\n");
                this.Write("OK>");
                this.Write("\r\n");
                return;
            }

            this.m_RemoteControlSoftClientIp = e.RemoteAddress;
        }

        private void m_SocketServer_OnReceive(object sender, ReceiveEventArgs e)
        {
            if (m_SocketServer != null && m_SocketServer.OnlineCount > 0 && e.RemoteAddress == this.m_RemoteSoftClientIp)
            {
                if (m_IsConnectEnable)
                {
                    m_Sources = m_Sources + this.m_ReadSTX;
                    m_Sources = m_Sources + e.Read();
                    m_Sources = m_Sources + this.m_ReadEND;
                    SplitCmd();
                }
                else if (e.Read().ToString() != "")
                {
                    Write("\r\n");
                    Write("NOTAG");
                    Write("\r\n");
                    Write("OK>");
                    Write("\r\n");
                }
                return;
            }            

            if (m_SocketServer != null && m_SocketServer.OnlineCount > 0 && e.RemoteAddress == this.m_RemoteControlSoftClientIp)
            {
                string tmp = e.Read().ToString().ToUpper();
                if (tmp == "START")
                {
                    this.m_IsConnectEnable = true;
                }
                else if (tmp == "STOP")
                {
                    this.m_IsConnectEnable = false;
                }                
            }
        }

        private void m_SocketServer_OnDisconnect(object sender, SocketEventArgs e)
        {
            if (e.RemoteAddress == m_RemoteControlSoftClientIp)
            {
                this.m_IsConnectEnable = false;
                this.m_RemoteControlSoftClientIp = "";
            }
        }

        private bool OpenSerialPortCom()
        {
            try
            {
                m_SerialPort = new SerialPort();
                m_SerialPort.ReadTimeout = 100;
                m_SerialPort.PortName = this.m_ComName;
                m_SerialPort.BaudRate = this.m_BaudRate;
                m_SerialPort.Parity =this.m_Parity;
                m_SerialPort.DataBits = this.m_DataBits;
                m_SerialPort.StopBits = this.m_StopBits;
                m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(m_SerialPort_DataReceived);
                m_SerialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool CloseSerialPortCom()
        {
            try
            {
                if (m_SerialPort != null && m_SerialPort.IsOpen)
                {
                    m_SerialPort.Close();                    
                }
                m_SerialPort = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void m_SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                if (this.m_SerialPort.BytesToRead == 0) { return; }
                int i;
                do
                {
                    byte[] btRecv = new byte[this.m_SerialPort.ReadBufferSize];
                    int nRead = this.m_SerialPort.Read(btRecv, 0, this.m_SerialPort.BytesToRead);
                    i = 0;
                    while (i < nRead)
                    {
                        m_Sources = m_Sources + (char)btRecv[i];
                        i++;
                    }
                }
                while ((this.m_SerialPort.BytesToRead != 0));
            }            
            SplitCmd();
        }

        private void SplitCmd()
        {
            if (m_Sources == "\r\n" || m_Sources == "\r" || m_Sources == "\n")
            {
                this.Write("\r\n");
                this.Write("OK>");
                this.Write("\r\n");
            }

            m_Sources = m_Sources.Replace("\r\n", this.m_ReadEND);

            string SplitCmd = "";
            if (m_Sources.LastIndexOf((this.m_ReadEND)) != -1)
            {
                SplitCmd = m_Sources.Substring(0, m_Sources.LastIndexOf(this.m_ReadEND) + this.m_ReadEND.Length);
                m_Sources = m_Sources.Substring(m_Sources.LastIndexOf(this.m_ReadEND) + this.m_ReadEND.Length - 1, m_Sources.Length - (m_Sources.LastIndexOf(this.m_ReadEND) + this.m_ReadEND.Length));
            }
            if (SplitCmd == "") { return; }

            string[] strOperator = new string[] { this.m_ReadSTX, this.m_ReadEND };
            string[] strCmd = SplitCmd.Split(strOperator, StringSplitOptions.RemoveEmptyEntries);
            if (strCmd.Length <= 0) { return; }

            for (int i = 0; i < strCmd.Length; i++)
            {
                Thread.Sleep(m_ReadBeforeSleepTime);
                this.Parser(strCmd[i]);
            }
        }

        private void Parser(string cmd)
        {
            string tmpData = cmd;
            cmd = cmd.Trim().ToUpper().Replace(" ", string.Empty);

            bool parserResult = false;
            DataReceivedEventArgs e = new DataReceivedEventArgs();
            string[] strCmd;
            if (cmd == "READTAGID")
            {
                e.m_CommandType = CommandType.ReadTagID;
                parserResult = true;
            }
            else if (cmd.StartsWith("READHEX"))
            {
                strCmd = cmd.Split(new string[] { "READHEX(",",",")WHERETAGID="}, StringSplitOptions.RemoveEmptyEntries);
                if (strCmd.Length == 3)
                {
                    e.m_CommandType = CommandType.ReadRawData;
                    e.m_Data = strCmd;
                    parserResult = true;
                }
            }
            else if (cmd.StartsWith("WRITEHEX"))
            {
                strCmd = cmd.Split(new string[] { "WRITEHEX(", ",", ")=", "WHERETAGID=" }, StringSplitOptions.RemoveEmptyEntries);
                if (strCmd.Length == 4)
                {
                    e.m_CommandType = CommandType.WriteRawData;
                    e.m_Data = strCmd;
                    parserResult = true;
                }
            }
            else if (cmd.StartsWith("WRITEGPIO"))
            {
                e.m_CommandType = CommandType.WriteWarningInfo;
                parserResult = true;
                switch (cmd)
                {
                    case "WRITEGPIO=6":
                        PlaySound.SayStop();
                        PlaySound.PlayAsync("ReadErr.wav");
                        PlaySound.SayAsync(m_ReadErr);
                        e.m_Data = 6;
                        break;
                    case "WRITEGPIO=11":
                        PlaySound.SayStop();
                        PlaySound.SayAsync(m_Reading);
                        PlaySound.PlayAsync("Reading.wav");                      
                        e.m_Data = 11;
                        break;
                    case "WRITEGPIO=13":
                        PlaySound.SayStop();
                        PlaySound.PlayAsync("ReadRuning.wav");
                        PlaySound.SayAsync(m_ReadRuning);
                        e.m_Data = 13;
                        break;
                    case "WRITEGPIO=15":
                        PlaySound.SayStop();
                        PlaySound.PlayAsync("ReadClose.wav");
                        PlaySound.SayAsync(m_ReadClose);
                        e.m_Data = 15;
                        break;
                    default:
                        parserResult = false;
                        break;
                }
                if (parserResult)
                {
                    WriteToControlSoftClient(cmd);
                }
            }
            else
            {
                e.m_CommandType = CommandType.Other;
                e.m_Data = tmpData;
                parserResult = true;
            }

            if(parserResult)
                OnDataReceived(e);
        }

        private void OnDataReceived(DataReceivedEventArgs e)
        {
            if (this.DataReceived != null)
            {
                DataReceived(this,e);
            }
        }

        public void Write(string cmdData)
        {
            try
            {
                if (m_IsSerialPort)
                {
                    if (m_SerialPort != null && m_SerialPort.IsOpen)
                    {
                        m_SerialPort.Write(cmdData);
                    }
                }
                else if (m_IsSocketServer)
                {
                    if (m_SocketServer != null && m_SocketServer.OnlineCount > 0)
                    {
                        System.Threading.Thread.Sleep(m_WriteBeforeSleepTime);
                        m_SocketServer.Write(m_RemoteSoftClientIp, cmdData);                       
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void WriteToControlSoftClient(string cmdData)
        {
            try
            {
                if (m_IsSocketServer && m_RemoteControlSoftClientIp !="")
                {
                    if (m_SocketServer != null && m_SocketServer.OnlineCount > 0)
                    {
                        System.Threading.Thread.Sleep(m_WriteBeforeSleepTime);
                        m_SocketServer.Write(m_RemoteControlSoftClientIp, cmdData);
                    }
                }                
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
