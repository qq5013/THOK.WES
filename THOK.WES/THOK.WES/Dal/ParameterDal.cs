using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WES.Dao;
namespace THOK.WES.Dal
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

        public void SaveParameter(Dictionary<string, string> parameters)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ParameterDao parameterDao = new ParameterDao();
                parameterDao.UpdateParameter(parameters);
            }
        }        
    }
}
