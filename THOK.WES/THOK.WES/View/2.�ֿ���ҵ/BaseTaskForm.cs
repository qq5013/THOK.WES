using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;
using THOK.WES;
using THOK.WES.Dao;
using THOK.WES.Interface;

namespace THOK.WES.View
{
    public partial class BaseTaskForm : THOK.AF.View.ToolbarForm
    {
        protected string billType = "0";
        private BillDal billDal = new BillDal();
        
        private ReadRFID readRFID = new ReadRFID();
        private StorageDal storageDal = new StorageDal();

        private TaskDal taskDal = new TaskDal();
        private SendUDP udp = new SendUDP();

        private string wesBillIDes = "";
        private string wesBillID = "";
        private string wmsBillID = "";
        private string wesDetailID = "";
        private string operateStorageName = "";
        private string targetStorageName = "";
        private string operateType = "";
        private string operateTypeName = "";
        private string operateTobaccoName = string.Empty;
        private int piece = 0;
        private int item = 0;
        private string rfid = "";

        //标记用户是否手动确认rfid成功。
        private bool rfidTag = false; 
        private bool isASC = true;
        protected int opType = 0;
        private int stockInBatchNo = 0 ;

        public BaseTaskForm()
        {
            InitializeComponent();

            if (new ConfigUtil().GetConfig("DeviceType")["Device"] == "1")
            {
                this.dgvMain.ColumnHeadersHeight = 22;
                this.dgvMain.RowTemplate.Height = 22;
                this.dgvMain.DefaultCellStyle.Font = new Font("宋体", 10);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 10);
            }
            else
            {
                this.dgvMain.ColumnHeadersHeight = 40;
                this.dgvMain.RowTemplate.Height = 40;
                this.dgvMain.DefaultCellStyle.Font = new Font("宋体", 16);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 13);
                this.btnBatConfirm.Visible = false;
            }
        }

        //查询
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable masterTable = billDal.GetTaskingMaster(billType);
                switch (masterTable.Rows.Count)
                {
                    case 0:
                        wesBillIDes = "";
                        break;
                    case 1:
                        wesBillIDes = masterTable.Rows[0]["BILLMASTERID"].ToString();                        
                        break;
                    default:
                        SelectDialog selectDialog = new SelectDialog(masterTable);
                        if (selectDialog.ShowDialog() == DialogResult.OK)
                        {
                            wesBillIDes = selectDialog.SelectedBillID;
                        }
                        break;
                }
                if (billType == "2" && wesBillIDes != "")
                    wesBillIDes = string.Format("'{0}','{1}M2'", wesBillIDes, wesBillIDes.Substring(0, wesBillIDes.Length - 1)); 
                else
                    wesBillIDes = string.Format("'{0}'", wesBillIDes);
                if (isASC)
                    RefreshData("ASC");
                else
                    RefreshData("DESC");
                isASC = !isASC;    
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //刷新数据
        private void RefreshData(string orderby)
        {
            sslBillID.Text = "单据号：" + wesBillIDes + "                              ";
            sslOperator.Text = "操作员：" + Environment.MachineName;
            btnOpType.Enabled = wesBillIDes != "";

            DataTable detailTable = billDal.GetTaskDetail(wesBillIDes, orderby, opType);
            if (detailTable.Rows.Count != 0)
            {
                DataRow[] rows = detailTable.Select(string.Format("STATENAME='{0}' AND OPERATOR='{1}'", "执行中", Environment.MachineName));
                if (rows.Length != 0)
                {
                    wesBillID = rows[0]["BILLMASTERID"].ToString();
                    wmsBillID = rows[0]["BILLID"].ToString();
                    wesDetailID = rows[0]["DETAILID"].ToString();
                    operateStorageName = rows[0]["STORAGEID"].ToString();
                    targetStorageName = rows[0]["TARGETSTORAGE"].ToString();
                    operateType = rows[0]["OPERATECODE"].ToString();
                    operateTypeName = rows[0]["OPERATENAME"].ToString();
                    operateTobaccoName = rows[0]["TOBACCONAME"].ToString();
                    piece = Convert.ToInt32(rows[0]["OPERATEPIECE"]);
                    item = Convert.ToInt32(rows[0]["OPERATEITEM"]);
                    InTask = true;
                }
                else
                {
                    wesBillID = "";
                    wmsBillID = "";
                    wesDetailID = "";
                    operateStorageName = "";
                    operateType = "";
                    operateTobaccoName = "";
                    piece = 0;
                    item = 0;
                    InTask = false;
                }
                btnApply.Enabled = !InTask;
                btnConfirm.Enabled = InTask;
                btnBatConfirm.Enabled = InTask;
                btnCancel.Enabled = InTask;
                dgvMain.DataSource = detailTable;
            }
            else
            {
                wesBillID = "";
                wmsBillID = "";
                wesDetailID = "";
                operateStorageName = "";
                operateType = "";
                operateTobaccoName = "";
                piece = 0;
                item = 0;
                InTask = false;
                btnApply.Enabled = false;
                btnConfirm.Enabled = false;
                btnCancel.Enabled = false;
                if(dgvMain.DataSource != null)
                    ((DataTable)dgvMain.DataSource).Clear();
            }
        }

        //申请
        private void btnApply_Click(object sender, EventArgs e)
        {
            isASC = false;
            try
            {
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        Application.DoEvents();
                        stockInBatchNo = opType == 1 ? Convert.ToInt32(row.Cells["BATCHNO"].Value) : 0;
                        if (!billDal.ApplyTask(row.Cells["BILLMASTERID"].Value.ToString(), row.Cells["DETAILID"].Value.ToString(), stockInBatchNo))
                        {
                            MessageBox.Show("您申请的单据已被其他车载申请,请重新选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            stockInBatchNo = 0;
                        }
                        else
                        {
                            wesBillID = row.Cells["BILLMASTERID"].Value.ToString();
                            wmsBillID = row.Cells["BILLID"].Value.ToString();
                            wesDetailID = row.Cells["DETAILID"].Value.ToString();
                            operateStorageName = row.Cells["STORAGENAME"].Value.ToString();
                            targetStorageName = row.Cells["TARGETSTORAGE"].Value.ToString();
                            operateType = row.Cells["OPERATECODE"].Value.ToString();
                            operateTobaccoName = row.Cells["TOBACCONAME"].Value.ToString();
                            piece = Convert.ToInt32(row.Cells["OPERATEPIECE"].Value);
                            item = Convert.ToInt32(row.Cells["OPERATEITEM"].Value);

                            if (new ConfigUtil().GetConfig("DeviceType")["Device"] != "0")
                            {
                                taskDal.SetTask(wesBillID, wesDetailID, operateStorageName,targetStorageName,operateType, operateTobaccoName, piece, item, "1");
                            }
                        }
                    }

                    if (isASC)
                        RefreshData("ASC");
                    else
                        RefreshData("DESC");
                    ClosePlWailt();
                }
                else
                    MessageBox.Show("请选择要执行的仓库作业。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("申请失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //取消申请
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        Application.DoEvents();
                        if (row.Cells["STATENAME"].Value.ToString() == "执行中")
                        {
                            stockInBatchNo = opType == 1 ? Convert.ToInt32(row.Cells["BATCHNO"].Value) : 0;
                            wesBillID = row.Cells["BILLMASTERID"].Value.ToString();
                            wmsBillID = row.Cells["BILLID"].Value.ToString();
                            wesDetailID = row.Cells["DETAILID"].Value.ToString();
                            operateStorageName = row.Cells["STORAGENAME"].Value.ToString();
                            operateType = row.Cells["OPERATECODE"].Value.ToString();
                            operateTobaccoName = row.Cells["TOBACCONAME"].Value.ToString();
                            piece = Convert.ToInt32(row.Cells["OPERATEPIECE"].Value);
                            item = Convert.ToInt32(row.Cells["OPERATEITEM"].Value);

                            StockInDao stockInDao = new StockInDao();
                            stockInDao.CancelUpdateState(stockInBatchNo);
                            stockInBatchNo = 0;
                            ConfirmTask("1", "", "");
                        }
                    }
                    if (isASC)
                        RefreshData("ASC");
                    else
                        RefreshData("DESC");
                    ClosePlWailt();
                }
                else
                    MessageBox.Show("请选择要取消的仓库作业。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("取消失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {                
                if (dgvMain.SelectedRows.Count == 1)
                {
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        if (row.Cells["STATENAME"].Value.ToString() == "执行中")
                        {
                            stockInBatchNo = opType == 1 ? Convert.ToInt32(row.Cells["BATCHNO"].Value) : 0;
                            wesBillID = row.Cells["BILLMASTERID"].Value.ToString();
                            wmsBillID = row.Cells["BILLID"].Value.ToString();
                            wesDetailID = row.Cells["DETAILID"].Value.ToString();
                            operateStorageName = row.Cells["STORAGENAME"].Value.ToString();
                            targetStorageName = row.Cells["TARGETSTORAGE"].Value.ToString();
                            operateType = row.Cells["OPERATECODE"].Value.ToString();
                            operateTypeName = row.Cells["OPERATENAME"].Value.ToString();
                            operateTobaccoName = row.Cells["TOBACCONAME"].Value.ToString();
                            piece = Convert.ToInt32(row.Cells["OPERATEPIECE"].Value);
                            item = Convert.ToInt32(row.Cells["OPERATEITEM"].Value);                            
                        }
                    }
                }

                ConfirmDialog confirmForm = new ConfirmDialog(billType, operateStorageName,targetStorageName,operateTypeName, operateTobaccoName);
                confirmForm.Piece = piece;
                confirmForm.Item = item;

                if (confirmForm.ShowDialog() == DialogResult.OK)
                {
                    int storageType = storageDal.GetStorageType(operateStorageName);
                    //零烟柜无托盘
                    if (storageType != 0 && new ConfigUtil().GetConfig("RFID")["USEDRFID"] == "1")
                    {
                        //盘点不读取RFID
                        if (billType != "3")
                        {
                            //读取RFID号
                            rfid = readRFID.GetRFID();
                            //判断读取RFID是否成功
                            if (rfid == "")
                            {
                                this.ConfirmRfid("获取托盘ID失败,请重试!");
                                if (!rfidTag)
                                {
                                    return;
                                }
                            }
                        }

                        if (billType == "2" || billType == "4")
                        {
                            //如果为出库单或为移出单
                            if (rfid != storageDal.GetRFID(operateStorageName))
                            {
                                this.ConfirmRfid("获取的托盘ID与货架ID不一致!");
                                if (!rfidTag)
                                {
                                    return;
                                }
                            }
                        }
                        else if (billType != "3")
                        {
                            //更新货架RFID
                            storageDal.UpdateRFID(operateStorageName, rfid);
                        }
                    }

                    IData dataInterface = WesContext.GetData();
                    ConfirmResult result = null;
                    if (billType == "3")
                    {
                        piece = confirmForm.Piece;
                        item = confirmForm.Item;
                    }
                    result = dataInterface.UploadData(billType, wesBillID, wesDetailID, operateType, piece, item, rfid);
                    if (result.IsSuccess)                        
                        ConfirmTask("3", result.State, result.Desc);
                    else
                        MessageBox.Show("无法完成作业确认，原因：" + result.Desc, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (isASC)
                        RefreshData("ASC");
                    else
                        RefreshData("DESC");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //批量确认
        private void btnBatConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("当前操作将批量确认已申请的所有任务！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }
            DisplayPlWailt();
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                Application.DoEvents();
                if (row.Cells["STATENAME"].Value.ToString() == "执行中")
                {
                    wesBillID = row.Cells["BILLMASTERID"].Value.ToString();
                    wmsBillID = row.Cells["BILLID"].Value.ToString();
                    wesDetailID = row.Cells["DETAILID"].Value.ToString();
                    operateStorageName = row.Cells["STORAGENAME"].Value.ToString();
                    operateType = row.Cells["OPERATECODE"].Value.ToString();
                    operateTobaccoName = row.Cells["TOBACCONAME"].Value.ToString();
                    piece = Convert.ToInt32(row.Cells["OPERATEPIECE"].Value);
                    item = Convert.ToInt32(row.Cells["OPERATEITEM"].Value);

                    IData dataInterface = WesContext.GetData();
                    ConfirmResult result = null;
                    result = dataInterface.UploadData(billType, wesBillID, wesDetailID, operateType, piece, item, rfid);
                    if (result.IsSuccess)
                        ConfirmTask("3", result.State, result.Desc);
                    else
                    {
                        MessageBox.Show("无法完成作业确认，原因：" + result.Desc, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }
            }
            if (isASC)
                RefreshData("ASC");
            else
                RefreshData("DESC");
            ClosePlWailt();
        }

        //模式切换
        private void btnOpType_Click(object sender, EventArgs e)
        {
            switch (opType)
            {
                case 0:
                    opType = 1;
                    btnOpType.Text = "实时";
                    timer1.Enabled = true;
                    btnSearch.Enabled = false;
                    break;
                case 1:
                    opType = 2;
                    btnOpType.Text = "手工";
                    timer1.Enabled = false;
                    btnSearch.Enabled = true;
                    break;
                case 2:
                    opType = 0;
                    btnOpType.Text = "正常";
                    timer1.Enabled = false;
                    btnSearch.Enabled = true;
                    break;
                default:
                    break;
            }
            if (isASC)
                RefreshData("ASC");
            else
                RefreshData("DESC");
        }
        //实时时钟
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isASC)
                RefreshData("ASC");
            else
                RefreshData("DESC");
        }
        //退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //RFID确认
        private void ConfirmRfid(string message)
        {
            if (DialogResult.Yes == MessageBox.Show(message + "\n请确认以下信息:\n货位:" + operateStorageName + "\n烟名:" + operateTobaccoName
                + "\n数量:" + piece + "件/" + item + "条", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                rfidTag = true;
            }
            else
            {
                rfidTag = false;
            }
        }            
        //状态确认
        private void ConfirmTask(string confirmState, string state, string msg)
        {
            if (InTask)
            {
                if (new ConfigUtil().GetConfig("DeviceType")["Device"] != "0")
                {
                    taskDal.SetTask(wesBillID, wesDetailID, operateStorageName,"",operateType,operateTobaccoName, piece, item, "2");
                }
                billDal.ConfirmTask(wesBillID, wesDetailID, confirmState, piece, item, state, msg, Environment.MachineName);
            }
        }

        public void DisplayPlWailt()
        {
            this.plWailt.Visible = true;
            this.plWailt.Left = (this.dgvMain.Width - this.plWailt.Width) / 2;
            this.plWailt.Top = (this.dgvMain.Height - this.plWailt.Height) / 2;
            this.btnSearch.Enabled = false;
            this.btnApply.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnConfirm.Enabled = false;
            this.btnBatConfirm.Enabled = false;
            this.btnOpType.Enabled = false;
            this.btnExit.Enabled = false;
            this.InTask = true;
        }

        public void ClosePlWailt()
        {
            this.plWailt.Visible = false;
            this.btnSearch.Enabled = true;
            this.btnExit.Enabled = true ;
        }
    }
}

