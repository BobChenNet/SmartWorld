using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OAuth2;
using SmartWorld.Web.Common;
using DotNetOpenAuth.Messaging;

namespace SmartWorld.Web.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AuthorizationServer _authorizationServer =
           new AuthorizationServer(new ServerHost(Common.Common.Configuration));

        //public async Task<ActionResult> Token()
        //{
        //    //var response = await _authorizationServer.Channel.WebRequestHandler.GetResponse(Request);
        //    //return response.AsActionResult();
        //}
    }
}