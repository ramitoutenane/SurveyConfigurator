using Autofac;
using Autofac.Integration.Mvc;
using QuestionManaging;
using SurveyConfiguratorApp;
using SurveyConfiguratorEntities;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            try
            {
                string tDatabaseServer = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_SERVER];
                string tDatabaseName = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_NAME];
                string tDatabaseUser = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_USER];
                string tDatabasePassword = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_PASSWORD];
                DatabaseSettings tDatabaseSettings = new DatabaseSettings(tDatabaseServer, tDatabaseName, tDatabaseUser, tDatabasePassword);

                var tBuilder = new ContainerBuilder();
                tBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
                tBuilder.RegisterType<QuestionManager>().As<IQuestionRepository>().WithParameter("pDatabaseSettings",tDatabaseSettings).InstancePerRequest();

                var pContainer = tBuilder.Build();
                DependencyResolver.SetResolver(new AutofacDependencyResolver(pContainer));
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}