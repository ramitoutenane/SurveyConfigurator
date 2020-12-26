using SurveyConfiguratorEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DatabaseOperations
{
    /// <summary>
    /// Class to support database operations on slider question table
    /// </summary>
    public class SliderQuestionDatabaseOperations : IDatabaseOperations<SliderQuestion>
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
        /// SliderQuestionDatabaseOperations constructor to initialize new SliderQuestionDatabaseOperations object
        /// </summary>
        /// <param name="pConnectionString">SQL database connection string</param>
        public SliderQuestionDatabaseOperations(string pConnectionString)
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
        /// SliderQuestionDatabaseOperations constructor to initialize new SliderQuestionDatabaseOperations object
        /// </summary>
        /// <param name="pDatabaseSettings">DatabaseSettings object</param>
        public SliderQuestionDatabaseOperations(DatabaseSettings pDatabaseSettings)
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
        /// <summary>
        /// Insert slider question into database slider question table
        /// </summary>
        /// <param name="pQuestion">question to be inserted</param>
        /// <returns>inserted question id</returns>
        #endregion
        #region Methods

        public Result Insert(SliderQuestion pQuestion)
        {
            Result tInsertResponse = Result.DefaultResult();
            try
            {
                // insert general question into question table and get question id to be used as foreign key
                tInsertResponse = mQuestionDatabaseOperation.Insert(pQuestion);
                // Slider question depend on base question primary key since there is foreign key relationship between tables
                // we insert general question and check if is inserted we insert slider question, otherwise we exit
                if (tInsertResponse.Value != ResultValue.Success)
                    return tInsertResponse;
                string tCommandString = $"INSERT INTO {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION} ({DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}, {DatabaseOperationsConstants.cCOLUMN_START_VALUE}, {DatabaseOperationsConstants.cCOLUMN_END_VALUE}, {DatabaseOperationsConstants.cCOLUMN_START_CAPTION}, {DatabaseOperationsConstants.cCOLUMN_END_CAPTION}) " +
                    $"OUTPUT INSERTED.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} VALUES ({DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}, {DatabaseOperationsConstants.cPARAMETER_QUESTION_START_VALUE}, " +
                    $"{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_VALUE},{DatabaseOperationsConstants.cPARAMETER_QUESTION_START_CAPTION},{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_CAPTION})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_START_VALUE}", pQuestion.StartValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_VALUE}", pQuestion.EndValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_START_CAPTION}", pQuestion.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_CAPTION}", pQuestion.EndValueCaption);
                        tConnection.Open();
                        if ((int)tCommand.ExecuteScalar() > 0)
                            return new Result(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cINSERT_SUCCESS_MESSAGE);
                        else
                        {
                            ErrorLogger.Log(DatabaseOperationsConstants.cINSERT_FAIL, new StackTrace());
                            return new Result(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cINSERT_FAIL_MESSAGE);
                        }
                    }
                }
            }
            catch (Exception pError)
            {
                // if exception raises on specific question data insertion then delete inserted general question from question table
                if (tInsertResponse.Value == ResultValue.Success)
                    mQuestionDatabaseOperation.Delete(pQuestion.Id);
                ErrorLogger.Log(pError);
                return new Result(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cINSERT_ERROR_MESSAGE);
            }
        }
        /// <summary>
        /// update slider question in database slider question table
        /// </summary>
        /// <param name="pQuestion">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public Result Update(SliderQuestion pQuestion)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                Result tUpdateResponse = mQuestionDatabaseOperation.Update(pQuestion);
                if (tUpdateResponse.Value != ResultValue.Success)
                {
                    return tUpdateResponse;
                }
                string tCommandString = $"UPDATE {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION} SET {DatabaseOperationsConstants.cCOLUMN_START_VALUE} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_START_VALUE}," +
                    $" {DatabaseOperationsConstants.cCOLUMN_END_VALUE} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_END_VALUE}, {DatabaseOperationsConstants.cCOLUMN_START_CAPTION} ={DatabaseOperationsConstants.cPARAMETER_QUESTION_START_CAPTION}, " +
                    $"{DatabaseOperationsConstants.cCOLUMN_END_CAPTION} ={DatabaseOperationsConstants.cPARAMETER_QUESTION_END_CAPTION} WHERE {DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_START_VALUE}", pQuestion.StartValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_VALUE}", pQuestion.EndValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_START_CAPTION}", pQuestion.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_END_CAPTION}", pQuestion.EndValueCaption);
                        tConnection.Open();
                        if (tCommand.ExecuteNonQuery() > 0)
                            return new Result(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cUPDATE_SUCCESS_MESSAGE);
                        else
                        {
                            ErrorLogger.Log(DatabaseOperationsConstants.cUPDATE_FAIL, new StackTrace());
                            return new Result(ResultValue.Fail, ResultConstantValues.cFAIL_STATUS_CODE, ResultConstantValues.cUPDATE_FAIL_MESSAGE);
                        }
                    }
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Result(ResultValue.Error, ResultConstantValues.cGENERAL_ERROR_STATUS_CODE, ResultConstantValues.cUPDATE_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete slider question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        // because table has on delete cascade constraint, just delete general question
        public Result Delete(int pId) => mQuestionDatabaseOperation.Delete(pId);
        /// <summary>
        /// Select specific question from the repository
        /// </summary>
        /// <param name="pId">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public SliderQuestion Read(int pId)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT}, {DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER}, {DatabaseOperationsConstants.cCOLUMN_START_VALUE}, {DatabaseOperationsConstants.cCOLUMN_END_VALUE}, {DatabaseOperationsConstants.cCOLUMN_START_CAPTION}, {DatabaseOperationsConstants.cCOLUMN_END_CAPTION}, {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID},{DatabaseOperationsConstants.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseOperationsConstants.cTABLE_QUESTION}, {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION} WHERE {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID} AND {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pId);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            if (tReader.HasRows && tReader.Read())
                            {
                                return new SliderQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3), tReader.GetString(4), tReader.GetString(5), tReader.GetInt32(6));
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
        /// Select all slider questions in given range from database
        /// </summary>
        /// <param name="pOffset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="pLimit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<SliderQuestion> SelectAll(int pOffset = 0, int pLimit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT}, {DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER}, {DatabaseOperationsConstants.cCOLUMN_START_VALUE}, {DatabaseOperationsConstants.cCOLUMN_END_VALUE}, {DatabaseOperationsConstants.cCOLUMN_START_CAPTION}, {DatabaseOperationsConstants.cCOLUMN_END_CAPTION}, {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID},{DatabaseOperationsConstants.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseOperationsConstants.cTABLE_QUESTION}, {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION} WHERE {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cTABLE_SLIDER_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} OFFSET {DatabaseOperationsConstants.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (pLimit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseOperationsConstants.cLIMIT} ROWS ONLY";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cOFFSET}", pOffset);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cLIMIT}", pLimit);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            List<SliderQuestion> tList = new List<SliderQuestion>();
                            // loop over retrieved data, create question object on each loop and add it to list
                            while (tReader.Read())
                            {
                                tList.Add(new SliderQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3), tReader.GetString(4), tReader.GetString(5), tReader.GetInt32(6)));
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
