using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
namespace CryptoLib.Cryptos
{
    public class KnapSack
    {

        private const byte DataLength = 8;

        private int[] _privateKey;
        private int[] _publicKey;

        private int _n;
        private int _m;
        private int _mInverse;


        public KnapSack()
        {
            _n = 491;
            _m = 41;
            _mInverse = 12;

            _privateKey = new int[] { 2, 3, 7, 14, 30, 57, 120, 251 };
            _publicKey = new int[] { 82, 123, 287, 83, 248, 373, 10, 471 };

        }

        private void ResetPublicKey()
        {
            for (int i = 0; i < DataLength; i++)
                _publicKey[i] = (_privateKey[i] * _m) % _n;
        }
        public bool SetKey(int[] input)
        {
            int sum = 0;
            for (int i =0; i < DataLength; i++)
            {
                if (input[i] <= sum)
                    throw new ArgumentException("Knapsack key needs to be superincreasing");
                sum += input[i];
            }

            // Set private key
            _privateKey = input;

            // Set public key
            ResetPublicKey();

            return true;

        }

        public byte[] Encrypt(byte[] input)
        {

            Stopwatch sw=new Stopwatch();
            sw.Start();
            var length = input.Length;

            var result = new int[length];

            for (var i = 0; i < length; i++)
            {
                var inputByte = new [] { input[i] };
                var bits = new BitArray(inputByte);
                for (var j = 0; j < DataLength; j++)
                    if (bits[DataLength- 1 - j])
                        result[i] += _publicKey[j];
            }

            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString());
                if (i != result.Length - 1)
                    sb.Append(" ");
            }
            sw.Stop();
            System.Console.WriteLine($"Milisecunde:{sw.ElapsedMilliseconds}");
            
            return Encoding.ASCII.GetBytes(sb.ToString());

        }

        public byte[] Decrypt(byte[] output)
        {
            Stopwatch sw=new Stopwatch();
            sw.Start();
            var stringOutput = Encoding.ASCII.GetString(output).Split(' ');

            var array = Array.ConvertAll(stringOutput, uint.Parse);

            var length = array.Length;

            var result = new byte[length];

            for (var i = 0; i < length; i++)
            {
                var TC = (array[i] * _mInverse) % _n;

                var bits = new BitArray(DataLength);

                for (var j = DataLength - 1; j >= 0; j--)
                {
                    if(TC <= 0)
                        break;
                    if (TC  >= _privateKey[j])
                    {
                        bits[DataLength - 1 - j] = true;
                        TC -= _privateKey[j];
                    }
                    else
                    {
                        bits[DataLength - 1 - j] = false;
                    }
                }

                bits.CopyTo(result, i);

            }
            sw.Stop();
            System.Console.WriteLine($"Milisecunde:{sw.ElapsedMilliseconds}");
            return result;
        }
    }
}