using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace HelperClass
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public class DESCrypt
    {
        #region ========加密========

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string key)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = des.IV = Getmd5(key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string key)
        {
            var des = new DESCryptoServiceProvider();
            var len = text.Length / 2;
            var inputByteArray = new byte[len];
            for (var x = 0; x < len; x++)
            {
                var i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = des.IV = Getmd5(key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }



        #endregion

        public static byte[] Getmd5(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bs = Encoding.ASCII.GetBytes(str);
            bs = md5.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (var b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            return Encoding.Default.GetBytes(s.ToString().Substring(0, 8));
        }
    }
}