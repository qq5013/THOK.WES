using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.WES.Dao;
using System.Data;

namespace THOK.WES.Dal
{
    public class ShelfDal
    {
        /// <summary>
        /// @查询仓库货位数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetShelf()
        {
            using (PersistentManager pm = new PersistentManager("WMSConnection"))
            {
                ShelfDao shelfDao = new ShelfDao();
                shelfDao.SetPersistentManager(pm);
                return shelfDao.Find();
            }
        }

        /// <summary>
        /// @查询仓库总数量
        /// </summary>
        /// <returns></returns>
        public int GetSumQuantity()
        {
            using (PersistentManager pm = new PersistentManager("WMSConnection"))
            {
                ShelfDao shelfDao = new ShelfDao();
                shelfDao.SetPersistentManager(pm);
                return shelfDao.FindSumQuantity();
            }
        }

        public int GetBillDetailSumPiece()
        {
            using (PersistentManager pm = new PersistentManager("WMSConnection"))
            {
                ShelfDao shelfDao = new ShelfDao();
                shelfDao.SetPersistentManager(pm);
                return shelfDao.FindBillDetailSumPiece();
            }
        }
    }
}
