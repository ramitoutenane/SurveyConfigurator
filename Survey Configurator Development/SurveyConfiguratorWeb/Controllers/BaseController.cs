using SurveyConfiguratorWeb.Helpers;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class BaseController : Controller
    {

        protected override void ExecuteCore()
        {
            try
            {
                if (Session == null || Session[ConstantStringResources.cSESSION_KEY_LANGUAGE] == null)
                {
                    Session[ConstantStringResources.cSESSION_KEY_LANGUAGE] = ConstantStringResources.cENGLISH_CULTURE;
                }
                Helper.ChangeCulture((string)Session[ConstantStringResources.cSESSION_KEY_LANGUAGE]);

                base.ExecuteCore();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

    }
}
