using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HelperClass
{
    public class AESCrypt
    {

        private byte[] _IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private int CRYPTO_KEY_LENGTH = 32;

        private AesCryptoServiceProvider m_aesCryptoServiceProvider;

        public AESCrypt()
        {
            m_aesCryptoServiceProvider = new AesCryptoServiceProvider();
        }

        private bool Encrypt(string sCrypto, byte[] key, bool containKey, out string str)
        {
            var sEncryped = string.Empty;
            try
            {
                var crypto = Encoding.UTF8.GetBytes(sCrypto);
                m_aesCryptoServiceProvider.Key = key;
                m_aesCryptoServiceProvider.IV = _IV;
                var ct = m_aesCryptoServiceProvider.CreateEncryptor();
                var encrypted = ct.TransformFinalBlock(crypto, 0, crypto.Length);
                if (containKey)
                {
                    sEncryped += Byte2HexString(key);
                }
                sEncryped += Byte2HexString(encrypted);
                str = sEncryped;
                return true;
            }
            catch (Exception)
            {
                str = string.Empty;
                return false;
            }
        }

        private bool Decrypt(string sEncrypted, byte[] key, out string str)
        {
            var sDecrypted = string.Empty;
            try
            {
                var encrypted = HexString2Byte(sEncrypted);
                m_aesCryptoServiceProvider.Key = key;
                m_aesCryptoServiceProvider.IV = _IV;
                var ct = m_aesCryptoServiceProvider.CreateDecryptor();
                var decrypted = ct.TransformFinalBlock(encrypted, 0, encrypted.Length);
                sDecrypted += Encoding.UTF8.GetString(decrypted);
                str = sDecrypted;
                return true;
            }
            catch (Exception)
            {
                str = string.Empty;
                return false;
            }
        }

        #region 指定密钥对明文进行AES加密、解密
        /// <summary>
        /// 指定密钥对明文进行AES加密
        /// </summary>
        /// <param name="sCrypto">明文</param>
        /// <param name="sKey">加密密钥</param>
        /// <returns></returns>
        public bool Encrypt(string sCrypto, string sKey, bool containKey, out string str)
        {
            var key = new byte[CRYPTO_KEY_LENGTH];
            var temp = Encoding.UTF8.GetBytes(sKey);
            if (temp.Length > key.Length)
            {
                str = string.Empty;
                return false;
            }
            key = Encoding.UTF8.GetBytes(sKey.PadRight(key.Length));
            return Encrypt(sCrypto, key, containKey, out str);
        }

        /// <summary>
        /// 指定密钥，并对密文进行解密
        /// </summary>
        /// <param name="sEncrypted">密文</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public bool Decrypt(string sEncrypted, string sKey, bool containKey, out string str)
        {
            var key = new byte[CRYPTO_KEY_LENGTH];

            var temp = Encoding.UTF8.GetBytes(sKey);
            if (temp.Length > key.Length)
            {
                str = string.Empty;
                return false;
            }
            key = Encoding.UTF8.GetBytes(sKey.PadRight(key.Length));
            if (containKey)
            {
                sEncrypted = sEncrypted.Substring(CRYPTO_KEY_LENGTH * 2);
            }
            return Decrypt(sEncrypted, key, out str);
        }
        #endregion

        #region 动态生成密钥，并对明文进行AES加密、解密

        /// <summary>
        /// 动态生成密钥，并对明文进行AES加密
        /// </summary>
        /// <param name="sCrypto">明文</param>
        /// <returns></returns>

        public bool Encrypt(string sCrypto, bool containKey, out string str)
        {
            m_aesCryptoServiceProvider.GenerateKey();
            var key = m_aesCryptoServiceProvider.Key;
            return Encrypt(sCrypto, key, containKey, out str);
        }

        /// <summary>
        /// 从密文中解析出密钥，并对密文进行解密
        /// </summary>
        /// <param name="sEncrypted">密文</param>
        /// <param name="containKey"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Decrypt(string sEncrypted, bool containKey, out string str)
        {
            var sKey = string.Empty;
            if (sEncrypted.Length <= CRYPTO_KEY_LENGTH * 2)
            {
                str = string.Empty;
                return false;
            }
            if (containKey)
            {
                sKey = sEncrypted.Substring(0, CRYPTO_KEY_LENGTH * 2);
                sEncrypted = sEncrypted.Substring(CRYPTO_KEY_LENGTH * 2);
            }
            var key = HexString2Byte(sKey);
            return Decrypt(sEncrypted, key, out str);
        }
        #endregion

        #region 私有方法

        private string Byte2HexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        private byte[] HexString2Byte(string hex)
        {
            var len = hex.Length / 2;
            var bytes = new byte[len];
            for (var i = 0; i < len; i++)
            {
                bytes[i] = (byte)Convert.ToInt32(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        #endregion
    }
}