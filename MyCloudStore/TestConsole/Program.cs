using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoLib.Cryptos;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var SS=new SimpleSub();
            String text="This question might appear to be a duplicate and/or too boring, but I want to do this using this specific method.";
            System.Console.WriteLine($"Teks koji se kodira:\n {text}");
            var e=SS.Encrypt(text);
            System.Console.WriteLine("Crypt");
            Console.WriteLine(e);
            var de=SS.Decrypt(e);
            System.Console.WriteLine("Decrypt");
            Console.WriteLine(de);

            return;
        }
    }
}
