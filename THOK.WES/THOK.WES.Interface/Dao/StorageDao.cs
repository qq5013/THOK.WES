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
        /// 获取所有零烟柜
        /// </summary>
        /// <returns>所有零烟柜</returns>
        public DataTable GetSmallBarStorageID()
        {
            return this.ExecuteQuery("SELECT STORAGEID FROM STORAGE WHERE STORAGETYPE = 0").Tables[0];  
        }

        /// <summary>
        /// @插入货位到数据库
        /// </summary>
        /// <param name="ds"></param>
        public void InsertStorage(DataSet ds)
        {
            this.BatchInsert(ds.Tables["STORAGE"], "Storage");
        }

        /// <summary>
        /// @下载车载系统中存在的货位名称
        /// </summary>
        /// <returns></returns>
        public DataTable QueryCellName()
        {
            string sql = "SELECT STORAGEID FROM STORAGE";
            return this.ExecuteQuery(sql).Tables[0];
        }
    }
}
