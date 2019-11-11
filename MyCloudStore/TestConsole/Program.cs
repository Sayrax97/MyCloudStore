using System;
using System.Collections;
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
            var text= "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            // var Knapsack= new KnapSack();
            // byte[] bytes=Encoding.ASCII.GetBytes(text);
            // var x=Knapsack.Encrypt(bytes);
            // var en=Encoding.ASCII.GetString(x).Split(' ');
            // System.Console.WriteLine(string.Join(" ",en));
            // var y=Knapsack.Decrypt(x);
            // var de=Encoding.ASCII.GetString(y).Split(' ');
            // System.Console.WriteLine(string.Join(" ",de));
            var SS=new SimpleSub();
            var x = SS.Encrypt(text);
            System.Console.WriteLine(x);
            var y=SS.Decrypt(x);
            System.Console.WriteLine(y);
            //System.Console.WriteLine(SS.Encrypt(text));
        }
    }
}
