using DatabaseOperations;
using SurveyConfiguratorEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QuestionManaging
{
    public class QuestionManager : IQuestionRepository
    {
        #region Variable deceleration
        /// <summary>
        /// List of questions to be maintained 
        /// </summary>
        private readonly DatabaseSettings mDatabaseSettings;
        private readonly BaseQuestionDatabaseOperations mQuestionDatabaseOperations;
        private readonly IDatabaseOperations<SliderQuestion> mSliderSQL;
        private readonly IDatabaseOperations<SmileyQuestion> mSmileySQL;
        private readonly IDatabaseOperations<StarsQuestion> mStarsSQL;
        public List<BaseQuestion> QuestionsList { get; private set; }
        private Thread mAutoCheckThread;
        private bool mKeepAutoCheckThreadAlive;
        public delegate void RefreshListDelegate();
        public event RefreshListDelegate QuestionListChangedEventHandler;



        #endregion
        #region Constructor
        /// <summary>
        /// QuestionManager constructor to initialize new QuestionManager object
        /// </summary>
        /// <param name="pDatabaseSettings">Database settings object</param>
        public QuestionManager(DatabaseSettings pDatabaseSettings)
        {
            try
            {
                QuestionsList = new List<BaseQuestion>();
                mDatabaseSettings = pDatabaseSettings;
                mSliderSQL = new SliderQuestionDatabaseOperations(mDatabaseSettings);
                mSmileySQL = new SmileyQuestionDatabaseOperations(mDatabaseSettings);
                mStarsSQL = new StarsQuestionDatabaseOperations(mDatabaseSettings);
                mQuestionDatabaseOperations = new BaseQuestionDatabaseOperations(mDatabaseSettings);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Synchronize local Questions list with latest version of questions from database
        /// </summary>
        /// <returns>The new refreshed Questions List</returns>
        public List<BaseQuestion> SelectAll()
        {
            try
            {
                // select all questions of each question type from database
                List<SliderQuestion> tSliderList = mSliderSQL.SelectAll();
                List<SmileyQuestion> tSmileyList = mSmileySQL.SelectAll();
                List<StarsQuestion> tStarsList = mStarsSQL.SelectAll();
                if(tSliderList == null || tSmileyList == null || tStarsList == null)
                {
                    ErrorLogger.Log(new Exception(ErrorMessages.cSELECT_ERROR));
                    return null;
                }
                // create new temporary list to merge previous lists,it's initial capacity is equal to the sum of all question lists 
                List<BaseQuestion> tAllQuestion = new List<BaseQuestion>(tSliderList.Count + tSmileyList.Count + tStarsList.Count);
                // add all question lists to the temporary list that contains all questions 
                tAllQuestion.AddRange(tSliderList);
                tAllQuestion.AddRange(tSmileyList);
                tAllQuestion.AddRange(tStarsList);
                // set the value of Questions list to the new created list
                QuestionsList = tAllQuestion;
                // sort the Questions list and return it
                return QuestionsList;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
            }
        }
        /// <summary>
        /// Insert question to database and local questions list
        /// </summary>
        /// <param name="pQuestion">The new question to be inserted</param>
        public bool Insert(BaseQuestion pQuestion)
        {
            try
            {
                // create temporary id variable and question object reference
                Reslut tInsertedResponse;
                if (pQuestion is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return false;
                }
                else if (!pQuestion.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return false;
                }
                //check question type then add it to database and get it's primary key from SQL insert method
                switch (pQuestion.Type)
                {
                    case QuestionType.Slider:
                        tInsertedResponse = mSliderSQL.Insert(pQuestion as SliderQuestion);
                        break;
                    case QuestionType.Smiley:
                        tInsertedResponse = mSmileySQL.Insert(pQuestion as SmileyQuestion);
                        break;
                    case QuestionType.Stars:
                        tInsertedResponse = mStarsSQL.Insert(pQuestion as StarsQuestion);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }

                // if the question inserted to database successfully the temporary id variable should change from -1 
                if (tInsertedResponse.Status  == ResultValue.Success)
                {
                    // add Question to local questions list 
                    QuestionsList.Add(pQuestion);
                    return true;
                }
                return false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Update question in database and local questions list
        /// </summary>
        /// <param name="pQuestion">The new question to be updated</param>
        public bool Update(BaseQuestion pQuestion)
        {
            try
            {
                if (pQuestion is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return false;
                }
                else if (!pQuestion.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return false;
                }
                // search for the question in local list 
                BaseQuestion tQuestionFindResult = QuestionsList.Find(tQuestion => tQuestion.Id == pQuestion.Id);
                if (tQuestionFindResult == null)
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cNO_QUESTION_ID));
                    return false;
                }

                Reslut tUpdatedResponse;
                // check question type then update it in database, if updated successfully in database then update it in local list.
                switch (pQuestion.Type)
                {
                    case QuestionType.Slider:
                        tUpdatedResponse = mSliderSQL.Update(pQuestion as SliderQuestion);
                        break;
                    case QuestionType.Smiley:
                        tUpdatedResponse = mSmileySQL.Update(pQuestion as SmileyQuestion);
                        break;
                    case QuestionType.Stars:
                        tUpdatedResponse = mStarsSQL.Update(pQuestion as StarsQuestion);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }
                // if updated successfully in database update it in local list.
                if (tUpdatedResponse.Status == ResultValue.Success)
                {
                    QuestionsList[QuestionsList.FindIndex(tQuestion => tQuestion.Id == pQuestion.Id)] = pQuestion;
                    return true;
                }
                return false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Delete question from database and local list
        /// </summary>
        /// <param name="pId">The id of question to be deleted</param>
        public bool Delete(int pId)
        {
            try
            {
                // check question type then delete it from database, if deleted successfully from database then delete it in local list.
                // search for the question in local list 
                BaseQuestion tQuestionFindResult = QuestionsList.Find(tQuestion => tQuestion.Id == pId);
                Reslut tDeletedResponse;
                if (tQuestionFindResult == null)
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cNO_QUESTION_ID));
                    return false;
                }
                switch (tQuestionFindResult.Type)
                {
                    case QuestionType.Slider:
                        tDeletedResponse = mSliderSQL.Delete(pId);
                        break;
                    case QuestionType.Smiley:
                        tDeletedResponse = mSmileySQL.Delete(pId);
                        break;
                    case QuestionType.Stars:
                        tDeletedResponse = mStarsSQL.Delete(pId);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }
                // if deleted successfully from database then delete it in local list.
                if (tDeletedResponse.Status == ResultValue.Success)
                {
                    QuestionsList.RemoveAll(tQuestion => tQuestion.Id == pId);
                    return true;
                }
                return false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        /// <summary>
        /// Check if source connection is available
        /// </summary>
        /// <returns>true if connected, false otherwise</returns>
        public bool IsConnected()
        {
            try
            {
                return mQuestionDatabaseOperations.IsConnected();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Check if local question list content and source content are equal
        /// </summary>
        /// <param name="pOldQuestionList">source list to compare to local list</param>
        /// <returns>true if equal, false otherwise</returns>
        private bool IsChanged(List<BaseQuestion> pOldQuestionList)
        {
            try
            {
                if (QuestionsList.Count != pOldQuestionList.Count)
                    return true;
                List<BaseQuestion> tOrderedOldQuestionList = pOldQuestionList.OrderBy(tQuestion => tQuestion.Id).ToList();
                List<BaseQuestion> tOrderedCurrentQuestionList = QuestionsList.OrderBy(tQuestion => tQuestion.Id).ToList();
                for (int i = 0; i < tOrderedCurrentQuestionList.Count; i++)
                {
                    if (!tOrderedCurrentQuestionList[i].Equals(tOrderedOldQuestionList[i]))
                        return true;
                }
                return false;
            }

            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return true;
            }
        }
        /// <summary>
        /// Start auto refresh thread
        /// </summary>
        /// <param name="pRefreshInterval">Time to refresh in millisecond</param>
        public void StartAutoRefresh(int pRefreshInterval)
        {
            try
            {
                if(QuestionListChangedEventHandler != null)
                {
                    if (mAutoCheckThread == null || !mAutoCheckThread.IsAlive)
                    {
                        //run new thread to call auto refresh delegate method
                        mAutoCheckThread = new Thread(new ThreadStart(() => AutoRefreshThreadWork(pRefreshInterval)));
                        mAutoCheckThread.IsBackground = true;
                        mAutoCheckThread.Start();
                    }

                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        private void AutoRefreshThreadWork(int pRefreshInterval)
        {
            try
            {
                mKeepAutoCheckThreadAlive = true;
                while (mKeepAutoCheckThreadAlive)
                {
                    if (IsConnected() && QuestionListChangedEventHandler != null)
                    {
                        Thread.Sleep(pRefreshInterval);
                        List<BaseQuestion> tOldQuestionsList = QuestionsList;
                        SelectAll();
                        if (QuestionsList != null && IsChanged(tOldQuestionsList))
                        {
                            //fire auto refresh event
                            QuestionListChangedEventHandler();
                        }

                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        public void StopAutoRefresh()
        {
            try
            {
                mKeepAutoCheckThreadAlive = false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
            
        }
        #endregion
    }
}