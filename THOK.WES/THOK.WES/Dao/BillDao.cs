using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Dao
{
    public class BillDao: BaseDao
    {
        //@
        public DataTable FindBillMaster(string billType)
        {
            string sql = @"SELECT CONVERT(CHAR(10),A.BILLDATE, 120) BILLDATE,A.BILLMASTERID,A.BILLID, B.BILLNAME,A.TERMINAL,C.STATENAME 
                            FROM BILLMASTER A 
                            LEFT JOIN BILLTYPE B ON A.BILLCODE=B.BILLCODE 
                            LEFT JOIN STATETYPE C ON A.STATE=C.STATECODE 
                            WHERE STATE!= '6' AND A.BILLCODE='{0}' AND A.TERMINAL = '1' ORDER BY A.BILLID";
            sql = string.Format(sql,billType);
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable FindBillDetail(string billID)
        {
            string sql = @"SELECT DETAILID,STORAGEID,B.OPERATENAME,LTRIM(RTRIM(TOBACCONAME)) TOBACCONAME,OPERATEPIECE,PIECE,OPERATEITEM,ITEM,C.STATENAME,A.OPERATOR,A.TARGETSTORAGE 
                           FROM BILLDETAIL A LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE 
                           LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE 
                           WHERE BILLMASTERID='{0}' ORDER BY STORAGEID,OPERATEITEM DESC,DETAILID,A.OPERATECODE";
            sql = string.Format(sql,billID);
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public void UpdateMasterState(string state, string billID)
        {
            string sql = string.Format("UPDATE BILLMASTER SET STATE='{0}' WHERE BILLMASTERID='{1}'", state, billID);
            ExecuteNonQuery(sql);
        }

        //@
        public int FindTaskedCount(string billID)
        {
            string sql = string.Format("SELECT COUNT(*) FROM BILLDETAIL WHERE BILLMASTERID='{0}' AND CONFIRMSTATE !='1'", billID);
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        //@
        public DataTable FindTaskingMaster(string billType)
        {
            string sql = string.Format("SELECT * FROM BILLMASTER WHERE STATE='5' AND BILLCODE='{0}'AND TERMINAL = '1'", billType);
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable FindTaskDetail(string billID, string layersNumber, string orderby, int opType)
        {
            //opType = 0:正常操作，opType = 1:实时出库（大品种），opType = 2:正常出库（小品牌）
            string sql = "SELECT DETAILID,A.STORAGEID,B.OPERATENAME,LTRIM(RTRIM(A.TOBACCONAME)) TOBACCONAME," +
                                        " OPERATEPIECE,OPERATEITEM,C.STATENAME,A.TARGETSTORAGE,A.OPERATOR,A.BILLMASTERID,A.BILLID,A.OPERATECODE," +
                                        " ISNULL(E.BATCHNO,0) BATCHNO " +
                                        " FROM BILLDETAIL A " +
                                        " LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE " +
                                        " LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE " +
                                        " LEFT JOIN STORAGE D ON A.STORAGEID = D.STORAGEID" +
                                        " LEFT JOIN (SELECT TOP 1 * FROM AS_STOCK_IN_BATCH WHERE STATE = 0 ORDER BY BATCHNO) E " +
                                        " ON A.TOBACCONAME = E.CIGARETTENAME" +
                                        " LEFT JOIN (SELECT DISTINCT CIGARETTENAME FROM AS_STOCK_IN_BATCH " +
                                                "  WHERE CIGARETTENAME IN " +
                                                " (SELECT DISTINCT CIGARETTENAME FROM BILLDETAIL WHERE BILLMASTERID IN ({0}) AND CONFIRMSTATE!='3')) F " +
                                        " ON A.TOBACCONAME = F.CIGARETTENAME" +
                                        " WHERE A.BILLMASTERID  IN ({0})  AND A.CONFIRMSTATE !='3' AND D.LOCATIONID IN ({1}) " +
                                        " AND (A.OPERATOR = '{3}' OR A.CONFIRMSTATE = 1)" +
                                        (opType == 1 ? " AND (E.CIGARETTENAME IS NOT NULL OR A.CONFIRMSTATE = 2)" : "") +
                                        (opType == 2 ? " AND (F.CIGARETTENAME IS     NULL OR A.CONFIRMSTATE = 2)" : "") +
                                        " ORDER BY CONFIRMSTATE DESC,A.OPERATOR,A.STORAGEID {2} ," +
                                        " OPERATECODE,DETAILID";
            sql = string.Format(sql, billID, layersNumber, orderby, Environment.MachineName);
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataRow FindDetailByBillIDAnddetailID(string billID, string detailID)
        {
            string sql = string.Format("SELECT * FROM BILLDETAIL WHERE BILLMASTERID = '{0}' AND DETAILID = '{1}'", billID, detailID);
            return this.ExecuteQuery(sql).Tables[0].Rows[0];
        }

        //@
        public void ApplyDetailByBillIDAnddetailID(string billID, string detailID)
        {
            string sql = string.Format("UPDATE BILLDETAIL SET CONFIRMSTATE = '2', OPERATOR = '{0}' WHERE BILLMASTERID = '{1}' AND DETAILID = '{2}' "
                        , Environment.MachineName, billID, detailID);
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

        /// <summary>
        /// @查找零烟柜订单
        /// </summary>
        /// <returns>零烟柜列表</returns>
        public DataTable FindRetailMaster()
        {
            string sql = @"SELECT CONVERT(CHAR(10), BILLDATE, 120) BILLDATE,A.BILLMASTERID, BILLID, B.BILLNAME,A.TERMINAL,C.STATENAME 
                            FROM BILLMASTER A 
                            LEFT JOIN BILLTYPE B ON A.BILLCODE=B.BILLCODE 
                            LEFT JOIN STATETYPE C ON A.STATE=C.STATECODE 
                            WHERE STATE!= '6'  AND A.TERMINAL = '2' 
                            ORDER BY A.BILLID";
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable GetBillDetail(string billID)
        {
            string sql = @"SELECT DETAILID,STORAGEID,A.BILLMASTERID,A.CONFIRMSTATE,B.OPERATENAME,
                            TOBACCONAME,OPERATEPIECE,PIECE,OPERATEITEM,ITEM,C.STATENAME,OPERATOR 
                            FROM BILLDETAIL A 
                            LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE 
                            LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE 
                            WHERE A.BILLMASTERID='{0}' 
                            ORDER BY DETAILID, A.OPERATECODE";
            sql = string.Format(sql,billID);
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// @获取订单数据
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns>订单数据</returns>
        public DataTable GetBill(string id)
        {
            return this.ExecuteQuery("SELECT * FROM BILLMASTER WHERE BILLMASTERID = '" + id + "'").Tables[0];
        }

        //@
        public void UpdateDetailListState(string billID, string state, string operatorName)
        {
            string sql = string.Format("UPDATE BILLDETAIL SET CONFIRMSTATE ='{0}',OPERATOR = '{1}' WHERE BILLMASTERID = '{2}' ", state, operatorName, billID);
            this.ExecuteNonQuery(sql);

        }

        //@修改明细表零烟柜的数据和主表零烟柜数据
        public void UpdateBillDetailFewColum(string state, string msg, string operators, string Masterid)
        {
            string sql = string.Format("UPDATE BILLDETAIL SET [STATE]={0}, CONFIRMSTATE=3, MSG='{1}', [OPERATOR] ='{2}' WHERE BILLMASTERID = '{3}' AND CONFIRMSTATE <> 3 ", state, msg, operators, Masterid);
            ExecuteNonQuery(sql);
            sql = string.Format("UPDATE BILLMASTER SET [STATE] = 6 WHERE BILLMASTERID ='{0}'", Masterid);
            ExecuteNonQuery(sql);
            ExecuteNonQuery("DELETETASK");
        }

        //@查询整单确认零烟柜的数据,查询条件是没有确认的数据。
        public DataTable FindBillDetailMasterId(string MasterId)
        {
            string sql = string.Format("SELECT * FROM BILLDETAIL WHERE BILLMASTERID ='{0}' AND CONFIRMSTATE<>3 ", MasterId);
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable GetAllBill()
        {
            string sql = @"SELECT CONVERT(CHAR(10), BILLDATE, 120) BILLDATE,A.BILLMASTERID,BILLID, B.BILLNAME,A.TERMINAL,C.STATENAME 
                            FROM BILLMASTER A 
                            LEFT JOIN BILLTYPE B ON A.BILLCODE=B.BILLCODE 
                            LEFT JOIN STATETYPE C ON A.STATE=C.STATECODE
                            WHERE STATE!= '6'  ORDER BY A.BILLID";
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable GetBillDetailByDetailId(string billId)
        {
            string sql = @"SELECT A.DETAILID,A.STORAGEID,A.TARGETSTORAGE MOVESTORAGE,B.OPERATENAME,
                            A.TOBACCONAME,A.OPERATEPIECE,A.OPERATEITEM,A.OPERATECODE,
                            C.STATENAME,A.OPERATOR ,A.BILLMASTERID MASTER,E.BILLCODE,F.BILLNAME,A.CONFIRMSTATE
                            FROM BILLDETAIL A 
                            LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE 
                            LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE 
                            LEFT JOIN BILLMASTER E ON A.BILLMASTERID = E.BILLMASTERID 
                            LEFT JOIN BILLTYPE F ON E.BILLCODE = F.BILLCODE 
                            LEFT JOIN STORAGE D ON A.STORAGEID = D.STORAGEID 
                            WHERE CONFIRMSTATE != '3' AND A.BILLMASTERID = '{0}'";
            sql = string.Format(sql, billId);
            return this.ExecuteQuery(sql).Tables[0];
        }

        //@
        public DataTable GetUploadData()
        {
            string sql = @"SELECT A.ID,B.DETAILID,B.BILLID,B.BILLMASTERID ,B.OPERATOR,B.STORAGEID,
                            B.TOBACCONAME,B.PIECE,B.ITEM ,B.OPERATECODE ,C.OPERATENAME,D.STATENAME,
                            CASE WHEN A.ISUPLOAD = 0 THEN '未上传' ELSE '已上传' END ISUPLOAD  
                           FROM TASKUPLOAD A 
                            LEFT JOIN BILLDETAIL B ON A.BILLMASTERID = B.BILLMASTERID AND A.BILLDETAILID = B.DETAILID 
                            LEFT JOIN OPERATETYPE C ON B.OPERATECODE = C.OPERATECODE 
                            LEFT JOIN STATETYPE D ON B.CONFIRMSTATE = D.STATECODE";
            return ExecuteQuery(sql).Tables[0];
        }

        //@
        public void DeleteUploadData(string parameters)
        {
            string sql = "DELETE TASKUPLOAD WHERE ID IN ({0}) ";
            sql = string.Format(sql, parameters);
            ExecuteNonQuery(sql);
        }

        //@
        public DataTable FindBillMaster()
        {
            string sql = @"SELECT CONVERT(CHAR(10), BILLDATE, 120) BILLDATE, BILLID,A.BILLMASTERID, B.BILLNAME,C.STATENAME 
                            FROM BILLMASTER A 
                            LEFT JOIN BILLTYPE B ON A.BILLCODE=B.BILLCODE 
                            LEFT JOIN STATETYPE C ON A.STATE=C.STATECODE 
                            ORDER BY A.BILLDATE DESC, A.BILLID";
            return ExecuteQuery(sql).Tables[0];
        }             
    }
}
