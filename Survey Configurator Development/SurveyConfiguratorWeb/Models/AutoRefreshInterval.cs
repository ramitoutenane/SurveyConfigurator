using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    public static class AutoRefreshInterval
    {
        public static readonly int mAutoRefreshInterval;
        static AutoRefreshInterval()
        {
            try
            {
                string tConfigRefreshInterval = ConfigurationManager.AppSettings[ConstantStringResources.cAUTO_REFRESH_INTERVAL];
                if (!int.TryParse(tConfigRefreshInterval, out mAutoRefreshInterval) || mAutoRefreshInterval < 20000)
                {
                    mAutoRefreshInterval = 20000;
                    ErrorLogger.Log(QuestionManaging.ErrorMessages.cREFRESH_INTERVAL_WARNING);
                }
            }catch(Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}