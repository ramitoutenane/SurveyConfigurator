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
        /// <returns>Result object of operation</returns>
        public Reslut RefreshQuestionList()
        {
            try
            {
                // select all questions of each question type from database
                List<SliderQuestion> tSliderList = mSliderSQL.SelectAll();
                if (tSliderList == null)
                    return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cREFRESH_FAIL_MESSAGE);


                List<SmileyQuestion> tSmileyList = mSmileySQL.SelectAll();
                if (tSmileyList == null)
                    return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cREFRESH_FAIL_MESSAGE);


                List<StarsQuestion> tStarsList = mStarsSQL.SelectAll();
                if (tStarsList == null)
                    return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cREFRESH_FAIL_MESSAGE);


                // create new temporary list to merge previous lists,it's initial capacity is equal to the sum of all question lists 
                List<BaseQuestion> tAllQuestion = new List<BaseQuestion>(tSliderList.Count + tSmileyList.Count + tStarsList.Count);

                // add all question lists to the temporary list that contains all questions 
                tAllQuestion.AddRange(tSliderList);
                tAllQuestion.AddRange(tSmileyList);
                tAllQuestion.AddRange(tStarsList);

                // set the value of Questions list to the new created list
                QuestionsList = tAllQuestion;
                // sort the Questions list and return it
                return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cREFRESH_SUCCESS_MESSAGE);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cREFRESH_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Insert question to database and local questions list
        /// </summary>
        /// <param name="pQuestion">The new question to be inserted</param>
        public Reslut Create(BaseQuestion pQuestion)
        {
            try
            {
                // create temporary id variable and question object reference
                Reslut tInsertedResponse;
                if (pQuestion is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
                }
                else if (!pQuestion.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
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
                        return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
                }

                // if the question inserted to database successfully the temporary id variable should change from -1 
                if (tInsertedResponse.Value == ResultValue.Success)
                {
                    // add Question to local questions list 
                    QuestionsList.Add(pQuestion);
                    return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cINSERT_SUCCESS_MESSAGE);
                }
                return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cINSERT_FAIL_MESSAGE);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Read question from repository
        /// </summary>
        /// <param name="pId">The id of question to read</param>
        /// <param name="pQuestion">Object to return selected question</param>
        /// <returns></returns>
        public Reslut Read(int pId, out BaseQuestion pQuestion)
        {
            try
            {
                pQuestion = QuestionsList.FirstOrDefault(tQuestion => tQuestion.Id == pId);
                if (pQuestion != null)
                    return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cREAD_SUCCESS_MESSAGE);
                else
                    return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cREAD_NOT_FOUND_MESSAGE);
                
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                pQuestion = null;
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cREAD_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Update question in database and local questions list
        /// </summary>
        /// <param name="pQuestion">The new question to be updated</param>
        public Reslut Update(BaseQuestion pQuestion)
        {
            try
            {
                if (pQuestion is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
                }
                else if (!pQuestion.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
                }
                // search for the question in local list 
                BaseQuestion tQuestionFindResult = QuestionsList.Find(tQuestion => tQuestion.Id == pQuestion.Id);
                if (tQuestionFindResult == null)
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cNO_QUESTION_ID));
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
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
                        return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
                }
                // if updated successfully in database update it in local list.
                if (tUpdatedResponse.Value == ResultValue.Success)
                {
                    QuestionsList[QuestionsList.FindIndex(tQuestion => tQuestion.Id == pQuestion.Id)] = pQuestion;
                    return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cUPDATE_SUCCESS_MESSAGE);
                }
                return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cUPDATE_FAIL_MESSAGE);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Delete question from database and local list
        /// </summary>
        /// <param name="pId">The id of question to be deleted</param>
        public Reslut Delete(int pId)
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
                    return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cDELETE_ERROR_MESSAGE);
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
                        return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cDELETE_ERROR_MESSAGE);
                }
                // if deleted successfully from database then delete it in local list.
                if (tDeletedResponse.Value == ResultValue.Success)
                {
                    QuestionsList.RemoveAll(tQuestion => tQuestion.Id == pId);
                    return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cDELETE_SUCCESS_MESSAGE);
                }
                return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cDELETE_FAIL_MESSAGE);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cDELETE_ERROR_MESSAGE);
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
        private bool IsChanged()
        {
            try
            {
                //get a copy of list and refresh it to get latest from database
                List<BaseQuestion> tOldQuestionsList = QuestionsList;
                RefreshQuestionList();

                //if lists count is not equal then it has been changed
                if (QuestionsList.Count != tOldQuestionsList.Count)
                    return true;

                //get the difference set between lists, if the difference count is 0 then they are equal and the list has not been changed
                return QuestionsList.Except(tOldQuestionsList).Count() != 0;

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
                if (QuestionListChangedEventHandler != null)
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
        /// <summary>
        /// Refresh the interface if the list is changed
        /// </summary>
        /// <param name="pRefreshInterval">Time to refresh in millisecond</param>
        private void AutoRefreshThreadWork(int pRefreshInterval)
        {
            try
            {
                mKeepAutoCheckThreadAlive = true;
                while (mKeepAutoCheckThreadAlive)
                {
                    Thread.Sleep(pRefreshInterval);
                    if (QuestionListChangedEventHandler == null)
                        break;
                    if (IsConnected() && QuestionsList != null && IsChanged())
                    {
                        //fire auto refresh event
                        QuestionListChangedEventHandler();
                    }

                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        /// <summary>
        /// Stop auto refresh thread loop
        /// </summary>
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