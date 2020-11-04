using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    public class QuestionDatabaseOperations : ICUDable<Question, SqlConnection>
    {


        public int Insert(SqlConnection connection, Question data)
        {
            string commandString = "INSERT INTO question (question_text, question_order, type_id) OUTPUT INSERTED.question_id VALUES (@text, @order, @type)";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@text", data.Text);
                    command.Parameters.AddWithValue("@order", data.Order);
                    command.Parameters.AddWithValue("@type", (int)data.type);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return -1;
            }

        }

        public bool Update(SqlConnection connection, Question data)
        {
            string commandString = "UPDATE question SET question_text = @text , question_order = @order WHERE question_id = @id ";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@text", data.Text);
                    command.Parameters.AddWithValue("@order", data.Order);
                    command.Parameters.AddWithValue("@id", data.ID);
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(SqlConnection connection, int id)
        {
            string commandString = "DELETE FROM question WHERE question_id = @id";
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
