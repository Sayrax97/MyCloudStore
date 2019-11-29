using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptoLib.Cryptos;
using TestConsole.ServiceReference1;

namespace TestConsole
{
    class Program
    {
        public static byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();

            
        }
        
        static void Main(string[] args)
        {
            var sv1=new Service1Client();
            var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var text2 = "Zastita informacija";
            Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            var imageBytes = ImageToByteArray(im);
            var file = File.ReadAllBytes(@"C:\Users\MICE\Desktop\ZIProjekat2019.docx");
            
            var enc = SimpleSub.Instance.Encrypt(text);
            var dec = SimpleSub.Instance.Decrypt(enc);
            //var fileinfo = new FileInfo(@"D:\ygopro.avi");
            //var file = File.ReadAllBytes(fileinfo.FullName);
               var x= SHA2.Hash(text.ToUpper());
                var y=SHA2.Hash(dec);
                if (x == y)
                {
                    Console.WriteLine("Radi");
                }
                else
                {
                    Console.WriteLine("Ne radi");
                }


            //sv1.Upload($"{Path.GetFileNameWithoutExtension(fileinfo.Name)}{fileinfo.Extension}", XXTEA.Encrypt(file), "WickeD");
            //var filed = sv1.Download($"{Path.GetFileNameWithoutExtension(fileinfo.Name)}.txt", "WickeD");
            //var decoded = XXTEA.Decrypt(filed);
            //File.WriteAllBytes(@"D:\grapha2.mp3", decoded);

            // var knap = new KnapSack();
            // var sw =System.Diagnostics.Stopwatch.StartNew();
            // var x=knap.Encrypt(txtb);
            // sw.Stop();
            // Console.WriteLine(sw.ElapsedMilliseconds);
            // sw = System.Diagnostics.Stopwatch.StartNew();
            // knap.Decrypt(x);
            // sw.Stop();
            // Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
