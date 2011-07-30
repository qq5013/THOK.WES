using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using OpenNETCF.Desktop.Communication;
using THOK.Util;
using THOK.WES.Dao;
using THOK.WES.Interface;

namespace THOK.WES.Dal
{
    public class BillDal
    {
        private string exportFileName = "ExportBill.xml";
        private ConfigUtil configUtil = new ConfigUtil();

        //@
        public DataTable GetBillMaster(string billType)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindBillMaster(billType);
            }
        }

        //@
        public DataTable GetBillDetail(string billID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindBillDetail(billID);
            }
        }

        //@
        public void SaveMasterState(string state, string billID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.UpdateMasterState(state, billID);
            }
        }

        //@
        public int GetTaskedCount(string billID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindTaskedCount(billID);
            }
        }

        //@
        public DataTable GetTaskingMaster(string billType)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindTaskingMaster(billType);
            }
        }

        //@
        public DataTable GetTaskDetail(string billID, string orderby, int opType)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                string layersNumber = configUtil.GetConfig("Layers")["Number"];
                BillDao billDao = new BillDao();
                return billDao.FindTaskDetail(billID, layersNumber, orderby, opType);
            }
        }

        //@
        public bool ApplyTask(string billID, string detailID, int stockInBatchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.SetPersistentManager(pm);
                DataRow detailRow = billDao.FindDetailByBillIDAnddetailID(billID, detailID);
                if (detailRow["ConfirmState"].ToString() != "1")
                {
                    return false;
                }
                if (stockInBatchNo != 0)
                {
                    StockInDao stockInDao = new StockInDao();
                    if (stockInDao.FindStockInTask(stockInBatchNo) != 1)
                    {
                        return false;
                    }
                    stockInDao.UpdateState(stockInBatchNo);
                }
                billDao.ApplyDetailByBillIDAnddetailID(billID, detailID);
                return true;
            }
        }

        //@
        public void ConfirmTask(string billID, string detailID, string confirmState, int piece, int item, string state, string msg, string operater)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.ConfirmTask(billID, detailID, confirmState, piece, item, state, msg, operater);
            }
        }

        /// <summary>
        /// @查找零烟柜订单
        /// </summary>
        /// <returns>零烟柜列表</returns>
        public DataTable GetRetailMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindRetailMaster();
            }
        }

        //@
        public DataTable FindBillDetail(string billID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.GetBillDetail(billID);
            }
        }

        /// <summary>
        /// @获取订单类型
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns>订单类型</returns>
        public string GetBillType(string id)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                return new BillDao().GetBill(id).Rows[0]["BillCode"].ToString();
            }
        }

        //@
        public void UpdateDetailListState(string billID, string state, string operatorName)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                new BillDao().UpdateDetailListState(billID, state, operatorName);
            }
        }

        //@
        public void GetUpdateBillDetailFewColum(string state, string msg, string operators, string Masterid)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.UpdateBillDetailFewColum(state, msg, operators, Masterid);
            }
        }

        //@
        public DataTable GetBillDetailMasterId(string Masterid)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindBillDetailMasterId(Masterid);
            }
        }

        //@
        public DataTable GetAllBill()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.GetAllBill();
            }
        }
        //@
        public void ExportBill(IList<string> billIdList)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao dao = new BillDao();
                dao.SetPersistentManager(pm);
                foreach (string billId in billIdList)
                {
                    this.WriteXML(dao.GetBillDetailByDetailId(billId));
                }
            }

            HHFiles myHH = new HHFiles();
            myHH.CopyFileToDevice(exportFileName, @"\DiskOnChip\" + exportFileName, true);
            myHH.Rapi.Disconnect();
            File.Delete(exportFileName);
        }

        //@
        public void WriteXML(DataTable table)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = null;
            if (File.Exists(exportFileName))
            {
                doc.Load(exportFileName);
                root = doc.DocumentElement;
            }
            else
            {
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "GB2312", ""));
                root = doc.CreateElement("Bill");
                doc.AppendChild(root);
            }

            if (table.Rows.Count > 0)
            {
                XmlElement node = doc.CreateElement("billInfo");
                node.SetAttribute("masteId", table.Rows[0]["MASTER"].ToString());
                node.SetAttribute("billType", table.Rows[0]["BillCode"].ToString());
                node.SetAttribute("billTypeName", table.Rows[0]["BillName"].ToString());
                root.AppendChild(node);

                foreach (DataRow row in table.Rows)
                {
                    XmlElement childNode = doc.CreateElement("detailInfo");

                    foreach (DataColumn column in table.Columns)
                    {
                        childNode.SetAttribute(column.ColumnName, row[column.ColumnName].ToString());
                    }

                    node.AppendChild(childNode);
                }
            }
            doc.Save(exportFileName);
        }

        //@
        public void SynchronizationBill()
        {
            string remoteFileName = @"\DiskOnChip\" + exportFileName;
            string LocalFileName = exportFileName;

            HHFiles myHH = new HHFiles();
            myHH.CopyFileFromDevice(LocalFileName, remoteFileName, true);
            myHH.DeleteFile(remoteFileName);
            myHH.Rapi.Disconnect();

            if (!File.Exists(LocalFileName))
            {
                return;
            }

            using (PersistentManager pm = new PersistentManager())
            {
                PDAUploadDataDao dao = new PDAUploadDataDao();
                dao.SetPersistentManager(pm);
                DataTable databaseTable = dao.GetDataList();
                DataTable insertTable = databaseTable.Clone();

                XmlDocument doc = new XmlDocument();
                doc.Load(LocalFileName);
                XmlElement root = doc.DocumentElement;

                if (root.ChildNodes.Count == 0)
                {
                    return;
                }

                XmlNodeList nodeList = doc.SelectNodes(@"/Bill/billInfo/detailInfo[@ConfirmState='3']");

                foreach (XmlElement node in nodeList)
                {
                    string masterId = node.GetAttribute("MASTER");
                    string detailId = node.GetAttribute("DETAILID");
                    if ((databaseTable.Select("BillMasterId = '" + masterId + "'AND  BillDetailId ='" + detailId + "'").Length == 0) &&
                         (insertTable.Select("BillMasterId = '" + masterId + "'AND BillDetailId ='" + detailId + "'").Length == 0))
                    {
                        DataRow row = insertTable.NewRow();
                        row["BillMasterId"] = masterId;
                        row["BillDetailId"] = detailId;
                        row["IsUpload"] = 0;
                        insertTable.Rows.Add(row);

                        if (false)//todo 未完成PDA修改
                        {
                            int piece = Convert.ToInt32(node.GetAttribute("OPERATEPIECE"));
                            int item = Convert.ToInt32(node.GetAttribute("OPERATEITEM"));
                            BillDao billDao = new BillDao();
                            billDao.ConfirmTask(masterId, detailId, "3", piece, item, "1", "操作成功", "PDA");
                        }
                    }
                }

                dao.InsertData(insertTable);
                IList<string> list = new List<string>();
                foreach (XmlElement billNode in root.ChildNodes)
                {
                    list.Add(billNode.GetAttribute("masteId"));
                }
                File.Delete(LocalFileName);
                this.UploadData(this.GetUploadData(), true);
                this.ExportBill(list);
            }
        }

        //@
        public DataTable GetUploadData()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.GetUploadData();
            }
        }

        //@
        public void UploadData(DataTable table, bool isConfirmation)
        {
            IData dataInterface = WesContext.GetData();
            ConfirmResult result = null;
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.SetPersistentManager(pm);
                foreach (DataRow row in table.Rows)
                {
                    string wesBillID = row["BILLMASTERID"].ToString();
                    string billType = billDao.GetBill(wesBillID).Rows[0]["BILLCODE"].ToString();
                    string billID = row["BILLID"].ToString();
                    string detailID =  row["DETAILID"].ToString();
                    string operateCode = row["OPERATECODE"].ToString();
                    int piece = Convert.ToInt32(row["PIECE"].ToString());
                    int item = Convert.ToInt32(row["ITEM"].ToString());
                    result = dataInterface.UploadData(billType, wesBillID, detailID, operateCode, piece, item, "");

                    if (result.IsSuccess)
                    {
                        if (isConfirmation)
                        {
                            billDao.ConfirmTask(wesBillID, detailID, "3",piece,item, "1", "操作成功", "PDA");
                        }
                        billDao.DeleteUploadData(row["ID"].ToString());
                    }
                }
            }
        }   

        //@
        public DataTable GetBillMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                return billDao.FindBillMaster();
            }
        }      
    }
}
