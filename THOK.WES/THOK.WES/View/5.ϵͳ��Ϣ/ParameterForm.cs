using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.Util;
using THOK.ParamUtil;
using THOK.WES.Dal;

namespace THOK.WES.View
{
    public partial class ParameterForm : THOK.AF.View.ToolbarForm
    {
        private Parameter parameter = new Parameter();
        private DBConfigUtil config = new DBConfigUtil("DefaultConnection", "SQLSERVER");
        private DBConfigUtil thokconfig = new DBConfigUtil("WMSConnection", "SQLSERVER");
        private DBConfigUtil zyConfig = new DBConfigUtil("DB2Connection", "DB2");
        private ParameterDal parameterDal = new ParameterDal();
        private ConfigUtil configUtil = new ConfigUtil();
        private Dictionary<string, string> udp = null;
        private Dictionary<string, string> rfid = null;
        private Dictionary<string, string> layers = null;
        private Dictionary<string, string> deviceType = null;
        Dictionary<string, string> dataParam = null;

        public ParameterForm()
        {
            InitializeComponent();
            ReadParameter();        
        }

        private void ReadParameter()
        {
            parameter.ServerName = config.Parameters["server"].ToString();
            parameter.DBName = config.Parameters["database"].ToString();
            parameter.DBUser = config.Parameters["UID"].ToString();
            parameter.Password = config.Parameters["Password"].ToString();

            parameter.THOKServerName = thokconfig.Parameters["server"].ToString();
            parameter.THOKDBName = thokconfig.Parameters["database"].ToString();
            parameter.THOKDBUser = thokconfig.Parameters["UID"].ToString();
            parameter.THOKPassword = thokconfig.Parameters["Password"].ToString();

            parameter.ZyServerName = zyConfig.Parameters["server"].ToString();
            parameter.ZyDBName = zyConfig.Parameters["database"].ToString();
            parameter.ZyDBUser = zyConfig.Parameters["UID"].ToString();
            parameter.ZyPassword = zyConfig.Parameters["Password"].ToString();

            udp = configUtil.GetConfig("UDP");
            parameter.UdpIP = udp["IP"];
            parameter.UdpPort = udp["PORT"];
            
            rfid = configUtil.GetConfig("RFID");
            if (rfid["USEDRFID"] == "0")
                parameter.UsedRFID = false;
            else
                parameter.UsedRFID = true;
            parameter.RfidPort = rfid["PORT"];

            layers = configUtil.GetConfig("Layers");
            parameter.LayersNumber = layers["Number"];          

            dataParam = parameterDal.GetParameter();
            parameter.ApplyURL = dataParam["ApplyURL"];
            parameter.ConfirmURL = dataParam["ConfirmURL"];

            deviceType = configUtil.GetConfig("DeviceType");
            parameter.SelectItem = Convert.ToInt32( deviceType["Device"]);
            propertyGrid.SelectedObject = parameter;            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                config.Parameters["server"] = parameter.ServerName;
                config.Parameters["database"] = parameter.DBName;
                config.Parameters["UID"] = parameter.DBUser;
                config.Parameters["Password"] = parameter.Password;
                config.Save();

                thokconfig.Parameters["server"] = parameter.THOKServerName;
                thokconfig.Parameters["database"] = parameter.THOKDBName;
                thokconfig.Parameters["UID"] = parameter.THOKDBUser;
                thokconfig.Parameters["Password"] = parameter.THOKPassword;
                thokconfig.Save();

                zyConfig.Parameters["server"] = parameter.ZyServerName;
                zyConfig.Parameters["database"] = parameter.ZyDBName;
                zyConfig.Parameters["UID"] = parameter.ZyDBUser;
                zyConfig.Parameters["Password"] = parameter.ZyPassword;
                zyConfig.Save();

                udp["IP"] = parameter.UdpIP;
                udp["PORT"] = parameter.UdpPort;
                configUtil.SaveConfig("UDP", udp);

                rfid["PORT"] = parameter.RfidPort;
                rfid["USEDRFID"] = parameter.UsedRFID ? "1" : "0";
                configUtil.SaveConfig("RFID", rfid);

                layers["Number"] = parameter.LayersNumber;
                configUtil.SaveConfig("Layers", layers);

                deviceType["Device"] = parameter.SelectItem.ToString();
                configUtil.SaveConfig("DeviceType", deviceType);

                dataParam["ApplyURL"] = parameter.ApplyURL;
                dataParam["ConfirmURL"] = parameter.ConfirmURL;
                parameterDal.SaveParameter(dataParam);             

                MessageBox.Show("系统参数保存成功，请重新启动本系统。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存系统参数过程中出现异常，原因：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}

