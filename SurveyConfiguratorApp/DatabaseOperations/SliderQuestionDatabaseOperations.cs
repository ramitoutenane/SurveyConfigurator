using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SurveyConfiguratorApp.DatabaseOperations;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on slider question table
    /// </summary>
    class SliderQuestionDatabaseOperations : ICUDable<SliderQuestion>, IQueryable<SliderQuestion>
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
            mConnectionString = connectionString;
            mQuestionDatabaseOperation = new QuestionDatabaseOperations(connectionString);
        }
        /// <summary>
        /// Insert slider question into database slider question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Insert(SliderQuestion data)
        {
            // insert general question into question table and get question id to be used as foreign key
            int tQuestionId = mQuestionDatabaseOperation.Insert(data);
            // question id is auto increment key that starts from 1, if question is inserted successfully the returned id is larger than 1
            // if id is less than 1 exit insert method to avoid foreign key reference error
            if (tQuestionId < 1)
                return tQuestionId;
            string tCommandString = $"INSERT INTO {SQLStringResources.cTABLE_SLIDER_QUESTION} ({SQLStringResources.cCOLUMN_QUESTION_ID}, {SQLStringResources.cCOLUMN_START_VALUE}, {SQLStringResources.cCOLUMN_END_VALUE}, {SQLStringResources.cCOLUMN_START_CAPTION}, {SQLStringResources.cCOLUMN_END_CAPTION}) " +
                $"OUTPUT INSERTED.{SQLStringResources.cCOLUMN_QUESTION_ID} VALUES ({SQLStringResources.cPARAMETER_QUESTION_ID}, {SQLStringResources.cPARAMETER_QUESTION_START_VALUE}, " +
                $"{SQLStringResources.cPARAMETER_QUESTION_END_VALUE},{SQLStringResources.cPARAMETER_QUESTION_START_CAPTION},{SQLStringResources.cPARAMETER_QUESTION_END_VALUE})";
            try
            {
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ID}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
                        tConnection.Open();
                        return (int)tCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // if exception raises on specific question data insertion then delete inserted general question from question table
                mQuestionDatabaseOperation.Delete(tQuestionId);
                throw ex;
            }
        }
        /// <summary>
        /// update slider question in database slider question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(SliderQuestion data)
        {
            // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
            if (!mQuestionDatabaseOperation.Update(data))
            {
                return false;
            }
            string tCommandString = $"UPDATE {SQLStringResources.cTABLE_SLIDER_QUESTION} SET {SQLStringResources.cCOLUMN_START_VALUE} = {SQLStringResources.cPARAMETER_QUESTION_START_VALUE}," +
                $" {SQLStringResources.cCOLUMN_END_VALUE} = {SQLStringResources.cPARAMETER_QUESTION_END_VALUE}, {SQLStringResources.cCOLUMN_START_CAPTION} ={SQLStringResources.cPARAMETER_QUESTION_START_CAPTION}, " +
                $"{SQLStringResources.cCOLUMN_END_CAPTION} ={SQLStringResources.cPARAMETER_QUESTION_END_VALUE} WHERE {SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cPARAMETER_QUESTION_ID} ";
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ID}", data.Id);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_START_VALUE}", data.StartValue);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_END_VALUE}", data.EndValue);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_START_CAPTION}", data.StartValueCaption);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_END_CAPTION}", data.EndValueCaption);
                    tConnection.Open();
                    return tCommand.ExecuteNonQuery() > 0;
                }
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
            string tQueryString = $"SELECT {SQLStringResources.cCOLUMN_QUESTION_TEXT}, {SQLStringResources.cCOLUMN_QUESTION_ORDER}, {SQLStringResources.cCOLUMN_START_VALUE}, {SQLStringResources.cCOLUMN_END_VALUE}, {SQLStringResources.cCOLUMN_START_CAPTION}, {SQLStringResources.cCOLUMN_END_CAPTION}, {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID},{SQLStringResources.cCOLUMN_TYPE_ID} " +
                $"FROM {SQLStringResources.cTABLE_QUESTION}, {SQLStringResources.cTABLE_SLIDER_QUESTION} WHERE {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cPARAMETER_QUESTION_ID} AND {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cTABLE_SLIDER_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID}";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cPARAMETER_QUESTION_ID}", id);
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
        /// <summary>
        /// Select all slider questions in given range from database
        /// </summary>
        /// <param name="offset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="limit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<SliderQuestion> SelectAll(int offset = 0, int limit = 0)
        {
            string tQueryString = $"SELECT {SQLStringResources.cCOLUMN_QUESTION_TEXT}, {SQLStringResources.cCOLUMN_QUESTION_ORDER}, {SQLStringResources.cCOLUMN_START_VALUE}, {SQLStringResources.cCOLUMN_END_VALUE}, {SQLStringResources.cCOLUMN_START_CAPTION}, {SQLStringResources.cCOLUMN_END_CAPTION}, {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID},{SQLStringResources.cCOLUMN_TYPE_ID} " +
                $"FROM {SQLStringResources.cTABLE_QUESTION}, {SQLStringResources.cTABLE_SLIDER_QUESTION} WHERE {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID} = {SQLStringResources.cTABLE_SLIDER_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID} " +
                $"ORDER BY {SQLStringResources.cTABLE_QUESTION}.{SQLStringResources.cCOLUMN_QUESTION_ID} OFFSET {SQLStringResources.cOFFSET} ROWS";
            // add Fetch clause if limit is larger than 0 which is default value
            if (limit > 0)
                tQueryString += $" FETCH NEXT {SQLStringResources.cLIMIT} ROWS ONLY";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cOFFSET}", offset);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.cLIMIT}", limit);
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
    }
}
