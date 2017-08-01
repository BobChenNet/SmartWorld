using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SmartWorld.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("mentuApi", "Mentu/{action}/{id}", new { controller = "Mentu", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{contrller}/{action}/{id}",
                defaults: new { controller = "Home",id = RouteParameter.Optional }
            );
        }
    }
}
