using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenNETCF.Desktop.Communication;

namespace THOK.WES
{
    public class HHFiles
    {
        private RAPI rapi = null;

        public HHFiles()
        {
            rapi = new RAPI();
            try
            {
                rapi.Connect(false);
            }
            catch
            {
                throw new Exception("Not able to connect to handheld device");
            }
        }

        public FileList MyDocuments
        {
            get
            {
                return rapi.EnumFiles("My Documents");
            }
        }

        public RAPI Rapi
        {
            get
            {
                return rapi;
            }
        }

        public FileList GetFileList(string path)
        {
            return Rapi.EnumFiles(path);
        }

        public void DeleteFilesFromDevice(string deviceStartingDirectory,
            string fileMask,
            bool includeSubDirectories)
        {
            FileList deviceDirectory = GetFileList(deviceStartingDirectory);

            if (deviceDirectory == null || deviceDirectory.Count != 1)
            {
                throw new System.IO.FileNotFoundException("Invalid Device Directory", deviceStartingDirectory);
            }

            FileList directoryList = GetFileList(deviceStartingDirectory + "\\*");

            foreach (FileInformation dirInfo in directoryList)
            {
                if (dirInfo.FileAttributes == (int)FileAttributes.Directory)
                {
                    if (!includeSubDirectories) continue;

                    string newDeviceDirectory =
                        deviceStartingDirectory + "\\" + dirInfo.FileName;
                    DeleteFilesFromDevice(newDeviceDirectory, fileMask, includeSubDirectories);
                }
                else
                {
                    if (!MatchesFileMask(dirInfo.FileName, fileMask)) continue;
                    string newDeviceFile =
                        deviceStartingDirectory + "\\" + dirInfo.FileName;
                    DeleteFile(newDeviceFile);
                }
            }
        }

        public void CopyFilesFromDevice(string localStartingDirectory,
            string deviceStartingDirectory,
            string fileMask,
            bool includeSubDirectories,
            bool overwrite)
        {
            CreateLocalStartingDirectory(localStartingDirectory);

            FileList deviceDirectory = GetFileList(deviceStartingDirectory);

            if (deviceDirectory == null || deviceDirectory.Count != 1)
            {
                throw new System.IO.FileNotFoundException("Invalid Device Directory", deviceStartingDirectory);
            }

            FileList directoryList = GetFileList(deviceStartingDirectory + "\\*");

            foreach (FileInformation dirInfo in directoryList)
            {
                if (dirInfo.FileAttributes == (int)FileAttributes.Directory)
                {
                    if (!includeSubDirectories) continue;

                    string newDeviceDirectory =
                        deviceStartingDirectory + "\\" + dirInfo.FileName;
                    string newLocalDirectory =
                        localStartingDirectory + "\\" + dirInfo.FileName;
                    CopyFilesFromDevice(newLocalDirectory,
                        newDeviceDirectory, fileMask, includeSubDirectories, overwrite);
                }
                else
                {
                    if (!MatchesFileMask(dirInfo.FileName, fileMask)) continue;
                    string newDeviceFile =
                        deviceStartingDirectory + "\\" + dirInfo.FileName;
                    string newLocalFile =
                        localStartingDirectory + "\\" + dirInfo.FileName;
                    CopyFileFromDevice(newLocalFile, newDeviceFile, overwrite);
                }
            }
        }

        private static void CreateLocalStartingDirectory(string localStartingDirectory)
        {
            if (!Directory.Exists(localStartingDirectory))
            {
                Directory.CreateDirectory(localStartingDirectory);
            }
        }



        public bool MatchesFileMask(string fileName, string fileMask)
        {
            if (fileMask.Length == 0) return true;
            if (fileName.ToLower().EndsWith(fileMask.ToLower())) return true;
            return false;
        }

        public void CopyFileFromDevice(string localFilePath, string deviceFilePath, bool overwrite)
        {
            Rapi.CopyFileFromDevice(localFilePath, deviceFilePath, overwrite);
        }

        public void CopyFileToDevice(string localFilePath, string deviceFilePath, bool overwriteExisting)
        {
            FileList file = GetFileList(deviceFilePath);
            if (file != null && overwriteExisting == false)
            {
                if (file.Count == 1)
                {
                    throw new Exception("Cannot overwrite existing file at: " + deviceFilePath);
                }
            }
            else if (file != null && file.Count == 1)
            {
                Rapi.DeleteDeviceFile(deviceFilePath);
            }

            Rapi.CopyFileToDevice(localFilePath, deviceFilePath);
        }

        public void DeleteFile(string deviceFilePath)
        {
            if (Rapi.DeviceFileExists(deviceFilePath))
            {
                Rapi.DeleteDeviceFile(deviceFilePath);
            }

        }

        public DateTime FileCreated(string deviceFilePath)
        {
            FileList file = GetFileList(deviceFilePath);

            if (file == null)
            {
                throw new System.IO.FileNotFoundException("File Not Found", deviceFilePath);
            }

            return file[0].CreateTime;
        }

        public DateTime FileUpdated(string deviceFilePath)
        {
            FileList file = GetFileList(deviceFilePath);

            if (file == null)
            {
                throw new System.IO.FileNotFoundException("File Not Found", deviceFilePath);
            }

            return file[0].LastWriteTime;
        }

        public DateTime FileAccessed(string deviceFilePath)
        {
            FileList file = GetFileList(deviceFilePath);

            if (file == null)
            {
                throw new System.IO.FileNotFoundException("File Not Found", deviceFilePath);
            }

            return file[0].LastAccessTime;
        }
    }
}
