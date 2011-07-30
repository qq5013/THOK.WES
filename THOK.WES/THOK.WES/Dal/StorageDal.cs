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
        /// @��ȡ��λ����
        /// </summary>
        /// <param name="storageId">��λID</param>
        /// <returns>��������</returns>
        public int GetStorageType(string storageId)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                StorageDao dao = new StorageDao();
                return Convert.ToInt32(dao.GetStroage(storageId).Rows[0]["StorageType"]);
            }
        }

        /// <summary>
        /// @��ȡ���ܵ�RFID
        /// </summary>
        /// <param name="storageID">��λ��</param>
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
        /// @���»���RFID
        /// </summary>
        /// <param name="storageID">��λ��</param>
        /// <param name="rfid">rfid</param>
        /// <param name="isRead">trueΪ���rfid,falseΪ���rfid</param>
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
