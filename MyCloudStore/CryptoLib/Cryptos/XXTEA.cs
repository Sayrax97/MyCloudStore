using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
namespace CryptoLib.Cryptos
{
    public class XXTEA
    {
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        private const UInt32 delta = 0x9E3779B9;

        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }
        public static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {

            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            UInt32 z = v[n], y, sum = 0, e;
            Int32 i, rounds = 6 + 52 / (n + 1);
            unchecked
            {
                while (0 < rounds--)
                {
                    sum += delta;
                    e = sum >> 2 & 3;
                    for (i = 0; i < n; i++)
                    {
                        y = v[i + 1];
                        z = v[i] += MX(sum, y, z, i, e, k);
                    }
                    y = v[0];
                    z = v[n] += MX(sum, y, z, i, e, k);
                }
            }
            return v;
        }
    }
}