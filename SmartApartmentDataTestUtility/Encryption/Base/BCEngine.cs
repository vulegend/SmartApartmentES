using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Parameters;

namespace SmartApartmentDataTestUtility.Encryption.Base
{
    public class BCEngine
    {
        #region Fields
        private readonly Encoding _encoding;
        private readonly IBlockCipher _blockCipher;
        private PaddedBufferedBlockCipher _cipher;
        private IBlockCipherPadding _padding;
        #endregion
        #region Constructors
        public BCEngine(IBlockCipher blockCipher, Encoding encoding)
        {
            _blockCipher = blockCipher;
            _encoding = encoding;
        }
        #endregion
        #region Public Methods

        /// <summary>
        /// Set the padding for BC Engine
        /// </summary>
        /// <param name="padding"></param>
        public void SetPadding(IBlockCipherPadding padding) => _padding = padding;

        /// <summary>
        /// Performs an encryption using BC
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt(string plain, string key)
        {
            var result = Execute(true, _encoding.GetBytes(plain), key);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Performs a decryption using BC
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Decrypt(string cipher, string key)
        {
            var result = Execute(false, Convert.FromBase64String(cipher), key);
            return _encoding.GetString(result);
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Executes either encryption or decryption of the given input and returns serialized data
        /// </summary>
        /// <param name="isEncryption"></param>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private byte[] Execute(bool isEncryption, byte[] input, string key)
        {
            try
            {
                _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
                byte[] keyByte = _encoding.GetBytes(key);
                _cipher.Init(isEncryption, new KeyParameter(keyByte));
                return _cipher.DoFinal(input);
            }
            catch (Org.BouncyCastle.Crypto.CryptoException ex)
            {
                throw new CryptoException(ex.Message);
            }
        }

        #endregion
    }
}
