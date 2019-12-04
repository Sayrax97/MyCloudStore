using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyCloudService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string  Upload(string fileName,byte[] data,string userName);
        [OperationContract]
        string  UploadWithChunks(string fileName,byte[] data,string userName,int chunkId,long length,bool last);
        
        [OperationContract]
        byte[]  Download(string fileName,string userName);

        [OperationContract]
        byte[] DownloadWithChunks(string fileName, string userName, int chunkId);

        [OperationContract]
        string[]  AllFiles(string userName);
        [OperationContract]
        bool FileExists(string fileName,string userName);
        [OperationContract]
        bool FolderExists(string folderName,string userName);
        [OperationContract]
        FileInfo FileInfo(string fileName, string userName);
        [OperationContract]
        void RenameFile(string fileName, string userName,string newName);
        [OperationContract]
        void DeleteFile(string fileName, string userName);
        [OperationContract]
        double StorageLeft(string userName);

        [OperationContract]
        bool Login(string userName,string password);

        [OperationContract]
        void CreateAccount(string userName, string password);

        [OperationContract]
        string GetFileHash(string userName,string fileName);

        [OperationContract]
        void SetFileHash(string userName, string fileName,string hash);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {

        [DataMember] public string Name { get; set; }

        [DataMember] public string Extension { get; set; }
    }
}
