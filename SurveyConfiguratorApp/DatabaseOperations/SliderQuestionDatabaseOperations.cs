using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on slider question table
    /// </summary>
    class SliderQuestionDatabaseOperations : IRepository<SliderQuestion>
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
                string tCommandString = $"INSERT INTO {SQLStringValues.cTABLE_SLIDER_QUESTION} ({SQLStringValues.cCOLUMN_QUESTION_ID}, {SQLStringValues.cCOLUMN_START_VALUE}, {SQLStringValues.cCOLUMN_END_VALUE}, {SQLStringValues.cCOLUMN_START_CAPTION}, {SQLStringValues.cCOLUMN_END_CAPTION}) " +
                    $"OUTPUT INSERTED.{SQLStringValues.cCOLUMN_QUESTION_ID} VALUES ({SQLStringValues.cPARAMETER_QUESTION_ID}, {SQLStringValues.cPARAMETER_QUESTION_START_VALUE}, " +
                    $"{SQLStringValues.cPARAMETER_QUESTION_END_VALUE},{SQLStringValues.cPARAMETER_QUESTION_START_CAPTION},{SQLStringValues.cPARAMETER_QUESTION_END_VALUE})";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ID}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
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
                string tCommandString = $"UPDATE {SQLStringValues.cTABLE_SLIDER_QUESTION} SET {SQLStringValues.cCOLUMN_START_VALUE} = {SQLStringValues.cPARAMETER_QUESTION_START_VALUE}," +
                    $" {SQLStringValues.cCOLUMN_END_VALUE} = {SQLStringValues.cPARAMETER_QUESTION_END_VALUE}, {SQLStringValues.cCOLUMN_START_CAPTION} ={SQLStringValues.cPARAMETER_QUESTION_START_CAPTION}, " +
                    $"{SQLStringValues.cCOLUMN_END_CAPTION} ={SQLStringValues.cPARAMETER_QUESTION_END_VALUE} WHERE {SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ID}", data.Id);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
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
        public SliderQuestion Select(int id)
        {
            try
            {
                string tQueryString = $"SELECT {SQLStringValues.cCOLUMN_QUESTION_TEXT}, {SQLStringValues.cCOLUMN_QUESTION_ORDER}, {SQLStringValues.cCOLUMN_START_VALUE}, {SQLStringValues.cCOLUMN_END_VALUE}, {SQLStringValues.cCOLUMN_START_CAPTION}, {SQLStringValues.cCOLUMN_END_CAPTION}, {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID},{SQLStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {SQLStringValues.cTABLE_QUESTION}, {SQLStringValues.cTABLE_SLIDER_QUESTION} WHERE {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cPARAMETER_QUESTION_ID} AND {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cTABLE_SLIDER_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID}";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ID}", id);
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
        public List<SliderQuestion> SelectAll(int offset = 0, int limit = 0)
        {
            try
            {
                string tQueryString = $"SELECT {SQLStringValues.cCOLUMN_QUESTION_TEXT}, {SQLStringValues.cCOLUMN_QUESTION_ORDER}, {SQLStringValues.cCOLUMN_START_VALUE}, {SQLStringValues.cCOLUMN_END_VALUE}, {SQLStringValues.cCOLUMN_START_CAPTION}, {SQLStringValues.cCOLUMN_END_CAPTION}, {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID},{SQLStringValues.cCOLUMN_TYPE_ID} " +
                    $"FROM {SQLStringValues.cTABLE_QUESTION}, {SQLStringValues.cTABLE_SLIDER_QUESTION} WHERE {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cTABLE_SLIDER_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID} " +
                    $"ORDER BY {SQLStringValues.cTABLE_QUESTION}.{SQLStringValues.cCOLUMN_QUESTION_ID} OFFSET {SQLStringValues.cOFFSET} ROWS";
                // add Fetch clause if limit is larger than 0 which is default value
                if (limit > 0)
                    tQueryString += $" FETCH NEXT {SQLStringValues.cLIMIT} ROWS ONLY";

                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cOFFSET}", offset);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cLIMIT}", limit);
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
