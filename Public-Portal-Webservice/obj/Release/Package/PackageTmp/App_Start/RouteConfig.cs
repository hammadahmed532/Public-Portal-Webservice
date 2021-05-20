using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Public_Portal_Webservice
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "Admin/{action}", new { controller = "Admin", action = "AdminPanel" });


            routes.MapRoute(null, "Account/{action}", new { controller = "Account", action = "AccountCheckAndRedirect" });

            routes.MapRoute(null, "Complaint/{action}/{id}", new { controller = "Complaint", action = "Index", id = UrlParameter.Optional });


            routes.MapRoute(null, "TownAccount/{action}", new { controller = "TownAccount", action = "TownPanelOverview" });

            routes.MapRoute(null, "PublicAccount/{action}", new { controller = "PublicAccount", action = "AccountProfile" });

            routes.MapRoute(null, "CHAccount/{action}", new { controller = "CHAccount", action = "CHPanelOverview" });


            routes.MapRoute(null, "UCAccount/{action}", new { controller = "UCAccount", action = "UCPanelOverview" });

            routes.MapRoute(null, "FOAccount/{action}", new { controller = "FOAccount", action = "FOPanelOverview" });

        }
    }
}
