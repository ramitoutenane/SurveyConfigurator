using SurveyConfiguratorApp.DatabaseOperations;
using System;
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
            try
            {
                string tCommandString = $"INSERT INTO {SQLStringResources.cTABLE_QUESTION} ({SQLStringResources.cCOLUMN_QUESTION_TEXT}, {SQLStringResources.cCOLUMN_QUESTION_ORDER}, {SQLStringResources.cCOLUMN_TYPE_ID}) OUTPUT INSERTED.{SQLStringResources.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({SQLStringResources.cPARAMETER_QUESTION_TEXT}, {SQLStringResources.cPARAMETER_QUESTION_ORDER}, {SQLStringResources.cPARAMETER_QUESTION_TYPE})";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_TYPE}", (int)data.Type);
                        tConnection.Open();
                        return (int)tCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return -1;
            }
        }
        /// <summary>
        /// update question in database question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(Question data)
        {
            try
            {
                string tCommandString = $"UPDATE {SQLStringResources.cTABLE_QUESTION} SET {SQLStringResources.cCOLUMN_QUESTION_TEXT} = {SQLStringResources.cPARAMETER_QUESTION_TEXT}, " +
                    $"{SQLStringResources.cCOLUMN_QUESTION_ORDER} = {SQLStringResources.cPARAMETER_QUESTION_ORDER} WHERE {SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ID}", data.Id);
                        tConnection.Open();
                        return tCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
        /// <summary>
        /// Delete question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        public bool Delete(int id)
        {
            try
            {
                string tCommandString = $"DELETE FROM {SQLStringResources.cTABLE_QUESTION} WHERE {SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ID}", id);
                        tConnection.Open();
                        return tCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
    }
}
