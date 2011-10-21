using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.Dal;

namespace THOK.WES
{
    public class WesContext
    {
        private static Dictionary<string, string> parameter = null;
        private static IData data = null;

        public static Dictionary<string, string> Parameters
        {
            get
            {
                if (parameter == null)
                    parameter = new ParameterDal().GetParameter();
                return parameter;
            }
        }

        public static IData GetData()
        {
            if (data == null)
            {
                switch (Parameters["DataCompany"])
                {
                    case "LangChao":
                        data = new LangChaoData();
                        break;
                    case "ZhongYan":
                        data = new ZhongYanData();
                        break;
                    case "THOK":
                        data = new THOKData();
                        break;
                }
            }
            return data;
        }
    }
}
