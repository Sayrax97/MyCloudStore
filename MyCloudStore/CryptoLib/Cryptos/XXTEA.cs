using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
namespace CryptoLib.Cryptos
{
    /// <summary>
    /// XXTEA algorithm for encryption and decryption
    /// </summary>
    public class XXTEA
    {
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        private const UInt32 delta = 0x9E3779B9;
        private const string key = "DusanJankovic";

        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }
        private static UInt32[] Encrypt(UInt32[] v)
        {
            var k= ToUInt32Array(Encoding.ASCII.GetBytes(FixKey(key)), false);
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
        private static UInt32[] Decrypt(UInt32[] v)
        {
            var k = ToUInt32Array(Encoding.ASCII.GetBytes(FixKey(key)), false);
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            UInt32 z, y = v[0], sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            unchecked
            {
                sum = (UInt32)(q * delta);
                while (sum != 0)
                {
                    e = sum >> 2 & 3;
                    for (p = n; p > 0; p--)
                    {
                        z = v[p - 1];
                        y = v[p] -= MX(sum, y, z, p, e, k);
                    }
                    z = v[n];
                    y = v[0] -= MX(sum, y, z, p, e, k);
                    sum -= delta;
                }
            }
            return v;
        }
        private static UInt32[] ToUInt32Array(Byte[] data, Boolean includeLength)
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
        private static Byte[] ToByteArray(UInt32[] data, Boolean includeLength)
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
        private static string FixKey(string k)
        {
            var key = Encoding.ASCII.GetBytes(k);
            if (key.Length == 16) return Encoding.ASCII.GetString(key);
            Byte[] fixedkey = new Byte[16];
            if (key.Length < 16)
            {
                key.CopyTo(fixedkey, 0);
            }
            else
            {
                Array.Copy(key, 0, fixedkey, 0, 16);
            }
            return Encoding.ASCII.GetString(fixedkey);
        }
        /// <summary>
        /// XXTEA encryption method
        /// </summary>
        /// <param name="data">Data to be encrypted-ByteArray</param>
        public static byte[] Encrypt(byte[] data)
        {
            return ToByteArray(Encrypt(ToUInt32Array(data, false)),false);
        }
        /// <summary>
        /// XXTEA encryption method
        /// </summary>
        /// <param name="data">Data to be encrypted-string</param>
        public static byte[] Encrypt(string data)
        {
            return ToByteArray(Encrypt(ToUInt32Array(Encoding.ASCII.GetBytes(data), false)), false);
        }
        /// <summary>
        /// XXTEA decryption method
        /// </summary>
        /// <param name="data">Data to be decrypted-ByteArray</param>
        public static byte[] Decrypt(byte[] data)
        {
            return ToByteArray(Decrypt(ToUInt32Array(data, false)), false);
        }
        
    }
}