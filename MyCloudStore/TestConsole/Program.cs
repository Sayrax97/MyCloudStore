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
        public static UInt32[] ToUInt32Array(Byte[] data, Boolean includeLength)
        {
            Int32 length = data.Length;
            Int32 n = (((length & 3) == 0) ? (length >> 2) : ((length >> 2) + 1));
            UInt32[] result;
            if (includeLength)
            {
                result = new UInt32[n + 1];
                result[n] = (UInt32)length;
            }
            else
            {
                result = new UInt32[n];
            }
            for (Int32 i = 0; i < length; i++)
            {
                result[i >> 2] |= (UInt32)data[i] << ((i & 3) << 3);
            }
            return result;
        }
        public static Byte[] ToByteArray(UInt32[] data, Boolean includeLength)
        {
            Int32 n = data.Length << 2;
            if (includeLength)
            {
                Int32 m = (Int32)data[data.Length - 1];
                n -= 4;
                if ((m < n - 3) || (m > n))
                {
                    return null;
                }
                n = m;
            }
            Byte[] result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                result[i] = (Byte)(data[i >> 2] >> ((i & 3) << 3));
            }
            return result;
        }
        static void Main(string[] args)
        {
            var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            var text2 = "macka";
            var textInt32Array = ToUInt32Array(Encoding.ASCII.GetBytes(text2), false);
            var key = "DusanJankovicbla";
            var keyInt32Array = ToUInt32Array(Encoding.ASCII.GetBytes(key), false);

            //Image im = new Bitmap("C:/Users/MICE/Documents/GitHub/MyCloudStore/MyCloudStore/TestConsole/sven.png");
            //var imageBytes = ImageToByteArray(im);
            //var str = Encoding.ASCII.GetString(imageBytes);
            // Console.WriteLine($"Image to be hashed: {System.Environment.NewLine} sven.png");
            //var x = Encoding.ASCII.GetBytes(text2);
            //var sha =new SHA2();
            //Console.WriteLine($"Text to be hashed: {System.Environment.NewLine} {text}");
            //Console.WriteLine(sha.Hash(x));
            Console.WriteLine(Encoding.ASCII.GetString(
                ToByteArray(
                    XXTEA.Decrypt(XXTEA.Encrypt(textInt32Array, keyInt32Array),keyInt32Array),false)));
            Console.WriteLine(Encoding.ASCII.GetString(ToByteArray(XXTEA.Encrypt(textInt32Array, keyInt32Array),false)));
            Console.WriteLine(Convert.ToBase64String(ToByteArray(XXTEA.Encrypt(textInt32Array, keyInt32Array), false)));
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
