using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.Interface.Dal;

namespace THOK.WES.Interface
{
    class ZYData : IData
    {
        private ZYDal zyDal = new ZYDal();
        private BillDal billDal = new BillDal();

        #region IData 成员

        public bool ImportData(string billType)
        {
            bool tag = false;

            //下载货位！
            StorageDal storageDal = new StorageDal();
            storageDal.GetCellTable();

            if (billType == "1")
            {
                tag = zyDal.GetInBill();
            }
            else if (billType == "2")
            {
                tag = zyDal.GetOutBill();
            }
            else if (billType == "3")
            {
                tag = zyDal.GetInventoryBill();
            }
            else if (billType == "4")
            {
                tag = zyDal.GetMobileBill();
            }
            return tag;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {
            
            try
            {
                ConfirmResult result = new ConfirmResult(false, "2", "确认失败!");
                string wmsBillID = billID.Remove(billID.Length - 1);

                //更新中烟确认标记和确认时间
                zyDal.ConfirmBill(wmsBillID, detailID);
                result = new ConfirmResult(true, "1", "确认成功!");
                if (result.IsSuccess)
                {
                    billDal.ConfirmTask(billID, detailID, "3", piece, item, result.State, result.Desc, Environment.MachineName);
                }   
                return result;
            }
            catch (Exception)
            {
                return new ConfirmResult(false, "2", "确认失败!");
            }
        }

        #endregion
    }
}
