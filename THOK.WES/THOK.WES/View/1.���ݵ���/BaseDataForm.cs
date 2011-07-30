 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using THOK.WES.Dal;
using THOK.WES.Interface;
using THOK.WES.Interface.Dal;

namespace THOK.WES.View
{
    public partial class BaseDataForm : THOK.AF.View.ToolbarForm
    {
        protected string BillType = "0";
        private BillDal billDal = new BillDal();
        private Thread importDataThread = null;
        private bool displayDetail = false;

        public BaseDataForm()
        {
            InitializeComponent();            
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (displayDetail)
            {
                dgvDetail.DataSource = billDal.GetBillDetail(dgvMaster.Rows[e.RowIndex].Cells["BILLMASTERID"].Value.ToString());
            }
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            foreach (DataGridViewRow row in dgvMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedRow = row;
                    break;
                }
            }

            if (selectedRow != null)
            {
                if (selectedRow.Cells["STATENAME"].Value.ToString() == "未执行")
                {
                    billDal.SaveMasterState("5", selectedRow.Cells["BILLMASTERID"].Value.ToString());//todo 作业
                    dgvMaster.DataSource = billDal.GetBillMaster(BillType);
                }
                else
                    MessageBox.Show("单据状态不是‘未执行’，请重新选择单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("请选择需要进行仓库作业的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            foreach (DataGridViewRow row in dgvMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedRow = row;
                    break;
                }
            }

            if (selectedRow != null)
            {
                if (selectedRow.Cells["STATENAME"].Value.ToString() == "执行中")
                {
                    string billID = selectedRow.Cells["BILLMASTERID"].Value.ToString();
                    if (billDal.GetTaskedCount(billID) == 0)
                    {
                        billDal.SaveMasterState("4", billID);//todo 取消作业
                        dgvMaster.DataSource = billDal.GetBillMaster(BillType);
                    }
                    else
                        MessageBox.Show("不能进行取消操作，已有子项开始仓库作业。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("单据状态不是‘执行中’，请重新选择单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("请选择需要取消仓库作业的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region 导入数据        

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayPlWailt();

                ImportDataThread importData = new ImportDataThread();
                importData.ClosePlWailt += new ImportDataThread.ClosePlWailtDelegete(ClosePlWailt);
                importData.BillType = BillType;
                importData.IData = WesContext.GetData();

                Form.CheckForIllegalCrossThreadCalls = false;
                importDataThread = new Thread(new ThreadStart(importData.Run));
                importDataThread.IsBackground = true;
                importDataThread.Start();

                if (dgvMaster.DataSource != null)
                {
                    ((DataTable)dgvMaster.DataSource).Clear();
                }
                if (dgvDetail.DataSource != null)
                {
                    ((DataTable)dgvDetail.DataSource).Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DisplayPlWailt()
        {
            this.plWailt.Visible = true;
            this.plWailt.Left = (this.dgvMaster.Width - this.plWailt.Width) / 2;
            this.plWailt.Top = (this.dgvMaster.Height - this.plWailt.Height) / 2;
            btnImport.Enabled = false;
            btnTask.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = false;
            displayDetail = false;
            InTask = true;
        }
        
        public void ClosePlWailt()
        {
            dgvMaster.DataSource = billDal.GetBillMaster(BillType);
            this.plWailt.Visible = false;
            btnImport.Enabled = true;
            btnTask.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = true;

            if (dgvMaster.Rows.Count > 0)
            {
                dgvDetail.DataSource = billDal.GetBillDetail(dgvMaster.Rows[0].Cells[2].Value.ToString());
            }
            displayDetail = true;
            InTask = false;
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }       
    }
}

