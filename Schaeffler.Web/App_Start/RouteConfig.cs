using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Schaeffler.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name: "TH",
                    url: "TH",
                    defaults: new { controller = "Home", action = "Index_TH" },
                    namespaces: new[] { "Sita_Aircraft.Controllers" }
            );

            routes.MapRoute(
                    name: "ID",
                    url: "ID",
                    defaults: new { controller = "Home", action = "Index" },
                    namespaces: new[] { "Sita_Aircraft.Controllers" }
            );

            routes.MapRoute(
                    name: "ExportToExcel",
                    url: "ExportToExcel/{Key}",
                    defaults: new { controller = "Home", action = "ExportToExcel" },
                    namespaces: new[] { "Sita_Aircraft.Controllers" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
