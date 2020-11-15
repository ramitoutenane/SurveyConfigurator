using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IMaintainable<Question>
    {
        /// <summary>
        /// List of questions to be maintained 
        /// </summary>
        public List<Question> Items { get; private set; }
        private readonly string mConnectionString;
        private readonly IRepository<SliderQuestion> mSliderSQL;
        private readonly IRepository<SmileyQuestion> mSmileySQL;
        private readonly IRepository<StarsQuestion> mStarsSQL;

        /// <summary>
        /// QuestionManager constructor to initialize new QuestionManager object
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public QuestionManager(string connectionString)
        {
            try
            {
                Items = new List<Question>();
                mConnectionString = connectionString;
                mSliderSQL = new SliderQuestionDatabaseOperations(mConnectionString);
                mSmileySQL = new SmileyQuestionDatabaseOperations(mConnectionString);
                mStarsSQL = new StarsQuestionDatabaseOperations(mConnectionString);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Synchronize local Items list with latest version of questions from database
        /// </summary>
        /// <returns>The new refreshed Items List</returns>
        public List<Question> Refresh()
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
                // set the value of Items list to the new created list
                Items = tAllQuestion;
                // sort the Items list and return it
                return Items;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
        /// <summary>
        /// Select specific question from the questions list
        /// </summary>
        /// <param name="id">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public Question Select(int id)
        {
            try
            {
                int tIndex = SearchByID(id);
                // SearchByID method returns the index of item if it exist, -1 otherwise
                if (tIndex != -1)
                    return Items[tIndex];
                return null;
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
        /// <param name="item">The new question to be inserted</param>
        public bool Insert(Question item)
        {
            try
            {
                // create temporary id variable and question object reference
                int tID = -1;
                Question tQuestion = null;
                //check question type then add it to database and get it's primary key from SQL insert method, set the new id to question object add it to local list.
                if (item is null)
                {
                    throw new ArgumentNullException(MessageStringResources.cQUESTION_NULL_Exception);
                }
                else if (item is SliderQuestion slider)
                {

                    tID = mSliderSQL.Create(slider);
                    tQuestion = new SliderQuestion(slider, tID);
                }
                else if (item is SmileyQuestion smiley)
                {
                    tID = mSmileySQL.Create(smiley);
                    tQuestion = new SmileyQuestion(smiley, tID);
                }
                else if (item is StarsQuestion stars)
                {
                    tID = mStarsSQL.Create(stars);
                    tQuestion = new StarsQuestion(stars, tID);
                }
                else
                {
                    throw new ArgumentException(MessageStringResources.cQUESTION_TYPE_Exception);
                }
                // if the question inserted to database successfully the temporary id variable should change from -1 and temporary question reference should point to new question object 
                if (tID >= 1 && tQuestion != null)
                {
                    // add item to local questions list and reorder the list 
                    Items.Add(tQuestion);
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
        /// <param name="item">The new question to be updated</param>
        public bool Update(Question item)
        {
            try
            {
                // check question type then update it in database, if updated successfully in database then update it in local list.
                if (item is null)
                {
                    throw new ArgumentNullException(MessageStringResources.cQUESTION_NULL_Exception);
                }
                // search for the question in local list 
                int tIndex = SearchByID(item.Id);
                bool tUpdated = false;
                if (tIndex < 0)
                {
                    throw new ArgumentException(MessageStringResources.cNO_QUESTION_ID);
                }
                if (item is SliderQuestion slider)
                {
                    tUpdated = mSliderSQL.Update(slider);
                }
                else if (item is SmileyQuestion smiley)
                {
                    tUpdated = mSmileySQL.Update(smiley);
                }
                else if (item is StarsQuestion stars)
                {
                    tUpdated = mStarsSQL.Update(stars);
                }
                else
                {
                    throw new ArgumentException(MessageStringResources.cQUESTION_TYPE_Exception);
                }
                // if updated successfully in database update it in local list.
                if (tUpdated)
                {
                    Items[tIndex] = item;
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
                int tIndex = SearchByID(id);
                bool tDeleted = false;
                if (tIndex < 0)
                {
                    throw new ArgumentException(MessageStringResources.cNO_QUESTION_ID);
                }
                if (Items[tIndex] is SliderQuestion slider)
                {
                    tDeleted = mSliderSQL.Delete(id);
                }
                else if (Items[tIndex] is SmileyQuestion smiley)
                {
                    tDeleted = mSmileySQL.Delete(id);

                }
                else if (Items[tIndex] is StarsQuestion stars)
                {
                    tDeleted = mStarsSQL.Delete(id);
                }
                else
                {
                    throw new ArgumentException(MessageStringResources.cQUESTION_TYPE_Exception);
                }
                // if deleted successfully from database then delete it in local list.
                if (tDeleted)
                {
                    Items.RemoveAt(tIndex);
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
        /// Search for question by it's id
        /// </summary>
        /// <param name="id">Id of question to search for</param>
        /// <returns>Index of question if found, -1 otherwise</returns>
        private int SearchByID(int id)
        {
            try
            {
                if (Items != null && Items.Count > 0)
                    // loop through all question and check if question id is equal to search id
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (Items[i].Id == id)
                            return i;
                    }
                return -1;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return -1;
            }

        }
    }
}