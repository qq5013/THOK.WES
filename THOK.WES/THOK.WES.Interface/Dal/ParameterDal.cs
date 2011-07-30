using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Interface.Dao;

namespace THOK.WES.Interface.Dal
{
    public class ParameterDal
    {
        public Dictionary<string, string> GetParameter()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ParameterDao parameterDao = new ParameterDao();
                return parameterDao.FindParameter();
            }
        }   
    }
}
