using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static byte[] Append(byte[] current, byte[] after)
        {
            var bytes = new byte[current.Length + after.Length];
            current.CopyTo(bytes, 0);
            after.CopyTo(bytes, current.Length);
            return bytes;
        }

        static void Main(string[] args)
        {
            var sv1 = new Service1Client();
            var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var text2 = "Zastita informacija";
            Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            var imageBytes = ImageToByteArray(im);
            var file = File.ReadAllBytes(@"C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            //var ost = file.Length % 4;
            //var fileFixed = new byte[file.Length+ost];
            //file.CopyTo(fileFixed,0);
            var ch = Encoding.ASCII.GetBytes("X");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var kn = SimpleSub.Instance;
            var enc = kn.Encrypt(file);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            File.WriteAllBytes(@"C:/Users/MICE/Desktop/sven.png",kn.Decrypt(enc));

            kn = SimpleSub.Instance;
            enc = kn.Encrypt(Encoding.ASCII.GetBytes(text2));
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(Encoding.ASCII.GetString(enc));
            var file1 = Append(enc, ch);
            var lastB = file1.Length - 1;
            var dec2 = new byte[lastB];
            Buffer.BlockCopy(file1, 0, dec2, 0, lastB);
            var dec = kn.Decrypt(dec2);
            //var xx = new byte[dec.Length - 1];
            //Buffer.BlockCopy(dec,0,xx,0,dec.Length-1);
            //File.WriteAllBytes(@"C:\Users\MICE\Desktop\sven.png", dec2);
            //var fileinfo = new FileInfo(@"D:\ygopro.avi");
            //var file = File.ReadAllBytes(fileinfo.FullName);

                //var sw =System.Diagnostics.Stopwatch.StartNew();
                //var x = SHA2.Hash(file);
                //var y=SHA2.Hash(dec);
                //sw.Stop();
                //Console.WriteLine(sw.ElapsedMilliseconds);
                //if (x == y)
                //{
                //    Console.WriteLine("Radi");
                //}
                //else
                //{
                //    Console.WriteLine("Ne radi");
                //}


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
