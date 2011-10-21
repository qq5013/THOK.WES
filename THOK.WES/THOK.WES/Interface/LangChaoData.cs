using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Net;
using THOK.WES.Dal;

namespace THOK.WES
{
    public class LangChaoData: IData
    {
        private string applyURL = "";
        private string confirmURL = "";
        private MessageDal messageDao = new MessageDal();
        //private TaskDal taskDal = new TaskDal();
        //private StorageDal storageDal = new StorageDal();

        public LangChaoData()
        {
            applyURL = WesContext.Parameters["ApplyURL"] + "?billtype={0}";
            confirmURL = WesContext.Parameters["ConfirmURL"] + "?billtype={0}&billid={1}&detailid={2}& OperateType ={3}& Piece={4}&item={5}&rfid={6}";
        }
        
        public bool ImportData(string billType)
        {
            bool result = false;
            try
            {
                applyURL = WesContext.Parameters["ApplyURL"] + "?billtype={0}";
                applyURL = string.Format(applyURL, billType);
                DateTime requestTime = DateTime.Now;
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(applyURL);
                string xml = System.Text.Encoding.GetEncoding("GB2312").GetString(data);
                DateTime responseTime = DateTime.Now;
                // string xml = "<?xml version=\"1.0\" encoding=\"GB2312\"?><message><issuccess text=\"true\"></issuccess><billinfo billtype=\"1\" billid=\"201004010001\"  quay=\"1\"><billdetail OrderDetailID=\"1\" StorageID=\"1\" OperateType=\"1\" TobaccoName=\"测试卷烟\" OperatePiece=\"30\" Piece=\"0\" OperateItem=\"0\" Item=\"0\" TagID=\"0\" ConfirmState=\"1\"/></billinfo></message>";
                messageDao.AddMessage(requestTime, applyURL, responseTime, xml, Environment.MachineName);
                DataSet ds = GenerateEmptyTables();
                if (ParseData(xml, ds))
                {
                    new SplitBillDal().AddOrder(ds);

                    result = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("从浪潮下载数据出错，原因：" + e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {

          

            confirmURL = WesContext.Parameters["ConfirmURL"] + "?billtype={0}&billid={1}&detailid={2}& OperateType ={3}& Piece={4}&item={5}&rfid={6}";
            confirmURL = string.Format(confirmURL, billType, billID, detailID, operateType, piece, item, rfid);
            DateTime requestTime = DateTime.Now;
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(confirmURL);
            string xml = System.Text.Encoding.GetEncoding("GB2312").GetString(data);
            DateTime responseTime = DateTime.Now;
           // string xml = "<?xml version=\"1.0\" encoding=\"GB2312\"?><message><issuccess text=\"true\"></issuccess><msg text=\"1\" desc=\"单据状态不对不能执行操作\"></msg></message>";
            
            messageDao.AddMessage(requestTime, confirmURL, responseTime, xml, Environment.MachineName);
            return ParseMsg(xml);
        }

        private bool ParseData(string xml, DataSet ds)
        {
            bool isSuccess = true;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
  //              isSuccess = Convert.ToBoolean(doc.GetElementsByTagName("ISSUCCESS")[0].Attributes["TEXT"].InnerText);
                if (isSuccess)
                {
                    foreach (XmlNode billNode in doc.GetElementsByTagName("billinfo"))
                    {
                        string billID = billNode.Attributes["BILLID"].InnerText;
                        DataRow masterRow = ds.Tables["master"].NewRow();
                        masterRow["BIILLCODE"] = billNode.Attributes["BILLTYPE"].InnerText;
                        masterRow["BILLID"] = billID;
                        masterRow["QUAY"] = billNode.Attributes["QUAY"].InnerText;
                        masterRow["STATE"] = "4";
                        string dateTime = billNode.Attributes["DATE"].InnerText;
                        dateTime = dateTime.Substring(0, 4) + "-" + dateTime.Substring(4, 2) + "-" + dateTime.Substring(6, 2);
                        masterRow["BILLDATE"] = DateTime.Parse(dateTime);
                        ds.Tables["MASTER"].Rows.Add(masterRow);

                        foreach (XmlNode detailNode in billNode.ChildNodes)
                        {
                            DataRow detailRow = ds.Tables["DETAIL"].NewRow();
                            detailRow["BILLID"] = billID;
                            detailRow["DETAILID"] = detailNode.Attributes["ORDERDETAILID"].InnerText;
                            detailRow["STORAGEID"] = detailNode.Attributes["STORAGENAME"].InnerText;
                            detailRow["OPERATECODE"] = detailNode.Attributes["OPERATETYPE"].InnerText;
                            detailRow["TOBACCONAME"] = detailNode.Attributes["TOBACCONAME"].InnerText;

                            double operatePiece = Convert.ToDouble(detailNode.Attributes["OPERATEPIECE"].InnerText);
                            operatePiece = operatePiece > 1 ? Math.Floor(operatePiece) : 0;
                            detailRow["OPERATEPIECE"] = Convert.ToInt32(operatePiece);

                            double piece = Convert.ToDouble(detailNode.Attributes["PIECE"].InnerText);
                            piece = piece > 1 ? Math.Floor(piece) : 0;
                            detailRow["PIECE"] = Convert.ToInt32(piece);

                            detailRow["OPERATEITEM"] = Convert.ToInt32( Convert.ToDouble( detailNode.Attributes["OPERATEITEM"].InnerText));
                            detailRow["ITEM"] = Convert.ToInt32( Convert.ToDouble( detailNode.Attributes["ITEM"].InnerText));
                            detailRow["TAGID"] = detailNode.Attributes["TAGID"].InnerText;
                            detailRow["MOVESTORAGE"] = detailNode.Attributes["MOVE_CELL_ID"].InnerText;
                           // detailRow["CONFIRMSTATE"] = detailNode.Attributes["CONFIRMSTATE"].InnerText;
                            
                            detailRow["CONFIRMSTATE"] = "1";
                            ds.Tables["DETAIL"].Rows.Add(detailRow);

                           
                        }

                    }
                }
                               
            }
            catch (Exception e)
            {
                MessageBox.Show("消息解析出错，原因：" + e.Message);
            }
            return isSuccess; 
        }







        private ConfirmResult ParseMsg(string xml)
        {
            XmlDocument doc = new XmlDocument();
            ConfirmResult result = null;
            try
            {
                doc.LoadXml(xml);

                bool isSuccess = Convert.ToBoolean(doc.GetElementsByTagName("issuccess")[0].Attributes["text"].InnerText);

                XmlNode node = doc.GetElementsByTagName("msg")[0];

                result = new ConfirmResult(isSuccess, node.Attributes["text"].InnerText, node.Attributes["desc"].InnerText);
            }
            catch (Exception e)
            {
                MessageBox.Show("消息解析出错，原因：" + e.Message);
            }
            return result;
        }

        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable master = ds.Tables.Add("MASTER");
            master.Columns.Add("ID");
            master.Columns.Add("BILLID");
            master.Columns.Add("BIILLCODE");
            master.Columns.Add("QUAY");
            master.Columns.Add("STATE");
            master.Columns.Add("BILLDATE");
            master.Columns.Add("TERMINAL");

            DataTable detail = ds.Tables.Add("DETAIL");
            detail.Columns.Add("ID");
            detail.Columns.Add("BILLID");
            detail.Columns.Add("DETAILID");
            detail.Columns.Add("STORAGEID");
            detail.Columns.Add("OPERATECODE");
            detail.Columns.Add("TOBACCONAME");
            detail.Columns.Add("OPERATEPIECE");
            detail.Columns.Add("PIECE");
            detail.Columns.Add("OPERATEITEM");
            detail.Columns.Add("ITEM");
            detail.Columns.Add("TAGID");
            detail.Columns.Add("CONFIRMSTATE");
            detail.Columns.Add("MOVESTORAGE");
            detail.Columns.Add("STATE");
            detail.Columns.Add("MSG");
            detail.Columns.Add("OPERATOR");
            detail.Columns.Add("MASTER");
            

            return ds;
        }
    }
}
