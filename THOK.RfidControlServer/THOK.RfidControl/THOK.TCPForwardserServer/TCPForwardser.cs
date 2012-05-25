using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO.Ports;
using THOK.TCP;

namespace THOK.TCPForwardserServer
{
    public class TCPForwardser
    {
        private Server m_SocketServerSource = null;
        private string m_ServerSourceIp = "";
        private int m_ServerSourcePort = 0;
        private string m_RemoteSoftClientIp = "";

        private Client m_SocketClientTarget = null;
        private string m_TargetServerIp = "";
        private int m_TargetServerPort = 0;

        public TCPForwardser(string sourceTcpConnectionString,string targetTcpConnectionString)
        {
            string[] param1 = sourceTcpConnectionString.Split(new string[] { "://", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);
            string[] param2 = targetTcpConnectionString.Split(new string[] { "://", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (param1[0].ToUpper() == "TCP" && param1.Length == 4)
            {
                SetSourceTcpServerInfo(param1[1], Convert.ToInt32(param1[2]), param1[3]);
            }

            if (param2[0].ToUpper() == "TCP" && param2.Length == 3)
            {
                SetTargetTcpServerInfo(param2[1], Convert.ToInt32(param2[2]));
            }
        }

        private void SetSourceTcpServerInfo(string serverIp, int port, string remoteServerIp)
        {
            this.m_ServerSourceIp = serverIp;
            this.m_ServerSourcePort = port;
            this.m_RemoteSoftClientIp = remoteServerIp;
        }

        private void SetTargetTcpServerInfo(string serverIp, int port)
        {
            this.m_TargetServerIp = serverIp;
            this.m_TargetServerPort = port;
        }

        public bool Open()
        {
            bool returns = false;

            returns = SocketServerStart();

            return returns;
        }

        public bool Close()
        {
            bool returns = false;

            returns = SocketServerStop();
            returns = DisConnectToTargetServer();

            return returns;
        }

        private bool SocketServerStart()
        {
            try
            {
                m_SocketServerSource = new Server();
                m_SocketServerSource.OnReceive += new ReceiveEventHandler(m_SocketServer_OnReceive);
                m_SocketServerSource.OnConnect += new SocketEventHandler(m_SocketServer_OnConnect);
                m_SocketServerSource.StartListen(m_ServerSourceIp, m_ServerSourcePort);
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
                if (m_SocketServerSource != null)
                {
                    m_SocketServerSource.StopListen();
                }
                m_SocketServerSource = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void m_SocketServer_OnConnect(object sender, SocketEventArgs e)
        {
            if (m_SocketServerSource != null && m_SocketServerSource.OnlineCount > 0 && (this.m_RemoteSoftClientIp == "" || e.RemoteAddress == this.m_RemoteSoftClientIp || e.RemoteAddress.Split(":"[0])[0] == this.m_RemoteSoftClientIp.Split(":"[0])[0]))
            {
                this.m_RemoteSoftClientIp = e.RemoteAddress;
                this.WriteToSource("\r\n");
                this.WriteToSource("OK>");
                this.WriteToSource("\r\n");
                return;
            }
        }

        private void m_SocketServer_OnReceive(object sender, ReceiveEventArgs e)
        {
            if (m_SocketServerSource != null && m_SocketServerSource.OnlineCount > 0 && e.RemoteAddress == this.m_RemoteSoftClientIp)
            {
                WriteToTarget(e.Read());
            }
        }

        private void WriteToSource(string cmdData)
        {
            try
            {
                if (m_SocketServerSource != null && m_SocketServerSource.OnlineCount > 0)
                {
                    m_SocketServerSource.Write(m_RemoteSoftClientIp, cmdData);
                    System.Diagnostics.Debug.WriteLine("w - s" + cmdData);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void WriteToTarget(string cmdData)
        {
            try
            {
                if (ConnectToTargetServer())
                {
                    if (m_SocketClientTarget != null && m_SocketClientTarget.IsConnected)
                    {
                        m_SocketClientTarget.Write(cmdData);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private bool ConnectToTargetServer()
        {
            try
            {
                if (m_SocketClientTarget == null || !m_SocketClientTarget.IsConnected)
                {
                    DisConnectToTargetServer();
                    m_SocketClientTarget = new Client();                    
                    m_SocketClientTarget.OnReceive += new ReceiveEventHandler(m_SocketClientTarget_OnReceive);
                    m_SocketClientTarget.Connect(m_TargetServerIp, m_TargetServerPort);
                }

                return m_SocketClientTarget.IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DisConnectToTargetServer()
        {
            if (m_SocketClientTarget != null && m_SocketClientTarget.IsConnected)
            {
                m_SocketClientTarget.Close();                
            }
            m_SocketClientTarget = null;

            return true;
        }

        private void m_SocketClientTarget_OnReceive(object sender, ReceiveEventArgs e)
        {
            WriteToSource(e.Read());
        }
    }
}
