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
        /// ��ֳ����̹�����ͨ����,����ӵ�WES���ݿ�
        /// </summary>
        /// <param name="ds">ԭ��������</param>
        public void AddOrder(DataSet ds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.SetPersistentManager(pm);

                //�������ж���
                foreach (DataRow row in ds.Tables["MASTER"].Rows)
                {
                    if (billDao.FindBillCount(row["BillID"].ToString()) > 0)
                    {
                        //WES���Ѵ���������
                        continue;
                    }
                    //���̻���
                    DataSet smallBarDataSet = TableUtil.GenerateEmptyTables();
                    //��ͨ����
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

                    //������̹��λ��
                    IList<string> SmallBarStorageIDList = storageDal.GetSmallBarStorageIDList();

                    DataRow[] tempDetails = ds.Tables["DETAIL"].Select("BILLID = '" + row["BILLID"].ToString() + "'");

                    //���������
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


                    //�����ͨ��������
                    if (commonlyDataSet.Tables["DETAIL"].Rows.Count > 0)
                    {
                        billDao.InsertBill(commonlyDataSet);
                    }

                    //������̹�����
                    if (smallBarDataSet.Tables["DETAIL"].Rows.Count > 0)
                    {
                        billDao.InsertBill(smallBarDataSet);
                    }
                }
            }
        }
    }
}
