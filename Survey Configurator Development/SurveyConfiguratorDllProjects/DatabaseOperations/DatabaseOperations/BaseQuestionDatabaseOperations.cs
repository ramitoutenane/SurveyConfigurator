using SurveyConfiguratorEntities;
using System;
using System.Data.SqlClient;
using System.Threading;
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
        private bool mConnectionStatus;
        private Thread mAutoRefreshThread;
        public delegate void AutoRefreshDelegate();
        public event AutoRefreshDelegate AutoRefreshEventHandler;

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
                mConnectionStatus = IsConnectionAvailable();
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
                mConnectionStatus = IsConnectionAvailable();

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
                            return new Reslut(ResultValue.Success, ResponseConstantValues.cSUCCESS_STATUS_CODE, ResponseConstantValues.cINSERT_SUCCESS_MESSAGE);
                        }
                        return new Reslut(ResultValue.Fail, ResponseConstantValues.cFAIL_STATUS_CODE, ResponseConstantValues.cINSERT_FAIL_MESSAGE);
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResponseConstantValues.cGENERAL_ERROR_STATUS_CODE, ResponseConstantValues.cINSERT_ERROR_MESSAGE);
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
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Reslut(ResultValue.Success, ResponseConstantValues.cSUCCESS_STATUS_CODE, ResponseConstantValues.cUPDATE_SUCCESS_MESSAGE);
                        else
                            return new Reslut(ResultValue.Fail, ResponseConstantValues.cFAIL_STATUS_CODE, ResponseConstantValues.cUPDATE_FAIL_MESSAGE);
                    }
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResponseConstantValues.cGENERAL_ERROR_STATUS_CODE, ResponseConstantValues.cUPDATE_ERROR_MESSAGE);
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
                string tCommandString = $"DELETE FROM {DatabaseParameters.cTABLE_QUESTION} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pId);
                        tConnection.Open();
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Reslut(ResultValue.Success, ResponseConstantValues.cSUCCESS_STATUS_CODE, ResponseConstantValues.cDELETE_SUCCESS_MESSAGE);
                        else
                            return new Reslut(ResultValue.Fail, ResponseConstantValues.cFAIL_STATUS_CODE, ResponseConstantValues.cDELETE_FAIL_MESSAGE);
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Reslut(ResultValue.Error, ResponseConstantValues.cGENERAL_ERROR_STATUS_CODE, ResponseConstantValues.cDELETE_ERROR_MESSAGE);
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
                Task.Run(() => mConnectionStatus = IsConnectionAvailable());
                return mConnectionStatus;
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
        public bool IsConnectionAvailable()
        {

            try
            {
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    // if data base is connected, no SqlException will be raised and true is returned
                    tConnection.Open();
                    tConnection.Close();
                }
                return true;
            }
            catch (SqlException)
            {
                // if database is disconnected, SqlException will be raised and false is returned
                return false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        /// <summary>
        /// Start auto refresh from database thread
        /// </summary>
        /// <param name="pRefreshInterval">Time to refresh in millisecond</param>
        public void StartAutoRefresh(int pRefreshInterval)
        {
            try
            {

                //if the thread is alive then kill it to start new one
                if (mAutoRefreshThread == null || !mAutoRefreshThread.IsAlive)
                {
                    //run new thread to call auto refresh delegate method
                    mAutoRefreshThread = new Thread(() => {
                        while (mAutoRefreshThread.IsAlive)
                        {
                            if (IsConnected() && AutoRefreshEventHandler != null)
                                //fire auto refresh event
                                AutoRefreshEventHandler();
                            Thread.Sleep(pRefreshInterval);
                        }
                    });
                    mAutoRefreshThread.IsBackground = true;
                    mAutoRefreshThread.Start();
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
    }
}
