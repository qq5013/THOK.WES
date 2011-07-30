using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.WES.Interface
{
    public class ImportDataThread
    {
        private string billType = "";     
        private IData iData = null;
        public delegate void ClosePlWailtDelegete();
        public event ClosePlWailtDelegete ClosePlWailt;

        public IData IData
        {
            set { iData = value; }
        }
        public string BillType
        {
            set { billType = value; }
        }

        private void OnClosePlWailt()
        {
            if (ClosePlWailt != null)
            {
                ClosePlWailt();
            }           
        }

        public void Run()
        {
            iData.ImportData(billType);
            OnClosePlWailt();
        }
    }
}
