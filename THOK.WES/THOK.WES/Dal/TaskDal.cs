using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Dao;

namespace THOK.WES.Dal
{
    public class TaskDal
    {   
        //@
        public void SetTask(string billId, string detailId, string storageId,string targetStorageName,string operateType,string tobaccoName, int piece, int item, string taskType)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao taskDao = new TaskDao();
                switch (taskType)
                {
                    case "1"://µãÁÁ
                        taskDao.OperateToLabelServer(billId, detailId, storageId, operateType, tobaccoName, piece, item, targetStorageName);
                        break;
                    case "2"://Ï¨Ãð
                        taskDao.CancelOperateToLabelServer(billId, detailId, storageId);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
