using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VotingSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //   name: "Vote/SubmitVote",
            //   url: "Vote/SubmitVote/{id}",
            //   defaults: new { controller = "Vote", action = "Index", id = UrlParameter.Optional });

            //routes.MapRoute(
            //    name: "Vote",
            //    url: "Vote/{id}",
            //    defaults: new { controller = "Vote", action = "Index", id = UrlParameter.Optional });

            //routes.MapRoute(
            //   name: "Result",
            //   url: "Result/{id}",
            //   defaults: new { controller = "Result", action = "Index", id = UrlParameter.Optional });

            //routes.MapRoute(
            //   name: "Index",
            //   url: "Index",
            //   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
        }
    }
}
