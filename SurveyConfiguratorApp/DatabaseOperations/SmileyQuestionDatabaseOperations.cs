using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SmileyQuestionDatabaseOperations : ICUDable<SmileyQuestion, SqlConnection>, IQueryable<SmileyQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        public SmileyQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }


        public int Insert(SqlConnection connection, SmileyQuestion data)
        {
            int questionID = qda.Insert(connection, data);
            if (questionID < 1)
                return questionID;
            string commandString = "INSERT INTO smiley_question (question_id, num_of_faces) OUTPUT INSERTED.question_id VALUES (@id, @numOfFaces)";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", questionID);
                    command.Parameters.AddWithValue("@numOfFaces", data.NumberOfFaces);
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
        public bool Update(SqlConnection connection, SmileyQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE smiley_question SET num_of_faces = @numOfFaces WHERE question_id = @id ";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@id", data.ID);
                command.Parameters.AddWithValue("@numOfFaces", data.NumberOfFaces);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return command.ExecuteNonQuery() > 0;
            }

        }
        public bool Delete(SqlConnection connection, int id) => qda.Delete(connection, id);

        public SmileyQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT question_text, question_order, num_of_faces, question.question_id, type_id " +
                "FROM question, smiley_question WHERE question.question_id = @id AND question.question_id = smiley_question.question_id";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("id", id);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        return new SmileyQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<SmileyQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "SELECT question_text, question_order, num_of_faces, question.question_id, type_id " +
                "FROM question, smiley_question WHERE question.question_id = smiley_question.question_id " +
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
                    List<SmileyQuestion> tempList = new List<SmileyQuestion>();

                    while (reader.Read())
                    {
                        tempList.Add(new SmileyQuestion(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3)));
                    }
                    return tempList;

                }
            }
        }

    }
}