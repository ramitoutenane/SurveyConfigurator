using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SliderQuestionDatabaseOperations : ICUDable<SliderQuestion, SqlConnection>, IQueryable<SliderQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        public SliderQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }

        public int Insert(SqlConnection connection, SliderQuestion data)
        {
            int questionID = qda.Insert(connection, data);
            if (questionID < 1)
                return questionID;
            string commandString = "INSERT INTO slider_question (question_id, start_value, end_value, start_value_caption, end_value_caption) " +
                "OUTPUT INSERTED.question_id VALUES (@id, @startValue, @endValue, @startCaption, @endCaption)";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", questionID);
                    command.Parameters.AddWithValue("@startValue", data.StartValue);
                    command.Parameters.AddWithValue("@endValue", data.EndValue);
                    command.Parameters.AddWithValue("@startCaption", data.StartValueCaption);
                    command.Parameters.AddWithValue("@endCaption", data.EndValueCaption);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                qda.Delete(connection, questionID);
                throw ex;
            }
        }
        public bool Update(SqlConnection connection, SliderQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE slider_question SET start_value = @startValue, end_value = @endValue, start_value_caption = @startCaption, end_value_caption = @endCaption WHERE question_id = @id ";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@id", data.ID);
                command.Parameters.AddWithValue("@startValue", data.StartValue);
                command.Parameters.AddWithValue("@endValue", data.EndValue);
                command.Parameters.AddWithValue("@startCaption", data.StartValueCaption);
                command.Parameters.AddWithValue("@endCaption", data.EndValueCaption);
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }

        }
        public bool Delete(SqlConnection connection, int id) => qda.Delete(connection, id);
        public SliderQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id " +
                "FROM question, slider_question WHERE question.question_id = @id AND question.question_id = slider_question.question_id";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        return new SliderQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6));
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public List<SliderQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "SELECT question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id " +
               "FROM question, slider_question WHERE question.question_id = slider_question.question_id " +
                "ORDER BY question.question_id OFFSET @offset ROWS";
            if (limit > 0)
                queryString += " FETCH NEXT @limit ROWS ONLY";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@offset", offsit);
                command.Parameters.AddWithValue("@limit", limit);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<SliderQuestion> tempList = new List<SliderQuestion>();
                    while (reader.Read())
                    {

                        tempList.Add(new SliderQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),reader.GetString(4), reader.GetString(5), reader.GetInt32(6)));
                    }
                    return tempList;

                }
            }
        }

    }
}
