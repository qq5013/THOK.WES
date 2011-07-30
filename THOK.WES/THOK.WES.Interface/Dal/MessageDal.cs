using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.WES.Interface.Dao;

namespace THOK.WES.Interface.Dal
{
    public class MessageDal
    {
        public void AddMessage(DateTime requestTime, string request, DateTime responseTime, string response, string pcname)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                MessageDao messageDao = new MessageDao();
                messageDao.InsertMessage(requestTime, request, responseTime, response, pcname);
            }
        }
    }
}
