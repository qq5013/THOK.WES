using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.WES.Interface.Util
{
    class TableUtil
    {
        #region 其他

        /// <summary>
        /// 去除重复行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnsname"></param>
        /// <returns></returns>
        public static DataTable GetDataTableDistinct(DataTable table, string[] columnsname)
        {
            DataView dv = new DataView(table);
            DataTable resultTable = dv.ToTable(true, columnsname);
            return resultTable;
        }

        /// <summary>
        /// 构建数据表
        /// </summary>
        /// <returns></returns>
        public static DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable master = ds.Tables.Add("MASTER");
            master.Columns.Add("BILLMASTERID");
            master.Columns.Add("BILLID");
            master.Columns.Add("BILLCODE");
            master.Columns.Add("STATE");
            master.Columns.Add("BILLDATE");
            master.Columns.Add("TERMINAL");

            DataTable detail = ds.Tables.Add("DETAIL");
            detail.Columns.Add("BILLMASTERID");
            detail.Columns.Add("BILLID");
            detail.Columns.Add("DETAILID");
            detail.Columns.Add("STORAGEID");
            detail.Columns.Add("OPERATECODE");
            detail.Columns.Add("TOBACCONAME");
            detail.Columns.Add("OPERATEPIECE");
            detail.Columns.Add("PIECE");
            detail.Columns.Add("OPERATEITEM");
            detail.Columns.Add("ITEM");
            detail.Columns.Add("CONFIRMSTATE");
            detail.Columns.Add("STATE");
            detail.Columns.Add("MSG");
            detail.Columns.Add("OPERATOR");
            detail.Columns.Add("TARGETSTORAGE");
            return ds;
        }

        #endregion

        #region 处理字符串

        /// <summary>
        /// 处理字符串，截取字符，传来的DataTable和字段
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static string StringMake(DataTable dt, string field)
        {
            string list = "";
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list += row["" + field + ""].ToString() + ",";
                }
                list = list.Substring(0, list.Length - 1);
            }
            return list;
        }

        /// <summary>
        /// 处理字符串,截取字符，传来的DataRow和字段
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public static string StringMake(DataRow[] dr, string field)
        {
            string list = "";
            if (dr.Length != 0)
            {
                foreach (DataRow row in dr)
                {
                    list += row["" + field + ""].ToString() + ",";
                }
                list = list.Substring(0, list.Length - 1);
            }
            return list;
        }

        /// <summary>
        /// 处理字符串，取得字符，传来的String
        /// </summary>
        /// <param name="stringList">字符串</param>
        /// <returns></returns>
        public static string StringMake(string stringList)
        {
            string list = "''";
            string[] arraryList = stringList.Split(',');
            for (int i = 0; i < arraryList.Length; i++)
            {
                list += ",'" + arraryList[i] + "'";
            }
            return list;
        }
        #endregion
    }
}
