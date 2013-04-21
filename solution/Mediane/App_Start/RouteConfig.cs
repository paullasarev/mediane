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
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id =" Main_Page" },
                constraints: new { controller = "Home", action =  "Index|About|Contact" }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "LogOn", id = "" }
            );

            routes.MapRoute(
                name: "Wrong",
                url: "{*a}",
                defaults: new { controller = "RootRedirector", action = "Redirect", id = UrlParameter.Optional }
            );

        }
    }
}