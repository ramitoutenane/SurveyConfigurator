using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on question table
    /// </summary>
    public class QuestionDatabaseOperations : IDatabaseProcessable<Question>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public QuestionDatabaseOperations(string connectionString)
        {
            try
            {
                mConnectionString = connectionString;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="databaseSettings">DatabaseSettings object</param>
        public QuestionDatabaseOperations(DatabaseSettings databaseSettings)
        {
            try
            {
                // create connection string from databaseSettings object data
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = databaseSettings.DatabaseServer,
                    InitialCatalog = databaseSettings.DatabaseName,
                    UserID = databaseSettings.DatabaseUser,
                    Password = databaseSettings.DatabasePassword
                };
                mConnectionString = tBuilder.ConnectionString;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
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
                string tCommandString = $"INSERT INTO {DatabaseParameters.cTABLE_QUESTION} ({DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_TYPE_ID}) OUTPUT INSERTED.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseParameters.cPARAMETER_QUESTION_TEXT}, {DatabaseParameters.cPARAMETER_QUESTION_ORDER}, {DatabaseParameters.cPARAMETER_QUESTION_TYPE})";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TYPE}", (int)data.Type);
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
                string tCommandString = $"UPDATE {DatabaseParameters.cTABLE_QUESTION} SET {DatabaseParameters.cCOLUMN_QUESTION_TEXT} = {DatabaseParameters.cPARAMETER_QUESTION_TEXT}, " +
                    $"{DatabaseParameters.cCOLUMN_QUESTION_ORDER} = {DatabaseParameters.cPARAMETER_QUESTION_ORDER} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", data.Id);
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
                string tCommandString = $"DELETE FROM {DatabaseParameters.cTABLE_QUESTION} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", id);
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
