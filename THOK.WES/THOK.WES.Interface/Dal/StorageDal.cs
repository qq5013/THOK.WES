using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dao;
using THOK.WES.Interface.Util;

namespace THOK.WES.Interface.Dal
{
    public class StorageDal
    {
        /// <summary>
        /// ��ȡ�������̹�Ļ�λ��
        /// </summary>
        /// <returns>�������̹��λ��</returns>
        public IList<string> GetSmallBarStorageIDList()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                StorageDao dao = new StorageDao();
                DataTable table = dao.GetSmallBarStorageID();
                IList<string> list = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row["STORAGEID"].ToString());
                }
                return list;
            }
        }

        #region ���ػ�λ

        /// <summary>
        /// �����ֲִ�ϵͳ���ػ�λ
        /// </summary>
        public void GetCellTable()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                using (PersistentManager dbpm = new PersistentManager("WMSConnection"))
                {
                    StorageDao storDao = new StorageDao();
                    storDao.SetPersistentManager(pm);
                    THOKDao dao = new THOKDao();
                    dao.SetPersistentManager(dbpm);

                    DataTable storageTable = storDao.QueryCellName();//��ѯ���ػ�λ
                    string storageList = TableUtil.StringMake(storageTable, "STORAGEID");
                    string storageName = TableUtil.StringMake(storageList);
                    DataTable cellTable = dao.GetCellTable();//����WMS��λ
                    DataRow[] cellDr = cellTable.Select("CELLNAME NOT IN (" + storageName + ")");//�ų����صĻ�λ
                    if (cellDr.Length > 0)
                    {
                        DataSet cellDs = InsertStorage(cellDr);
                        storDao.InsertStorage(cellDs);
                    }
                }
            }
        }

        /// <summary>
        /// ������ݵ������
        /// </summary>
        /// <param name="cellDr"></param>
        /// <returns></returns>
        public DataSet InsertStorage(DataRow[] cellDr)
        {
            DataSet ds = this.GenerateStorageTables();
            foreach (DataRow row in cellDr)
            {
                DataRow storagerow = ds.Tables["STORAGE"].NewRow();
                storagerow["StorageID"] = row["CELLNAME"].ToString().Trim();
                storagerow["StorageType"] = 1;
                storagerow["LocationID"] = Convert.ToInt32(row["LAYER_NO"].ToString());
                if (row["AREATYPE"].ToString() == "1")
                {
                    //storagerow["StorageType"] = 0;
                    storagerow["LocationID"] = 0;
                }
                ds.Tables["STORAGE"].Rows.Add(storagerow);
            }
            return ds;
        }

        /// <summary>
        /// ������λ�����
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateStorageTables()
        {
            DataSet ds = new DataSet();
            DataTable storage = ds.Tables.Add("STORAGE");
            storage.Columns.Add("StorageID");
            storage.Columns.Add("StorageType");
            storage.Columns.Add("LocationID");
            storage.Columns.Add("Rfid");
            return ds;
        }

        #endregion
    }
}
