using Microsoft.AspNet.SignalR;
using SurveyConfiguratorWeb.Hubs;
using System;
using System.Threading;

namespace SurveyConfiguratorWeb.Helpers
{
    public static class Helper
    {
        public static bool ChangeCulture(string pSelectedCalture)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(pSelectedCalture);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                return true;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }

        }


        public static void SendChangeNotification()
        {
            try
            {
                var QuestionListHubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionListHub>();
                QuestionListHubContext.Clients.All.UpdateQuestionList();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }

    }
}