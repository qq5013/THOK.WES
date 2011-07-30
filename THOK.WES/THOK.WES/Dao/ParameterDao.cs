using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Dao
{
    public class ParameterDao: BaseDao
    {
        //@
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

        //@
        public void UpdateParameter(Dictionary<string, string> parameters)
        {
            foreach (string key in parameters.Keys)
            {
                string sql = string.Format("UPDATE PARAMETER SET PARAMETERVALUE='{0}' WHERE PARAMETERNAME='{1}'", parameters[key], key);
                ExecuteNonQuery(sql);
            }
        }
    }
}
