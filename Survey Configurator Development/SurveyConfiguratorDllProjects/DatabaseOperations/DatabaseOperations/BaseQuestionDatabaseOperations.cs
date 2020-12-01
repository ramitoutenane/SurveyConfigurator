using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on question table
    /// </summary>
    public class BaseQuestionDatabaseOperations : IDatabaseProcessable<BaseQuestion>
    {
        #region Variable deceleration
        private readonly string mConnectionString;
        #endregion
        #region Constructors
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="pConnectionString">SQL database connection string</param>
        public BaseQuestionDatabaseOperations(string pConnectionString)
        {
            try
            {
                mConnectionString = pConnectionString;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="pDatabaseSettings">DatabaseSettings object</param>
        public BaseQuestionDatabaseOperations(DatabaseSettings pDatabaseSettings)
        {
            try
            {
                // create connection string from databaseSettings object data
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = pDatabaseSettings.DatabaseServer,
                    InitialCatalog = pDatabaseSettings.DatabaseName,
                    UserID = pDatabaseSettings.DatabaseUser,
                    Password = pDatabaseSettings.DatabasePassword
                };
                mConnectionString = tBuilder.ConnectionString;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Insert question into database question table
        /// </summary>
        /// <param name="pQuestion">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public bool Insert(BaseQuestion pQuestion)
        {
            try
            {
                string tCommandString = $"INSERT INTO {DatabaseParameters.cTABLE_QUESTION} ({DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_TYPE_ID}) OUTPUT INSERTED.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseParameters.cPARAMETER_QUESTION_TEXT}, {DatabaseParameters.cPARAMETER_QUESTION_ORDER}, {DatabaseParameters.cPARAMETER_QUESTION_TYPE})";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TEXT}", pQuestion.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ORDER}", pQuestion.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TYPE}", (int)pQuestion.Type);
                        tConnection.Open();
                        int tID = (int)tCommand.ExecuteScalar();
                        if (tID > 0)
                        {
                            pQuestion.ChangeId(tID);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// update question in database question table
        /// </summary>
        /// <param name="pQuestion">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(BaseQuestion pQuestion)
        {
            try
            {
                string tCommandString = $"UPDATE {DatabaseParameters.cTABLE_QUESTION} SET {DatabaseParameters.cCOLUMN_QUESTION_TEXT} = {DatabaseParameters.cPARAMETER_QUESTION_TEXT}, " +
                    $"{DatabaseParameters.cCOLUMN_QUESTION_ORDER} = {DatabaseParameters.cPARAMETER_QUESTION_ORDER} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_TEXT}", pQuestion.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ORDER}", pQuestion.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tConnection.Open();
                        return tCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Delete question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        public bool Delete(int pId)
        {
            try
            {
                string tCommandString = $"DELETE FROM {DatabaseParameters.cTABLE_QUESTION} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pId);
                        tConnection.Open();
                        return tCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// check if database connection is available
        /// </summary>
        /// <returns>true if connected, false otherwise</returns>
        public bool IsConnected()
        {

            try
            {
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    // if data base is connected, no SqlException will be raised and true is returned
                    tConnection.Open();
                    tConnection.Close();
                }
            }
            catch (SqlException)
            {
                // if database is disconnected, SqlException will be raised and false is returned
                return false;
            }
            return true;
        }

        #endregion
    }
}
