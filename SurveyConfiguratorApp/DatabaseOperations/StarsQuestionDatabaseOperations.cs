﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class StarsQuestionDatabaseOperations : ICUDable<StarsQuestion, SqlConnection>, IQueryable<StarsQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        public StarsQuestionDatabaseOperations()
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
                    if (connection.State != ConnectionState.Open)
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
        public bool Update(SqlConnection connection, StarsQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE star_question SET num_of_stars = @NumberOfStars WHERE question_id = @id ";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@id", data.Id);
                command.Parameters.AddWithValue("@NumberOfStars", data.NumberOfStars);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return command.ExecuteNonQuery() > 0;
            }

        }
        public bool Delete(SqlConnection connection, int id) => qda.Delete(connection, id);

        public StarsQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT question_text, question_order, num_of_stars, question.question_id, type_id " +
                "FROM question, star_question WHERE question.question_id = @id AND question.question_id = star_question.question_id";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("id", id);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        return new StarsQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public List<StarsQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "SELECT question_text, question_order, num_of_stars, question.question_id, type_id " +
                "FROM question, star_question WHERE question.question_id = star_question.question_id " +
                "ORDER BY question.question_id OFFSET @offset ROWS";
            if (limit > 0)
                queryString += " FETCH NEXT @limit ROWS ONLY";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@offset", offsit);
                command.Parameters.AddWithValue("@limit", limit);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<StarsQuestion> tempList = new List<StarsQuestion>();

                    while (reader.Read())
                    {
                        tempList.Add(new StarsQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3)));
                    }
                    return tempList;

                }
            }

        }
    }


}
