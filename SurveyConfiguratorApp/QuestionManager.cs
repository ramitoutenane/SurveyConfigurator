using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IRepository<Question>
    {
        /// <summary>
        /// List of questions to be maintained 
        /// </summary>
        public List<Question> Items { get; private set; }
        /// <summary>
        /// The direction of list sorting (Ascending, Descending)
        /// </summary>
        public SortOrder SortOrder { get; private set; }
        /// <summary>
        /// The ordering criteria of the list By(ID, Type, Text, Order)
        /// </summary>
        public SortMethod OrderingMethod { get; private set; }
        private readonly string mConnectionString;
        private readonly SliderQuestionDatabaseOperations mSliderSQL;
        private readonly SmileyQuestionDatabaseOperations mSmileySQL;
        private readonly StarsQuestionDatabaseOperations mStarsSQL;

        /// <summary>
        /// QuestionManager constructor to initialize new QuestionManager object
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public QuestionManager(string connectionString)
        {
            Items = new List<Question>();
            mConnectionString = connectionString;
            mSliderSQL = new SliderQuestionDatabaseOperations(mConnectionString);
            mSmileySQL = new SmileyQuestionDatabaseOperations(mConnectionString);
            mStarsSQL = new StarsQuestionDatabaseOperations(mConnectionString);
            SortOrder = SortOrder.Ascending;
            OrderingMethod = SortMethod.ByID;
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
                SortList(OrderingMethod, SortOrder);
                return Items;
            }
            catch (Exception ex)
            {
                if (IsConnected())
                    throw new QuestionListRefreshException("An Error occurred while refreshing list, Please try again");
                else
                    throw new Exception("Connection to Database is not Available");
            }
        }
        /// <summary>
        /// Select specific question from the questions list
        /// </summary>
        /// <param name="id">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public Question Select(int id)
        {
            int tIndex = SearchByID(id);
            // SearchByID method returns the index of item if it exist, -1 otherwise
            if (tIndex != -1)
                return Items[tIndex];
            return null;
        }
        /// <summary>
        /// Insert question to database and local questions list
        /// </summary>
        /// <param name="item">The new question to be inserted</param>
        public void Insert(Question item)
        {
            try
            {
                // create temporary id variable and question object reference
                int tID = -1;
                Question tQuestion = null;
                //check question type then add it to database and get it's primary key from SQL insert method, set the new id to question object add it to local list.
                if (item is null)
                {
                    throw new ArgumentNullException("Trying to add invalid (null) Question");
                }
                else if (item is SliderQuestion slider)
                {

                    tID = mSliderSQL.Insert(slider);
                    tQuestion = new SliderQuestion(slider, tID);
                }
                else if (item is SmileyQuestion smiley)
                {
                    tID = mSmileySQL.Insert(smiley);
                    tQuestion = new SmileyQuestion(smiley, tID);
                }
                else if (item is StarsQuestion stars)
                {
                    tID = mStarsSQL.Insert(stars);
                    tQuestion = new StarsQuestion(stars, tID);
                }
                else
                {
                    throw new ArgumentException("Question type is not recognized");

                }
                // if the question inserted to database successfully the temporary id variable should change from -1 and temporary question reference should point to new question object 
                if (tID >= 1 && tQuestion != null)
                {
                    // add item to local questions list and reorder the list 
                    Items.Add(tQuestion);
                    SortList(OrderingMethod, SortOrder);
                }
                else
                {
                    throw new QuestionInsertException("couldn't insert question to database");
                }

            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch
            {
                if (IsConnected())
                    throw new QuestionInsertException("An Error occurred while inserting question, Please try again");
                else
                    throw new Exception("Connection to Database is not Available");
            }
        }
        /// <summary>
        /// Update question in database and local questions list
        /// </summary>
        /// <param name="item">The new question to be updated</param>
        public void Update(Question item)
        {
            try
            {
                // check question type then update it in database, if updated successfully in database then update it in local list.
                if (item is null)
                {
                    throw new ArgumentNullException("Trying to update invalid (null) Question");
                }
                // search for the question in local list 
                int tIndex = SearchByID(item.Id);
                bool tUpdated = false;
                if (tIndex < 0)
                {
                    throw new QuestionUpdateException("Couldn't find an item with given id");
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
                    throw new ArgumentException("Question type is not recognized");

                }
                // if updated successfully in database update it in local list.
                if (tUpdated)
                {
                    Items[tIndex] = item;
                }
                else
                {
                    throw new QuestionUpdateException("couldn't update question");
                }
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch
            {
                if (IsConnected())
                    throw new QuestionUpdateException("An Error occurred while updating question, Please try again");
                else
                    throw new Exception("Connection to Database is not Available");

            }
        }
        /// <summary>
        /// Delete question from database and local list
        /// </summary>
        /// <param name="id">The id of question to be deleted</param>
        public void Delete(int id)
        {
            try
            {
                // check question type then delete it from database, if deleted successfully from database then delete it in local list.
                // search for the question in local list 
                int tIndex = SearchByID(id);
                bool tDeleted = false;
                if (tIndex < 0)
                {
                    throw new QuestionDeleteException("Couldn't find an item with given id");
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
                    throw new ArgumentException("Question type is not recognized");
                }
                // if deleted successfully from database then delete it in local list.
                if (tDeleted)
                {
                    Items.RemoveAt(tIndex);
                }
                else
                {
                    throw new QuestionDeleteException("couldn't delete question");
                }
            }
            catch
            {
                if (IsConnected())
                    throw new QuestionDeleteException("An Error occurred while deleting question, Please try again");
                else
                    throw new Exception("Connection to Database is not Available");

            }
        }
        /// <summary>
        ///  Sort Items list according to given order and method
        /// </summary>
        /// <param name="orderingMethod">The ordering criteria of the list By(ID, Type, Text, Order)</param>
        /// <param name="sortOrder">The direction of list sorting (Ascending, Descending)</param>
        public void SortList(SortMethod orderingMethod, SortOrder sortOrder)
        {
            SortOrder = sortOrder;
            OrderingMethod = orderingMethod;
            // initialize temporary list with old list capacity to avoid list resizing.
            List<Question> tSortedList = new List<Question>(Items.Count);
            //Sort Items list according to given ordering method using linq
            switch (OrderingMethod)
            {
                case SortMethod.ByID:
                    tSortedList = Items.OrderBy(Item => Item.Id).ToList();
                    break;
                case SortMethod.ByOrder:
                    tSortedList = Items.OrderBy(Item => Item.Order).ToList();
                    break;
                case SortMethod.ByQuestionText:
                    tSortedList = Items.OrderBy(Item => Item.Text).ToList();
                    break;
                case SortMethod.ByType:
                    tSortedList = Items.OrderBy(Item => Item.Type).ToList();
                    break;
            }
            // temporary ordered list is sorted in ascending order, if the required order is descending then reverse it
            if (SortOrder == SortOrder.Descending)
            {
                tSortedList.Reverse();
            }
            // set local Items list to the new sorted list
            Items = tSortedList;
        }
        /// <summary>
        /// Check if database of given connection string is connected
        /// </summary>
        /// <returns>true if database is connected, false otherwise</returns>
        public bool IsConnected()
        {
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                try
                {
                    // if data base is connected, no SqlException will be raised and true is returned
                    tConnection.Open();
                    tConnection.Close();
                }
                catch (SqlException)
                {
                    // if database is disconnected, SqlException will be raised and false is returned
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Search for question by it's id
        /// </summary>
        /// <param name="id">Id of question to search for</param>
        /// <returns>Index of question if found, -1 otherwise</returns>
        private int SearchByID(int id)
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
    }
}
