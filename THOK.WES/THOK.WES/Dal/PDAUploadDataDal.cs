using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using OpenNETCF.Desktop.Communication;
using THOK.WES;
using THOK.Util;
using THOK.WES.Dao;
using System.Windows.Forms;
namespace THOK.WES.Dal
{
    public class PDAUploadDataDal
    {
        //@
        public void InsertData()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PDAUploadDataDao dao = new PDAUploadDataDao();
                dao.SetPersistentManager(pm);
                dao.InsertData(this.ReadPadUploadData());
            }
        }
        //@
        public DataTable ReadPadUploadData()
        {
            string uploadDataFileName = @"\DiskOnChip\upLoadFile.xml";
            string LocalFileName = "upLoadFile.xml";
            HHFiles myHH = new HHFiles();
            myHH.CopyFileFromDevice(LocalFileName, uploadDataFileName, true);
            myHH.DeleteFile(uploadDataFileName);
            myHH.Rapi.Disconnect();

            using (PersistentManager pm = new PersistentManager())
            {
                PDAUploadDataDao dao = new PDAUploadDataDao();
                dao.SetPersistentManager(pm);
                DataTable databaseTable = dao.GetDataList();
                DataTable insertTable = databaseTable.Clone();
                 
                XmlDocument doc = new XmlDocument();
                doc.Load(LocalFileName);
                XmlElement root = doc.DocumentElement;

                foreach (XmlElement node in root.ChildNodes)
                {
                    string masterId = node.GetAttribute("masteId");
                    string detailId = node.GetAttribute("detailId");

                    if ((databaseTable.Select("BillMasterId = '" + masterId + "'AND  BillDetailId ='" + detailId+"'").Length == 0) &&
                         (insertTable.Select("BillMasterId = '" + masterId + "'AND BillDetailId ='" + detailId+"'").Length == 0))
                    {
                        DataRow row = insertTable.NewRow();
                        row["BillMasterId"] = masterId;
                        row["BillDetailId"] = detailId;
                        row["IsUpload"] = 0;
                        insertTable.Rows.Add(row);

                        if (false)//todo 未完成PDA修改
                        {
                            int piece = Convert.ToInt32(node.GetAttribute("OPERATEPIECE"));
                            int item = Convert.ToInt32(node.GetAttribute("OPERATEITEM"));
                            BillDao billDao = new BillDao();
                            billDao.ConfirmTask(masterId, detailId, "3", piece, item, "1", "操作成功", "PDA");
                        }
                    }
                }

                File.Delete(LocalFileName);
                return insertTable;
            }
        }
        //@
        public bool IsConnetion()
        {
            RAPI rApi = null;
            try
            {
                bool isConnetion = true;
                rApi = new RAPI();
                isConnetion = rApi.DevicePresent;
                rApi.Disconnect();
                return isConnetion;
            }
            catch (Exception ex)
            {
                rApi = null;
                MessageBox.Show(string.Format("连接PDA失败，程序将自动退出，请检查PDA驱动和连接状态！错误信息：{0}",ex.Message));
                Application.Exit();
                return false;
            }
        }
    }
}
