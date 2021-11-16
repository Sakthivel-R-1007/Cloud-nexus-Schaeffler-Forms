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

            //    routes.MapRoute(
            //         name: "EN",
            //         url: "EN/products-and-solutions",
            //         defaults: new { controller = "Home", action = "Index_EN" },
            //         namespaces: new[] { "Schaeffler.Web.Controllers" }
            //     );
            //    routes.MapRoute(
            //        name: "index_en-thankyou",
            //        url: "index_en-thankyou",
            //        defaults: new { controller = "Home", action = "Index_EN_ThankYou" },
            //        namespaces: new[] { "Schaeffler.Web.Controllers" }
            //);
            //    routes.MapRoute(
            //            name: "TH",
            //            url: "TH/products-and-solutions",
            //            defaults: new { controller = "Home", action = "Index_TH" },
            //            namespaces: new[] { "Schaeffler.Web.Controllers" }
            //    );
            //    routes.MapRoute(
            //         name: "index_th-thankyou",
            //         url: "index_th-thankyou",
            //         defaults: new { controller = "Home", action = "Index_Th_ThankYou" },
            //         namespaces: new[] { "Schaeffler.Web.Controllers" }
            // );

            //    routes.MapRoute(
            //            name: "ID",
            //            url: "ID/products-and-solutions",
            //            defaults: new { controller = "Home", action = "Index" },
            //            namespaces: new[] { "Schaeffler.Web.Controllers" }
            //    );

            routes.MapRoute(
                   name: "index_jp-thankyou",
                   url: "JP/index_jp-thankyou",
                   defaults: new { controller = "Japan", action = "Index_JP_ThankYou" },
                   namespaces: new[] { "Schaeffler.Web.Controllers" }
           );

            routes.MapRoute(
                   name: "Index",
                   url: "JP",
                   defaults: new { controller = "Japan", action = "Index" },
                   namespaces: new[] { "Schaeffler.Web.Controllers" }
           );

            // routes.MapRoute(
            //         name: "ExportToExcel",
            //         url: "ExportToExcel/{Key}",
            //         defaults: new { controller = "Home", action = "ExportToExcel" },
            //         namespaces: new[] { "Schaeffler.Web.Controllers" }
            // );

            // routes.MapRoute(
            //        name: "Export",
            //        url: "Export/{Key}",
            //        defaults: new { controller = "Home", action = "Export" },
            //        namespaces: new[] { "Schaeffler.Web.Controllers" }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Japan", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
