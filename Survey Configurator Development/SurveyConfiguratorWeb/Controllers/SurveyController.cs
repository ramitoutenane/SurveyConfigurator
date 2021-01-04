using QuestionManaging;
using SurveyConfiguratorEntities;
using SurveyConfiguratorWeb.Helpers;
using SurveyConfiguratorWeb.Models;
using SurveyConfiguratorWeb.Properties;
using System;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class SurveyController : BaseController
    {
        private IQuestionRepository mQuestionManager;
        public SurveyController(IQuestionRepository pQuestionManager)
        {
            try
            {
                mQuestionManager = pQuestionManager;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        public ActionResult Home()
        {
            try
            {
                return View();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        [HttpGet]
        public ActionResult Edit([Bind(Prefix = ConstantStringResources.cACTION_PREFIX_ID)] int? pId)
        {
            try
            {
                if (pId == null)
                    return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.INVALID_QUESTION_ID_TITLE, ErrorMessage = Errors.INVALID_QUESTION_ID_MESSAGE });

                BaseQuestion tQuestion;
                Result tReadResult = mQuestionManager.Read(pId.Value, out tQuestion);

                if (tReadResult.Value == ResultValue.Success)
                    return View(tQuestion);
                else if (tReadResult.Value == ResultValue.Error)
                {
                    return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
                }
                else
                    return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.QUESTION_NOT_FOUND_TITLE, ErrorMessage = Errors.QUESTION_NOT_FOUND_MESSAGE });
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        [HttpPost]
        public ActionResult Edit(BaseQuestion pQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Result tResult = mQuestionManager.Update(pQuestion);

                    if (tResult.Value == ResultValue.Success)
                        return RedirectToAction(ConstantStringResources.cHOME_ACTION);
                    else
                    {
                        ViewBag.MessageTitle = Errors.UPDATE_ERROR_TITLE;
                        ViewBag.Message = Errors.UPDATE_ERROR_MESSAGE;
                    }
                }
                return View(pQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        [HttpGet]
        public ActionResult Create([Bind(Prefix = ConstantStringResources.cACTION_PREFIX_QuestionType)] QuestionType? pQuestionType)
        {
            try
            {
                if (pQuestionType == null)
                    return View(ConstantStringResources.cERROR_VIEW,
                        new ErrorViewModel() { ErrorTitle = Errors.INVALID_TYPE_TITLE, ErrorMessage = Errors.INVALID_TYPE_MESSAGE });

                switch (pQuestionType.Value)
                {
                    case QuestionType.Slider:
                        return View(new SliderQuestion());
                    case QuestionType.Stars:
                        return View(new StarsQuestion());
                    case QuestionType.Smiley:
                        return View(new SmileyQuestion());
                    default:
                        return View(ConstantStringResources.cERROR_VIEW,
                            new ErrorViewModel() { ErrorTitle = Errors.INVALID_TYPE_TITLE, ErrorMessage = Errors.INVALID_TYPE_MESSAGE });
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BaseQuestion pQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Result tResult = mQuestionManager.Create(pQuestion);

                    if (tResult.Value == ResultValue.Success)
                        return RedirectToAction(ConstantStringResources.cHOME_ACTION);
                    else
                    {
                        ViewBag.MessageTitle = Errors.INSERT_ERROR_TITLE;
                        ViewBag.Message = Errors.INSERT_ERROR_MESSAGE;
                    }
                }
                return View(pQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int pId)
        {
            try
            {
                Result tResult = mQuestionManager.Delete(pId);

                if (tResult.Value == ResultValue.Success)
                    return RedirectToAction(ConstantStringResources.cHOME_ACTION);
                else
                {
                    ViewBag.MessageTitle = Errors.DELETE_ERROR_TITLE;
                    ViewBag.Message = Errors.DELETE_ERROR_MESSAGE;
                    var tModel = mQuestionManager.QuestionsList;
                    return View(ConstantStringResources.cHOME_ACTION, tModel);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        public ActionResult QuestionList()
        {
            try
            {
                    return Json(mQuestionManager.QuestionsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult SetLanguage([Bind(Prefix = ConstantStringResources.cACTION_PREFIX_LANGUAGE)] string pSelectedCalture)
        {
            try
            {
                if (Helper.ChangeCulture(pSelectedCalture))
                {
                    Session[ConstantStringResources.cSESSION_KEY_LANGUAGE] = pSelectedCalture;
                    return Redirect(Request.UrlReferrer.ToString());
                }
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }

    }
}