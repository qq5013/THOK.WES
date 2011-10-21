using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.WES
{
    public interface IData
    {
        bool ImportData(string billType);
        ConfirmResult UploadData(string billType, string billID, string detailID, string operateType, int piece, int item, string rfid);
    }
}
