using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class DeliveryTaskForm : THOK.WES.View.BaseTaskForm
    {
        public DeliveryTaskForm()
        {
            InitializeComponent();
            billType = "2";

            btnOpType.Visible = true;
            btnOpType.Enabled = false;
            opType = 0;
            btnOpType.Text = "Õý³£";
            timer1.Enabled = false;
            btnSearch.Enabled = true;
            dgvMain.Columns["TARGETSTORAGE"].Visible = true;
        }
    }
}

