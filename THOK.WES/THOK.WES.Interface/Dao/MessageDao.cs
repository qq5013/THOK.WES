using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WES.Interface.Dao
{
    public class MessageDao: BaseDao
    {
        public void InsertMessage(DateTime requestTime, string request, DateTime responseTime, string response, string pcname)
        {
            SqlCreate sqlCreate = new SqlCreate("Messages", SqlType.INSERT);
            sqlCreate.AppendQuote("REQUESTMSG", request);
            sqlCreate.AppendQuote("REQUESTTIME", requestTime.ToString());
            sqlCreate.AppendQuote("RESPONSEMSG", response);
            sqlCreate.AppendQuote("RESPONSETIME", responseTime);
            sqlCreate.AppendQuote("PCNAME", pcname);
            ExecuteNonQuery(sqlCreate.GetSQL());
        }
    }
}
