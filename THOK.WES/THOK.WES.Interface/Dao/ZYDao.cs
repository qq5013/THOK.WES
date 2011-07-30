using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;


namespace THOK.WES.Interface.Dao
{
    /// <summary>
    /// ����Dao
    /// </summary>
    public class ZYDao : BaseDao
    {
        #region ��ⵥ        
        
        /// <summary>
        /// ��ȡ������ⵥ
        /// </summary>
        /// <returns>��ⵥ</returns>
        public DataTable RederInBill()
        {
            //�ֿ���ⵥ������ 
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '1' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// ����������ⵥΪ�ѵ���
        /// </summary>
        /// <param name="billID">��ⵥ���</param>
        public void UpdateInBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region ���ⵥ        
        
        /// <summary>
        /// ��ȡ���̳��ⵥ
        /// </summary>
        /// <returns>���ⵥ</returns>
        public DataTable RederOutBill()
        {
            //�ֿ���ⵥ������ 
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '2' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }

       
        /// <summary>
        /// �������̳��ⵥΪ�ѵ���
        /// </summary>
        /// <param name="billID">���ⵥ���</param>
        public void UpdateOutBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region �̵㵥        

        /// <summary>
        /// ��ȡ�����̵㵥
        /// </summary>
        /// <returns>�̵㵥</returns>
        public DataTable RederInventoryBill()
        {
            //�ֿ��̵㵥������
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '3' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���������̵㵥Ϊ�ѵ���
        /// </summary>
        /// <param name="billID">�̵㵥���</param>
        public void UpdateInventoryBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region �ƿⵥ        

        /// <summary>
        /// ��ȡ�����ƿⵥ
        /// </summary>
        /// <returns>�̵㵥</returns>
        public DataTable RederMobileBill()
        {
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE IN( '4','5') AND READ_STATUS = '0' ";   
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// ���������ƿⵥΪ�ѵ���
        /// </summary>
        /// <param name="billID">�̵㵥���</param>
        public void UpdateMobileBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

         #endregion

        /// <summary>
        /// ��������ȷ�ϱ�Ǻ�ȷ��ʱ��
        /// </summary>
        /// <param name="billID">�����</param>
        public void ConfirmBill(string billID,string detailID)
        {          
            string sql = string.Format(@"UPDATE DWV_OUT_STORE_ACT_LOG SET AUDIT_STATUS = '1',AUDIT_DATETIME = '{0}',UP_STATUS = '1' 
                                         WHERE STORE_BILL_ID = '{1}' AND STORE_BILL_DETAIL_ID= '{2}' ", DateTime.Now.ToString("yy/MM/dd HH:mm"), billID, detailID);
            this.ExecuteNonQuery(sql);
        }
    }
}
