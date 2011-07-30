using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using THOK.WES.Dal;

namespace THOK.WES.View
{
    public partial class BillExportForm : THOK.AF.View.ToolbarForm
    {
        
        private BillDal billDal = new BillDal();
        private PDAUploadDataDal dal = new PDAUploadDataDal();

        public BillExportForm()
        {
            InitializeComponent();            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable t = billDal.GetAllBill();
                dgvMaster.DataSource = t;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {            
            dgvDetail.DataSource = billDal.GetBillDetail(dgvMaster.Rows[e.RowIndex].Cells["BILLMASTERID"].Value.ToString());
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            try
            {
                lock (this)
                {
                    if (!dal.IsConnetion())
                    {
                        MessageBox.Show("请连接PDA!", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        IList<string> list = new List<string>();
                        foreach (DataGridViewRow row in dgvMaster.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells[0].Value))
                            {
                                list.Add(row.Cells["BILLMASTERID"].Value.ToString());
                            }
                        }

                        if (list.Count != 0)
                        {
                            billDal.ExportBill(list);
                            MessageBox.Show("单据已保存到PDA中");
                        }
                        else
                        {
                            MessageBox.Show("请选择需要进行仓库作业的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }     

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(new PDAUploadDataDal().IsConnetion()))
                {
                    MessageBox.Show("请连接PDA!");
                    return;
                }

                billDal.SynchronizationBill();
                MessageBox.Show("同步成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}

