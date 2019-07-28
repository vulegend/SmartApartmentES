using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using SmartApartmentDataTestUtility.Encryption.Base;

namespace SmartApartmentDataTestUtility.Encryption.AES
{
    public class AESEncryption
    {
        public static string Encrypt(string plain, string key)
        {
            BCEngine bcEngine = new BCEngine(new AesEngine(), Encoding.ASCII);
            bcEngine.SetPadding(new Pkcs7Padding());
            return bcEngine.Encrypt(plain, key);
        }

        public static string Decrypt(string cipher, string key)
        {
            BCEngine bcEngine = new BCEngine(new AesEngine(), Encoding.ASCII);
            bcEngine.SetPadding(new Pkcs7Padding());
            return bcEngine.Decrypt(cipher, key);
        }
    }
}
