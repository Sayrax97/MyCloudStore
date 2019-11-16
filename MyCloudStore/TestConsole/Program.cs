﻿using System;
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


            //Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            //var imageBytes = ImageToByteArray(im);
            var fileinfo = new FileInfo(@"D:\audio_sample.mp3");
            var file = File.ReadAllBytes(fileinfo.FullName);

            sv1.Upload($"{Path.GetFileNameWithoutExtension(fileinfo.Name)}.txt", XXTEA.Encrypt(file), "WickeD");
            var filed = sv1.Download($"{Path.GetFileNameWithoutExtension(fileinfo.Name)}.txt", "WickeD");
            var decoded = XXTEA.Decrypt(filed);
            File.WriteAllBytes(@"D:\grapha2.mp3", decoded);
            var x = sv1.AllFiles("WickeD");
            foreach (var item in x)
            {
                Console.WriteLine(item);
            }
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
