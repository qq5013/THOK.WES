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
    public partial class HistoryBillQueryFrom : THOK.AF.View.ToolbarForm
    {
        private BillDal billDal = new BillDal();

        public HistoryBillQueryFrom()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataTable billMaster = new DataTable();
            string billMasterID = " ";
            string billDetailID = " ";
            billMaster = billDal.GetBillMasterAll();
            if (billMaster.Rows.Count > 0)
            {
                for (int i = 0; i < billMaster.Rows.Count; i++)
                {
                    DataTable billDetail = new DataTable();
                    billMasterID += "'" + billMaster.Rows[i]["billID"].ToString() + "',";
                    billDetail = billDal.GetBillDetailAll(billMaster.Rows[i]["billID"].ToString());
                    billDetailID = billMaster.Rows[i]["billID"].ToString();
                    billDal.BuckupDateDetail(billDetail, billDetailID);
                }
                billMasterID = billMasterID.TrimEnd(',');
                billDal.BuckupDateMaster(billMaster, billMasterID);
            }
            bsMaster.DataSource = billDal.GetHistoryBillMaster();
            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = billDal.GetHistoryBillDetail(dgvMaster.Rows[e.RowIndex].Cells["ID"].Value.ToString());
        }

        private void dgvMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
        }
    }
}

