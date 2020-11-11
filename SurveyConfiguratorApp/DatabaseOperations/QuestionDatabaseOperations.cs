using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    public class QuestionDatabaseOperations : ICUDable<Question, SqlConnection>
    {


        public int Insert(SqlConnection connection, Question data)
        {
            string commandString = "INSERT INTO question (question_text, question_order, type_id) OUTPUT INSERTED.question_id VALUES (@text, @order, @type)";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@text", data.Text);
                command.Parameters.AddWithValue("@order", data.Order);
                command.Parameters.AddWithValue("@type", (int)data.type);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return (int)command.ExecuteScalar();
            }

        }

        public bool Update(SqlConnection connection, Question data)
        {
            string commandString = "UPDATE question SET question_text = @text , question_order = @order WHERE question_id = @id ";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@text", data.Text);
                command.Parameters.AddWithValue("@order", data.Order);
                command.Parameters.AddWithValue("@id", data.ID);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return command.ExecuteNonQuery() > 0;
            }

        }
        public bool Delete(SqlConnection connection, int id)
        {
            string commandString = "DELETE FROM question WHERE question_id = @id";

            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

    }

}
