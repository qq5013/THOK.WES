using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Dao;

namespace THOK.WES.Dal
{
    public class StorageDal:BaseDao 
    {
        /// <summary>
        /// @获取货位类型
        /// </summary>
        /// <param name="storageId">货位ID</param>
        /// <returns>返回类型</returns>
        public int GetStorageType(string storageId)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                StorageDao dao = new StorageDao();
                return Convert.ToInt32(dao.GetStroage(storageId).Rows[0]["StorageType"]);
            }
        }

        /// <summary>
        /// @获取货架的RFID
        /// </summary>
        /// <param name="storageID">货位号</param>
        /// <returns>RFID</returns>
        public string GetRFID(string storageID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                StorageDao dao = new StorageDao();
                return dao.GetRFID(storageID);
            }
        }
        /// <summary>
        /// @更新货架RFID
        /// </summary>
        /// <param name="storageID">货位号</param>
        /// <param name="rfid">rfid</param>
        /// <param name="isRead">true为添加rfid,false为清空rfid</param>
        public void UpdateRFID(string storageID, string rfid)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                StorageDao dao = new StorageDao();
                dao.UpdateRFID(storageID, rfid);
            }
        }
    }
}
