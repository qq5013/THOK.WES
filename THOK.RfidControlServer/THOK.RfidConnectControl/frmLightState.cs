using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace THOK.RfidConnectControl
{
    public partial class frmLightState : Form, IConnectConntrol
    {
        delegate void StartCall();
        delegate void StopCall();
        delegate void SetRunStateCall(StateType stateType);

        public frmLightState()
        {
            InitializeComponent();          
        }

        #region IConnectConntrol 成员

        public void SetSize()
        {
            this.Left = (System.Windows.Forms.Screen.PrimaryScreen).Bounds.Width - this.Width - 10;
            this.Top = 10;
            this.Height = 50;
            this.Width = 50;
        }

        public void Start()
        {
            if (!this.InvokeRequired)
            {
                this.Show();
                this.SetSize();                
            }
            else
            {
                this.Invoke(new StartCall(Start));
            }            
        }

        public void Stop()
        {
            if (!this.InvokeRequired)
            {
                this.Hide();
            }
            else
            {
                this.Invoke(new StopCall(Stop));
            }   
        }

        public void SetRunState(StateType stateType)
        {
            if (!this.InvokeRequired)
            {
                switch (stateType)
                {
                    case StateType.ReadErr:
                        p.BackColor = Color.Red;
                        break;
                    case StateType.Reading:
                        p.BackColor = Color.Yellow;
                        break;
                    case StateType.ReadRuning:
                        p.BackColor = Color.LawnGreen;
                        break;
                    case StateType.ReadClose:
                        p.BackColor = Color.Transparent;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.Invoke(new SetRunStateCall(SetRunState),stateType);
            }   
        }

        #endregion
    }
}
