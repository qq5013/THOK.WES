using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;  
using THOK.ParamUtil;


namespace THOK.WES
{
    public class Parameter: BaseObject
    {
        private string serverName;
        [CategoryAttribute("[01] �ִ�ִ��ϵͳ���ݿ����"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private string dbName;
        [CategoryAttribute("[01] �ִ�ִ��ϵͳ���ݿ����"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        public string DBName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        private string dbUser;
        [CategoryAttribute("[01] �ִ�ִ��ϵͳ���ݿ����"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string DBUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }

        private string password;
        [CategoryAttribute("[01] �ִ�ִ��ϵͳ���ݿ����"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        int _comboBoxItems = 0;
        [Category("[02] �豸����"), Description("�豸"), TypeConverter(typeof(PropertyGridComboBoxItem)), Chinese("�豸����")]
        public int SelectItem
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; }
        }

        private string udpIP;
        [CategoryAttribute("[03] ���ӱ�ǩ����"), DescriptionAttribute("��ǩ�����ַ"), Chinese("��ַIP")]
        public string UdpIP
        {
            get { return udpIP; }
            set { udpIP = value; }
        }

        private string udpPort;
        [CategoryAttribute("[03] ���ӱ�ǩ����"), DescriptionAttribute("��ǩ����˿�"), Chinese("�˿�")]
        public string UdpPort
        {
            get { return udpPort; }
            set { udpPort = value; }
        }

        private bool usedRFID;
        [CategoryAttribute("[04] RFID����"), DescriptionAttribute("ʹ��RFID"), Chinese("ʹ��RFID")]
        public bool UsedRFID
        {
            get { return usedRFID; }
            set { usedRFID = value; }
        }

        private string rfidPort;
        [CategoryAttribute("[04] RFID����"), DescriptionAttribute("RFID�˿�"), Chinese("�˿�")]
        public string RfidPort
        {
            get { return rfidPort; }
            set { rfidPort = value; }
        }

        private string layersNumber;
        [CategoryAttribute("[05] ��ҵ��Χ"), DescriptionAttribute("�����ܲ�������"), Chinese("����")]
        public string LayersNumber
        {
            get { return layersNumber; }
            set { layersNumber = value; }
        }

        private string thokServerName;
        [CategoryAttribute("[06] ŷ���ִ����ݽӿ�"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string THOKServerName
        {
            get { return thokServerName; }
            set { thokServerName = value; }
        }

        private string thokDbName;
        [CategoryAttribute("[06] ŷ���ִ����ݽӿ�"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        public string THOKDBName
        {
            get { return thokDbName; }
            set { thokDbName = value; }
        }

        private string thokDbUser;
        [CategoryAttribute("[06] ŷ���ִ����ݽӿ�"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string THOKDBUser
        {
            get { return thokDbUser; }
            set { thokDbUser = value; }
        }
        private string thokPassword;
        [CategoryAttribute("[06] ŷ���ִ����ݽӿ�"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string THOKPassword
        {
            get { return thokPassword; }
            set { thokPassword = value; }
        }

        private string applyURL;
        [CategoryAttribute("[07] �˳��ִ����ݽӿ�"), DescriptionAttribute("��������ӿڵ�ַ"), Chinese("��������URL")]
        public string ApplyURL
        {
            get { return applyURL; }
            set { applyURL = value; }
        }

        private string confirmURL;
        [CategoryAttribute("[07] �˳��ִ����ݽӿ�"), DescriptionAttribute("����ȷ�Ͻӿڵ�ַ"), Chinese("����ȷ��URL")]
        public string ConfirmURL
        {
            get { return confirmURL; }
            set { confirmURL = value; }
        }

        private string zyServerName;
        [CategoryAttribute("[08] ���ִ̲����ݽӿ�"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string ZyServerName
        {
            get { return zyServerName; }
            set { zyServerName = value; }
        }

        private string zyDbName;

        [CategoryAttribute("[08] ���ִ̲����ݽӿ�"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        public string ZyDBName
        {
            get { return zyDbName; }
            set { zyDbName = value; }
        }

        private string zyDbUser;

        [CategoryAttribute("[08] ���ִ̲����ݽӿ�"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string ZyDBUser
        {
            get { return zyDbUser; }
            set { zyDbUser = value; }
        }
        private string zyPassword;

        [CategoryAttribute("[08] ���ִ̲����ݽӿ�"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string ZyPassword
        {
            get { return zyPassword; }
            set { zyPassword = value; }
        }       
    }

    public abstract class ComboBoxItemTypeConvert : TypeConverter
    {
        public Hashtable _hash = null;
        public ComboBoxItemTypeConvert()
        {
            _hash = new Hashtable();
            GetConvertHash();
        }

        public abstract void GetConvertHash();
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int[] ids = new int[_hash.Values.Count];
            int i = 0;
            foreach (DictionaryEntry myDE in _hash)
            {
                ids[i++] = (int)(myDE.Key);
            }
            return new StandardValuesCollection(ids);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object v)
        {
            if (v is string)
            {
                foreach (DictionaryEntry myDE in _hash)
                {
                    if (myDE.Value.Equals((v.ToString())))
                        return myDE.Key;
                }
            }
            return base.ConvertFrom(context, culture, v);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,object v, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                foreach (DictionaryEntry myDE in _hash)
                {
                    if (myDE.Key.Equals(v))
                        return myDE.Value.ToString();
                }
                return "";
            }
            return base.ConvertTo(context, culture, v, destinationType);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class PropertyGridComboBoxItem : ComboBoxItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "�泵");

            _hash.Add(1, "���ӱ�ǩ");

            _hash.Add(2, "�泵����ӱ�ǩ");
        }
    }   
}
