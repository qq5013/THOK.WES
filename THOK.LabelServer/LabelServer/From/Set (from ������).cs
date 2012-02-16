using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.Application;
using DataRabbit.HashOrm;

namespace THOK.Application.LabelServer
{
    public partial class Set : Form
    {
        public Set()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Create();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            storagesAccesser.ExecuteNonQuery("delete  from storages ");
        }

        private void Create()
        {
            IList<Storages> ModifiyStorages = new List<Storages>();
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            Storages storage;
            string[] strABCS = {"P"};
            int j = 33;
            foreach (string strABC1 in strABCS)
            {
                int k = 13;
                for (int i = 1; i <= 30; i++)
                {
                    string strABC = strABC1 + "-";
                    storage = new Storages();// A-01-上
                    storage.StorageID = strABC + k.ToString().PadLeft(2, "0"[0]) + "-上";
                    storage.StorageName = strABC + k.ToString().PadLeft(2, "0"[0]) + "-上";
                    storage.Address = j.ToString() + i.ToString().PadLeft(3, "0"[0]);
                    storage.Port = j.ToString();
                    storage.Row = "3";
                    ModifiyStorages.Add(storage);
                    i++;

                    storage = new Storages();// A-01-中
                    storage.StorageID = strABC + k.ToString().PadLeft(2, "0"[0]) + "-中";
                    storage.StorageName = strABC + k.ToString().PadLeft(2, "0"[0]) + "-中";
                    storage.Address = j.ToString() + i.ToString().PadLeft(3, "0"[0]);
                    storage.Port = j.ToString();
                    storage.Row = "2";
                    ModifiyStorages.Add(storage);
                    i++;

                    storage = new Storages();// A-01-下
                    storage.StorageID =   strABC + k.ToString().PadLeft(2, "0"[0]) + "-下";
                    storage.StorageName = strABC + k.ToString().PadLeft(2, "0"[0]) + "-下";
                    storage.Address = j.ToString() + i.ToString().PadLeft(3, "0"[0]);
                    storage.Port = j.ToString();
                    storage.Row = "1";
                    ModifiyStorages.Add(storage);
                    k++;
                }
                j++;
                j++;
            }
            storagesAccesser.Insert<Storages>(ModifiyStorages, false);
        }
    }
}
