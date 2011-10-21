using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Util
{
    public class BaseDao
    {
        protected void ExecuteNonQuery(string sqlString)
        {
        }

        protected void ExecuteNonQuery(string procedureName, StoredProcParameter param)
        {
        }

        protected void BatchInsert(DataTable dataTable, string tableName)
        {
        }

        protected IDataReader ExecuteReader(string sqlString)
        {
        }

        protected IDataReader ExecuteReader(string procedureName, StoredProcParameter param)
        {
        }

        protected object ExecuteScalar(string sqlString)
        {
        }

        protected object ExecuteScalar(string procedureName, StoredProcParameter param)
        {
        }

        protected DataSet ExecuteQuery(string sqlString)
        {
        }

        protected DataSet ExecuteQuery(string procedureName, StoredProcParameter param)
        {
        }

        protected DataSet ExecuteQuery(string sqlString, string tableName)
        {
        }

        protected DataSet ExecuteQuery(string procedureName, string tableName, StoredProcParameter param)
        {
        }

        protected DataSet ExecuteEmptyDataSet(string tableName)
        {
        }

        /// <summary>
        /// 分页查询数据，指定数据集表名TableName
        /// </summary>
        /// <param name="TableViewName">表名或视图名</param>
        /// <param name="PrimaryKey">表主键字段名称</param>
        /// <param name="QueryFields">查询字段字符串，字段名称逗号隔开</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="orderBy">排序字段和方式</param>
        /// <param name="filter">查询条件</param>
        /// <param name="strTableName">返回数据集填充的表名</param>
        /// <returns>返回DataSet</returns>
        protected DataSet ExecuteQuery(string TableViewName, string PrimaryKey, string QueryFields, int pageIndex, int pageSize, string orderBy, string filter, string strTableName)
        {
            int preRec = (pageIndex - 1) * pageSize;
            string sql = string.Format("select top {4} {2} from {0} " +
                                        " where {1} not in ( select top {3} {1} from {0} where {6} order by {5}) " +
                                        " and {6} order by {5}"
                                        , TableViewName, PrimaryKey, QueryFields, preRec.ToString(), pageSize.ToString(), orderBy, filter);
            return persistentManager.ExecuteQuery(sql, strTableName);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql">查询语句（查询符合条件的所有记录）</param>
        /// <param name="tableName">表名</param>
        /// <param name="startRecord">返回的起始记录</param>
        /// <param name="count">返回记录的条数</param>
        /// <returns>返回数据集</returns>
        protected DataSet ExecuteQuery(string sql, string tableName, int startRecord, int count)
        {
        }
    }
}
