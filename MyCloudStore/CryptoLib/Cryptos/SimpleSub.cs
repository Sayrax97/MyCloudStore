using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CryptoLib.Cryptos
{
    public class SimpleSub:ICryptos
    {
        private  byte[] Azbuka { get; set; }
        private static readonly object padlock=new object();
        private static SimpleSub _instance = null;
        private  Dictionary<byte, byte> CryptKeyPair { get; set; }
        private  Dictionary<byte, byte> DecryptKeyPair { get; set; }

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
            var azbuka = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
            Azbuka= new byte[azbuka.Count];
            for (int i = 0; i < azbuka.Count; i++)
            {
                Azbuka[i] = Encoding.ASCII.GetBytes(azbuka[i].ToString())[0];
            }
            char[] shuffled = new char[azbuka.Count];
            byte[] shuffledBytes = new byte[azbuka.Count];
            var rnd = new Random();

            shuffled = azbuka.OrderBy(x => rnd.Next()).ToArray();
            for (int i = 0; i < shuffled.Length; i++)
            {
                shuffledBytes[i] = Encoding.ASCII.GetBytes(shuffled[i].ToString())[0];
            }
            CryptKeyPair = shuffledBytes.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            DecryptKeyPair = shuffledBytes.Zip(Azbuka, (k, v) => new { k, v })
              .ToDictionary(x => x.v, x => x.k);
        }

        public byte[] Encrypt(byte[] value)
        {
            var encrypted = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                if(CryptKeyPair.ContainsKey(value[i]))
                {
                    encrypted[i] = CryptKeyPair[value[i]];
                }
                else
                {
                    encrypted[i] = value[i];
                }
            }
            return encrypted;
        }
        public byte[] Decrypt(byte[] value)
        {
            var decrypted = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                if (DecryptKeyPair.ContainsKey(value[i]))
                {
                    decrypted[i] = DecryptKeyPair[value[i]];
                }
                else
                {
                    decrypted[i] = value[i];
                }
            }
            return decrypted;
        }
        public string Encrypt(string sentence)
        {
            return Encoding.ASCII.GetString(Encrypt(Encoding.ASCII.GetBytes(sentence)));

        }
        public string Decrypt(string sentence)
        {
            return Encoding.ASCII.GetString(Decrypt(Encoding.ASCII.GetBytes(sentence)));

        }
    }
}