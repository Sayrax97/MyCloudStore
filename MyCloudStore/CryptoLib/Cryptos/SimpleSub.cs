using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CryptoLib.Cryptos
{
    public class SimpleSub
    {
        #region withString
        private  List<Char> Azbuka { get; set; }
        private static readonly object padlock=new object();
        private static SimpleSub _instance = null;
        private  Dictionary<Char, Char> CryptKeyPair { get; set; }
        private  Dictionary<Char, Char> DecryptKeyPair { get; set; }

        public static SimpleSub Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance=new SimpleSub();
                    }

                    return _instance;
                }
            }
        }
        private SimpleSub()
        {
            Azbuka = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

            List<Char> shuffled = new List<char>();
            var rnd = new Random();

            shuffled = Azbuka.OrderBy(x => rnd.Next()).ToList();

            CryptKeyPair = shuffled.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            DecryptKeyPair = shuffled.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.v, x => x.k);
        }
        public string Encrypt(string sentence)
        {
            sentence = sentence.ToUpper();
            var encrypted = "";
            foreach (Char item in sentence)
            {
                if (char.IsLetter(item))
                    encrypted += CryptKeyPair[item];
                else
                {
                    encrypted += item;
                }
            }
            return encrypted;

        }
        public string Decrypt(string sentence)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            sentence = sentence.ToUpper();
            var decrypted = "";
            foreach (Char item in sentence)
            {
                if (char.IsLetter(item))
                    decrypted += DecryptKeyPair[item];
                else
                {
                    decrypted += item;
                }
            }
            stopwatch.Stop();
            System.Console.WriteLine($"Milisecunde:{stopwatch.ElapsedMilliseconds}");
            return decrypted;

        }
        #endregion

        #region withByte
        //private static List<Char> Azbuka { get; set; }
        //public static Dictionary<byte,byte> CryptKeyPair { get; set; }
        //public static Dictionary<byte,byte> DecryptKeyPair { get; set; }
        //public SimpleSub()
        //{
        //    Azbuka ="ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

        //    List <Char> shuffled= new List<char>();
        //    var rnd=new Random();

        //    shuffled= Azbuka.OrderBy(x => rnd.Next()).ToList();

        //    CryptKeyPair = shuffled.Zip(Azbuka, ( k, v) => new { k, v })
        //      .ToDictionary(x => (byte)x.k, x => (byte)x.v);
        //    DecryptKeyPair = shuffled.Zip(Azbuka, (k, v) => new { k, v })
        //      .ToDictionary(x => (byte)x.v, x => (byte)x.k);
        //}
        //public string Encrypt(string sentence)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    sentence = sentence.ToUpper();
        //    var encrypted = new byte[sentence.Length];
        //    for (var i=0;i<sentence.Length;i++)
        //    {
        //        if (char.IsLetter(sentence[i]))
        //            encrypted[i]= CryptKeyPair[(byte)sentence[i]];
        //        else
        //        {
        //            encrypted[i] = (byte)sentence[i];
        //        }
        //    }
        //    stopwatch.Stop();
        //    System.Console.WriteLine($"Milisecunde:{stopwatch.ElapsedMilliseconds}");
        //    var x=Encoding.ASCII.GetChars(encrypted);
        //    return string.Join("",x);

        //}
        //public string Decrypt(string sentence)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var decrypted = new byte[sentence.Length];
        //    for (var i=0;i<sentence.Length;i++)
        //    {
        //        if (sentence[i] > 64 && sentence[i] < 91)
        //            decrypted[i] = DecryptKeyPair[(byte)sentence[i]];
        //        else
        //        {
        //            decrypted[i] = (byte)sentence[i];
        //        }
        //    }
        //    stopwatch.Stop();
        //    System.Console.WriteLine($"Milisecunde:{stopwatch.ElapsedMilliseconds}");
        //    var x = Encoding.ASCII.GetString(decrypted);
        //    return x;

        //}
        #endregion
    }
}