using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on slider question table
    /// </summary>
    class SliderQuestionDatabaseOperations : ICRUDable<SliderQuestion>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations object to access base question insert , update and delete operations
        /// </summary>
        private QuestionDatabaseOperations mQuestionDatabaseOperation;
        /// <summary>
        /// SliderQuestionDatabaseOperations constructor to initialize new SliderQuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public SliderQuestionDatabaseOperations(string connectionString)
        {
            try
            {
                mConnectionString = connectionString;
                mQuestionDatabaseOperation = new QuestionDatabaseOperations(connectionString);
            }
            catch(Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// SliderQuestionDatabaseOperations constructor to initialize new SliderQuestionDatabaseOperations object
        /// </summary>
        /// <param name="databaseSettings">DatabaseSettings object</param>
        public SliderQuestionDatabaseOperations(DatabaseSettings databaseSettings)
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
        /// Insert slider question into database slider question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Create(SliderQuestion data)
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
                string tCommandString = $"INSERT INTO {DatabaseStringValues.cTABLE_SLIDER_QUESTION} ({DatabaseStringValues.cCOLUMN_QUESTION_ID}, {DatabaseStringValues.cCOLUMN_START_VALUE}, {DatabaseStringValues.cCOLUMN_END_VALUE}, {DatabaseStringValues.cCOLUMN_START_CAPTION}, {DatabaseStringValues.cCOLUMN_END_CAPTION}) " +
                    $"OUTPUT INSERTED.{DatabaseStringValues.cCOLUMN_QUESTION_ID} VALUES ({DatabaseStringValues.cPARAMETER_QUESTION_ID}, {DatabaseStringValues.cPARAMETER_QUESTION_START_VALUE}, " +
                    $"{DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE},{DatabaseStringValues.cPARAMETER_QUESTION_START_CAPTION},{DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_ID}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
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
        /// update slider question in database slider question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(SliderQuestion data)
        {
            try
            {
                // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
                if (!mQuestionDatabaseOperation.Update(data))
                {
                    return false;
                }
                string tCommandString = $"UPDATE {DatabaseStringValues.cTABLE_SLIDER_QUESTION} SET {DatabaseStringValues.cCOLUMN_START_VALUE} = {DatabaseStringValues.cPARAMETER_QUESTION_START_VALUE}," +
                    $" {DatabaseStringValues.cCOLUMN_END_VALUE} = {DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE}, {DatabaseStringValues.cCOLUMN_START_CAPTION} ={DatabaseStringValues.cPARAMETER_QUESTION_START_CAPTION}, " +
                    $"{DatabaseStringValues.cCOLUMN_END_CAPTION} ={DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE} WHERE {DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_ID}", data.Id);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
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
        /// Delete slider question from database
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
        public SliderQuestion Read(int id)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseStringValues.cCOLUMN_QUESTION_TEXT}, {DatabaseStringValues.cCOLUMN_QUESTION_ORDER}, {DatabaseStringValues.cCOLUMN_START_VALUE}, {DatabaseStringValues.cCOLUMN_END_VALUE}, {DatabaseStringValues.cCOLUMN_START_CAPTION}, {DatabaseStringValues.cCOLUMN_END_CAPTION}, {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID},{DatabaseStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseStringValues.cTABLE_QUESTION}, {DatabaseStringValues.cTABLE_SLIDER_QUESTION} WHERE {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cPARAMETER_QUESTION_ID} AND {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cTABLE_SLIDER_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID}";

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
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
        /// <summary>
        /// Select all slider questions in given range from database
        /// </summary>
        /// <param name="offset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="limit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<SliderQuestion> ReadAll(int offset = 0, int limit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {DatabaseStringValues.cCOLUMN_QUESTION_TEXT}, {DatabaseStringValues.cCOLUMN_QUESTION_ORDER}, {DatabaseStringValues.cCOLUMN_START_VALUE}, {DatabaseStringValues.cCOLUMN_END_VALUE}, {DatabaseStringValues.cCOLUMN_START_CAPTION}, {DatabaseStringValues.cCOLUMN_END_CAPTION}, {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID},{DatabaseStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {DatabaseStringValues.cTABLE_QUESTION}, {DatabaseStringValues.cTABLE_SLIDER_QUESTION} WHERE {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} = {DatabaseStringValues.cTABLE_SLIDER_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {DatabaseStringValues.cTABLE_QUESTION}.{DatabaseStringValues.cCOLUMN_QUESTION_ID} OFFSET {DatabaseStringValues.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (limit > 0)
                    tQueryString += $" FETCH NEXT {DatabaseStringValues.cLIMIT} ROWS ONLY";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cOFFSET}", offset);
                        tCommand.Parameters.AddWithValue($"{DatabaseStringValues.cLIMIT}", limit);
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
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
    }
}
