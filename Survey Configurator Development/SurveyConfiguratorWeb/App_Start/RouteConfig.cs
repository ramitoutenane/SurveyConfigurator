using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyConfiguratorWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Survey", action = "Home", id = UrlParameter.Optional }
                );
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}
