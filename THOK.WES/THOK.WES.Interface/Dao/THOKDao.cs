using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class THOKDao : BaseDao
    {
        #region 入库单

        /// <summary>
        /// 入库单视图【V_WMS_IN_AF_ALLOT】中状态为“0”息（未读）
        /// </summary>
        /// <returns></returns>
        public DataTable ReadInBill()
        {
            string sql = @"SELECT * FROM V_WMS_IN_AF_ALLOT WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 更新入库单细表【WMS_IN_BILLMASTER】和入库单分配表【WMS_IN_ALLOT】状态，把“未读取”状态置为“已读”
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
        /// 更新入库单【WMS_IN_ALLOT】的确认标记（INPUTQUANTITY）、完成时间（FINISHTIME）和状态（STATUS）
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

        #region 出库单

        /// <summary>
        /// 出库单视图【V_WMS_OUT_AF_ALLOTDETAIL】中状态为“0”信息（未读）
        /// </summary>
        /// <returns></returns>
        public DataTable ReadOutBill()
        {
            string sql = @"SELECT * FROM V_WMS_OUT_AF_ALLOTDETAIL WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 更新出库单分配主表【WMS_OUT_ALLOTMASTER】【wms_out_billmaster】【WMS_OUT_ALLOTDETAIL】状态
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
        /// 出库单完成
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
        
        #region 盘点单

        /// <summary>
        /// 盘点单视图【V_WMS_CHECK_AF_BILLDETAIL】中状态为“0”信息（未读）
        /// </summary>
        /// <returns></returns>
        public DataTable ReadInventoryBill()
        {
            string sql = @"SELECT * FROM V_WMS_CHECK_AF_BILLDETAIL WHERE STATUS='0'";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 更新【WMS_CHECK_BILLMASTER】【WMS_Check_BILLDETAIL】状态
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
        /// 完成盘点单
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

        #region 移库单

        /// <summary>
        /// 移库单视图【V_WMS_MOVE_AF_BILLDETAIL 】中状态为“0”信息（未读）
        /// </summary>
        /// <returns></returns>
        public DataTable ReadMoveBill()
        {
            string sql = @"SELECT * FROM V_WMS_MOVE_AF_BILLDETAIL WHERE STATUS='0' ORDER BY ID";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 更新【WMS_MOVE_BILLMASTER】【WMS_MOVE_BILLDETAIL】状态
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
        /// 移库单完成
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

        #region 下载货位

        /// <summary>
        /// 从数字仓储系统下载货位
        /// </summary>
        public DataTable GetCellTable()
        {
            string sql = "SELECT CELLNAME,LAYER_NO,ISVIRTUAL,AREATYPE FROM V_WMS_WH_CELL";
            return this.ExecuteQuery(sql).Tables[0];
        }

        #endregion
    }
}
