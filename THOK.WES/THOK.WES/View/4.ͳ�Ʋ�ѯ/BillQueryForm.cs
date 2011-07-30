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
    public partial class BillQueryForm : THOK.AF.View.ToolbarForm
    {
        private BillDal billDal = new BillDal();

        public BillQueryForm()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            bsMaster.DataSource = billDal.GetBillMaster();
            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = billDal.GetBillDetail(dgvMaster.Rows[e.RowIndex].Cells["BILLMASTERID"].Value.ToString());
        }

        private void dgvMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
        }
    }
}

