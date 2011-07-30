using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;


namespace THOK.WES.Interface.Dao
{
    /// <summary>
    /// 中烟Dao
    /// </summary>
    public class ZYDao : BaseDao
    {
        #region 入库单        
        
        /// <summary>
        /// 读取中烟入库单
        /// </summary>
        /// <returns>入库单</returns>
        public DataTable RederInBill()
        {
            //仓库入库单据主表 
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '1' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 更新中烟入库单为已导入
        /// </summary>
        /// <param name="billID">入库单编号</param>
        public void UpdateInBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region 出库单        
        
        /// <summary>
        /// 读取中烟出库单
        /// </summary>
        /// <returns>出库单</returns>
        public DataTable RederOutBill()
        {
            //仓库出库单据主表 
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '2' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }

       
        /// <summary>
        /// 更新中烟出库单为已导入
        /// </summary>
        /// <param name="billID">出库单编号</param>
        public void UpdateOutBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region 盘点单        

        /// <summary>
        /// 读取中烟盘点单
        /// </summary>
        /// <returns>盘点单</returns>
        public DataTable RederInventoryBill()
        {
            //仓库盘点单据主表
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE = '3' AND READ_STATUS = '0' ";    
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 更新中烟盘点单为已导入
        /// </summary>
        /// <param name="billID">盘点单编号</param>
        public void UpdateInventoryBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

        #endregion

        #region 移库单        

        /// <summary>
        /// 读取中烟移库单
        /// </summary>
        /// <returns>盘点单</returns>
        public DataTable RederMobileBill()
        {
            string sql = @"SELECT * FROM DWV_OUT_STORE_ACT_LOG WHERE ACT_TYPE IN( '4','5') AND READ_STATUS = '0' ";   
            return this.ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 更新中烟移库单为已导入
        /// </summary>
        /// <param name="billID">盘点单编号</param>
        public void UpdateMobileBillStatus(string billID)
        {
            string sql = string.Format("UPDATE DWV_OUT_STORE_ACT_LOG SET READ_STATUS = '1' WHERE STORE_BILL_ID = '{0}' ", billID);
            this.ExecuteNonQuery(sql);
        }

         #endregion

        /// <summary>
        /// 更新中烟确认标记和确认时间
        /// </summary>
        /// <param name="billID">表单编号</param>
        public void ConfirmBill(string billID,string detailID)
        {          
            string sql = string.Format(@"UPDATE DWV_OUT_STORE_ACT_LOG SET AUDIT_STATUS = '1',AUDIT_DATETIME = '{0}',UP_STATUS = '1' 
                                         WHERE STORE_BILL_ID = '{1}' AND STORE_BILL_DETAIL_ID= '{2}' ", DateTime.Now.ToString("yy/MM/dd HH:mm"), billID, detailID);
            this.ExecuteNonQuery(sql);
        }
    }
}
