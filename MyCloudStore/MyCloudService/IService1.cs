using System;
using System.Collections.Generic;
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
        byte[]  Download(string fileName,string userName);

        [OperationContract]
        string[]  AllFiles(string userName);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {

        [DataMember] public string Name { get; set; }

        [DataMember] public string Extension { get; set; }
    }
}
