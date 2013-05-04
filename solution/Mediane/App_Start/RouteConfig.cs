using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mediane
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Root",
                url: "",
                defaults: new { controller = "RootRedirector", action = "Redirect"}
            );

            routes.MapRoute(
                name: "HomeIndex",
                url: "Home/Index",
                defaults: new { controller = "RootRedirector", action = "Redirect" }
                //constraints: new { controller = "Home", action =  "Index|Edit|Save" }
            );

            routes.MapRoute(
                name: "HomeEdit",
                url: "Home/Edit",
                defaults: new { controller = "RootRedirector", action = "Redirect" }
                //constraints: new { controller = "Home", action =  "Index|Edit|Save" }
            );

            routes.MapRoute(
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional /*"Main_Page"*/ },
                constraints: new { controller = "Home", action =  "Index|About|Contact|Edit|Save" }
            );

            //routes.MapRoute(
            //    name: "HomeRoot",
            //    url: "Home",
            //    defaults: new { controller = "RootRedirector", action = "Redirect" }
            //);

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "LogOn", id = "" }
            );

            routes.MapRoute(
                name: "Wrong",
                url: "{*a}",
                defaults: new { controller = "RootRedirector", action = "Redirect" }
            );

        }
    }
}