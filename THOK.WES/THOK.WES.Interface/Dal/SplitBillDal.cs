using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dao;
using THOK.WES.Interface.Util;


namespace THOK.WES.Interface.Dal
{
    public class SplitBillDal
    {
        private StorageDal storageDal = new StorageDal();

        /// <summary>
        /// 拆分出零烟柜与普通货架,并添加到WES数据库
        /// </summary>
        /// <param name="ds">原订单数据</param>
        public void AddOrder(DataSet ds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.SetPersistentManager(pm);

                //遍历所有订单
                foreach (DataRow row in ds.Tables["MASTER"].Rows)
                {
                    if (billDao.FindBillCount(row["BillID"].ToString()) > 0)
                    {
                        //WES中已存在跳过。
                        continue;
                    }
                    //条烟货架
                    DataSet smallBarDataSet = TableUtil.GenerateEmptyTables();
                    //普通货架
                    DataSet commonlyDataSet = TableUtil.GenerateEmptyTables();

                    object[] tempData;
                    string smallBarBillID = row["BILLID"].ToString() + "1";
                    string commonlyBillID = row["BILLID"].ToString() + "2";

                    row["BILLMASTERID"] = smallBarBillID;
                    row["TERMINAL"] = 2;
                    tempData = row.ItemArray;
                    smallBarDataSet.Tables["MASTER"].Rows.Add(tempData);

                    row["BILLMASTERID"] = commonlyBillID;
                    row["TERMINAL"] = 1;
                    tempData = row.ItemArray;
                    commonlyDataSet.Tables["MASTER"].Rows.Add(tempData);

                    //存放零烟柜货位。
                    IList<string> SmallBarStorageIDList = storageDal.GetSmallBarStorageIDList();

                    DataRow[] tempDetails = ds.Tables["DETAIL"].Select("BILLID = '" + row["BILLID"].ToString() + "'");

                    //分离出数据
                    for (int i = 0; i < tempDetails.Length; i++)
                    {
                        if (SmallBarStorageIDList.Contains(tempDetails[i]["STORAGEID"].ToString()))
                        {
                            tempDetails[i]["BILLMASTERID"] = smallBarBillID;
                            smallBarDataSet.Tables["DETAIL"].ImportRow(tempDetails[i]);
                        }
                        else
                        {
                            tempDetails[i]["BILLMASTERID"] = commonlyBillID;
                            commonlyDataSet.Tables["DETAIL"].ImportRow(tempDetails[i]);
                        }
                    }


                    //添加普通货柜数据
                    if (commonlyDataSet.Tables["DETAIL"].Rows.Count > 0)
                    {
                        billDao.InsertBill(commonlyDataSet);
                    }

                    //添加零烟柜数据
                    if (smallBarDataSet.Tables["DETAIL"].Rows.Count > 0)
                    {
                        billDao.InsertBill(smallBarDataSet);
                    }
                }
            }
        }
    }
}
