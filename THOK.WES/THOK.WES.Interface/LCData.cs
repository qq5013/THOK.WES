using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.Interface.Dal;
using System.Net;
using System.Data;
using THOK.WES.Interface.Util;
using System.Windows.Forms;
using System.Xml;

namespace THOK.WES.Interface
{
    class LCData : IData
    {
        private string applyURL = "";
        private string confirmURL = "";
        private MessageDal messageDao = new MessageDal();
        private BillDal billDal = new BillDal();

        public LCData()
        {
            applyURL = WesContext.Parameters["ApplyURL"] + "?billtype={0}";
            confirmURL = WesContext.Parameters["ConfirmURL"] + "?billtype={0}&billid={1}&detailid={2}& OperateType ={3}& Piece={4}&item={5}&rfid={6}";
        }

        #region IData 成员

        public bool ImportData(string billType)
        {
            bool result = false;

            //下载货位！
            StorageDal storageDal = new StorageDal();
            storageDal.GetCellTable();

            try
            {
                applyURL = WesContext.Parameters["ApplyURL"] + "?billtype={0}";
                applyURL = string.Format(applyURL, billType);
                DateTime requestTime = DateTime.Now;
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(applyURL);
                string xml = System.Text.Encoding.GetEncoding("GB2312").GetString(data);
                DateTime responseTime = DateTime.Now;
                messageDao.AddMessage(requestTime, applyURL, responseTime, xml, Environment.MachineName);
                DataSet ds = TableUtil.GenerateEmptyTables();
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
            ConfirmResult result = new ConfirmResult(false, "2", "确认失败!");
            string wmsBillID = billID.Remove(billID.Length - 1);

            confirmURL = WesContext.Parameters["ConfirmURL"] + "?billtype={0}&billid={1}&detailid={2}& OperateType ={3}& Piece={4}&item={5}&rfid={6}";
            confirmURL = string.Format(confirmURL, billType, wmsBillID, detailID, operateType, piece, item, rfid);
            DateTime requestTime = DateTime.Now;
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(confirmURL);
            string xml = System.Text.Encoding.GetEncoding("GB2312").GetString(data);
            DateTime responseTime = DateTime.Now;
            messageDao.AddMessage(requestTime, confirmURL, responseTime, xml, Environment.MachineName);
            result = ParseMsg(xml);
            if (result.IsSuccess)
            {
                billDal.ConfirmTask(billID, detailID, "3", piece, item, result.State, result.Desc, Environment.MachineName);
            }   
            return result;
        }

        #endregion

        private bool ParseData(string xml, DataSet ds)
        {
            bool isSuccess = true;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                if (isSuccess)
                {
                    foreach (XmlNode billNode in doc.GetElementsByTagName("billinfo"))
                    {
                        string billID = billNode.Attributes["BILLID"].InnerText;
                        DataRow masterRow = ds.Tables["master"].NewRow();                        
                        masterRow["BILLID"] = billID;
                        masterRow["BIILLCODE"] = billNode.Attributes["BILLTYPE"].InnerText;
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

                            detailRow["OPERATEITEM"] = Convert.ToInt32(Convert.ToDouble(detailNode.Attributes["OPERATEITEM"].InnerText));
                            detailRow["ITEM"] = Convert.ToInt32(Convert.ToDouble(detailNode.Attributes["ITEM"].InnerText));
                            
                            detailRow["CONFIRMSTATE"] = "1";
                            detailRow["TARGETSTORAGE"] = detailNode.Attributes["MOVE_CELL_ID"].InnerText;
                            ds.Tables["DETAIL"].Rows.Add(detailRow);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("浪潮接口，消息解析出错，原因：" + e.Message);
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
                MessageBox.Show("浪潮接口，消息解析出错，原因：" + e.Message);
            }
            return result;
        }
    }
}
