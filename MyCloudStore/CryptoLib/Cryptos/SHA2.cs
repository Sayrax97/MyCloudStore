
using System.Collections;
using System.Linq;
using System;
using System.Diagnostics;
using System.Text;


namespace CryptoLib.Cryptos
{
    /// <summary>
    /// SHA2 hashing algorithm
    /// </summary>
    public class SHA2
    {
        private static int INT_BITS = 32;
        private static UInt32[] H=
        {
            0x6a09e667,0xbb67ae85,0x3c6ef372,0xa54ff53a,0x510e527f,0x9b05688c,0x1f83d9ab,0x5be0cd19
        };
        private static UInt32[] K=
        {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b,0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74,0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f,0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3,0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354,0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819,0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3,0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa,0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        /// <summary>
        /// SHA2 hashing method
        /// </summary>
        /// <param name="message">Message to be hashed</param>
        public static string Hash(byte[] message)
        {
            var bitMessage = new BitArray(message);
            bitMessage = Append(bitMessage, new BitArray(new[] { true }));
            var bitsLength = bitMessage.Length + 1;
            var remainder = bitsLength % 512;
            if(remainder<=448)
                remainder = 448 - remainder;
            else
            {
                remainder = 512 - remainder + 448;
            }
            bitMessage = Append(bitMessage, new BitArray(remainder, false));
            UInt64 integer64 = (ulong)bitMessage.Length;
            var bit64Array = new BitArray(BitConverter.GetBytes(integer64));
            bitMessage = Append(bitMessage, new BitArray(bit64Array));

            //var bitMessage = new BitArray(message);
            //var bitsLength = bitMessage.Length + 1;
            //var remainder = bitsLength % 512;
            //bitsLength += 448 - remainder;

            //var bits =new BitArray(bitsLength+64);
            //bits.Set(bitMessage.Length, true);

            //for (int i = 0; i < bitMessage.Length; i++)
            //{
            //    bits[i] = bits[i] | bitMessage[i];
            //}
            //UInt64 integer64 = (ulong)bitMessage.Length;
            //var bit64Array = new BitArray(BitConverter.GetBytes(integer64));
            //for (int i = 0, j=448; i < bit64Array.Length; i++,j++)
            //{
            //    bitMessage[j] = bitMessage[j] | bit64Array[i];
            //}
            //Process the message in successive 512-bit chunks
            int nOfchunks = bitMessage.Length/512;
            var chunkBoolArray = new bool[bitMessage.Length];
            bitMessage.CopyTo(chunkBoolArray, 0);
            for (int i = 0; i < nOfchunks; i++)
            {
                var chunk = new bool[512];
                Buffer.BlockCopy(chunkBoolArray,i*512,chunk,0,512);
                //var chunk = new BitArray(chunkBoolArray.Skip(512*i).Take(512).ToArray());
                var w =new uint[64];
                for (int j = 0; j < 16; j++)
                {
                    var niz=new BitArray(32);
                    for (int k = 0; k < 32; k++)
                    {
                        niz[k] = chunk[j * 32 + k];
                    }

                    var str = ToByteArray(niz);
                    w[j] = BitConverter.ToUInt32(str, 0);
                }
                for (int j = 16; j < 64; j++)
                {
                    var s0 = (RightRotate(w[j - 15], 7) ^ RightRotate(w[j - 15], 18) ^ (w[j - 15] >> 3));
                    var s1 = (RightRotate(w[j - 2], 17) ^ RightRotate(w[j - 2], 19) ^ (w[j - 15] >> 10));
                    w[j] = w[j - 16] + s0 + w[j - 7] + s1;
                }
                //Initialize working variables to current hash value:
                uint a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];

                //Compression function main loop:
                for (int j = 0; j <64 ; j++)
                {
                    var s1 = (RightRotate(e, 6) ^ RightRotate(e, 11) ^ RightRotate(e, 25));
                    var ch = (e & f) ^ ((~e) & g);
                    var t1 = h + s1 + ch + K[j] + w[j];
                    var s0 = (RightRotate(a, 2) ^ RightRotate(a, 13) ^ RightRotate(a, 22));
                    var ma = (a & b) ^ (a & c) ^ (b & c);
                    var t2 = s0 + ma;
                    h = g;
                    g = f;
                    f = e;
                    e = d + t1;
                    d = c;
                    c = b;
                    b = a;
                    a = t1 + t2;
                }
                //Add the compressed chunk to the current hash value:
                H[0] = H[0] + a;
                H[1] = H[1] + b;
                H[2] = H[2] + c;
                H[3] = H[3] + d;
                H[4] = H[4] + e;
                H[5] = H[5] + f;
                H[6] = H[6] + g;
                H[7] = H[7] + h;
            }

            //Produce the final hash value(big-endian):
            var hesh="";
            for (int i = 0; i < 8; i++)
            {
                hesh+=H[i].ToString("X");
            }
            H[0] = 0x6a09e667;
            H[1] = 0xbb67ae85;
            H[2] = 0x3c6ef372;
            H[3] = 0xa54ff53a;
            H[4] = 0x510e527f;
            H[5] = 0x9b05688c;
            H[6] = 0x1f83d9ab;
            H[7] = 0x5be0cd19;
            return hesh;
        }

        /// <summary>
        /// SHA2 hashing method
        /// </summary>
        /// <param name="message">Message to be hashed</param>
        public static string Hash(string message)
        {
            return Hash(Encoding.ASCII.GetBytes(message));
        }
        private static uint RightRotate(uint n, int d)
        {
            return (n >> d) | (n << (INT_BITS - d));
        }
        private static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
        private static byte[] ToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
        private static BitArray Prepend(BitArray current, BitArray before)
        {
            var bools = new bool[current.Count + before.Count];
            before.CopyTo(bools, 0);
            current.CopyTo(bools, before.Count);
            return new BitArray(bools);
        }

        private static BitArray Append(BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }
    }
}