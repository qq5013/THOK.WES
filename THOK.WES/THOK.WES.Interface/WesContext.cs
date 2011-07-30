using System;
using System.Collections.Generic;
using System.Text;
using THOK.WES.Interface.Dal;
using System.Reflection;

namespace THOK.WES.Interface
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
                lock (Parameters)
                {
                    if (data == null)
                    {
                        string strDataType = "THOK.WES.Interface.{0}";
                        strDataType = string.Format(strDataType, Parameters["DataCompany"]);
                        data = (IData)Create("THOK.WES.Interface.dll",strDataType);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// ���򼯻��档
        /// </summary>
        private static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        /// <summary>
        /// ���������͵�һ��ʵ����
        /// </summary>
        /// <param name="assemblyFile">�����ļ���</param>
        /// <param name="typeName">Ҫ����������</param>
        /// <returns>Ҫ���������͵�һ��ʵ��</returns>
        private static object Create(string assemblyFile, string typeName)
        {
            Assembly assembly = null;
            if (assemblies.ContainsKey(assemblyFile))
            {
                assembly = assemblies[assemblyFile];
            }
            else
            {
                lock (assemblies)
                {
                    assembly = Assembly.LoadFrom(assemblyFile);
                    assemblies.Add(assemblyFile, assembly);
                }
            }
            return assembly.CreateInstance(typeName);
        }
    }
}
