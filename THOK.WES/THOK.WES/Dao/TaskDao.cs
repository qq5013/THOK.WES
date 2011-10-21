using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Dao
{
    public class TaskDao : BaseDao
    {        
        internal void OperateToLabelServer(string billId, string detailId, string storageId, string operateType, string tobaccoName, int piece, int item, string targetStorageName)
        {
            string sql = @"INSERT INTO SY_SHOWINFO 
                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',0,0,0,'{7}');";
            string tmp = "";
            tmp = tmp + (piece > 0 ? string.Format("{0}¼þ", piece) : "");
            tmp = tmp + (item > 0 ? string.Format("{0}Ìõ", item) : "");
            tmp = tmp + (targetStorageName.Length > 0 ? string.Format(@"->{0}", targetStorageName) : "");
            sql = string.Format(sql, billId, detailId, storageId, operateType ,tobaccoName, piece, item, tmp);
            ExecuteNonQuery(sql);
        }

        internal void CancelOperateToLabelServer(string billId, string detailId, string storageId)
        {
            string sql = @" DELETE SY_SHOWINFO 
                            WHERE STORAGEID = '{0}' AND ORDERMASTERID = '{1}' AND ORDERDETAILID = '{2}'

                            UPDATE SY_SHOWINFO SET
                            READSTATE = 0,HARDWAREREADSTATE=0
                            WHERE STORAGEID = '{0}' AND CONFIRMSTATE = 0 AND READSTATE = 1

                            UPDATE STORAGES SET
                            ACT = '',PRODUCTNAME='',CONTENTS='',NUMBERSHOW='',[SIGN]=0
                            WHERE STORAGEID = '{0}'";
            sql = string.Format(sql,storageId, billId, detailId );
            ExecuteNonQuery(sql);
        }
    }
}
