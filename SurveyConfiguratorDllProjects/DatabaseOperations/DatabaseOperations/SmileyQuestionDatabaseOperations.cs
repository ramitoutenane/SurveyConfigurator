using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on smiley question table
    /// </summary>
    class SmileyQuestionDatabaseOperations : IDatabaseOperations<SmileyQuestion>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations object to access base question insert , update and delete operations
        /// </summary>
        private QuestionDatabaseOperations mQuestionDatabaseOperation;
        /// <summary>
        /// SmileyQuestionDatabaseOperations constructor to initialize new SmileyQuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public SmileyQuestionDatabaseOperations(string connectionString)
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
        /// SmileyQuestionDatabaseOperations constructor to initialize new SmileyQuestionDatabaseOperations object
        /// </summary>
        /// <param name="databaseSettings">DatabaseSettings object</param>
        public SmileyQuestionDatabaseOperations(DatabaseSettings databaseSettings)
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
                mQuestionDatabaseOperation = new QuestionDatabaseOperations(mConnectionString);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Insert smiley question into database smiley question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Insert(SmileyQuestion data)
        {
            int tQuestionId = -1;
            try
            {
                // insert general question into question table and get question id to be used as foreign key
                tQuestionId = mQuestionDatabaseOperation.Insert(data);
                // question id is auto increment key that starts from 1, if question is inserted successfully the returned id is larger than 1
                // if id is less than 1 exit insert method to avoid foreign key reference error
                if (tQuestionId < 1)
                    return tQuestionId;
                string tCommandString = $"INSERT INTO {DatabaseParameters.cTABLE_SMILEY_QUESTION} ({DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_FACES_NUMBER}) OUTPUT INSERTED.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({DatabaseParameters.cPARAMETER_QUESTION_ID}, {DatabaseParameters.cPARAMETER_QUESTION_FACES_NUMBER})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_FACES_NUMBER}", data.NumberOfFaces);
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
        /// update smiley question in database smiley question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(SmileyQuestion data)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                if (!mQuestionDatabaseOperation.Update(data))
                {
                    return false;
                }
                string tCommandString = $"UPDATE {DatabaseParameters.cTABLE_SMILEY_QUESTION} SET {DatabaseParameters.cCOLUMN_FACES_NUMBER} = {DatabaseParameters.cPARAMETER_QUESTION_FACES_NUMBER} WHERE {DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} ";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", data.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_FACES_NUMBER}", data.NumberOfFaces);
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
        /// Delete smiley question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        // because table has on delete cascade constraint, just delete general question
        public bool Delete(int id) => mQuestionDatabaseOperation.Delete(id);
        /// <summary>
        /// Select specific question from the repository
        /// </summary>
        /// <param name="id">Id of question to be selected</param>
        /// <returns>The selected question if exist, null otherwise</returns>
        public SmileyQuestion Select(int id)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_FACES_NUMBER}, {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseParameters.cTABLE_QUESTION}, {DatabaseParameters.cTABLE_SMILEY_QUESTION} WHERE {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cPARAMETER_QUESTION_ID} AND {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cTABLE_SMILEY_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cPARAMETER_QUESTION_ID}", id);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            if (tReader.HasRows && tReader.Read())
                            {
                                return new SmileyQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3));
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
        /// Select all smiley questions in given range from database
        /// </summary>
        /// <param name="offset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="limit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<SmileyQuestion> SelectAll(int offset = 0, int limit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseParameters.cCOLUMN_QUESTION_TEXT}, {DatabaseParameters.cCOLUMN_QUESTION_ORDER}, {DatabaseParameters.cCOLUMN_FACES_NUMBER}, {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID}, {DatabaseParameters.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseParameters.cTABLE_QUESTION}, {DatabaseParameters.cTABLE_SMILEY_QUESTION} WHERE {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} = {DatabaseParameters.cTABLE_SMILEY_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseParameters.cTABLE_QUESTION}.{DatabaseParameters.cCOLUMN_QUESTION_ID} OFFSET {DatabaseParameters.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (limit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseParameters.cLIMIT} ROWS ONLY";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cOFFSET}", offset);
                        tCommand.Parameters.AddWithValue($"{DatabaseParameters.cLIMIT}", limit);
                        tConnection.Open();
                        using (SqlDataReader tReader = tCommand.ExecuteReader())
                        {
                            List<SmileyQuestion> tList = new List<SmileyQuestion>();
                            // loop over retrieved data, create question object on each loop and add it to list
                            while (tReader.Read())
                            {
                                tList.Add(new SmileyQuestion(tReader.GetString(0), tReader.GetInt32(1), tReader.GetInt32(2), tReader.GetInt32(3)));
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