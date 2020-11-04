using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SmileyQuestionDatabaseOperations : ICUDable<SmileyQuestion, SqlConnection>, IQueryable<SmileyQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        SmileyQuestionDatabaseOperations()
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
        public bool Update(SqlConnection connection, SmileyQuestion data)
        {
            if (!qda.Update(connection, data))
            {
                return false;
            }
            string commandString = "UPDATE smiley_question SET num_of_faces = @numOfFaces WHERE question_id = @id ";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", data.ID);
                    command.Parameters.AddWithValue("@numOfFaces", data.NumberOfFaces);
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

        public SmileyQuestion Select(SqlConnection connection, int id)
        {
            string queryString = "SELECT question_text, question_order, num_of_faces, question.question_id, type_id " +
                "FROM question, smiley_question WHERE question.question_id = @id AND question.question_id = smiley_question.question_id";

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
                            return new SmileyQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3]);
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

        public List<SmileyQuestion> SelectAll(SqlConnection connection, int offsit = 0, int limit = 0)
        {
            string queryString = "question_text, question_order, num_of_faces, question.question_id, type_id " +
                "FROM question, smiley_question WHERE question.question_id = smiley_question.question_id" +
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
                        List<SmileyQuestion> tempList = new List<SmileyQuestion>();

                        while (reader.Read())
                        {
                            tempList.Add(new SmileyQuestion(reader[0].ToString(), (int)reader[1], (int)reader[2], (int)reader[3]));
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