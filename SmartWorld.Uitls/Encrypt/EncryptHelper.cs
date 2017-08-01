using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SmartWorld.Uitls
{
    /// <summary>
    /// 加密解密助手类。
    /// </summary>
    public class EncryptHelper
    {
        /// <summary> 
        /// MD5 加密函数 
        /// </summary> 
        /// <param name="content"></param> 
        /// <param name="code">16 or 32</param> 
        /// <returns></returns> 
        public static string MD5Encode(string content, int codeLength)
        {
            if (codeLength == 16) //16位加密
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5").ToLower().Substring(8, 16);
            }
            else if (codeLength == 32) //32位加密
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5");
            }

            return "0";
        }

        /// <summary> 
        /// MD5 加密函数 
        /// </summary> 
        /// <param name="content"></param> 
        /// <returns></returns> 
        public static string MD5Encode(string content)
        {
            if(string.IsNullOrEmpty(content))
            {
                return "";
            }
            return MD5Encode(content, 32);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string content, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //把字符串放到byte数组中
                byte[] inputByteArray = Encoding.Default.GetBytes(content);

                //建立加密对象的密钥和偏移量
                //使得输入密码必须输入英文文本
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="content">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string content, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[content.Length / 2];
                for (int x = 0; x < content.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(content.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
            }

            return string.Empty;
        }

        public static void RSACreateKey(ref string str_PublicKey, ref string str_PrivateKey)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(512);

            str_PublicKey = RSA.ToXmlString(false);
            str_PrivateKey = RSA.ToXmlString(true);
        }

        public static string RSAEncrypt(string source, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.FromXmlString(publicKey);

                byte[] DataToEncrypt = Encoding.UTF8.GetBytes(source);

                byte[] bs = RSA.Encrypt(DataToEncrypt, false);
                string encrypttxt = Convert.ToBase64String(bs);

                return encrypttxt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string RSADecrypt(string strRSA, string privateKey)
        {
            try
            {
                byte[] DataToDecrypt = Convert.FromBase64String(strRSA);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.FromXmlString(privateKey);

                byte[] bsdecrypt = RSA.Decrypt(DataToDecrypt, false);

                string strRE = Encoding.UTF8.GetString(bsdecrypt);
                return strRE;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}
