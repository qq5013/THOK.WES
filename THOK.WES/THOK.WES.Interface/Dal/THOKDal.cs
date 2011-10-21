using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dao;
using THOK.WES.Interface.Util;
using System.Windows.Forms;

namespace THOK.WES.Interface.Dal
{
    public class THOKDal
    {
        #region ��ⵥ

        /// <summary>
        /// ��ⵥ����
        /// </summary>
        /// <returns></returns>
        public bool GetInBill()
        {
            bool tag = true;
            try
            {
                using (PersistentManager wesdbpm = new PersistentManager())
                {
                    using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
                    {
                        BillDao wesBillDao = new BillDao();
                        wesBillDao.SetPersistentManager(wesdbpm);
                        THOKDao wmsTHOKDao = new THOKDao();
                        wmsTHOKDao.SetPersistentManager(wmsdbpm);
                        //��ȡ��ⵥ
                        DataTable inBillTable = wmsTHOKDao.ReadInBill();
                        //�޼�¼��ֱ�ӷ���
                        if (inBillTable.Rows.Count == 0)
                            return true;
                        //��ȡ���ظ���
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(inBillTable, new string[] { "BILLNO" });

                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet tmpdataSet = TableUtil.GenerateEmptyTables();
                            //��ȡ��ͬ��������
                            DataRow[] detailRowList = inBillTable.Select("BILLNO='" + row["BILLNO"].ToString() + "'");

                            DataRow billRow = tmpdataSet.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["BILLNO"];
                            billRow["BILLCODE"] = "1";
                            billRow["STATE"] = "4";
                            billRow["BILLDATE"] = detailRowList[0]["BILLDATE"].ToString();
                            tmpdataSet.Tables["MASTER"].Rows.Add(billRow);

                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = tmpdataSet.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["BILLNO"];
                                newDetailRow["DETAILID"] = detailRow["ID"];
                                newDetailRow["STORAGEID"] = detailRow["CELLNAME"];
                                newDetailRow["OPERATECODE"] = "1";
                                newDetailRow["TOBACCONAME"] = detailRow["PRODUCTNAME"];
                                newDetailRow["OPERATEPIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                                newDetailRow["PIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                                newDetailRow["OPERATEITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                                newDetailRow["ITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = "";
                                tmpdataSet.Tables["DETAIL"].Rows.Add(newDetailRow);

                            }
                            new SplitBillDal().AddOrder(tmpdataSet);
                            wmsTHOKDao.UpdateInBillStatus(row["BILLNO"].ToString());
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                tag = false;
            }
            
            return tag;
        }

        /// <summary>
        /// ��ⵥȷ��
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        /// <param name="inputquantity"></param>
        public void ConfirmInBill(string billID, long billDetailID, string status)
        {
            using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
            {
                THOKDao wmsTHOKDao = new THOKDao();
                wmsTHOKDao.SetPersistentManager(wmsdbpm);
                wmsTHOKDao.ConfirmInBill(billID, billDetailID, status);
            }
        }

        #endregion

        #region ���ⵥ

        /// <summary>
        /// ���ⵥ����
        /// </summary>
        /// <returns></returns>
        public bool GetOutBill()
        {
            bool tag = true;
            using (PersistentManager wesdbpm = new PersistentManager())
            {
                using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
                {
                    BillDao wesBillDao = new BillDao();
                    wesBillDao.SetPersistentManager(wesdbpm);
                    THOKDao wmsTHOKDao = new THOKDao();
                    wmsTHOKDao.SetPersistentManager(wmsdbpm);
                    //��ȡ���ⵥ
                    DataTable outBillTable = wmsTHOKDao.ReadOutBill();
                    //�޼�¼��ֱ�ӷ���
                    if (outBillTable.Rows.Count == 0)
                        return true;
                    //��ȡ���ظ���
                    DataTable distinctTable = TableUtil.GetDataTableDistinct(outBillTable, new string[] {"BILLNO"});

                    foreach (DataRow row in distinctTable.Rows)
                    {
                        DataSet tmpdataSet = TableUtil.GenerateEmptyTables();
                        //��ȡ��ͬ��������
                        DataRow[] detailRowList = outBillTable.Select("BILLNO='" + row["BILLNO"].ToString() + "'");

                        DataRow billRow = tmpdataSet.Tables["MASTER"].NewRow();
                        billRow["BILLID"] = row["BILLNO"];
                        billRow["BILLCODE"] = "2";
                        billRow["STATE"] = "4";
                        billRow["BILLDATE"] = detailRowList[0]["BILLDATE"].ToString();
                        tmpdataSet.Tables["MASTER"].Rows.Add(billRow);
                        
                        foreach (DataRow detailRow in detailRowList)
                        {
                            DataRow newDetailRow = tmpdataSet.Tables["DETAIL"].NewRow();                         
                            newDetailRow["BILLID"] = detailRow["BILLNO"];
                            newDetailRow["DETAILID"] = detailRow["ID"];
                            newDetailRow["STORAGEID"] = detailRow["CELLNAME"];
                            newDetailRow["OPERATECODE"] = "2";
                            newDetailRow["TOBACCONAME"] = detailRow["PRODUCTNAME"];
                            newDetailRow["OPERATEPIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                            newDetailRow["PIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                            newDetailRow["OPERATEITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                            newDetailRow["ITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                            newDetailRow["CONFIRMSTATE"] = "1";
                            newDetailRow["TARGETSTORAGE"] = "";
                            tmpdataSet.Tables["DETAIL"].Rows.Add(newDetailRow);
                        }
                        new SplitBillDal().AddOrder(tmpdataSet);
                        wmsTHOKDao.UpdateOutBillStatus(row["BILLNO"].ToString());
                    }
                }
            }
            return tag;
        }

        /// <summary>
        /// ���ⵥȷ��
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        public void ConfirmOutBill(string billID, long billDetailID, string status)
        {
            using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
            {
                THOKDao wmsTHOKDao = new THOKDao();
                wmsTHOKDao.SetPersistentManager(wmsdbpm);
                wmsTHOKDao.ConfirmOutBill(billID, billDetailID, status);
            }
        }

        #endregion

        #region �̵㵥

        /// <summary>
        /// �̵㵥����
        /// </summary>
        /// <returns></returns>
        public bool GetInventoryBill()
        {
            bool tag = true;
            using (PersistentManager wesdbpm = new PersistentManager())
            {
                using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
                {
                    BillDao wesBillDao = new BillDao();
                    wesBillDao.SetPersistentManager(wesdbpm);
                    THOKDao wmsTHOKDao = new THOKDao();
                    wmsTHOKDao.SetPersistentManager(wmsdbpm);
                    //��ȡ�̵㵥
                    DataTable inventoryBillTable = wmsTHOKDao.ReadInventoryBill();
                    //�޼�¼��ֱ�ӷ���
                    if (inventoryBillTable.Rows.Count == 0)
                        return true;
                    //��ȡ���ظ���
                    DataTable distinctTable = TableUtil.GetDataTableDistinct(inventoryBillTable, new string[] {"BILLNO"});

                    foreach (DataRow row in distinctTable.Rows)
                    {
                        DataSet tmpdataSet = TableUtil.GenerateEmptyTables();
                        //��ȡ��ͬ��������
                        DataRow[] detailRowList = inventoryBillTable.Select("BILLNO='" + row["BILLNO"].ToString() + "'");

                        DataRow billRow = tmpdataSet.Tables["MASTER"].NewRow();
                        billRow["BILLID"] = row["BILLNO"];
                        billRow["BILLCODE"] = "3";
                        billRow["STATE"] = "4";
                        billRow["BILLDATE"] = detailRowList[0]["BILLDATE"].ToString();
                        tmpdataSet.Tables["MASTER"].Rows.Add(billRow);
                        
                        foreach (DataRow detailRow in detailRowList)
                        {
                            DataRow newDetailRow = tmpdataSet.Tables["DETAIL"].NewRow();
                            newDetailRow["BILLID"] = detailRow["BILLNO"];
                            newDetailRow["DETAILID"] = detailRow["ID"];
                            newDetailRow["STORAGEID"] = detailRow["CELLNAME"];
                            newDetailRow["OPERATECODE"] = "3";
                            if (detailRow["PRODUCTNAME"] != DBNull.Value)
                                newDetailRow["TOBACCONAME"] = detailRow["PRODUCTNAME"];
                            else
                                newDetailRow["TOBACCONAME"] = "";
                            newDetailRow["OPERATEPIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                            newDetailRow["PIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                            newDetailRow["OPERATEITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                            newDetailRow["ITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                            newDetailRow["CONFIRMSTATE"] = "1";
                            newDetailRow["TARGETSTORAGE"] = "";       
                            tmpdataSet.Tables["DETAIL"].Rows.Add(newDetailRow);
                        }
                        new SplitBillDal().AddOrder(tmpdataSet);
                        wmsTHOKDao.UpdateInventoryBillStatus(row["BILLNO"].ToString());
                    }
                }
            }
            return tag;
        }

        /// <summary>
        /// �̵㵥ȷ��
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        /// <param name="piece"></param>
        public void ConfirmInventoryBill(string billID, long billDetailID, string status, int piece,int item)
        {
            using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
            {
                THOKDao wmsTHOKDao = new THOKDao();
                wmsTHOKDao.SetPersistentManager(wmsdbpm);
                wmsTHOKDao.ConfirmInventoryBill(billID, billDetailID, status, piece, item);
            }

            using (PersistentManager wesdbpm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.SetPersistentManager(wesdbpm);
                billDao.ConfirmInventoryBill(billID, billDetailID, piece, item); 
            }
        }

        #endregion

        #region �ƿⵥ

        /// <summary>
        /// �ƿⵥ����
        /// </summary>
        /// <returns></returns>
        public bool GetMoveBill()
        {
            bool tag = true;
            try
            {
                using (PersistentManager wesdbpm = new PersistentManager())
                {
                    using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
                    {
                        BillDao wesBillDao = new BillDao();
                        wesBillDao.SetPersistentManager(wesdbpm);
                        THOKDao wmsTHOKDao = new THOKDao();
                        wmsTHOKDao.SetPersistentManager(wmsdbpm);
                        //��ȡ���ⵥ
                        DataTable moveBillTable = wmsTHOKDao.ReadMoveBill();
                        //�޼�¼��ֱ�ӷ���
                        if (moveBillTable.Rows.Count == 0)
                            return true;
                        //��ȡ���ظ���
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(moveBillTable, new string[] {"BILLNO"});

                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet tmpdataSet = TableUtil.GenerateEmptyTables();
                            //��ȡ��ͬ��������
                            DataRow[] detailRowList = moveBillTable.Select("BILLNO='" + row["BILLNO"].ToString() + "'");

                            DataRow billRow = tmpdataSet.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["BILLNO"];
                            billRow["BILLCODE"] = "4";
                            billRow["STATE"] = "4";
                            billRow["BILLDATE"] = detailRowList[0]["BILLDATE"].ToString();
                            tmpdataSet.Tables["MASTER"].Rows.Add(billRow);
                            
                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = tmpdataSet.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["BILLNO"];
                                newDetailRow["DETAILID"] = detailRow["ID"];
                                newDetailRow["STORAGEID"] = detailRow["OUT_CELLNAME"];
                                newDetailRow["OPERATECODE"] = "6";
                                newDetailRow["TOBACCONAME"] = detailRow["PRODUCTNAME"];
                                newDetailRow["OPERATEPIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                                newDetailRow["PIECE"] = Convert.ToInt32(detailRow["OPERATEPIECE"]);
                                newDetailRow["OPERATEITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                                newDetailRow["ITEM"] = Convert.ToInt32(detailRow["OPERATEITEM"]);
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = detailRow["IN_CELLNAME"];
                                tmpdataSet.Tables["DETAIL"].Rows.Add(newDetailRow);  
                            }
                            new SplitBillDal().AddOrder(tmpdataSet);
                            wmsTHOKDao.UpdateMoveBillStatus(row["BILLNO"].ToString());
                        }
                    }
                }
            }
            catch
            {
                tag = false;
            }
            return tag;
        }

        /// <summary>
        /// �ƿⵥȷ��
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        public void ConfirmMoveBill(string billID, long billDetailID, string status)
        {
            using (PersistentManager wmsdbpm = new PersistentManager("WMSConnection"))
            {
                THOKDao wmsTHOKDao = new THOKDao();
                wmsTHOKDao.SetPersistentManager(wmsdbpm);
                wmsTHOKDao.ConfirmMoveBill(billID, billDetailID, status);
            }
        }

        #endregion        
    }
}
