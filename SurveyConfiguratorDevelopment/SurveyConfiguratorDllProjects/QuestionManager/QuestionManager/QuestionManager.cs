using System;
using System.Collections.Generic;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IQuestionRepository
    {
        /// <summary>
        /// List of questions to be maintained 
        /// </summary>
        public List<Question> QuestionsList { get; private set; }
        private readonly DatabaseSettings mDatabaseSettings;
        private readonly IDatabaseOperations<SliderQuestion> mSliderSQL;
        private readonly IDatabaseOperations<SmileyQuestion> mSmileySQL;
        private readonly IDatabaseOperations<StarsQuestion> mStarsSQL;

        /// <summary>
        /// QuestionManager constructor to initialize new QuestionManager object
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public QuestionManager(DatabaseSettings databaseSettings)
        {
            try
            {
                QuestionsList = new List<Question>();
                mDatabaseSettings = databaseSettings;
                mSliderSQL = new SliderQuestionDatabaseOperations(mDatabaseSettings);
                mSmileySQL = new SmileyQuestionDatabaseOperations(mDatabaseSettings);
                mStarsSQL = new StarsQuestionDatabaseOperations(mDatabaseSettings);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Synchronize local Questions list with latest version of questions from database
        /// </summary>
        /// <returns>The new refreshed Questions List</returns>
        public List<Question> SelectAll()
        {
            try
            {
                // select all questions of each question type from database
                List<SliderQuestion> tSliderList = mSliderSQL.SelectAll();
                List<SmileyQuestion> tSmileyList = mSmileySQL.SelectAll();
                List<StarsQuestion> tStarsList = mStarsSQL.SelectAll();
                // create new temporary list to merge previous lists,it's initial capacity is equal to the sum of all question lists 
                List<Question> tAllQuestion = new List<Question>(tSliderList.Count + tSmileyList.Count + tStarsList.Count);
                // add all question lists to the temporary list that contains all questions 
                tAllQuestion.AddRange(tSliderList);
                tAllQuestion.AddRange(tSmileyList);
                tAllQuestion.AddRange(tStarsList);
                // set the value of Questions list to the new created list
                QuestionsList = tAllQuestion;
                // sort the Questions list and return it
                return QuestionsList;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
        /// <summary>
        /// Insert question to database and local questions list
        /// </summary>
        /// <param name="question">The new question to be inserted</param>
        public bool Insert(Question question)
        {
            try
            {
                // create temporary id variable and question object reference
                int tID = -1;
                Question tQuestion = null;

                if (question is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return false;
                }
                else if (!question.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return false;
                }
                //check question type then add it to database and get it's primary key from SQL insert method
                switch (question.Type)
                {
                    case QuestionType.Slider:
                        tID = mSliderSQL.Insert(question as SliderQuestion);
                        break;
                    case QuestionType.Smiley:
                        tID = mSmileySQL.Insert(question as SmileyQuestion);
                        break;
                    case QuestionType.Stars:
                        tID = mStarsSQL.Insert(question as StarsQuestion);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }
                
                // if the question inserted to database successfully the temporary id variable should change from -1 
                if (tID >= 1)
                {
                    tQuestion = question.CopyWithNewId(tID);
                    // add Question to local questions list and reorder the list 
                    if(tQuestion != null)
                        QuestionsList.Add(tQuestion);
                        return true;
                }
                return false;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
        /// <summary>
        /// Update question in database and local questions list
        /// </summary>
        /// <param name="question">The new question to be updated</param>
        public bool Update(Question question)
        {
            try
            {

                if (question is null)
                {
                    ErrorLogger.Log(new ArgumentNullException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    return false;
                }
                else if (!question.IsValid())
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_VALIDATION_EXCEPTION));
                    return false;
                }
                // search for the question in local list 
                Question tQuestionFindResult = QuestionsList.Find(tQuestion => tQuestion.Id == question.Id);
                if (tQuestionFindResult == null)
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cNO_QUESTION_ID));
                    return false;
                }

                bool tUpdated = false;
                // check question type then update it in database, if updated successfully in database then update it in local list.
                switch (question.Type)
                {
                    case QuestionType.Slider:
                        tUpdated = mSliderSQL.Update(question as SliderQuestion);
                        break;
                    case QuestionType.Smiley:
                        tUpdated = mSmileySQL.Update(question as SmileyQuestion);
                        break;
                    case QuestionType.Stars:
                        tUpdated = mStarsSQL.Update(question as StarsQuestion);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }
                // if updated successfully in database update it in local list.
                if (tUpdated)
                {
                    QuestionsList[QuestionsList.FindIndex(tQuestion => tQuestion.Id == question.Id)] = question;
                    return true;
                }
                return false;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
        /// <summary>
        /// Delete question from database and local list
        /// </summary>
        /// <param name="id">The id of question to be deleted</param>
        public bool Delete(int id)
        {
            try
            {
                // check question type then delete it from database, if deleted successfully from database then delete it in local list.
                // search for the question in local list 
                Question tQuestionFindResult = QuestionsList.Find(tQuestion => tQuestion.Id == id);
                bool tDeleted = false;
                if (tQuestionFindResult == null)
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cNO_QUESTION_ID));
                    return false;
                }
                switch (tQuestionFindResult.Type)
                {
                    case QuestionType.Slider:
                        tDeleted = mSliderSQL.Delete(id);
                        break;
                    case QuestionType.Smiley:
                        tDeleted = mSmileySQL.Delete(id);
                        break;
                    case QuestionType.Stars:
                        tDeleted = mStarsSQL.Delete(id);
                        break;
                    default:
                        ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                        return false;
                }
                // if deleted successfully from database then delete it in local list.
                if (tDeleted)
                {
                    QuestionsList.RemoveAll(tQuestion => tQuestion.Id == id);
                    return true;
                }
                return false;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
    }
}