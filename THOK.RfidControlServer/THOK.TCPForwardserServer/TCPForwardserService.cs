using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

using System.ServiceProcess;
using System.Text;

namespace THOK.TCPForwardserServer
{
    public partial class TCPForwardserService : ServiceBase
    {
        TCPForwardserServer m_TCPForwardserServer = new TCPForwardserServer();
        public TCPForwardserService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            m_TCPForwardserServer.Start();
        }

        protected override void OnStop()
        {
            m_TCPForwardserServer.Stop();
        }

        protected override void OnShutdown()
        {
            m_TCPForwardserServer.Stop();
            base.OnShutdown();
        }
    }
}
