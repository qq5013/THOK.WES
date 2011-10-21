using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Dal;

namespace THOK.WES
{
    public class THOKData :IData
    {
        private THOKDal dal = new THOKDal();

        #region IData 成员

        public bool ImportData(string billType)
        {
            bool tag = false;
            switch (billType)
            {
                case "1":
                    tag = dal.GetInBill();
                    break;
                case "2":
                    tag = dal.GetOutBill();
                    break;
                case "3":
                    tag = dal.GetInventoryBill();
                    break;
                case "4":
                    tag = dal.GetMoveBill();
                    break;
                default:
                    break;
            }
            return tag;
        }

        public ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid)
        {
            
            switch (billType)
            {
                case "1":
                    long id = Convert.ToInt64(detailID);
                    //decimal num = Convert.ToDecimal(piece);
                    decimal num = 0.00M;
                    //判断是否零烟
                    if (piece != 0)
                    {
                        num = Convert.ToDecimal(piece);
                    }
                    else
                    {
                        num = Convert.ToDecimal(item);
                    }
                    dal.ConfirmInBill(billID, id, "2", num);
                    return new ConfirmResult(true, "1", "确认成功!");
                    //break;
                case "2":
                    long dtid = Convert.ToInt64(detailID);
                    dal.ConfirmOutBill(billID, dtid, "2");
                    return new ConfirmResult(true, "1", "确认成功!");
                case "3":
                    long did = Convert.ToInt64(detailID);
                    decimal number = 0.00M;
                    if (piece != 0)
                    {
                        number = Convert.ToDecimal(piece);
                    }
                    else
                    {
                        number = Convert.ToDecimal(item);
                    }
                    dal.ConfirmInventoryBill(billID, did, "2",number);
                    return new ConfirmResult(true, "1", "确认成功!");
                case "4":
                    dal.UpdateState(billID, detailID);
                    string detail = detailID.Substring(0, detailID.IndexOf("-"));
                    DataTable dt = dal.ReadMoveBillRow(detail);
                    DataTable sdt = dal.MoveBillStorage(detail);
                    string storageType1 = dal.StorageType(sdt.Rows[0]["StorageID"].ToString());
                    string storageType2 = dal.StorageType(sdt.Rows[1]["StorageID"].ToString());
                    //判断移出移入的是否都为零烟柜
                    if (storageType1 == "0" && storageType2 == "0")
                    {
                        if (dt.Rows[0]["State"].ToString() == "1" && dt.Rows[1]["State"].ToString() == "1")
                        {
                            long detid = Convert.ToInt64(detail);
                            dal.ConfirmMoveBill(billID, detid, "2");
                        }
                    }
                    else
                    {
                        if (dt.Rows.Count == 1)
                        {
                            long detid = Convert.ToInt64(detail);
                            dal.ConfirmMoveBill(billID, detid, "2");
                        }
                    }
                    return new ConfirmResult(true, "1", "确认成功!");
                default:
                    return new ConfirmResult(false, "2", "确认失败!");
            }
        }        

        #endregion
    }
}
