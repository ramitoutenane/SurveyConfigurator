﻿using SurveyConfiguratorEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

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
        public Result Insert(StarsQuestion pQuestion)
        {
            Result tInsertResponse = Result.DefaultResult();
            try
            {
                // insert general question into question table and get question id to be used as foreign key
                tInsertResponse = mQuestionDatabaseOperation.Insert(pQuestion);
                // Stars question depend on base question primary key since there is foreign key relationship between tables
                // we insert general question and check if is inserted we insert stars question, otherwise we exit
                if (tInsertResponse.Value != ResultValue.Success)
                    return tInsertResponse;
                string tCommandString = $"INSERT INTO {DatabaseOperationsConstants.cTABLE_STARS_QUESTION} ({DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}, {DatabaseOperationsConstants.cCOLUMN_STARS_NUMBER}) OUTPUT INSERTED.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}, {DatabaseOperationsConstants.cPARAMETER_QUESTION_STARS_NUMBER})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_STARS_NUMBER}", pQuestion.NumberOfStars);
                        tConnection.Open();
                        if ((int)tCommand.ExecuteScalar() > 0)
                            return new Result(ResultValue.Success, ResultConstantValues.cSUCCESS_STATUS_CODE, ResultConstantValues.cINSERT_SUCCESS_MESSAGE);
                        else{
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
        /// update stars question in database stars question table
        /// </summary>
        /// <param name="pQuestion">question to be updated</param>
        /// <returns>true if question
        public Result Update(StarsQuestion pQuestion)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                Result tUpdateResponse = mQuestionDatabaseOperation.Update(pQuestion);
                if (tUpdateResponse.Value != ResultValue.Success)
                {
                    return tUpdateResponse;
                }
                string tCommandString = $"UPDATE {DatabaseOperationsConstants.cTABLE_STARS_QUESTION} SET {DatabaseOperationsConstants.cCOLUMN_STARS_NUMBER} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_STARS_NUMBER} WHERE {DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_ID}", pQuestion.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cPARAMETER_QUESTION_STARS_NUMBER}", pQuestion.NumberOfStars);
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
        /// Delete stars question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        // because table has on delete
        public Result Delete(int id) => mQuestionDatabaseOperation.Delete(id);
        /// <summary>
        /// Select specific question from the repository
        /// </summary>
        /// <param name="pId">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public StarsQuestion Read(int pId)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT}, {DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER}, {DatabaseOperationsConstants.cCOLUMN_STARS_NUMBER}, {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}, {DatabaseOperationsConstants.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseOperationsConstants.cTABLE_QUESTION}, {DatabaseOperationsConstants.cTABLE_STARS_QUESTION} WHERE {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cPARAMETER_QUESTION_ID} AND {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cTABLE_STARS_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}";
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
                string tQueryString = $"SELECT {DatabaseOperationsConstants.cCOLUMN_QUESTION_TEXT}, {DatabaseOperationsConstants.cCOLUMN_QUESTION_ORDER}, {DatabaseOperationsConstants.cCOLUMN_STARS_NUMBER}, {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID}, {DatabaseOperationsConstants.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseOperationsConstants.cTABLE_QUESTION}, {DatabaseOperationsConstants.cTABLE_STARS_QUESTION} WHERE {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} = {DatabaseOperationsConstants.cTABLE_STARS_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseOperationsConstants.cTABLE_QUESTION}.{DatabaseOperationsConstants.cCOLUMN_QUESTION_ID} OFFSET {DatabaseOperationsConstants.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (pLimit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseOperationsConstants.cLIMIT} ROWS ONLY";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cOFFSET} ", pOffset);
                        tCommand.Parameters.AddWithValue($"{DatabaseOperationsConstants.cLIMIT}", pLimit);
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

