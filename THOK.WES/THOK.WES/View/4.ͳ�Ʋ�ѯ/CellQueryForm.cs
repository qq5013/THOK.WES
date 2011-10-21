using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;
using THOK.Util;

namespace THOK.WES.View
{
    public partial class CellQueryForm : THOK.AF.View.ToolbarForm
    {
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();

        private DataTable cellTable = null;
        private ShelfDal shelfDal = new ShelfDal();
        private bool needDraw = false;
        private bool filtered = false;

        private int columns = 21;
        private int rows = 3;
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[8];
        private int left = 5;
        
        public CellQueryForm()
        {
            InitializeComponent();

            //����˫����
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);

            THOKUtil.EnableFilter(dgvMain);            

            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;

            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);
        }
       
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Filter.Trim().Length != 0)
                {
                    DialogResult result = MessageBox.Show("���¶���������ѡ��'��(Y)',�������������ѡ��'��(N)'", "ѯ��", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.No:
                            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                            return;
                        case DialogResult.Cancel:
                            return;
                    }
                }

                btnRefresh.Enabled = false;
                btnChart.Enabled = false;

                pnlProgress.Top = (pnlMain.Height - pnlProgress.Height) / 3;
                pnlProgress.Left = (pnlMain.Width - pnlProgress.Width) / 2;
                pnlProgress.Visible = true;
                Application.DoEvents();

                cellTable = shelfDal.GetShelf();
                bsMain.DataSource = cellTable;

                pnlProgress.Visible = false;
                btnRefresh.Enabled = true;
                btnChart.Enabled = true;
            }
            catch (Exception exp)
            {
                THOKUtil.ShowInfo("��������ʧ�ܣ�ԭ��" + exp.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (cellTable != null && cellTable.Rows.Count != 0)
            {
                if (pnlData.Visible)
                {
                    filtered = bsMain.Filter != null;
                    needDraw = true;
                    btnRefresh.Enabled = false;
                    pnlData.Visible = false;
                    pnlChart.Visible = true;
                    btnChart.Text = "�б�";
                    progressBar1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = false;
                    button4.Visible = true;
                    button5.Visible = true;
                   
                    this.label2.Visible = true;
                    this.label3.Visible = true;
                }
                else
                {
                    needDraw = false;
                    btnRefresh.Enabled = true;
                    pnlData.Visible = true;
                    pnlChart.Visible = false;
                    btnChart.Text = "ͼ��";
                    progressBar1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    this.label3.Visible = false;
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Visible = false;
                    button4.Visible = false;
                    button5.Visible = false;
                }
            }
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            if (needDraw)
            {
                Font font = new Font("����", 9);
                SizeF size = e.Graphics.MeasureString("��1��", font);
                float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                size = e.Graphics.MeasureString("13", font);
                float adjustWidth = (cellWidth - size.Width) / 2;

                int qu = shelfDal.GetSumQuantity();
                
                if (qu >= 34680)
                {
                    this.label1.Visible = true;
                    this.label3.Visible = false;
                }
                else
                {
                    this.label1.Visible = false;
                    this.label3.Visible = false;
                    this.progressBar1.Value = qu;
                }

                label2.Text = "��棺" + qu + "��";
                for (int i = 0; i <= 7; i++)
                {
                    int key = currentPage * 8 - (top.Length - (i + 1));
                    if (!shelf.ContainsKey(key))
                    {
                        DataRow[] rows = cellTable.Select(string.Format("SHELF= '{0}'", key), "CELLCODE");
                        shelf.Add(key, rows);
                    }

                    DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth,e);
                    int tmpLeft = left + columns * cellWidth + 5 + cellWidth;
                    for (int j = 0; j < rows; j++)
                    {
                        string s = string.Format("��{0}�ŵ�{1}��", shelf[key][i]["SHELFNAME"], Convert.ToString(j + 1).PadLeft(2, '0'));
                        e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i]-12 + (j + 1) * cellHeight + adjustHeight);//���ұߵ�����
                    }
                }

                if (filtered)
                {
                    int i = currentPage * top.Length;
                    foreach (DataGridViewRow gridRow in dgvMain.Rows)
                    {
                        DataRowView cellRow = (DataRowView)gridRow.DataBoundItem;
                        int shelf = Convert.ToInt32(cellRow["SHELF"]);
                        int column = Convert.ToInt32(cellRow["CELLCOLUMN"]) - 1;
                        int row = Convert.ToInt32(cellRow["CELLROW"]);
                        int quantity = Convert.ToInt32(cellRow["QUANTITY"]);
                        string storagename = cellRow["CELLNAME"].ToString();
                        DateTime outDate = Convert.ToDateTime(cellRow["INDATE"]);
                        int topa = 0;
                        if (shelf <= i)
                        {
                            if (currentPage == 1)
                            {
                                topa = top[shelf - 1];
                                FillCell(e.Graphics, topa, row, column, quantity, storagename, e, outDate);
                            }
                            else if (currentPage == 2)
                            {
                                if (shelf >= 9)
                                {
                                    topa = top[shelf - 9];
                                    FillCell(e.Graphics, topa, row, column, quantity, storagename, e, outDate);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth, PaintEventArgs e)
        {
            int z = 0;
            for (int j = 0; j < columns; j++)
            {
                if (j + 1 == 19)
                {
                    z = cellWidth;//�ճ�������������
                }
                g.DrawString(Convert.ToString(j + 1), font, Brushes.DarkCyan, left + j * cellWidth + adjustWidth + z, top);//�����������
            }
            foreach (DataRow cellRow in cellRows)
            {
                int column = Convert.ToInt32(cellRow["CELLCOLUMN"]) - 1;
                int row = Convert.ToInt32(cellRow["CELLROW"]);
                int quantity = Convert.ToInt32(cellRow["QUANTITY"]);
                string storagename = cellRow["CELLNAME"].ToString();
                DateTime outDate = Convert.ToDateTime(cellRow["INDATE"]);

                int x = left + column * cellWidth;
                int y = top + row * cellHeight-7;
                if (column >= 18)
                    x = x + cellWidth;//�ճ�������������
                g.DrawRectangle(Pens.Blue, new Rectangle(x, y , cellWidth, cellHeight));//����λ�߿�,y ����ɵ����߿�
                
                if (!filtered)
                    FillCell(g, top, row, column, quantity, storagename, e, outDate);
            }
        }

        private void FillCell(Graphics g, int top, int row, int column, int quantity, string storAgeName, PaintEventArgs e, DateTime outDate)
        {
            int x = left + column * cellWidth;
            int y = top + row * cellHeight-5;
           
            if (column >= 18)
                x = x + cellWidth;
            if (quantity >= 30)
                g.FillRectangle(Brushes.Gold, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//�������̻�λ
            else if (quantity > 0)
                g.FillRectangle(Brushes.RoyalBlue, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//�������̻�λ
            else if (quantity <= 0)
                g.FillRectangle(Brushes.White, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//���ջ�λ

            TimeSpan timeSpan = DateTime.Now - outDate;
            int day =timeSpan.Days;
            if (day > 180)
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y , cellWidth - 3, cellHeight - 4));//��Ԥ����Ϣ
        }

        private void pnlChart_Resize(object sender, EventArgs e)
        {
            cellWidth = (pnlContent.Width - 90 - sbShelf.Width - 20) / columns;
            cellHeight = ((pnlContent.Height / 8) / rows) - 7;
            
            top[0] = 0;
            for (int i = 1; i < top.Length; i++)
            {
                top[i] = pnlContent.Height / top.Length * i;
            }
        }

        private void pnlChart_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            for (int j = 0; j < top.Length; j++)
            {
                if (e.Y > top[top.Length-1])
                {
                    i = 7; break;
                }
                if (e.Y < top[j + 1])
                {
                    i = j;
                    break;
                }
            }
            int shelf = 0;
            if (currentPage == 1)
                shelf = i + 1;
            else if (currentPage == 2)
                shelf = i + top.Length+1;
            int column = (e.X - left) / cellWidth + 1;
            if (column == 19)
                return;
            if (column >= 20)
                column = column - 1;
            int row = (e.Y - (top[i]) +7) / cellHeight;

            if (column <= columns+1 && row <= rows)
            {
                DataRow[] cellRows = cellTable.Select(string.Format("SHELF='{0}' AND CELLCOLUMN='{1}' AND CELLROW='{2}'", shelf, column, row));
                if (cellRows.Length != 0)
                {
                    Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
                    Dictionary<string, object> property = new Dictionary<string, object>();

                    //wareHouse wh = new wareHouse();
                    //wh.ProductCode = cellRows[0]["CURRENTPRODUCT"].ToString();
                    //wh.ProductName = cellRows[0]["PRODUCTNAME"].ToString();
                    //wh.Quantity = cellRows[0]["QUANTITY"] + "����";
                    //wh.QuantityBar = cellRows[0]["QUT_TIAO"] + "����";
                    //wh.InDate = cellRows[0]["INDATE"].ToString();

                    //wh.WhCode = cellRows[0]["WH_CODE"].ToString();
                    //wh.WhName = cellRows[0]["WH_NAME"].ToString();
                    //wh.ShelfName = "��" + cellRows[0]["SHELFNAME"] + "�Ż���";
                    //wh.CellCode = cellRows[0]["CELLCODE"].ToString();
                    //wh.CellName = cellRows[0]["CELLNAME"].ToString();
                    //wh.Column = "��" + column + "��";
                    //wh.Row = "��" + row + "��";

                    property.Add("���̱���", cellRows[0]["CURRENTPRODUCT"]);
                    property.Add("��������", cellRows[0]["PRODUCTNAME"]);
                    property.Add("���̼���", cellRows[0]["QUANTITY"] + "����");
                    property.Add("��������", cellRows[0]["QUT_TIAO"] + "����");
                    property.Add("���ʱ��", cellRows[0]["INDATE"]);                    
                    properties.Add("��Ʒ��Ϣ", property);

                    property = new Dictionary<string, object>();
                    property.Add("�ֿ����", cellRows[0]["WH_CODE"]);
                    property.Add("�ֿ�����", cellRows[0]["WH_NAME"]);
                    property.Add("��������", "��" + cellRows[0]["SHELFNAME"] + "�Ż���");
                    property.Add("��λ����", cellRows[0]["CELLCODE"]);
                    property.Add("��λ����", cellRows[0]["CELLNAME"]);
                    property.Add("��", "��" + column + "��");
                    property.Add("��", "��" + row + "��");
                    property.Add("�Ƿ����", cellRows[0]["ACTIVEIS"]);
                    properties.Add("�ֿ���Ϣ", property);

                    CellDialog cellDialog = new CellDialog(properties);
                    cellDialog.ShowDialog();
                }
            }
        }

        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && currentPage + 1 <= 3)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos != currentPage)
            {
                currentPage = pos;
                pnlChart.Invalidate();
            }
        }
    }
}