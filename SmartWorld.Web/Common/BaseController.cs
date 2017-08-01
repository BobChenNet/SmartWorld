using System.Web.Mvc;
using SmartWorld.Uitls;

namespace SmartWorld.Web.Common
{
    public class BaseController : Controller
    {
        public int CurrentPage = 1;
        protected int PageSize = 10;
        public PagingInfo Pageinfo;

        public BaseController()
        {
            Pageinfo = new PagingInfo
            {

                nPage = CurrentPage,
                nPageSize = PageSize,
                nSumCount = 20,
                strPageUrl = ""
            };
        }

    }
}