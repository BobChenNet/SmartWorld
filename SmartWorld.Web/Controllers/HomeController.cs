using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartWorld.Business;
using SmartWorld.Uitls;
using SmartWorld.Web.Common;

namespace SmartWorld.Web.Controllers
{
    public class HomeController : BaseController
    {
        readonly HouseProvider _houseProvider = new HouseProvider();
        readonly InfoProvider _infoProvider = new InfoProvider();
        public ActionResult Index()
        {
            ViewBag.data = _houseProvider.GetTimeTest();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "关于我们";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "联系我们";

            return View();
        }
    }
}