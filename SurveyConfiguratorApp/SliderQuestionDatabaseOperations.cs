﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SliderQuestionDatabaseOperations : ICUDable<SliderQuestion, SqlConnection>, IQueryable<SliderQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        SliderQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }

        public int Insert(SqlConnection connection, SliderQuestion data)
        {
            int questionID = qda.Insert(connection, data);
            if (questionID < 1)
                return questionID;
            string commandString = "INSERT INTO slider_question (question_id, start_value, end_value, start_value_caption, end_value_caption) OUTPUT INSERTED.question_id VALUES (@id, @startValue, @endValue, @startCaption, @endCaption)";
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
            catch (Exception)
            {
                qda.Delete(connection, questionID);
                return -1;
            }
        }
        public bool Update(SqlConnection connection, SliderQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE slider_question SET start_value = @startValue, end_value = @endValue, start_value_caption = @startCaption, end_value_caption = @endCaption WHERE question_id = @id ";
            try
            {
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
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(SqlConnection connection, int id) => qda.Delete(connection, id);
        public SliderQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT  question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id" +
                "FROM question, slider_question WHERE question.question_id = @id AND question.question_id = slider_question.question_id";

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
                            return new SliderQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3], reader[4].ToString(), reader[5].ToString(), (int)reader[6]);
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

        public List<SliderQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "SELECT question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id" +
               "FROM question, slider_question WHERE question.question_id = slider_question.question_id" +
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
                        List<SliderQuestion> tempList = new List<SliderQuestion>();

                        while (reader.Read())
                        {
                            tempList.Add(new SliderQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3], reader[4].ToString(), reader[5].ToString(), (int)reader[6]));
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