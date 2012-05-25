using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace THOK.RfidControlServer
{
    public partial class RfidControlService : ServiceBase
    {
        private RfidControlAdapter rfidControlAdapter;

        public RfidControlService()
        {
            InitializeComponent();
            rfidControlAdapter = new RfidControlAdapter();
        }

        protected override void OnStart(string[] args)
        {
            rfidControlAdapter.Start();
        }

        protected override void OnStop()
        {
            rfidControlAdapter.Stop();
        }

        protected override void OnShutdown()
        {
            rfidControlAdapter.Stop();
            base.OnShutdown();
        }       
    }
}
