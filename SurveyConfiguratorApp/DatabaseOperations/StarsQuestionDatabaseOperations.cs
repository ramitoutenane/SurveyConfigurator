using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on stars question table
    /// </summary>
    class StarsQuestionDatabaseOperations : ICRUDable<StarsQuestion>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations object to access base question insert , update and delete operations
        /// </summary>
        private QuestionDatabaseOperations mQuestionDatabaseOperation;
        /// <summary>
        /// StarsQuestionDatabaseOperations constructor to initialize new StarsQuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public StarsQuestionDatabaseOperations(string connectionString)
        {
            try
            {
                mConnectionString = connectionString;
                mQuestionDatabaseOperation = new QuestionDatabaseOperations(connectionString);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// StarsQuestionDatabaseOperations constructor to initialize new StarsQuestionDatabaseOperations object
        /// </summary>
        /// <param name="databaseSettings">DatabaseSettings object</param>
        public StarsQuestionDatabaseOperations(DatabaseSettings databaseSettings)
        {
            try
            {
                // create connection string from databaseSettings object data
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = databaseSettings.DataSource,
                    InitialCatalog = databaseSettings.InitialCatalog,
                    UserID = databaseSettings.UserID,
                    Password = databaseSettings.Password
                };
                mConnectionString = tBuilder.ConnectionString;
                mQuestionDatabaseOperation = new QuestionDatabaseOperations(mConnectionString);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Insert stars question into database stars question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Create(StarsQuestion data)
        {
            int tQuestionId = -1;
            try
            {
                // insert general question into question table and get question id to be used as foreign key
                tQuestionId = mQuestionDatabaseOperation.Create(data);
                // question id is auto increment key that starts from 1, if question is inserted successfully the returned id is larger than 1
                // if id is less than 1 exit insert method to avoid foreign key reference error
                if (tQuestionId < 1)
                    return tQuestionId;
                string tCommandString = $"INSERT INTO {DatabaseStringValues.cTABLE_STARS_QUESTION} ({DatabaseStringValues.cCOLUMN_QUESTION_ID}, {DatabaseStringValues.cCOLUMN_STARS_NUMBER}) OUTPUT INSERTED.{DatabaseStringValues.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseStringValues.cPARAMETER_QUESTION_ID}, {DatabaseStringValues.cPARAMETER_QUESTION_STARS_NUMBER})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_ID}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_STARS_NUMBER}", data.NumberOfStars);
                        tConnection.Open();
                        return (int)tCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception error)
            {
                // if exception raises on specific question data insertion then delete inserted general question from question table
                if (tQuestionId != -1)
                    mQuestionDatabaseOperation.Delete(tQuestionId);
                ErrorLogger.Log(error);
                return -1;
            }
        }
        /// <summary>
        /// update stars question in database stars question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question
        public bool Update(StarsQuestion data)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                if (!mQuestionDatabaseOperation.Update(data))
                {
                    return false;
                }
                string tCommandString = $"UPDATE {DatabaseStringValues.cTABLE_STARS_QUESTION} SET {DatabaseStringValues.cCOLUMN_STARS_NUMBER} = {DatabaseStringValues.cPARAMETER_QUESTION_STARS_NUMBER} WHERE {DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_ID}", data.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_STARS_NUMBER}", data.NumberOfStars);
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
        /// Delete stars question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        // because table has on delete
        public bool Delete(int id) => mQuestionDatabaseOperation.Delete(id);
        /// <summary>
        /// Select specific question from the repository
        /// </summary>
        /// <param name="id">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public StarsQuestion Read(int id)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseStringValues.cCOLUMN_QUESTION_TEXT}, {DatabaseStringValues.cCOLUMN_QUESTION_ORDER}, {DatabaseStringValues.cCOLUMN_STARS_NUMBER}, {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID}, {DatabaseStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseStringValues.cTABLE_QUESTION}, {DatabaseStringValues.cTABLE_STARS_QUESTION} WHERE {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cPARAMETER_QUESTION_ID} AND {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cTABLE_STARS_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_ID}", id);
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
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
        /// <summary>
        /// Select all stars questions in given range from database
        /// </summary>
        /// <param name="offset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="limit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<StarsQuestion> ReadAll(int offset = 0, int limit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseStringValues.cCOLUMN_QUESTION_TEXT}, {DatabaseStringValues.cCOLUMN_QUESTION_ORDER}, {DatabaseStringValues.cCOLUMN_STARS_NUMBER}, {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID}, {DatabaseStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseStringValues.cTABLE_QUESTION}, {DatabaseStringValues.cTABLE_STARS_QUESTION} WHERE {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cTABLE_STARS_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} OFFSET {DatabaseStringValues.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (limit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseStringValues.cLIMIT} ROWS ONLY";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cOFFSET} ", offset);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cLIMIT}", limit);
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
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
    }
}

