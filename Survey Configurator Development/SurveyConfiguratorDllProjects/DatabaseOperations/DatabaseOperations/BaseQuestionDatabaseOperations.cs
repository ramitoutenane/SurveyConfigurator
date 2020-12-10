using SurveyConfiguratorEntities;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DatabaseOperations
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
        public Reslut Insert(BaseQuestion pQuestion)
        {
            try
            {
                string tCommandString = $"INSERT INTO {DatabaseOperationsConstants.cTABLE_QUESTION} ({DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT}, {DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER}, {DatabaseOperationsConstants.cCOLUMN_TYPE_ID}) OUTPUT INSERTED.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseOperationsConstants.cPARAMETER_QUESTION_TEXT}, {DatabaseOperationsConstants.cPARAMETER_QUESTION_ORDER}, {DatabaseOperationsConstants.cPARAMETER_QUESTION_TYPE})";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_TEXT}", pQuestion.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ORDER}", pQuestion.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_TYPE}", (int)pQuestion.Type);
                        tConnection.Open();
                        int tID = (int)tCommand.ExecuteScalar();
                        if (tID > 0)
                        {
                            pQuestion.ChangeId(tID);
                            return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cINSERT_SUCCESS_MESSAGE);
                        }
                        {
                            ErrorLogger.Log(DatabaseOperationsConstants.cINSERT_FAIL, new StackFrame(true));
                            return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cINSERT_FAIL_MESSAGE);
                        }
                            
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// update question in database question table
        /// </summary>
        /// <param name="pQuestion">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public Reslut Update(BaseQuestion pQuestion)
        {
            try
            {
                string tCommandString = $"UPDATE {DatabaseOperationsConstants.cTABLE_QUESTION} SET {DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_TEXT}, " +
                    $"{DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ORDER} WHERE {DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_TEXT}", pQuestion.Text);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ORDER}", pQuestion.Order);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tConnection.Open();
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cUPDATE_SUCCESS_MESSAGE);
                        else
                        {
                            ErrorLogger.Log(DatabaseOperationsConstants.cUPDATE_FAIL, new StackFrame(true));
                            return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cUPDATE_FAIL_MESSAGE);
                        }
                    }
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Delete question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        public Reslut Delete(int pId)
        {
            try
            {
                string tCommandString = $"DELETE FROM {DatabaseOperationsConstants.cTABLE_QUESTION} WHERE {DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pId);
                        tConnection.Open();
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Reslut(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cDELETE_SUCCESS_MESSAGE);
                        else
                        {                           
                            ErrorLogger.Log(DatabaseOperationsConstants.cDELETE_FAIL, new StackFrame(true));
                            return new Reslut(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cDELETE_FAIL_MESSAGE);
                        }
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cDELETE_ERROR_MESSAGE);
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
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
