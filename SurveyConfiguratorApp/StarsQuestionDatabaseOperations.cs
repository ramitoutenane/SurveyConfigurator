using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class StarsQuestionDatabaseOperations : ICUDable<StarsQuestion, SqlConnection>, IQueryable<StarsQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        StarsQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }


        public int Insert(SqlConnection connection, StarsQuestion data)
        {
            int questionID = qda.Insert(connection, data);
            if (questionID < 1)
                return questionID;
            string commandString = "INSERT INTO star_question (question_id, num_of_stars) OUTPUT INSERTED.question_id VALUES (@id, @NumberOfStars)";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", questionID);
                    command.Parameters.AddWithValue("@NumberOfStars", data.NumberOfStars);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                qda.Delete(connection, questionID);
                return -1;
            }
        }
        public bool Update(SqlConnection connection, StarsQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE star_question SET num_of_stars = @NumberOfStars WHERE question_id = @id ";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", data.ID);
                    command.Parameters.AddWithValue("@NumberOfStars", data.NumberOfStars);
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(SqlConnection connection, int id) => qda.Delete(connection, id);

        public StarsQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT question_text, question_order, num_of_stars, question.question_id, type_id " +
                "FROM question, star_question WHERE question.question_id = @id AND question.question_id = star_question.question_id";

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            return new StarsQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public List<StarsQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "question_text, question_order, num_of_stars, question.question_id, type_id " +
                "FROM question, star_question WHERE question.question_id = star_question.question_id" +
                "ORDER BY question.question_id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@offset", offsit);
                    command.Parameters.AddWithValue("@limit", limit);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<StarsQuestion> tempList = new List<StarsQuestion>();

                        while (reader.Read())
                        {
                            tempList.Add(new StarsQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3]));
                        }
                        return tempList;

                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }


}

