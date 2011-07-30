using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class DeliveryDataForm : THOK.WES.View.BaseDataForm
    {
        public DeliveryDataForm()
        {
            InitializeComponent();
            BillType = "2";
        }
    }
}

