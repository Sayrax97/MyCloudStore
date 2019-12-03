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
        private static Dictionary<string, byte[]> _chunks=new Dictionary<string, byte[]>();
        private const int CHUNK_SIZE= 1048576;

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

        public string UploadWithChunks(string fileName, byte[] data, string userName, int chunkId, long length,bool last)
        {
            try
            {
                var path = $@"{_directoryPath}\{userName}";
                if(chunkId==0)
                {
                    if(!_chunks.ContainsKey(userName))
                        _chunks.Add(userName,new byte[length]);
                    else
                    {
                        _chunks[userName]=new byte[length];
                    }
                    Buffer.BlockCopy(data, 0, _chunks[userName], 0,
                        data.Length < CHUNK_SIZE ? data.Length : CHUNK_SIZE);
                }
                else
                {
                    Buffer.BlockCopy(data, 0, _chunks[userName], chunkId*CHUNK_SIZE,
                         last?data.Length:CHUNK_SIZE);
                }

                if (last)
                {
                    Directory.CreateDirectory(path);
                    File.WriteAllBytes($@"{path}\{fileName}", _chunks[userName]);
                }
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

        public byte[] DownloadWithChunks(string fileName, string userName, int chunkId)
        {
            try
            {
                FileInfo fi = FileInfo(fileName, userName);
                var data = new byte[fi.Length - chunkId * CHUNK_SIZE < CHUNK_SIZE ? fi.Length - chunkId * CHUNK_SIZE : CHUNK_SIZE];
                var path = $@"{_directoryPath}\{userName}\{fileName}";
                if (chunkId == 0)
                {
                    if (!_chunks.ContainsKey(userName))
                        _chunks.Add(userName, new byte[fi.Length]);
                    else
                        _chunks[userName] = new byte[fi.Length];
                    Buffer.BlockCopy(File.ReadAllBytes(path),0,_chunks[userName],0,_chunks[userName].Length);
                    Buffer.BlockCopy(_chunks[userName], 0, data, 0,
                        data.Length);
                }
                else
                {
                    Buffer.BlockCopy(_chunks[userName], chunkId*CHUNK_SIZE, data, 0,
                        data.Length);
                }
                return data;

            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
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
        private byte[] Append(byte[] current, byte[] after)
        {
            var bytes = new byte[current.Length + after.Length];
            current.CopyTo(bytes, 0);
            after.CopyTo(bytes, current.Length);
            return bytes;
        }
    }
}
