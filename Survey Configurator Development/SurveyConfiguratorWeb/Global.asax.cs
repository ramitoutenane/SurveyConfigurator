using SurveyConfiguratorWeb.Models;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SurveyConfiguratorWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                ModelBinders.Binders.DefaultBinder = new QuestionModelBinder();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ContainerConfig.RegisterContainer();
            }catch(Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}
