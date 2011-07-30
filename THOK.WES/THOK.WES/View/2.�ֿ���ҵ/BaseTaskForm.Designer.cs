namespace THOK.WES.View
{
    partial class BaseTaskForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.sslBillID = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslOperator = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.BILLID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DETAILID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STORAGENAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPERATENAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOBACCONAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPERATEPIECE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPERATEITEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATENAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TARGETSTORAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPERATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BILLMASTERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBatConfirm = new System.Windows.Forms.Button();
            this.btnOpType = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.plWailt = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.plWailt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnOpType);
            this.pnlTool.Controls.Add(this.btnBatConfirm);
            this.pnlTool.Controls.Add(this.btnConfirm);
            this.pnlTool.Controls.Add(this.btnCancel);
            this.pnlTool.Controls.Add(this.btnApply);
            this.pnlTool.Controls.Add(this.btnSearch);
            this.pnlTool.Size = new System.Drawing.Size(804, 46);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.plWailt);
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Controls.Add(this.ssMain);
            this.pnlContent.Location = new System.Drawing.Point(0, 46);
            this.pnlContent.Size = new System.Drawing.Size(804, 162);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(804, 208);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Image = global::THOK.WES.Properties.Resources.accept;
            this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfirm.Location = new System.Drawing.Point(144, 0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(48, 44);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "完成";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Enabled = false;
            this.btnCancel.Image = global::THOK.WES.Properties.Resources.onebit_24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(96, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 44);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnApply.Enabled = false;
            this.btnApply.Image = global::THOK.WES.Properties.Resources.onebit_23;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.Location = new System.Drawing.Point(48, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(48, 44);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "申请";
            this.btnApply.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearch.Image = global::THOK.WES.Properties.Resources.onebit_02;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 44);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslBillID,
            this.sslOperator});
            this.ssMain.Location = new System.Drawing.Point(0, 140);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(804, 22);
            this.ssMain.TabIndex = 0;
            this.ssMain.Text = "statusStrip1";
            // 
            // sslBillID
            // 
            this.sslBillID.Name = "sslBillID";
            this.sslBillID.Size = new System.Drawing.Size(53, 17);
            this.sslBillID.Text = "单据号：";
            // 
            // sslOperator
            // 
            this.sslOperator.Name = "sslOperator";
            this.sslOperator.Size = new System.Drawing.Size(53, 17);
            this.sslOperator.Text = "操作员：";
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeight = 22;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BILLID,
            this.DETAILID,
            this.STORAGENAME,
            this.OPERATENAME,
            this.TOBACCONAME,
            this.OPERATEPIECE,
            this.OPERATEITEM,
            this.STATENAME,
            this.Column17,
            this.TARGETSTORAGE,
            this.OPERATECODE,
            this.Column4,
            this.BILLMASTERID});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMain.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(804, 140);
            this.dgvMain.TabIndex = 1;
            // 
            // BILLID
            // 
            this.BILLID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BILLID.DataPropertyName = "BILLID";
            this.BILLID.HeaderText = "订单编号";
            this.BILLID.Name = "BILLID";
            this.BILLID.ReadOnly = true;
            this.BILLID.Visible = false;
            // 
            // DETAILID
            // 
            this.DETAILID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DETAILID.DataPropertyName = "DETAILID";
            this.DETAILID.HeaderText = "明细编号";
            this.DETAILID.Name = "DETAILID";
            this.DETAILID.ReadOnly = true;
            this.DETAILID.Visible = false;
            this.DETAILID.Width = 80;
            // 
            // STORAGENAME
            // 
            this.STORAGENAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.STORAGENAME.DataPropertyName = "STORAGEID";
            this.STORAGENAME.HeaderText = "作业储位";
            this.STORAGENAME.Name = "STORAGENAME";
            this.STORAGENAME.ReadOnly = true;
            this.STORAGENAME.Width = 135;
            // 
            // OPERATENAME
            // 
            this.OPERATENAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OPERATENAME.DataPropertyName = "OPERATENAME";
            this.OPERATENAME.HeaderText = "类型";
            this.OPERATENAME.Name = "OPERATENAME";
            this.OPERATENAME.ReadOnly = true;
            this.OPERATENAME.Width = 60;
            // 
            // TOBACCONAME
            // 
            this.TOBACCONAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TOBACCONAME.DataPropertyName = "TOBACCONAME";
            this.TOBACCONAME.HeaderText = "卷烟名称";
            this.TOBACCONAME.Name = "TOBACCONAME";
            this.TOBACCONAME.ReadOnly = true;
            this.TOBACCONAME.Width = 210;
            // 
            // OPERATEPIECE
            // 
            this.OPERATEPIECE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OPERATEPIECE.DataPropertyName = "OPERATEPIECE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.OPERATEPIECE.DefaultCellStyle = dataGridViewCellStyle3;
            this.OPERATEPIECE.HeaderText = "件数";
            this.OPERATEPIECE.Name = "OPERATEPIECE";
            this.OPERATEPIECE.ReadOnly = true;
            this.OPERATEPIECE.Width = 60;
            // 
            // OPERATEITEM
            // 
            this.OPERATEITEM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OPERATEITEM.DataPropertyName = "OPERATEITEM";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.OPERATEITEM.DefaultCellStyle = dataGridViewCellStyle4;
            this.OPERATEITEM.HeaderText = "条数";
            this.OPERATEITEM.Name = "OPERATEITEM";
            this.OPERATEITEM.ReadOnly = true;
            this.OPERATEITEM.Width = 60;
            // 
            // STATENAME
            // 
            this.STATENAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.STATENAME.DataPropertyName = "STATENAME";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.STATENAME.DefaultCellStyle = dataGridViewCellStyle5;
            this.STATENAME.HeaderText = "状态";
            this.STATENAME.Name = "STATENAME";
            this.STATENAME.ReadOnly = true;
            this.STATENAME.Width = 80;
            // 
            // Column17
            // 
            this.Column17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column17.DataPropertyName = "OPERATOR";
            this.Column17.HeaderText = "操作员";
            this.Column17.Name = "Column17";
            this.Column17.ReadOnly = true;
            this.Column17.Width = 70;
            // 
            // TARGETSTORAGE
            // 
            this.TARGETSTORAGE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TARGETSTORAGE.DataPropertyName = "TARGETSTORAGE";
            this.TARGETSTORAGE.HeaderText = "目标储位";
            this.TARGETSTORAGE.Name = "TARGETSTORAGE";
            this.TARGETSTORAGE.ReadOnly = true;
            this.TARGETSTORAGE.Visible = false;
            // 
            // OPERATECODE
            // 
            this.OPERATECODE.DataPropertyName = "OPERATECODE";
            this.OPERATECODE.HeaderText = "Column3";
            this.OPERATECODE.Name = "OPERATECODE";
            this.OPERATECODE.ReadOnly = true;
            this.OPERATECODE.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "BATCHNO";
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // BILLMASTERID
            // 
            this.BILLMASTERID.DataPropertyName = "BILLMASTERID";
            this.BILLMASTERID.HeaderText = "Column5";
            this.BILLMASTERID.Name = "BILLMASTERID";
            this.BILLMASTERID.ReadOnly = true;
            this.BILLMASTERID.Visible = false;
            // 
            // btnBatConfirm
            // 
            this.btnBatConfirm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBatConfirm.Enabled = false;
            this.btnBatConfirm.Image = global::THOK.WES.Properties.Resources.accept;
            this.btnBatConfirm.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBatConfirm.Location = new System.Drawing.Point(192, 0);
            this.btnBatConfirm.Name = "btnBatConfirm";
            this.btnBatConfirm.Size = new System.Drawing.Size(48, 44);
            this.btnBatConfirm.TabIndex = 10;
            this.btnBatConfirm.Text = "批量";
            this.btnBatConfirm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBatConfirm.UseVisualStyleBackColor = true;
            this.btnBatConfirm.Click += new System.EventHandler(this.btnBatConfirm_Click);
            // 
            // btnOpType
            // 
            this.btnOpType.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOpType.Image = global::THOK.WES.Properties.Resources.onebit_10;
            this.btnOpType.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOpType.Location = new System.Drawing.Point(240, 0);
            this.btnOpType.Name = "btnOpType";
            this.btnOpType.Size = new System.Drawing.Size(48, 44);
            this.btnOpType.TabIndex = 12;
            this.btnOpType.Text = "正常";
            this.btnOpType.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOpType.UseVisualStyleBackColor = true;
            this.btnOpType.Visible = false;
            this.btnOpType.Click += new System.EventHandler(this.btnOpType_Click);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExit.Image = global::THOK.WES.Properties.Resources.shut_down;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(288, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 44);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // plWailt
            // 
            this.plWailt.Controls.Add(this.label1);
            this.plWailt.Controls.Add(this.pictureBox1);
            this.plWailt.Location = new System.Drawing.Point(273, 39);
            this.plWailt.Name = "plWailt";
            this.plWailt.Size = new System.Drawing.Size(258, 85);
            this.plWailt.TabIndex = 2;
            this.plWailt.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "正在处理数据，请稍等";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::THOK.WES.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(158, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 38);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // BaseTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(804, 208);
            this.Name = "BaseTaskForm";
            this.Text = "盘点作业";
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.plWailt.ResumeLayout(false);
            this.plWailt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        protected System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel sslBillID;
        private System.Windows.Forms.ToolStripStatusLabel sslOperator;
        protected System.Windows.Forms.Timer timer1;
        protected System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.Button btnExit;
        protected System.Windows.Forms.Button btnOpType;
        protected System.Windows.Forms.Button btnBatConfirm;
        private System.Windows.Forms.Panel plWailt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn BILLID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DETAILID;
        private System.Windows.Forms.DataGridViewTextBoxColumn STORAGENAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPERATENAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOBACCONAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPERATEPIECE;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPERATEITEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATENAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn TARGETSTORAGE;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPERATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn BILLMASTERID;
    }
}
