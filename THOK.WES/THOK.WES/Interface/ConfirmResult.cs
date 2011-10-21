using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.WES
{
    public class ConfirmResult
    {
        private bool isSuccess = false;
        private string state = "0";
        private string desc = "";

        public ConfirmResult(bool isSuccess, string state, string desc)
        {
            this.isSuccess = isSuccess;
            this.state = state;
            this.desc = desc;
        }

        public bool IsSuccess
        {
            get { return isSuccess; }
        }

        public string State
        {
            get { return state; }
        }

        public string Desc
        {
            get { return desc; }
        }
    }
}
