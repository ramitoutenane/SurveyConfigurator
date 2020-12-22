using QuestionManaging;
using SurveyConfiguratorEntities;
using SurveyConfiguratorWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Controllers
{
    public class SurveyController : Controller
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

            }

        }
        public ActionResult Index(SortMethod? pSortMethod)
        {
            try
            {
                if(Session["SortMethod"] == null || Session["SortOrder"] == null)
                {
                    Session["SortMethod"] = SortMethod.ByQuestionID; 
                    Session["SortOrder"] = SortOrder.Ascending;
                }
                if(pSortMethod != null)
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
                    return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
                }


            }
            catch (Exception pError)
            {
                return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                BaseQuestion tQuestion;
                Reslut tReadResult = mQuestionManager.Read(Id, out tQuestion);
                if (tReadResult.Value == ResultValue.Success)
                    return View(tQuestion);
                else
                    return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });

            }
            catch (Exception pError)
            {
                return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
            }
        }
        public ActionResult Create(QuestionType pQuestionType)
        {
            try
            {
                switch (pQuestionType)
                {
                    case QuestionType.Slider:
                        return View("CreateSlider");
                        break;
                    case QuestionType.Stars:
                        return View("CreateStars");
                        break;
                    case QuestionType.Smiley:
                        return View("CreateSmiley");
                        break;
                    default:
                        return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
                        break;
                }
            }
            catch (Exception pError)
            {
                return View("Error", new ErrorViewModel() { ErrorTitle = "", ErrorMessage = "" });
            }
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