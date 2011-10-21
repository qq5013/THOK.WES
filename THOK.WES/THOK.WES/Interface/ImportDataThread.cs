using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.View;

namespace THOK.WES
{
    public class ImportDataThread
    {
        private string billType = "";
     
        private IData iData = null;

        public IData IData
        {
            set { iData = value; }
        }
        public string BillType
        {

            set { billType = value; }
        }
        public delegate void ClosePlWailtDelegete();
        public event ClosePlWailtDelegete ClosePlWailt;
        private void OnClosePlWailt()
        {
            ClosePlWailt();
        }

        public void Run()
        {
            iData.ImportData(billType);
            OnClosePlWailt();
        }
    }
}
