using Newtonsoft.Json;
using QuestionManaging;
using SurveyConfiguratorEntities;
using SurveyConfiguratorWeb.Models;
using SurveyConfiguratorWeb.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
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
                mQuestionManager.RefreshQuestionList();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        public ActionResult Index()
        {
            try
            {
                var tModel = mQuestionManager.QuestionsList;
                return View(tModel);
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
                //validate slider question start value less than end value
                if (pQuestion.Type == QuestionType.Slider)
                    ValidateSlider(pQuestion as SliderQuestion);
                if (ModelState.IsValid)
                {
                    Result tResult = mQuestionManager.Update(pQuestion);
                    if (tResult.Value == ResultValue.Success)
                        return RedirectToAction(ConstantStringResources.cINDEX_ACTION);
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
                //validate slider question start value less than end value
                if (pQuestion.Type == QuestionType.Slider)
                    ValidateSlider(pQuestion as SliderQuestion);
                if (ModelState.IsValid)
                {
                    Result tResult = mQuestionManager.Create(pQuestion);
                    if (tResult.Value == ResultValue.Success)
                        return RedirectToAction(ConstantStringResources.cINDEX_ACTION);
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
                    return RedirectToAction(ConstantStringResources.cINDEX_ACTION);
                else
                {
                    ViewBag.MessageTitle = Errors.DELETE_ERROR_TITLE;
                    ViewBag.Message = Errors.DELETE_ERROR_MESSAGE;
                    var tModel = mQuestionManager.QuestionsList;
                    return View(ConstantStringResources.cINDEX_ACTION, tModel);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        public ActionResult QuestionList([Bind(Prefix = ConstantStringResources.cACTION_PREFIX_HASH)] string pClientHash)
        {
            try
            {
                Result tResult = mQuestionManager.RefreshQuestionList();
                if (tResult.Value != ResultValue.Success)
                    return new HttpStatusCodeResult(500);
                if (pClientHash == null)
                    return Json(mQuestionManager.QuestionsList, JsonRequestBehavior.AllowGet);

                string pServerHash = MD5CheckSum(JsonConvert.SerializeObject(mQuestionManager.QuestionsList));
                if (pServerHash == pClientHash)
                    return new HttpStatusCodeResult(304);
                else
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
                if (ChangeCulture(pSelectedCalture))
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
        [NonAction]
        private string MD5CheckSum(string pInput)
        {
            try
            {
                // Use input string to calculate MD5 hash
                using (System.Security.Cryptography.MD5 tMD5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] tInputBytes = System.Text.Encoding.ASCII.GetBytes(pInput);
                    byte[] tHashBytes = tMD5.ComputeHash(tInputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder tStringBuilder = new StringBuilder();
                    for (int i = 0; i < tHashBytes.Length; i++)
                    {
                        tStringBuilder.Append(tHashBytes[i].ToString("X2"));
                    }
                    return tStringBuilder.ToString();
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return String.Empty;
            }
        }
        [NonAction]
        private void ValidateSlider(SliderQuestion pSliderQuestion)
        {
            try
            {
                //add start value less than end value validation to model
                //all other validations are set using data annotation attributes
                if (pSliderQuestion.StartValue >= pSliderQuestion.EndValue)
                    ModelState.AddModelError(nameof(SliderQuestion.StartValue), Errors.START_LARGER_THAN_END_ERROR);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
    }
}