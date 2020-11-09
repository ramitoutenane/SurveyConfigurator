using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IRepository<Question>
    {
        private readonly SliderQuestionDatabaseOperations sliderSQL = new SliderQuestionDatabaseOperations();
        private readonly SmileyQuestionDatabaseOperations smileySQL = new SmileyQuestionDatabaseOperations();
        private readonly StarsQuestionDatabaseOperations starsSQL = new StarsQuestionDatabaseOperations();
        private readonly string connectionString;
        public List<Question> Items { get; private set; }
        private SortOrder sortOrder = SortOrder.Ascending;
        private OrderingMethod orderingMethod = OrderingMethod.ByID;

        public QuestionManager(string connectionString)
        {
            Items = new List<Question>();
            this.connectionString = connectionString;
        }

        public List<Question> Refresh()
        {
            try
            {

                List<SliderQuestion> slider;
                List<SmileyQuestion> smiley;
                List<StarsQuestion> stars;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    slider = sliderSQL.SelectAll(connection);
                   
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    smiley = smileySQL.SelectAll(connection);
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    stars = starsSQL.SelectAll(connection);
                }
                List<Question> allQuestion = new List<Question>(slider.Count + smiley.Count + stars.Count);
                allQuestion.AddRange(slider);
                allQuestion.AddRange(smiley);
                allQuestion.AddRange(stars);
                Items = allQuestion;
                OrderList(orderingMethod, sortOrder);
                return Items;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (IsAvailable(connection))
                        throw new QuestionListRefreshException("An Error occurred while refreshing list, Please try again");
                    else
                        throw new Exception("Connection to Database is not Available");
                }

            }
        }
        public Question Select(int id)
        {
            if (Items.Count > 0)
            {
                int index = SearchByID(0, Items.Count, id);
                if (index != -1)
                    return Items[index];
            }
            return null;
        }
        public void Insert(Question item)
        {
            try
            {
                int id = -1;
                Question question = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (item is null)
                    {
                        throw new ArgumentNullException("Trying to add invalid (null) Question");
                    }
                    else if (item is SliderQuestion slider)
                    {

                        id = sliderSQL.Insert(connection, slider);
                        question = new SliderQuestion(slider, id);
                    }
                    else if (item is SmileyQuestion smiley)
                    {
                        id = smileySQL.Insert(connection, smiley);
                        question = new SmileyQuestion(smiley, id);
                    }
                    else if (item is StarsQuestion stars)
                    {
                        id = starsSQL.Insert(connection, stars);
                        question = new StarsQuestion(stars, id);
                    }
                    else
                    {
                        throw new ArgumentException("Question type is not recognized");

                    }

                }
                if (id >= 1 && question != null)
                {
                    Items.Add(question);
                    OrderList(orderingMethod, sortOrder);
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (IsAvailable(connection))
                        throw new QuestionInsertException("An Error occurred while inserting question, Please try again");
                    else
                        throw new Exception("Connection to Database is not Available");
                }

            }
        }
        public void Update(Question item)
        {
            try
            {
                if (item is null)
                {
                    throw new ArgumentNullException("Trying to update invalid (null) Question");
                }

                int index = SearchByID(0, Items.Count, item.ID);
                if (index < 0)
                {
                    throw new QuestionUpdateException("Couldn't find an item with given id");
                }
                bool updated = false;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (item is SliderQuestion slider)
                    {
                        updated = sliderSQL.Update(connection, slider);

                    }
                    else if (item is SmileyQuestion smiley)
                    {
                        updated = smileySQL.Update(connection, smiley);

                    }
                    else if (item is StarsQuestion stars)
                    {
                        updated = starsSQL.Update(connection, stars);
                    }
                    else
                    {
                        throw new ArgumentException("Question type is not recognized");

                    }

                }
                if (updated)
                {
                    Items[index] = item;
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (IsAvailable(connection))
                        throw new QuestionUpdateException("An Error occurred while updating question, Please try again");
                    else
                        throw new Exception("Connection to Database is not Available");
                }

            }
        }
        public void Delete(int id)
        {
            try
            {
                int index = SearchByID(0, Items.Count, id);
                if (index < 0)
                {
                    throw new QuestionDeleteException("Couldn't find an item with given id");
                }
                bool deleted = false;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (Items[index] is SliderQuestion slider)
                    {
                        deleted = sliderSQL.Delete(connection, id);
                    }
                    else if (Items[index] is SmileyQuestion smiley)
                    {
                        deleted = smileySQL.Delete(connection, id);

                    }
                    else if (Items[index] is StarsQuestion stars)
                    {
                        deleted = starsSQL.Delete(connection, id);
                    }
                    else
                    {
                        throw new ArgumentException("Question type is not recognized");
                    }

                }
                if (deleted)
                {
                    Items.RemoveAt(index);
                }
                else
                {
                    throw new QuestionDeleteException("couldn't delete question");
                }
            }
            catch
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (IsAvailable(connection))
                        throw new QuestionDeleteException("An Error occurred while deleting question, Please try again");
                    else
                        throw new Exception("Connection to Database is not Available");
                }

            }
        }

        public void OrderList(OrderingMethod orderingMethod, SortOrder sortOrder)
        {
            this.sortOrder = sortOrder;
            this.orderingMethod = orderingMethod;
            List<Question> orderedList = new List<Question>(Items.Count);
            switch (this.orderingMethod)
            {
                case OrderingMethod.ByID:
                    orderedList = Items.OrderBy(Item => Item.ID).ToList();
                    break;
                case OrderingMethod.ByOrder:
                    orderedList = Items.OrderBy(Item => Item.Order).ToList();
                    break;
                case OrderingMethod.ByQuestionText:
                    orderedList = Items.OrderBy(Item => Item.Text).ToList();
                    break;
                case OrderingMethod.ByType:
                    orderedList = Items.OrderBy(Item => Item.Type).ToList();
                    break;
            }
            if(this.sortOrder == SortOrder.Descending)
            {
                orderedList.Reverse();
            }
            Items = orderedList;
        }

        public bool isConnectedToDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return IsAvailable(connection);
            }
        }

        private bool IsAvailable(SqlConnection connection)
        {
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }

        private int SearchByID(int start, int end, int id)
        {
            for(int i = start; i<end; i++)
            {
                if (Items[i].ID == id)
                    return i;
            }
            return -1;
        }
    }
}
