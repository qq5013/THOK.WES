using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class THOKDao : BaseDao
    {
        #region ��ⵥ

        /// <summary>
        /// ��ⵥ��ͼ��V_WMS_IN_AF_ALLOT����״̬Ϊ��0��Ϣ��δ����
        /// </summary>
        /// <returns></returns>
        public DataTable ReadInBill()
        {
            string sql = @"SELECT * FROM V_WMS_IN_AF_ALLOT WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ������ⵥϸ��WMS_IN_BILLMASTER������ⵥ�����WMS_IN_ALLOT��״̬���ѡ�δ��ȡ��״̬��Ϊ���Ѷ���
        /// </summary>
        /// <param name="billID"></param>
        public void UpdateInBillStatus(string billID)
        {
            string machin = Environment.MachineName;
            string sql = @"UPDATE WMS_IN_BILLMASTER SET [STATUS] = '5' 
                                WHERE BILLNO='{0}' AND [STATUS]<>'5'; 
                                UPDATE WMS_IN_ALLOT SET [STATUS]='1',OPERATEPERSON='{1}',STARTTIME=GETDATE() WHERE BILLNO='{0}'";
            sql = string.Format(sql, billID, machin);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// zys-2011-7-1
        /// ������ⵥ��WMS_IN_ALLOT����ȷ�ϱ�ǣ�INPUTQUANTITY�������ʱ�䣨FINISHTIME����״̬��STATUS��
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="state"></param>
        /// <param name="inputQuantity"></param>
        public void ConfirmInBill(string billID, long billDetailID, string status)
        {
            StoredProcParameter p = new StoredProcParameter();
            p.AddParameter("@BILLNO", billID);
            p.AddParameter("@TASKID", billDetailID);
            p.AddParameter("@STATUS", status);
            this.ExecuteNonQuery("cp_FinishEntryTask", p);
        }

        #endregion

        #region ���ⵥ

        /// <summary>
        /// ���ⵥ��ͼ��V_WMS_OUT_AF_ALLOTDETAIL����״̬Ϊ��0����Ϣ��δ����
        /// </summary>
        /// <returns></returns>
        public DataTable ReadOutBill()
        {
            string sql = @"SELECT * FROM V_WMS_OUT_AF_ALLOTDETAIL WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���³��ⵥ��������WMS_OUT_ALLOTMASTER����wms_out_billmaster����WMS_OUT_ALLOTDETAIL��״̬
        /// </summary>
        /// <param name="billID"></param>
        public void UpdateOutBillStatus(string billID)
        {
            string machin = Environment.MachineName;
            string sql = @"UPDATE WMS_OUT_BILLMASTER SET STATUS='5' WHERE BILLNO='{0}';
                           UPDATE WMS_OUT_ALLOTMASTER SET [STATUS]='1' WHERE BILLNO='{0}';
                           UPDATE WMS_OUT_ALLOTDETAIL SET [STATUS]='1',OPERATEPERSON='{1}',STARTTIME=GETDATE() WHERE BILLNO='{0}'";
            sql = string.Format(sql, billID, machin);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// ���ⵥ���
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void ConfirmOutBill(string billID, long billDetailID, string status)
        {
            StoredProcParameter p = new StoredProcParameter();
            p.AddParameter("@BILLNO", billID);
            p.AddParameter("@ID", billDetailID);
            p.AddParameter("@STATUS", status);
            this.ExecuteNonQuery("cp_FinishDeliveryTask_AF", p);
        }

        #endregion
        
        #region �̵㵥

        /// <summary>
        /// �̵㵥��ͼ��V_WMS_CHECK_AF_BILLDETAIL����״̬Ϊ��0����Ϣ��δ����
        /// </summary>
        /// <returns></returns>
        public DataTable ReadInventoryBill()
        {
            string sql = @"SELECT * FROM V_WMS_CHECK_AF_BILLDETAIL WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���¡�WMS_CHECK_BILLMASTER����WMS_Check_BILLDETAIL��״̬
        /// </summary>
        /// <param name="billID"></param>
        public void UpdateInventoryBillStatus(string billID)
        {
            string machin = Environment.MachineName;
            string sql = @"UPDATE WMS_CHECK_BILLMASTER SET [STATUS]='3' WHERE BILLNO='{0}';
                         UPDATE WMS_CHECK_BILLDETAIL SET [STATUS]='1',OPERATEPERSON='{1}',STARTTIME=GETDATE() WHERE BILLNO='{0}'";
            sql = string.Format(sql, billID, machin);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// ����̵㵥
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        /// <param name="piece"></param>
        public void ConfirmInventoryBill(string billID, long billDetailID, string status, int piece,int item)
        {
            StoredProcParameter p = new StoredProcParameter();
            p.AddParameter("@BILLNO", billID);
            p.AddParameter("@TASKID", billDetailID);
            p.AddParameter("@STATUS", status);
            p.AddParameter("@PIECE", piece);
            p.AddParameter("@ITEM", item);
            this.ExecuteNonQuery("cp_FinishCheckTask_AF", p);
        }

        #endregion

        #region �ƿⵥ

        /// <summary>
        /// �ƿⵥ��ͼ��V_WMS_MOVE_AF_BILLDETAIL ����״̬Ϊ��0����Ϣ��δ����
        /// </summary>
        /// <returns></returns>
        public DataTable ReadMoveBill()
        {
            string sql = @"SELECT * FROM V_WMS_MOVE_AF_BILLDETAIL WHERE STATUS='0' ORDER BY ID";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���¡�WMS_MOVE_BILLMASTER����WMS_MOVE_BILLDETAIL��״̬
        /// </summary>
        /// <param name="billID"></param>
        public void UpdateMoveBillStatus(string billID)
        {
            string machin = Environment.MachineName;
            string sql = @"UPDATE WMS_MOVE_BILLMASTER SET STATUS='3' WHERE BILLNO='{0}';
                           UPDATE WMS_MOVE_BILLDETAIL SET [STATUS]='1',OPERATEPERSON='{1}',STARTTIME = GETDATE() WHERE BILLNO='{0}'";
            sql = string.Format(sql, billID, machin);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// �ƿⵥ���
        /// </summary>
        /// <param name="billID"></param>
        /// <param name="detailID"></param>
        /// <param name="status"></param>
        public void ConfirmMoveBill(string billID, long billDetailID, string status)
        {
            StoredProcParameter p = new StoredProcParameter();
            p.AddParameter("@BILLNO", billID);
            p.AddParameter("@TASKID", billDetailID);
            p.AddParameter("@STATUS", status);
            this.ExecuteNonQuery("cp_FinishMoveTask", p);
        }

        #endregion

        #region ���ػ�λ

        /// <summary>
        /// �����ֲִ�ϵͳ���ػ�λ
        /// </summary>
        public DataTable GetCellTable()
        {
            string sql = "SELECT CELLNAME,LAYER_NO,ISVIRTUAL,AREATYPE FROM V_WMS_WH_CELL";
            return this.ExecuteQuery(sql).Tables[0];
        }

        #endregion
    }
}
