namespace THOK.WES.View
{
    partial class MoveTaskForm
    {
        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        /// <param name="disposing">���Ӧ�ͷ��й���Դ��Ϊ true������Ϊ false��</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���

        /// <summary>
        /// �����֧������ķ��� - ��Ҫ
        /// ʹ�ô���༭���޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Size = new System.Drawing.Size(699, 46);
            // 
            // pnlContent
            // 
            this.pnlContent.Size = new System.Drawing.Size(699, 504);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(699, 550);
            // 
            // MoveTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(699, 550);
            this.Name = "MoveTaskForm";
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
