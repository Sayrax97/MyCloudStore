using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            //var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            var imageBytes = ImageToByteArray(im);
            var str = Encoding.ASCII.GetString(imageBytes);
            Console.WriteLine($"Image to be hashed: {System.Environment.NewLine} sven.png");
            //var x = Encoding.ASCII.GetBytes(text);
            var sha =new SHA2();
            Console.WriteLine("Hashed:");
            Console.WriteLine(sha.Encrypt(imageBytes));
            // var Knapsack = new KnapSack();
            // // byte[] bytes=Encoding.ASCII.GetBytes(text);
            // var x = Knapsack.Encrypt(imageBytes);
            // //var en = Encoding.ASCII.GetString(x).Split(' ');
            // //System.Console.WriteLine(string.Join(" ", en));
            // var y = Knapsack.Decrypt(x);
            // var de = Encoding.ASCII.GetString(y).Split(' ');
            // System.Console.WriteLine(string.Join(" ", de));

            //var SS=new SimpleSub();
            //var x = SS.Encrypt(text);
            //System.Console.WriteLine(x);
            //var y=SS.Decrypt(x);
            //System.Console.WriteLine(y);
            //System.Console.WriteLine(SS.Encrypt(text));
        }
    }
}
