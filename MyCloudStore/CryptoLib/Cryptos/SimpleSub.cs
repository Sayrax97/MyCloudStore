using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
namespace CryptoLib.Cryptos
{
    public class SimpleSub
    {
        private static List<Char> Azbuka { get; set; }
        public static Dictionary<Char,Char> CryptKeyPair { get; set; }
        public static Dictionary<Char,Char> DecryptKeyPair { get; set; }
        public SimpleSub()
        {
            Azbuka ="ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

            List <Char> shuffled= new List<char>();
            var rnd=new Random();

            shuffled= Azbuka.OrderBy(x => rnd.Next()).ToList();

            CryptKeyPair = shuffled.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            DecryptKeyPair = shuffled.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.v, x => x.k);
        }
        public string Encrypt(string sentence)
        {
            sentence=sentence.ToUpper();
            var encrypted="";
            foreach (Char item in sentence)
            {
                if(char.IsLetter(item))
                    encrypted+=CryptKeyPair[item];
                else
                {
                    encrypted+=item;
                }
            }
            return encrypted;

        }
        public string Decrypt(string sentence)
        {
            sentence=sentence.ToUpper();
            var decrypted="";
            foreach (Char item in sentence)
            {
                if(char.IsLetter(item))
                    decrypted+=DecryptKeyPair[item];
                else
                {
                    decrypted+=item;
                }
            }
            return decrypted;

        }
    }
}