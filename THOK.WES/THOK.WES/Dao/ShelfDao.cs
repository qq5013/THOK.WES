using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
namespace THOK.WES.Dao
{
    public class ShelfDao:BaseDao
    {
        /// <summary>
        /// 查询仓库货位数据
        /// </summary>
        /// <returns></returns>
        public DataTable Find()
        {
            string sql = @"SELECT A.*,B.SHELFNAME,C.PRODUCTNAME,D.WH_NAME,
                            CASE WHEN (SUBSTRING(A.SHELFCODE,9,2))<10 THEN (SUBSTRING(A.SHELFCODE,10,1)) ELSE (SUBSTRING(A.SHELFCODE,9,2))END AS SHELF,
                            CASE WHEN A.INPUTDATE IS NULL THEN GETDATE() ELSE A.INPUTDATE END AS INDATE,
                            CASE WHEN A.ISACTIVE =1 THEN '是' ELSE '否' END AS ACTIVEIS,
                            A.QTY_STA /(SELECT U.STANDARDRATE FROM WMS_UNIT U LEFT OUTER JOIN WMS_PRODUCT P ON U.UNITCODE = P.TIAOCODE 
                            WHERE P.PRODUCTCODE = A.CURRENTPRODUCT) AS QUT_TIAO FROM V_WMS_WH_CELL A
                            LEFT JOIN WMS_WH_SHELF B ON A.SHELFCODE=B.SHELFCODE
                            LEFT JOIN WMS_PRODUCT C ON A.CURRENTPRODUCT=C.PRODUCTCODE
                            LEFT JOIN WMS_WAREHOUSE D ON A.WH_CODE=D.WH_CODE
                            LEFT JOIN WMS_UNIT U ON A.UNITCODE=U.UNITCODE
                            WHERE B.AREATYPE=0 AND A.ISACTIVE=1";
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 查询仓库总数量
        /// </summary>
        /// <returns></returns>
        public int FindSumQuantity()
        {
            string sql = "SELECT SUM(QUANTITY) FROM V_WMS_WH_CELL WHERE AREATYPE != 1";                   
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        
        public int FindBillDetailSumPiece()
        {
            string sql = "SELECT SUM(OPERATEPIECE) FROM BILLDETAIL WHERE BILLDATE > "+
                        " (SELECT CONVERT(VARCHAR(100),GETDATE(),23)) AND OPERATECODE=3 AND STORAGEID='F暂存位'";
            return Convert.ToInt32(ExecuteScalar(sql));
        }
    }
}
