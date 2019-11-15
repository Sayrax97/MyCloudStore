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
            var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var text2 = "Zastita informacija";


            Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            var txt = File.ReadAllText("C:/Users/MICE/Desktop/test.txt.txt");
            var imageBytes = ImageToByteArray(im);
            var txtb = Encoding.ASCII.GetBytes(txt);
            var sw=System.Diagnostics.Stopwatch.StartNew();
            var x = XXTEA.Encrypt(txt);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            //Console.WriteLine(Encoding.ASCII.GetString(x));
            sw = System.Diagnostics.Stopwatch.StartNew();
            var y=Encoding.ASCII.GetString(XXTEA.Decrypt(x));
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
