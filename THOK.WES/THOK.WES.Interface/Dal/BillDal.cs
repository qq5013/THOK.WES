using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.WES.Interface.Dao;

namespace THOK.WES.Interface.Dal
{
    class BillDal
    {
        //@
        public void ConfirmTask(string billID, string detailID, string confirmState, int piece, int item, string state, string msg, string operater)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BillDao billDao = new BillDao();
                billDao.ConfirmTask(billID, detailID, confirmState, piece, item, state, msg, operater);
            }
        }        
    }
}
