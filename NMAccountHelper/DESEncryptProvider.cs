using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace NMAccountHelper
{

    /// <summary>
    /// 对称加解密处理
    /// </summary>
    public class DESEncryptProvider
    {
        private byte[] _key = Encoding.UTF8.GetBytes("JaY5qfXmuba2bNxlKoMKkimY");
        private byte[] _iv = Encoding.UTF8.GetBytes("OyJq7IN1");

        public static DESEncryptProvider Create()
        {
            DESEncryptProvider provider = new DESEncryptProvider();
            return provider;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] bytes)
        {
            //创建一个内存流
            MemoryStream memoryStream = new MemoryStream();
            //使用传递的私钥和IV创建加密流
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                new TripleDESCryptoServiceProvider().CreateEncryptor(this._key, this._iv),
                CryptoStreamMode.Write);

            try
            {
                //将字节数组写入加密流,并清除缓冲区
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                //得到加密后的字节数组
                byte[] encryptedBytes = memoryStream.ToArray();
                return encryptedBytes;
            }
            catch (CryptographicException err)
            {
                throw new Exception("加密出错：" + err.Message);
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] bytes)
        {
            //根据加密后的字节数组创建一个内存流
            MemoryStream memoryStream = new MemoryStream(bytes);
            //使用传递的私钥、IV和内存流创建解密流
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                new TripleDESCryptoServiceProvider().CreateDecryptor(this._key, this._iv),
                CryptoStreamMode.Read);
            //创建一个字节数组保存解密后的数据
            byte[] decryptBytes = new byte[bytes.Length];
            try
            {
                //从解密流中将解密后的数据读到字节数组中
                cryptoStream.Read(decryptBytes, 0, decryptBytes.Length);
                return decryptBytes;
            }
            catch (CryptographicException err)
            {
                throw new Exception("解密出错：" + err.Message);
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }
        }

        private byte[] getNorMingDesKey()
        {
            byte[] key = new byte[24];
            String norInfo = "nORMING cORPORATION, fzy 2008";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            for (int i = 0; i < 3; i++)
            {

                byte[] buf = md5.ComputeHash(Encoding.UTF8.GetBytes(norInfo.Substring(i)));
                for (int j = 0; j < 8; j++)
                {
                    key[(j + i * 8)] = buf[j];
                }
            }
            return key;
        }


        #region CBC模式**
        /// <summary>
        /// DES3 CBC模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            //复制于MSDN
            // Create a MemoryStream.
            MemoryStream mStream = new MemoryStream();
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.CBC;             //默认值
            tdsp.Padding = PaddingMode.PKCS7;       //默认值
            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream cStream = new CryptoStream(mStream,
                tdsp.CreateEncryptor(key, iv),
                CryptoStreamMode.Write);
            // Write the byte array to the crypto stream and flush it.
            cStream.Write(data, 0, data.Length);
            cStream.FlushFinalBlock();
            // Get an array of bytes from the 
            // MemoryStream that holds the 
            // encrypted data.
            byte[] ret = mStream.ToArray();
            // Close the streams.
            cStream.Close();
            mStream.Close();
            // Return the encrypted buffer.
            return ret;
        }
        /// <summary>
        /// DES3 CBC模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public byte[] Des3DecodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            // Create a new MemoryStream using the passed 
            // array of encrypted data.
            MemoryStream msDecrypt = new MemoryStream(data);
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.CBC;
            tdsp.Padding = PaddingMode.PKCS7;
            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                tdsp.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);
            // Create buffer to hold the decrypted data.
            byte[] fromEncrypt = new byte[data.Length];
            // Read the decrypted data out of the crypto stream
            // and place it into the temporary buffer.
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
            //Convert the buffer into a string and return it.
            return fromEncrypt;
        }
        #endregion

        #region ECB模式
        /// <summary>
        /// DES3 ECB模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>
        /// <param name="str">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
        {
            // Create a MemoryStream.
            MemoryStream mStream = new MemoryStream();
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.ECB;
            tdsp.Padding = PaddingMode.PKCS7;
            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream cStream = new CryptoStream(mStream,
                tdsp.CreateEncryptor(key, iv),
                CryptoStreamMode.Write);
            // Write the byte array to the crypto stream and flush it.
            cStream.Write(data, 0, data.Length);
            cStream.FlushFinalBlock();
            // Get an array of bytes from the 
            // MemoryStream that holds the 
            // encrypted data.
            byte[] ret = mStream.ToArray();
            // Close the streams.
            cStream.Close();
            mStream.Close();
            // Return the encrypted buffer.
            return ret;
        }
        /// <summary>
        /// DES3 ECB模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>
        /// <param name="str">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public byte[] Des3DecodeECB(byte[] key, byte[] iv, byte[] data)
        {
            // Create a new MemoryStream using the passed 
            // array of encrypted data.
            MemoryStream msDecrypt = new MemoryStream(data);
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.ECB;
            tdsp.Padding = PaddingMode.PKCS7;
            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                tdsp.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);
            // Create buffer to hold the decrypted data.
            byte[] fromEncrypt = new byte[data.Length];
            // Read the decrypted data out of the crypto stream
            // and place it into the temporary buffer.
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
            //Convert the buffer into a string and return it.
            return fromEncrypt;
        }
        #endregion

        /// <summary>
        /// 诺明密码加密
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string NMEncrypt(string info)
        {
            //诺明密码加密
            byte[] key = getNorMingDesKey();
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
            byte[] bytes = Encoding.UTF8.GetBytes(info);
            byte[] str1 = this.Des3EncodeECB(key, iv, bytes);
            return Convert.ToBase64String(str1);
        }

        /// <summary>
        /// 诺明密码解密
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string NMDecrypt(string info)
        {
            //诺明密码解密
            byte[] key = getNorMingDesKey();
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
            byte[] data = Convert.FromBase64String(info);
            byte[] str2 = this.Des3DecodeECB(key, iv, data);
            return System.Text.Encoding.UTF8.GetString(str2);
        }


    }
}
