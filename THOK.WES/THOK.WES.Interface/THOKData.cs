using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dal;

namespace THOK.WES.Interface
{
    public class THOKData :IData
    {
        private THOKDal thokDal = new THOKDal();
        private BillDal billDal = new BillDal();

        #region IData ��Ա

        public bool ImportData(string billType)
        {
            bool tag = false;

            //���ػ�λ��
            StorageDal storageDal = new StorageDal();
            storageDal.GetCellTable();

            switch (billType)
            {
                case "1":
                    tag = thokDal.GetInBill();
                    break;
                case "2":
                    tag = thokDal.GetOutBill();
                    break;
                case "3":
                    tag = thokDal.GetInventoryBill();
                    break;
                case "4":
                    tag = thokDal.GetMoveBill();
                    break;
                default:
                    tag = false;
                    break;
            }
            return tag;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {
            ConfirmResult result = new ConfirmResult(false, "2", "ȷ��ʧ��!"); 
            long billDetailID = Convert.ToInt64(detailID);
            string wmsBillID = billID.Remove(billID.Length - 1);
            switch (operateType)
            {
                case "1":
                    //���ȷ�ϣ�
                    thokDal.ConfirmInBill(wmsBillID, billDetailID, "2");
                    result = new ConfirmResult(true, "1", "ȷ�ϳɹ�!");
                    break;
                case "2": 
                    //����ȷ�ϣ�
                    thokDal.ConfirmOutBill(wmsBillID, billDetailID, "2");
                    result = new ConfirmResult(true, "1", "ȷ�ϳɹ�!");
                    break;
                case "3":
                    //�̵�ȷ�ϣ�
                    thokDal.ConfirmInventoryBill(wmsBillID, billDetailID, "2", piece, item);
                    result = new ConfirmResult(true, "1", "ȷ�ϳɹ�!");
                    break;
                case "6":
                    //�ƿ�ȷ�ϣ�
                    thokDal.ConfirmMoveBill(wmsBillID, billDetailID, "2");
                    result = new ConfirmResult(true, "1", "ȷ�ϳɹ�!");
                    break;
                default:
                    result = new ConfirmResult(false, "2", "ȷ��ʧ��!");
                    break;
            }
            if (result.IsSuccess)
            {
                billDal.ConfirmTask(billID, detailID, "3", piece, item, result.State, result.Desc, Environment.MachineName);
            }            
            return result;
        }        

        #endregion
    }
}
