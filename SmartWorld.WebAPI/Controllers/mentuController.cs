using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartWorld.WebAPI.Controllers
{
    /// <summary>
    /// 专属接口控制器
    /// </summary>
    public class MentuController : ApiController
    {
        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns
        [HttpGet]
        public IEnumerable<string> Mentu()
        {
            return new[] { "value1", "value2" };
        }

    }
}
