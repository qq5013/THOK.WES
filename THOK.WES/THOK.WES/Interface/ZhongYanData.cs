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
    /// �������̽ӿڱ�
    /// </summary>
    public class ZhongYanData :IData
    {

        private ZhongYanDal dal = new ZhongYanDal();
        //private TaskDal taskDal = new TaskDal();

        public ZhongYanData()
        {
            
        }




        #region IData ��Ա
        //1:��ⵥ 2:���ⵥ 3:�̵㵥 4:�ƿⵥ
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
            //��ѯ�Ƿ������̹�,���ҵ���
          //  taskDal.FindNotOperatingScattered();
            return tag;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {
           
            //��������ȷ�ϱ�Ǻ�ȷ��ʱ��
            try
            {
                dal.ConfirmBill(billID,detailID);
                return new ConfirmResult(true, "1", "ȷ�ϳɹ�!");
            }
            catch (Exception)
            {

                return new ConfirmResult(false, "2", "ȷ��ʧ��!");
            }
            
        }

        #endregion

        
    }
}
