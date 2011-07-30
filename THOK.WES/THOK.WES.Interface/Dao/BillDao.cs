using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class BillDao : BaseDao
    {
        public int FindBillCount(string billID)
        {
            string sql = string.Format("SELECT COUNT(*) FROM BILLMASTER WHERE BILLID='{0}'", billID);
            return Convert.ToInt32(ExecuteQuery(sql).Tables[0].Rows[0][0]);
        }

        public void InsertBill(DataSet ds)
        {
            BatchInsert(ds.Tables["MASTER"], "BILLMASTER");
            BatchInsert(ds.Tables["DETAIL"], "BILLDETAIL");
        }

        public void ConfirmInventoryBill(string billID, long detailID, int piece,int item)
        {
            string sql = "UPDATE BILLDETAIL SET OPERATEPIECE = {0}, OPERATEITEM = {1} WHERE BILLID='{2}' AND DETAILID='{3}'";
            sql = string.Format(sql, piece, item, billID, detailID);
            this.ExecuteNonQuery(sql);
        }

        //@
        public void ConfirmTask(string billID, string detailID, string confirmState, int piece, int item, string state, string msg, string operater)
        {
            StoredProcParameter p = new StoredProcParameter();
            p.AddParameter("@BillID", billID);
            p.AddParameter("@DetailID", detailID);
            p.AddParameter("@ConfirmState", confirmState);
            p.AddParameter("@Piece", piece);
            p.AddParameter("@Item", item);
            p.AddParameter("@State", state);
            p.AddParameter("@Msg", msg);
            p.AddParameter("@Operator", operater);
            ExecuteNonQuery("ConfirmTask", p);
        }
    }
}