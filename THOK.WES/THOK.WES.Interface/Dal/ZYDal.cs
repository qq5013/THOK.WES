using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dao;
using THOK.WES.Interface.Util;

namespace THOK.WES.Interface.Dal
{
    public class ZYDal
    {
        public ZYDal()
        {

        }

        /// <summary>
        /// 获取中烟入库单操作
        /// </summary>
        /// <returns>操作结果</returns>
        public bool GetInBill()
        {
            bool tag = true;
            try
            {
                using (PersistentManager wesdbpm = new PersistentManager())
                {
                    using (PersistentManager wmsdbpm = new PersistentManager("DB2Connection"))
                    {
                        BillDao billDao = new BillDao();
                        billDao.SetPersistentManager(wesdbpm);
                        ZYDao zydao = new ZYDao();
                        zydao.SetPersistentManager(wmsdbpm);
                        
                        DataTable dbTable = zydao.RederInBill();
                        //如果无记录直接返回
                        if (dbTable.Rows.Count == 0)
                            return true;        
                        //获取无重复行
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(dbTable, new string[] { "STORE_BILL_ID" });

                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet tmpdataSet = TableUtil.GenerateEmptyTables();
                            //获取相同单号的数据
                            DataRow[] detailRowList = dbTable.Select("STORE_BILL_ID='" + row["STORE_BILL_ID"].ToString() + "'");

                            DataRow billRow = tmpdataSet.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["STORE_BILL_ID"];
                            billRow["BIILLCODE"] = "1";
                            billRow["STATE"] = "4";                          
                            billRow["BILLDATE"] = DateTime.ParseExact(detailRowList[0]["CAT_DATETIME"].ToString(), "yyyyMMddHHmmss", new System.Globalization.CultureInfo("zh-CN", true)).ToString();
                            tmpdataSet.Tables["MASTER"].Rows.Add(billRow);

                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = tmpdataSet.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["STORE_BILL_ID"];
                                newDetailRow["DETAILID"] = detailRow["STORE_BILL_DETAIL_ID"];
                                newDetailRow["STORAGEID"] = detailRow["STORE_PLACE_NAME"];
                                newDetailRow["OPERATECODE"] = "1";
                                newDetailRow["TOBACCONAME"] = detailRow["BRAND_NAME"];
                                newDetailRow["OPERATEPIECE"] = Convert.ToInt32(detailRow["PIECE_NUM"]);
                                newDetailRow["PIECE"] = Convert.ToInt32(detailRow["PIECE_NUM"]);
                                newDetailRow["OPERATEITEM"] = Convert.ToInt32(detailRow["BAR_NUM"]);
                                newDetailRow["ITEM"] = Convert.ToInt32(detailRow["BAR_NUM"]);
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = "";
                                tmpdataSet.Tables["DETAIL"].Rows.Add(newDetailRow);
                            }
                            new SplitBillDal().AddOrder(tmpdataSet);
                            zydao.UpdateInBillStatus(row["STORE_BILL_ID"].ToString());
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
        /// 获取中烟出库单操作
        /// </summary>
        /// <returns>操作结果</returns>
        public bool GetOutBill()
        {
            bool tag = true;
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    using (PersistentManager dbPm = new PersistentManager("DB2Connection"))
                    {
                        BillDao billDao = new BillDao();
                        billDao.SetPersistentManager(pm);
                        ZYDao dao = new ZYDao();
                        dao.SetPersistentManager(dbPm);
                        //读取出库单数据
                        DataTable dbTable = dao.RederOutBill();     
                        //如果无记录直接返回
                        if (dbTable.Rows.Count == 0)
                            return true;       
                        //获取无重复行
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(dbTable, new string[] { "STORE_BILL_ID" });

                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet sqlDs = TableUtil.GenerateEmptyTables();
                            //获取相同单号的数据
                            DataRow[] detailRowList = dbTable.Select("STORE_BILL_ID='" + row["STORE_BILL_ID"].ToString() + "'");

                            DataRow billRow = sqlDs.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["STORE_BILL_ID"];
                            billRow["BIILLCODE"] = "2";
                            billRow["STATE"] = "4";
                            billRow["BILLDATE"] = DateTime.ParseExact(detailRowList[0]["CAT_DATETIME"].ToString(), "yyyyMMddHHmmss", new System.Globalization.CultureInfo("zh-CN", true)).ToString();
                            sqlDs.Tables["MASTER"].Rows.Add(billRow);

                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = sqlDs.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["STORE_BILL_ID"];
                                newDetailRow["DETAILID"] = detailRow["STORE_BILL_DETAIL_ID"];
                                newDetailRow["STORAGEID"] = detailRow["STORE_PLACE_NAME"];
                                newDetailRow["OPERATECODE"] = "2";
                                newDetailRow["TOBACCONAME"] = detailRow["BRAND_NAME"];
                                newDetailRow["OPERATEPIECE"] = detailRow["PIECE_NUM"];
                                newDetailRow["PIECE"] = detailRow["PIECE_NUM"];
                                newDetailRow["OPERATEITEM"] = detailRow["BAR_NUM"];
                                newDetailRow["ITEM"] = detailRow["BAR_NUM"];
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = "";
                                sqlDs.Tables["DETAIL"].Rows.Add(newDetailRow);
                            }
                            new SplitBillDal().AddOrder(sqlDs);
                            dao.UpdateOutBillStatus(row["STORE_BILL_ID"].ToString());       
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
        /// 获取中烟盘点单操作
        /// </summary>
        /// <returns>操作结果</returns>
        public bool GetInventoryBill()
        {
            bool tag = true;
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    using (PersistentManager dbPm = new PersistentManager("DB2Connection"))
                    {
                        BillDao billDao = new BillDao();
                        billDao.SetPersistentManager(pm);
                        ZYDao dao = new ZYDao();
                        dao.SetPersistentManager(dbPm);
                        //读取出库单数据
                        DataTable dbTable = dao.RederInventoryBill();
                        //如果无记录直接返回
                        if (dbTable.Rows.Count == 0)
                            return true;        
                        //获取无重复行
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(dbTable, new string[] { "STORE_BILL_ID" });

                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet sqlDs = TableUtil.GenerateEmptyTables();
                            //获取相同单号的数据
                            DataRow[] detailRowList = dbTable.Select("STORE_BILL_ID='" + row["STORE_BILL_ID"].ToString() + "'");

                            DataRow billRow = sqlDs.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["STORE_BILL_ID"];
                            billRow["BIILLCODE"] = "3";
                            billRow["STATE"] = "4";
                            billRow["BILLDATE"] = DateTime.ParseExact(detailRowList[0]["CAT_DATETIME"].ToString(), "yyyyMMddHHmmss", new System.Globalization.CultureInfo("zh-CN", true)).ToString();
                            sqlDs.Tables["MASTER"].Rows.Add(billRow);

                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = sqlDs.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["STORE_BILL_ID"];
                                newDetailRow["DETAILID"] = detailRow["STORE_BILL_DETAIL_ID"];
                                newDetailRow["STORAGEID"] = detailRow["STORE_PLACE_NAME"];
                                newDetailRow["OPERATECODE"] = "3";
                                newDetailRow["TOBACCONAME"] = detailRow["BRAND_NAME"];
                                newDetailRow["OPERATEPIECE"] = detailRow["BEGIN_STOCK_PIECE_NUM"];
                                newDetailRow["PIECE"] = detailRow["BEGIN_STOCK_PIECE_NUM"];
                                newDetailRow["OPERATEITEM"] = detailRow["BEGIN_STOCK_BAR_NUM"];
                                newDetailRow["ITEM"] = detailRow["BEGIN_STOCK_BAR_NUM"];
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = "";
                                sqlDs.Tables["DETAIL"].Rows.Add(newDetailRow);
                            }
                            new SplitBillDal().AddOrder(sqlDs);
                            dao.UpdateInventoryBillStatus(row["STORE_BILL_ID"].ToString());     
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
        /// 获取中烟移库单操作
        /// </summary>
        /// <returns>操作结果</returns>
        public bool GetMobileBill()
        {

            bool tag = true;
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    using (PersistentManager dbPm = new PersistentManager("DB2Connection"))
                    {
                        BillDao billDao = new BillDao();
                        billDao.SetPersistentManager(pm);
                        ZYDao dao = new ZYDao();
                        dao.SetPersistentManager(dbPm);
                       
                        DataTable dbTable = dao.RederMobileBill();     //读取出库单数据
                        if (dbTable.Rows.Count == 0)
                        {
                            return true;        //如果无记录直接返回
                        }
                        //获取无重复行
                        DataTable distinctTable = TableUtil.GetDataTableDistinct(dbTable, new string[] { "STORE_BILL_ID" });
                        foreach (DataRow row in distinctTable.Rows)
                        {
                            DataSet sqlDs = TableUtil.GenerateEmptyTables();
                            //获取相同单号的数据
                            DataRow[] detailRowList = dbTable.Select("STORE_BILL_ID='" + row["STORE_BILL_ID"].ToString() + "'");

                            DataRow billRow = sqlDs.Tables["MASTER"].NewRow();
                            billRow["BILLID"] = row["STORE_BILL_ID"];
                            billRow["BIILLCODE"] = "4";
                            billRow["STATE"] = "4";
                            billRow["BILLDATE"] = DateTime.ParseExact(detailRowList[0]["CAT_DATETIME"].ToString(), "yyyyMMddHHmmss", new System.Globalization.CultureInfo("zh-CN", true)).ToString();
                            sqlDs.Tables["MASTER"].Rows.Add(billRow);

                            foreach (DataRow detailRow in detailRowList)
                            {
                                DataRow newDetailRow = sqlDs.Tables["DETAIL"].NewRow();
                                newDetailRow["BILLID"] = detailRow["STORE_BILL_ID"];
                                newDetailRow["DETAILID"] = detailRow["STORE_BILL_DETAIL_ID"];
                                newDetailRow["STORAGEID"] = detailRow["STORE_PLACE_NAME"];
                                newDetailRow["OPERATECODE"] = detailRow["ACT_TYPE"];
                                newDetailRow["TOBACCONAME"] = detailRow["BRAND_NAME"];
                                newDetailRow["OPERATEPIECE"] = detailRow["PIECE_NUM"];
                                newDetailRow["PIECE"] = detailRow["PIECE_NUM"];
                                newDetailRow["OPERATEITEM"] = detailRow["BAR_NUM"];
                                newDetailRow["ITEM"] = detailRow["BAR_NUM"];
                                newDetailRow["CONFIRMSTATE"] = "1";
                                newDetailRow["TARGETSTORAGE"] = "";
                                sqlDs.Tables["DETAIL"].Rows.Add(newDetailRow);
                            }
                            new SplitBillDal().AddOrder(sqlDs);
                            dao.UpdateMobileBillStatus(row["STORE_BILL_ID"].ToString());      
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
        /// 更新中烟确认标记和确认时间
        /// </summary>
        /// <param name="billID">表单编号</param>
        public void ConfirmBill(string billID, string detailID)
        {          
            using (PersistentManager dbPm = new PersistentManager("DB2Connection"))
            {
                ZYDao zydao = new ZYDao();
                zydao.SetPersistentManager(dbPm);
                zydao.ConfirmBill(billID, detailID);
            }
        }
    }
}
