using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using THOK.Util;
using THOK.WES.Dal;

namespace THOK.WES
{
    /// <summary>
    /// 访问中烟接口表
    /// </summary>
    public class ZhongYanData :IData
    {

        private ZhongYanDal dal = new ZhongYanDal();
        //private TaskDal taskDal = new TaskDal();

        public ZhongYanData()
        {
            
        }




        #region IData 成员
        //1:入库单 2:出库单 3:盘点单 4:移库单
        public bool ImportData(string billType)
        {
           
            bool tag = false;
            if (billType == "1")
            {
                tag = dal.GetInBill();
            }
            else if(billType == "2")
            {
                tag = dal.GetOutBill();
            }
            else if (billType == "3")
            {
                tag = dal.GetInventoryBill();
            }
            else if (billType == "4")
            {
                tag = dal.GetMobileBill();
            }
            //查询是否有零烟柜,并且点亮
          //  taskDal.FindNotOperatingScattered();
            return tag;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {
           
            //更新中烟确认标记和确认时间
            try
            {
                dal.ConfirmBill(billID,detailID);
                return new ConfirmResult(true, "1", "确认成功!");
            }
            catch (Exception)
            {

                return new ConfirmResult(false, "2", "确认失败!");
            }
            
        }

        #endregion

        
    }
}
