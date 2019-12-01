using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace MyCloudService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private string _directoryPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath+"Temp";


        public string Upload(string fileName, byte[] data, string userName)
        {
            try
            {
                var path = $@"{_directoryPath}\{userName}";
                Directory.CreateDirectory(path);
                File.WriteAllBytes($@"{path}\{fileName}", data);
                return "Uploaded";
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public byte[] Download(string fileName, string userName)
        {
            try
            {
                return File.ReadAllBytes($@"{_directoryPath}\{userName}\{fileName}");
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }
        }

        public string[] AllFiles(string userName)
        {
            var path = $@"{_directoryPath}\{userName}";
            Directory.CreateDirectory(path);
            string[] files=Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }

            return files;
        }

        public bool FileExists(string fileName, string userName)
        {
            var path = $@"{_directoryPath}\{userName}";
            return File.Exists($@"{path}\{fileName}");
        }
        public bool FolderExists(string folderName, string userName)
        {
            var path = $@"{_directoryPath}\{userName}";
            return Directory.Exists($@"{path}\{folderName}");
        }

        public FileInfo FileInfo(string fileName, string userName)
        {
            var path = $@"{_directoryPath}\{userName}\{fileName}";
            return new FileInfo(path);
        }

        public void RenameFile(string fileName, string userName, string newName)
        {
            var path1 = $@"{_directoryPath}\{userName}\{fileName}";
            var path2 = $@"{_directoryPath}\{userName}\{newName}";
            File.Move(path1, path2);
        }

        public void DeleteFile(string fileName, string userName)
        {
            var path1 = $@"{_directoryPath}\{userName}\{fileName}";
            File.Delete(path1);
        }

        public double StorageLeft(string userName)
        {
            var  maxStorage = 2147483648;
            var path = $@"{_directoryPath}\{userName}";
            double size = 0;
            var files=Directory.GetFiles(path, "*.*");
            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                size +=fi.Length;
            }

            double left = maxStorage - size;
            return left;
        }
    }
}
