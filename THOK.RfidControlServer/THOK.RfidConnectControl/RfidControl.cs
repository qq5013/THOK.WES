using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using THOK.TCP;

namespace THOK.RfidConnectControl
{
    public partial class RfidControl : UserControl 
    {
        private bool m_IsStart = false;
        private IConnectConntrol m_ConnectConntrol = new frmLightState();

        private Client m_SocketClientTarget = null;
        private string m_TargetServerIp = "";
        private int m_TargetServerPort = 0;

        public RfidControl()
        {
            InitializeComponent();
        }

        private void SetTargetTcpServerInfo(string serverIp, int port)
        {
            this.m_TargetServerIp = serverIp;
            this.m_TargetServerPort = port;
        }

        public void Rfid_Click()
        {
            ConfigurationRead.m_Path = AppDomain.CurrentDomain.BaseDirectory + "RfidControlServerConfig.Xml";
            string targetServerTcpConnectionString = ConfigurationRead.GetPara("RfidControlServerConfig", "TargetServerTcpConnectionString");
            string[] param = targetServerTcpConnectionString.Split(new string[] { "://", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (param[0].ToUpper() == "TCP" && param.Length == 3)
            {
                SetTargetTcpServerInfo(param[1], Convert.ToInt32(param[2]));
            }

            if (m_IsStart)
            {
                m_IsStart = false;
                WriteToTarget("STOP");
                m_ConnectConntrol.Stop();
            }
            else
            {
                m_IsStart = true;
                WriteToTarget("START");
                m_ConnectConntrol.Start();
                m_ConnectConntrol.SetRunState(StateType.ReadRuning);
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
            if (m_SocketClientTarget != null && m_SocketClientTarget.IsConnected)
            {
                string cmd = e.Read().ToString().ToUpper();
                switch (cmd)
                {
                    case "WRITEGPIO=6":
                        m_ConnectConntrol.SetRunState(StateType.ReadErr);
                        break;
                    case "WRITEGPIO=11":
                        m_ConnectConntrol.SetRunState(StateType.Reading);
                        break;
                    case "WRITEGPIO=13":
                        m_ConnectConntrol.SetRunState(StateType.ReadRuning);
                        break;
                    case "WRITEGPIO=15":
                        m_ConnectConntrol.SetRunState(StateType.ReadClose);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Release()
        {
            WriteToTarget("STOP");
            DisConnectToTargetServer();
            m_ConnectConntrol.Stop();
        }
    }
}
