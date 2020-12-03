using SurveyConfiguratorEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseOperations
{
    /// <summary>
    /// Class to support database operations on stars question table
    /// </summary>
    public class StarsQuestionDatabaseOperations : IDatabaseOperations<StarsQuestion>
    {
        #region Variable deceleration
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations object to access base question insert , update and delete operations
        /// </summary>
        private BaseQuestionDatabaseOperations mQuestionDatabaseOperation;
        #endregion
        #region Constructors
        /// <summary>
        /// StarsQuestionDatabaseOperations constructor to initialize new StarsQuestionDatabaseOperations object
        /// </summary>
        /// <param name="pConnectionString">SQL database connection string</param>
        public StarsQuestionDatabaseOperations(string pConnectionString)
        {
            try
            {
                mConnectionString = pConnectionString;
                mQuestionDatabaseOperation = new BaseQuestionDatabaseOperations(pConnectionString);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        /// <summary>
        /// StarsQuestionDatabaseOperations constructor to initialize new StarsQuestionDatabaseOperations object
        /// </summary>
        /// <param name="pDatabaseSettings">DatabaseSettings object</param>
        public StarsQuestionDatabaseOperations(DatabaseSettings pDatabaseSettings)
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
                mQuestionDatabaseOperation = new BaseQuestionDatabaseOperations(mConnectionString);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Insert stars question into database stars question table
        /// </summary>
        /// <param name="pQuestion">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public Response Insert(StarsQuestion pQuestion)
        {
            Response tInsertResponse = Response.DefaultResponse();
            try
            {
                // insert general question into question table and get question id to be used as foreign key
                tInsertResponse = mQuestionDatabaseOperation.Insert(pQuestion);
                // Stars question depend on base question primary key since there is foreign key relationship between tables
                // we insert general question and check if is inserted we insert stars question, otherwise we exit
                if (tInsertResponse.Status != ResponseStatus.Success)
                    return tInsertResponse;
                string tCommandString = $"INSERT INTO {DatabaseParameters.cTABLE_STARS_QUESTION} ({DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_STARS_NUMBER}) OUTPUT INSERTED.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseParameters.cPARAMETER_QUESTION_ID}, {DatabaseParameters.cPARAMETER_QUESTION_STARS_NUMBER})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_STARS_NUMBER}", pQuestion.NumberOfStars);
                        tConnection.Open();
                        if ((int)tCommand.ExecuteScalar() > 0)
                            return new Response(ResponseStatus.Success, ResponseConstantValues.cSUCCESS_STATUS_CODE, ResponseConstantValues.cINSERT_SUCCESS_MESSAGE);
                        else
                            return new Response(ResponseStatus.Fail, ResponseConstantValues.cFAIL_STATUS_CODE, ResponseConstantValues.cINSERT_FAIL_MESSAGE);
                    }
                }
            }
            catch (Exception pError)
            {
                // if exception raises on specific question data insertion then delete inserted general question from question table
                if (tInsertResponse.Status == ResponseStatus.Success)
                    mQuestionDatabaseOperation.Delete(pQuestion.Id);
                ErrorLogger.Log(pError);
                return new Response(ResponseStatus.Error, ResponseConstantValues.cGENERAL_ERROR_STATUS_CODE, ResponseConstantValues.cINSERT_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// update stars question in database stars question table
        /// </summary>
        /// <param name="pQuestion">question to be updated</param>
        /// <returns>true if question
        public Response Update(StarsQuestion pQuestion)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                Response tUpdateResponse = mQuestionDatabaseOperation.Update(pQuestion);
                if (tUpdateResponse.Status != ResponseStatus.Success)
                {
                    return tUpdateResponse;
                }
                string tCommandString = $"UPDATE {DatabaseParameters.cTABLE_STARS_QUESTION} SET {DatabaseParameters.cCOLUMN_STARS_NUMBER} = {DatabaseParameters.cPARAMETER_QUESTION_STARS_NUMBER} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_STARS_NUMBER}", pQuestion.NumberOfStars);
                        tConnection.Open();
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Response(ResponseStatus.Success, ResponseConstantValues.cSUCCESS_STATUS_CODE, ResponseConstantValues.cUPDATE_SUCCESS_MESSAGE);
                        else
                            return new Response(ResponseStatus.Fail, ResponseConstantValues.cFAIL_STATUS_CODE, ResponseConstantValues.cUPDATE_FAIL_MESSAGE);
                    }
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Response(ResponseStatus.Error, ResponseConstantValues.cGENERAL_ERROR_STATUS_CODE, ResponseConstantValues.cUPDATE_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// Delete stars question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        // because table has on delete
        public Response Delete(int id) => mQuestionDatabaseOperation.Delete(id);
        /// <summary>
        /// Select specific question from the repository
        /// </summary>
        /// <param name="pId">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public StarsQuestion Select(int pId)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_STARS_NUMBER}, {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseParameters.cTABLE_QUESTION}, {DatabaseParameters.cTABLE_STARS_QUESTION} WHERE {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} AND {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cTABLE_STARS_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", pId);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            if (tReader.HasRows && tReader.Read())
                            {
                                return new StarsQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3));
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
            }
        }
        /// <summary>
        /// Select all stars questions in given range from database
        /// </summary>
        /// <param name="pOffset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="pLimit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<StarsQuestion> SelectAll(int pOffset = 0, int pLimit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_STARS_NUMBER}, {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseParameters.cTABLE_QUESTION}, {DatabaseParameters.cTABLE_STARS_QUESTION} WHERE {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cTABLE_STARS_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} OFFSET {DatabaseParameters.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (pLimit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseParameters.cLIMIT} ROWS ONLY";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cOFFSET} ", pOffset);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cLIMIT}", pLimit);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            List<StarsQuestion> tList = new List<StarsQuestion>();
                            // loop over retrieved data, create question object on each loop and add it to list
                            while (tReader.Read())
                            {
                                tList.Add(new StarsQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3)));
                            }
                            return tList;
                        }
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
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
                return mQuestionDatabaseOperation.IsConnected();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }


        }
        #endregion
    }
}

