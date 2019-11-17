﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestConsole.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Upload", ReplyAction="http://tempuri.org/IService1/UploadResponse")]
        string Upload(string fileName, byte[] data, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Upload", ReplyAction="http://tempuri.org/IService1/UploadResponse")]
        System.Threading.Tasks.Task<string> UploadAsync(string fileName, byte[] data, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Download", ReplyAction="http://tempuri.org/IService1/DownloadResponse")]
        byte[] Download(string fileName, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Download", ReplyAction="http://tempuri.org/IService1/DownloadResponse")]
        System.Threading.Tasks.Task<byte[]> DownloadAsync(string fileName, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AllFiles", ReplyAction="http://tempuri.org/IService1/AllFilesResponse")]
        string[] AllFiles(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AllFiles", ReplyAction="http://tempuri.org/IService1/AllFilesResponse")]
        System.Threading.Tasks.Task<string[]> AllFilesAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/FileExists", ReplyAction="http://tempuri.org/IService1/FileExistsResponse")]
        bool FileExists(string fileName, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/FileExists", ReplyAction="http://tempuri.org/IService1/FileExistsResponse")]
        System.Threading.Tasks.Task<bool> FileExistsAsync(string fileName, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/FolderExists", ReplyAction="http://tempuri.org/IService1/FolderExistsResponse")]
        bool FolderExists(string folderName, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/FolderExists", ReplyAction="http://tempuri.org/IService1/FolderExistsResponse")]
        System.Threading.Tasks.Task<bool> FolderExistsAsync(string folderName, string userName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : TestConsole.ServiceReference1.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<TestConsole.ServiceReference1.IService1>, TestConsole.ServiceReference1.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Upload(string fileName, byte[] data, string userName) {
            return base.Channel.Upload(fileName, data, userName);
        }
        
        public System.Threading.Tasks.Task<string> UploadAsync(string fileName, byte[] data, string userName) {
            return base.Channel.UploadAsync(fileName, data, userName);
        }
        
        public byte[] Download(string fileName, string userName) {
            return base.Channel.Download(fileName, userName);
        }
        
        public System.Threading.Tasks.Task<byte[]> DownloadAsync(string fileName, string userName) {
            return base.Channel.DownloadAsync(fileName, userName);
        }
        
        public string[] AllFiles(string userName) {
            return base.Channel.AllFiles(userName);
        }
        
        public System.Threading.Tasks.Task<string[]> AllFilesAsync(string userName) {
            return base.Channel.AllFilesAsync(userName);
        }
        
        public bool FileExists(string fileName, string userName) {
            return base.Channel.FileExists(fileName, userName);
        }
        
        public System.Threading.Tasks.Task<bool> FileExistsAsync(string fileName, string userName) {
            return base.Channel.FileExistsAsync(fileName, userName);
        }
        
        public bool FolderExists(string folderName, string userName) {
            return base.Channel.FolderExists(folderName, userName);
        }
        
        public System.Threading.Tasks.Task<bool> FolderExistsAsync(string folderName, string userName) {
            return base.Channel.FolderExistsAsync(folderName, userName);
        }
    }
}
