using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLib.Cryptos
{
    public interface ICryptos
    {
        byte[] Encrypt(byte[] value);
        byte[] Decrypt(byte[] value);

    }
}
