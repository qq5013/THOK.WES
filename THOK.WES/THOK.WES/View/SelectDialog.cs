using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class SelectDialog : Form
    {
        public string SelectedBillID
        {
            get { return cbBillID.SelectedValue.ToString(); }
        }

        public SelectDialog(DataTable masterTable)
        {
            InitializeComponent();
            cbBillID.DataSource = masterTable;
            cbBillID.DisplayMember = "BILLMASTERID";
            cbBillID.ValueMember = "BILLMASTERID";
            cbBillID.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}