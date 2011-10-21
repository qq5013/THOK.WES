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
        /// ��ҳ��ѯ���ݣ�ָ�����ݼ�����TableName
        /// </summary>
        /// <param name="TableViewName">��������ͼ��</param>
        /// <param name="PrimaryKey">�������ֶ�����</param>
        /// <param name="QueryFields">��ѯ�ֶ��ַ������ֶ����ƶ��Ÿ���</param>
        /// <param name="pageIndex">��ѯҳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="orderBy">�����ֶκͷ�ʽ</param>
        /// <param name="filter">��ѯ����</param>
        /// <param name="strTableName">�������ݼ����ı���</param>
        /// <returns>����DataSet</returns>
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
        /// ��ҳ��ѯ
        /// </summary>
        /// <param name="sql">��ѯ��䣨��ѯ�������������м�¼��</param>
        /// <param name="tableName">����</param>
        /// <param name="startRecord">���ص���ʼ��¼</param>
        /// <param name="count">���ؼ�¼������</param>
        /// <returns>�������ݼ�</returns>
        protected DataSet ExecuteQuery(string sql, string tableName, int startRecord, int count)
        {
        }
    }
}
