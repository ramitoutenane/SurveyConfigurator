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
        private bool mListChanged;
        public SurveyController(IQuestionRepository pQuestionManager)
        {
            try
            {

                mQuestionManager = pQuestionManager;
                mAutoRefreshInterval = 5000;
                /*
                //get refresh interval from configuration file, if invalid or less than 20000 milliseconds then set to 20000 milliseconds
                string tConfigRefreshInterval = ConfigurationManager.AppSettings[ConstantStringResources.cAUTO_REFRESH_INTERVAL];
                if (!int.TryParse(tConfigRefreshInterval, out mAutoRefreshInterval) || mAutoRefreshInterval < 20000)
                {
                    mAutoRefreshInterval = 20000;
                    ErrorLogger.Log(ErrorMessages.cREFRESH_INTERVAL_WARNING);
                }*/

                Result tResult = mQuestionManager.RefreshQuestionList();
                if (tResult.Value == ResultValue.Success)
                    mListChanged = true;
                else
                    mListChanged = false;
                StartAutoRefreshThread();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        public ActionResult Index(SortMethod? pSortMethod)
        {
            try
            {
                if (Session["SortMethod"] == null || Session["SortOrder"] == null)
                {
                    Session["SortMethod"] = SortMethod.ByQuestionID;
                    Session["SortOrder"] = SortOrder.Ascending;
                }
                if (pSortMethod != null)
                {
                    SetSortMethod(pSortMethod.Value);
                }

                ResultValue pResultValue = mQuestionManager.RefreshQuestionList().Value;
                if (pResultValue == ResultValue.Success)
                {
                    var tModel = SortQuestionsList(mQuestionManager.QuestionsList);
                    return View(tModel);
                }
                else
                {
                    return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
                }


            }
            catch (Exception pError)
            {
                return View(ConstantStringResources.cERROR_VIEW, new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
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
                    var tModel = SortQuestionsList(mQuestionManager.QuestionsList);
                    return View("Index", tModel);
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
                if (mListChanged) {
                    mListChanged = false;
                    return Json(mQuestionManager.QuestionsList, JsonRequestBehavior.AllowGet);
                }
                else
                    return new HttpStatusCodeResult(304);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new HttpStatusCodeResult(500);
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
        private void StartAutoRefreshThread()
        {
            try
            {
                //call auto refresh method from question manager
                if (mQuestionManager != null)
                {
                    QuestionManager tQuestionManager = mQuestionManager as QuestionManager;
                    tQuestionManager.QuestionListChangedEventHandler += new QuestionManager.RefreshListDelegate(QuestionListChanged);
                    mQuestionManager.StartAutoRefresh(mAutoRefreshInterval);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        private void QuestionListChanged()
        {
            mListChanged = true;
        }
        #region Sort questions list
        /// <summary>
        /// Toggle SortOrder between Ascending and Descending
        /// </summary>
        private void ToggleSortOrder()
        {
            try
            {

                if ((Session["SortOrder"] as SortOrder?) == SortOrder.Ascending)
                    Session["SortOrder"] = SortOrder.Descending;
                else
                    Session["SortOrder"] = SortOrder.Ascending;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        /// <summary>
        /// Change list sort method
        /// </summary>
        /// <param name="pNewSortMethod">list Sort Method</param>
        private void SetSortMethod(SortMethod pNewSortMethod)
        {
            try
            {
                // if the new sort method is the same as old one , just toggle sort order, else change sort method to new one with Ascending order
                SortMethod? tSortMethod = Session["SortMethod"] as SortMethod?;
                if (tSortMethod == pNewSortMethod)
                    ToggleSortOrder();
                else
                {
                    Session["SortOrder"] = SortOrder.Ascending;
                    Session["SortMethod"] = pNewSortMethod;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        /// <summary>
        ///  Sort Items list according to given order and method
        /// </summary>
        public IEnumerable<BaseQuestion> SortQuestionsList(IEnumerable<BaseQuestion> pQuestionsList)
        {
            try
            {
                // initialize temporary list with old list capacity to avoid list resizing.
                IEnumerable<BaseQuestion> tSortedList = new List<BaseQuestion>(pQuestionsList.Count());
                //Sort Items list according to given ordering method using linq
                SortMethod? tSortMethod = Session["SortMethod"] as SortMethod?;
                SortOrder? tSortOrder = Session["SortOrder"] as SortOrder?;
                switch (tSortMethod)
                {
                    case SortMethod.ByQuestionID:
                        tSortedList = pQuestionsList.OrderBy(Item => Item.Id).ToList();
                        break;
                    case SortMethod.ByQuestionOrder:
                        tSortedList = pQuestionsList.OrderBy(Item => Item.Order).ToList();
                        break;
                    case SortMethod.ByQuestionText:
                        tSortedList = pQuestionsList.OrderBy(Item => Item.Text).ToList();
                        break;
                    case SortMethod.ByQuestionType:
                        tSortedList = pQuestionsList.OrderBy(Item => Item.Type).ToList();
                        break;
                }
                // temporary ordered list is sorted in ascending order, if the required order is descending then reverse it
                if (tSortOrder == SortOrder.Descending)
                {
                    return tSortedList.Reverse();
                }
                return tSortedList;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return pQuestionsList;
            }
        }
        #endregion
      
    }
}