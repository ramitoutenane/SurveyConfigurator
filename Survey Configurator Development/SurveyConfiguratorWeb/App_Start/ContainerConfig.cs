using Autofac;
using Autofac.Integration.Mvc;
using QuestionManaging;
using SurveyConfiguratorEntities;
using SurveyConfiguratorWeb.Helpers;
using SurveyConfiguratorWeb.Models;
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
                int tAutoRefreshInterval;
                string tDatabaseServer = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_SERVER];
                string tDatabaseName = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_NAME];
                string tDatabaseUser = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_USER];
                string tDatabasePassword = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_PASSWORD];
                DatabaseSettings tDatabaseSettings = new DatabaseSettings(tDatabaseServer, tDatabaseName, tDatabaseUser, tDatabasePassword);

                string tConfigRefreshInterval = ConfigurationManager.AppSettings[ConstantStringResources.cAUTO_REFRESH_INTERVAL];
                if (!int.TryParse(tConfigRefreshInterval, out tAutoRefreshInterval) || tAutoRefreshInterval < 20000)
                {
                    tAutoRefreshInterval = 20000;
                    ErrorLogger.Log(ErrorMessages.cREFRESH_INTERVAL_WARNING);
                }

                var tBuilder = new ContainerBuilder();
                tBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
                tBuilder.RegisterType<QuestionManager>().As<IQuestionRepository>().WithParameter("pDatabaseSettings", tDatabaseSettings).SingleInstance().OnActivated(tType => { tType.Instance.RefreshQuestionList(); tType.Instance.QuestionListChangedEventHandler += Helper.SendChangeNotification; tType.Instance.StartAutoRefresh(tAutoRefreshInterval); });

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