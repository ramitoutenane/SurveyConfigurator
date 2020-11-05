using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IRepository<Question>
    {
        private readonly SliderQuestionDatabaseOperations sliderSQL = new SliderQuestionDatabaseOperations();
        private readonly SmileyQuestionDatabaseOperations smileySQL = new SmileyQuestionDatabaseOperations();
        private readonly StarsQuestionDatabaseOperations starsSQL = new StarsQuestionDatabaseOperations();
        private readonly String connectionString;
        public List<Question> Items { get; private set; }
        QuestionManager(string connectionString)
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
                    smiley = smileySQL.SelectAll(connection);
                    stars = starsSQL.SelectAll(connection);
                }
                List<Question> allQuestion = new List<Question>(slider.Count + smiley.Count + stars.Count);
                allQuestion.AddRange(slider);
                allQuestion.AddRange(smiley);
                allQuestion.AddRange(stars);
                IEnumerable<Question> sortedQuestions = from Question question in allQuestion
                                                        orderby question.ID
                                                        select question;
                Items = sortedQuestions.ToList();
                return Items;
            }
            catch
            {
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    switch (item)
                    {
                        case SliderQuestion slider:
                            break;
                        case SmileyQuestion smiley:
                            break;
                        case StarsQuestion stars:
                            break;
                        case null:
                            throw new ArgumentNullException("Trying to add invalid (null) Question");
                        default:
                            throw new ArgumentException("Question type is not recognized");
                    }
                }

            }
            catch(ArgumentNullException ex)
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
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
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
            if (end >= start)
            {
                int mid = start + (end - start) / 2;

                if (Items[mid].ID == id)
                    return mid;

                if (Items[mid].ID > id)
                    return SearchByID(start, mid - 1, id);

                return SearchByID(mid + 1, end, id);
            }

            return -1;
        }
    }
}
