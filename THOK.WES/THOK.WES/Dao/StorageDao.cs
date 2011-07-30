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
        /// @��ȡ������Ϣ
        /// </summary>
        /// <param name="storageID">����ID</param>
        /// <returns>����</returns>
        public DataTable GetStroage(string storageID)
        {
            string sql = string.Format("SELECT * FROM STORAGE WHERE STORAGEID = '{0}'", storageID);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// @��ȡ���ܵ�RFID
        /// </summary>
        /// <param name="storageID">��λ��</param>
        /// <returns>RFID</returns>
        public string GetRFID(string storageID)
        {
            string sql = string.Format("SELECT RFID FROM STORAGE WHERE STORAGEID = '{0}'", storageID);
            return this.ExecuteQuery(sql).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// @���»���RFID
        /// </summary>
        /// <param name="storageID">��λ��</param>
        /// <param name="rfid">rfid</param>
        /// <param name="isRead">trueΪ���rfid,falseΪ���rfid</param>
        public void UpdateRFID(string storageID, string rfid)
        {
            string sql = string.Format( "UPDATE STORAGE SET RFID = '{0}' WHERE STORAGEID = '{1}'",rfid,storageID);
            this.ExecuteNonQuery(sql);
        }
    }
}
