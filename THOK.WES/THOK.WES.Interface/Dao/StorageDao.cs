using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class StorageDao : BaseDao
    {
        /// <summary>
        /// ��ȡ�������̹�
        /// </summary>
        /// <returns>�������̹�</returns>
        public DataTable GetSmallBarStorageID()
        {
            return this.ExecuteQuery("SELECT STORAGEID FROM STORAGE WHERE STORAGETYPE = 0").Tables[0];  
        }

        /// <summary>
        /// @�����λ�����ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void InsertStorage(DataSet ds)
        {
            this.BatchInsert(ds.Tables["STORAGE"], "Storage");
        }

        /// <summary>
        /// @���س���ϵͳ�д��ڵĻ�λ����
        /// </summary>
        /// <returns></returns>
        public DataTable QueryCellName()
        {
            string sql = "SELECT STORAGEID FROM STORAGE";
            return this.ExecuteQuery(sql).Tables[0];
        }
    }
}
