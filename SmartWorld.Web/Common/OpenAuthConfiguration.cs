using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace SmartWorld.Web.Common
{
    public class OpenAuthConfiguration
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OpenAuthConfiguration()
        {
            TokenLifetime = TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// 签名证书
        /// </summary>
        public X509Certificate2 SigningCertificate { get; set; }

        /// <summary>
        /// 加密证书
        /// </summary>
        public X509Certificate2 EncryptionCertificate { get; set; }

        /// <summary>
        /// Token有效时间
        /// </summary>
        public TimeSpan TokenLifetime { get; set; }
    }
}