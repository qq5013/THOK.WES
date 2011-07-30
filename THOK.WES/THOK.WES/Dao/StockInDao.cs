using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;

namespace THOK.WES.Dao
{
    class StockInDao : BaseDao
    {
        //@
        internal int FindStockInTask(int stockInBatchNo)
        {
            string sql = string.Format("SELECT COUNT(*) FROM AS_STOCK_IN_BATCH WHERE STATE = 0 AND BATCHNO = {0}", stockInBatchNo);
            return Convert.ToInt32(ExecuteScalar(sql));
        }
        //@
        internal void UpdateState(int stockInBatchNo)
        {
            string sql = string.Format("UPDATE AS_STOCK_IN_BATCH SET STATE = 1 WHERE  BATCHNO = {0} ", stockInBatchNo);
            ExecuteNonQuery(sql);
        }

        //@
        internal void CancelUpdateState(int stockInBatchNo)
        {
            string sql = string.Format("UPDATE AS_STOCK_IN_BATCH SET STATE = 0 WHERE  BATCHNO = {0} ", stockInBatchNo);
            ExecuteNonQuery(sql);
        }
    }
}
