using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;

namespace THOK.WES.View
{
    public partial class EntryDataForm : THOK.WES.View.BaseDataForm
    {
        
        public EntryDataForm()
        {
            InitializeComponent();
            BillType = "1";
        }
    }
}

