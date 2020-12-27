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
    public class SurveyController : Controller
    {
        private IQuestionRepository mQuestionManager;
        private int mAutoRefreshInterval;
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
        public ActionResult Edit(int? Id)
        {
            try
            {
                if (Id == null)
                    return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.INVALID_QUESTION_ID_TITLE, ErrorMessage = Errors.INVALID_QUESTION_ID_MESSAGE });

                BaseQuestion tQuestion;
                Result tReadResult = mQuestionManager.Read(Id.Value, out tQuestion);
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
                        return RedirectToAction("Index");
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
        public ActionResult Create([Bind(Prefix = "QuestionType")] QuestionType? pQuestionType)
        {
            try
            {
                if (pQuestionType == null)
                    return View(ConstantStringResources.cERROR_VIEW,
                        new ErrorViewModel() { ErrorTitle = Errors.INVALID_TYPE_TITLE, ErrorMessage = Errors.INVALID_TYPE_MESSAGE });
                switch (pQuestionType.Value)
                {
                    case QuestionType.Slider:
                        return View(ConstantStringResources.cCREATE_SLIDER_VIEW);
                    case QuestionType.Stars:
                        return View(ConstantStringResources.cCREATE_STARS_VIEW);
                    case QuestionType.Smiley:
                        return View(ConstantStringResources.cCREATE_SMILEY_VIEW);
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
                        return RedirectToAction("Index");
                    else
                    {
                        ViewBag.MessageTitle = Errors.INSERT_ERROR_TITLE;
                        ViewBag.Message = Errors.INSERT_ERROR_MESSAGE;
                    }
                }
                switch (pQuestion.Type)
                {
                    case QuestionType.Stars:
                        return View(ConstantStringResources.cCREATE_STARS_VIEW, pQuestion);
                    case QuestionType.Smiley:
                        return View(ConstantStringResources.cCREATE_SMILEY_VIEW, pQuestion);
                    case QuestionType.Slider:
                        return View(ConstantStringResources.cCREATE_SLIDER_VIEW, pQuestion);
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
        public ActionResult Delete(int pId)
        {
            try
            {
                Result tResult = mQuestionManager.Delete(pId);
                if (tResult.Value == ResultValue.Success)
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.MessageTitle = Errors.DELETE_ERROR_TITLE;
                    ViewBag.Message = Errors.DELETE_ERROR_MESSAGE;
                    var tModel = mQuestionManager.QuestionsList;
                    return View("Index", tModel);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = Errors.GENERAL_ERROR_TITLE, ErrorMessage = Errors.GENERAL_ERROR_MESSAGE });
            }
        }
        public ActionResult QuestionList([Bind(Prefix = "Hash")] string pClientHash)
        {
            try
            {
                Result tResult = mQuestionManager.RefreshQuestionList();
                if(tResult.Value != ResultValue.Success)
                    return new HttpStatusCodeResult(500);
                if (pClientHash == null)
                    return Json(mQuestionManager.QuestionsList, JsonRequestBehavior.AllowGet);

                string pServerHash = CreateMD5(JsonConvert.SerializeObject(mQuestionManager.QuestionsList));
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
        [NonAction]
        private string CreateMD5(string pInput)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] tInputBytes = System.Text.Encoding.ASCII.GetBytes(pInput);
                byte[] tHashBytes = md5.ComputeHash(tInputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder tStringBuilder = new StringBuilder();
                for (int i = 0; i < tHashBytes.Length; i++)
                {
                    tStringBuilder.Append(tHashBytes[i].ToString("X2"));
                }
                return tStringBuilder.ToString();
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