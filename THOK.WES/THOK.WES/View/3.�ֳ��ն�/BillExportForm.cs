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
                MessageBox.Show("��ȡ����ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("������PDA!", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            MessageBox.Show("�����ѱ��浽PDA��");
                        }
                        else
                        {
                            MessageBox.Show("��ѡ����Ҫ���вֿ���ҵ�ĵ��ݡ�", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("������PDA!");
                    return;
                }

                billDal.SynchronizationBill();
                MessageBox.Show("ͬ���ɹ�!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}

