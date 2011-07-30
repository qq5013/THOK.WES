using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Dao
{
    public class PDAUploadDataDao : BaseDao
    {
        //@
        public DataTable GetDataList()
        {
            string sql = "SELECT * FROM TASKUPLOAD";
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public void InsertData(DataTable table)
        {
            BatchInsert(table, "TASKUPLOAD");
        }
    }
}
