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
        private const string RootFolder= @"C:\Users\MICE\Documents\GitHub\MyCloudStore\MyCloudStore\MyCloudService\Temp";


        public string Upload(string fileName, byte[] data, string userName)
        {
            try
            {
                var path = $"{RootFolder}/{userName}";
                Directory.CreateDirectory(path);
                File.WriteAllBytes($"{path}/{fileName}", data);
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
                return File.ReadAllBytes($"{RootFolder}/{userName}/{fileName}");
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }
        }

        public string[] AllFiles(string userName)
        {
            var path = $"{RootFolder}/{userName}";
            string[] files=Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }

            return files;
        }

        public bool FileExists(string fileName, string userName)
        {
            var path = $"{RootFolder}/{userName}";
            return File.Exists($"{path}/{fileName}");
        }
        public bool FolderExists(string folderName, string userName)
        {
            var path = $"{RootFolder}/{userName}";
            return Directory.Exists($"{path}/{folderName}");
        }
    }
}
