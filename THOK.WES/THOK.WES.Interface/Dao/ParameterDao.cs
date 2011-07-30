using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class ParameterDao: BaseDao
    {
        public Dictionary<string, string> FindParameter()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string sql = "SELECT * FROM PARAMETER";
            DataTable table = ExecuteQuery(sql).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                parameters.Add(row["PARAMETERNAME"].ToString(), row["PARAMETERVALUE"].ToString());
            }
            return parameters;
        }
    }
}
