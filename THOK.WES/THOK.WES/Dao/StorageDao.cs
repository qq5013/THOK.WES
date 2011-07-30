using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Dao
{
    public class StorageDao : BaseDao
    {
        /// <summary>
        /// @获取货架信息
        /// </summary>
        /// <param name="storageID">货架ID</param>
        /// <returns>数据</returns>
        public DataTable GetStroage(string storageID)
        {
            string sql = string.Format("SELECT * FROM STORAGE WHERE STORAGEID = '{0}'", storageID);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// @获取货架的RFID
        /// </summary>
        /// <param name="storageID">货位号</param>
        /// <returns>RFID</returns>
        public string GetRFID(string storageID)
        {
            string sql = string.Format("SELECT RFID FROM STORAGE WHERE STORAGEID = '{0}'", storageID);
            return this.ExecuteQuery(sql).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// @更新货架RFID
        /// </summary>
        /// <param name="storageID">货位号</param>
        /// <param name="rfid">rfid</param>
        /// <param name="isRead">true为添加rfid,false为清空rfid</param>
        public void UpdateRFID(string storageID, string rfid)
        {
            string sql = string.Format( "UPDATE STORAGE SET RFID = '{0}' WHERE STORAGEID = '{1}'",rfid,storageID);
            this.ExecuteNonQuery(sql);
        }
    }
}
