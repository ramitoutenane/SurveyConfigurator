using SurveyConfiguratorApp.DatabaseOperations;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on question table
    /// </summary>
    public class QuestionDatabaseOperations : ICUDable<Question>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public QuestionDatabaseOperations(string connectionString)
        {
            mConnectionString = connectionString;
        }
        /// <summary>
        /// Insert question into database question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Insert(Question data)
        {
            string tCommandString = $"INSERT INTO question (question_text, question_order, type_id) OUTPUT INSERTED.question_id " +
                $"VALUES ({SQLStringResources.QuestionText}, {SQLStringResources.QuestionOrder}, {SQLStringResources.QuestionType})";
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionText}", data.Text);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionOrder}", data.Order);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionType}", (int)data.Type);
                    return (int)tCommand.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// update question in database question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(Question data)
        {
            string tCommandString = $"UPDATE question SET question_text = {SQLStringResources.QuestionText}, " +
                $"question_order = {SQLStringResources.QuestionOrder} WHERE question_id = {SQLStringResources.QuestionId} ";
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionText}", data.Text);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionOrder}", data.Order);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", data.Id);
                    return tCommand.ExecuteNonQuery() > 0;
                }
            }
        }
        /// <summary>
        /// Delete question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        public bool Delete(int id)
        {
            string tCommandString = $"DELETE FROM question WHERE question_id = {SQLStringResources.QuestionId}";
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", id);
                    return tCommand.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
