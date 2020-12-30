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
                ChangeCulture((string)Session[ConstantStringResources.cSESSION_KEY_LANGUAGE]);

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

        [NonAction]
        protected bool ChangeCulture(string pSelectedCalture)
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

    }
}
