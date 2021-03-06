using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using THOK.PDA.Util;
using THOK.PDA.Dal;

namespace THOK.PDA.View
{
    public partial class MainForm : Form
    {     
        public MainForm()
        {
            InitializeComponent();
            SystemCache.MainFrom = this;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void btnParamenter_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                ParameterForm parameterFrom = new ParameterForm();
                parameterFrom.Show();
                this.Visible = false;
            }
            catch (Exception)
            {
                WaitCursor.Restore();               
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            this.ReadMasterBill("1");
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.ReadMasterBill("2");
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            this.ReadMasterBill("3");
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            this.ReadMasterBill("4");
        }

        private void ReadMasterBill(string billType)
        {
            try
            {
                WaitCursor.Set();
                if (SystemCache.ConnetionType == "USB连接")
                {
                    new XMLBillDal().ReadBill();
                    if (SystemCache.MasterTable == null)
                    {
                        MessageBox.Show("没有需操作的数据!!");
                        WaitCursor.Restore();
                        return;
                    }
                }
                BillMasterForm from = new BillMasterForm(billType);
                WaitCursor.Restore();
                from.Show();
                this.Visible = false;                
            }
            catch (Exception ex)
            {
                WaitCursor.Restore();
                WriteLoggerFile("获取数据出错!" + ex.Message + ex.StackTrace + ex.ToString());
                MessageBox.Show("获取数据出错!" + ex.Message + ex.StackTrace + ex.ToString());
            }            
        }

        private void CreateDirectory(string directoryName)
        {
            if (!System.IO.Directory.Exists(directoryName))
                System.IO.Directory.CreateDirectory(directoryName);
        }

        private void WriteLoggerFile(string text)
        {
            try
            {
                string path = "";
                CreateDirectory("日志");
                path = "日志";
                path = path + @"/" + DateTime.Now.ToString().Substring(0, 4).Trim();
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToString().Substring(0, 7).Trim();
                path = path.TrimEnd(new char[] { '-' });
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToShortDateString() + ".txt";
                System.IO.File.AppendText(path).WriteLine(string.Format("{0} {1}", DateTime.Now, text));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}