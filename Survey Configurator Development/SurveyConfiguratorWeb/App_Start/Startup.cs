using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(SurveyConfiguratorWeb.Startup))]

namespace SurveyConfiguratorWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                app.MapSignalR();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}
