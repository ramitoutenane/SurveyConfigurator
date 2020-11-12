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
            string tCommandString = $"INSERT INTO slider_question (question_id, start_value, end_value, start_value_caption, end_value_caption) " +
                $"OUTPUT INSERTED.question_id VALUES ({SQLStringResources.QuestionId}, {SQLStringResources.QuestionStartValue}, " +
                $"{SQLStringResources.QuestionEndValue},{SQLStringResources.QuestionStartCaption},{SQLStringResources.QuestionEndValue})";
            try
            {
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionStartValue}", data.StartValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionEndValue}", data.EndValue);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionStartCaption}", data.StartValueCaption);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionEndCaption}", data.EndValueCaption);
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
            string tCommandString = $"UPDATE slider_question SET start_value = {SQLStringResources.QuestionStartValue}," +
                $" end_value = {SQLStringResources.QuestionEndValue}, start_value_caption ={SQLStringResources.QuestionStartCaption}, " +
                $"end_value_caption ={SQLStringResources.QuestionEndValue} WHERE question_id = {SQLStringResources.QuestionId} ";
            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", data.Id);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionStartValue}", data.StartValue);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionEndValue}", data.EndValue);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionStartCaption}", data.StartValueCaption);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionEndCaption}", data.EndValueCaption);
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
            string tQueryString = $"SELECT question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id " +
                $"FROM question, slider_question WHERE question.question_id = {SQLStringResources.QuestionId} AND question.question_id = slider_question.question_id";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", id);
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
            string tQueryString = $"SELECT question_text, question_order, start_value, end_value, start_value_caption, end_value_caption, question.question_id,type_id " +
               $"FROM question, slider_question WHERE question.question_id = slider_question.question_id " +
                $"ORDER BY question.question_id OFFSET {SQLStringResources.Offset} ROWS";
            // add Fetch clause if limit is larger than 0 which is default value
            if (limit > 0)
                tQueryString += $" FETCH NEXT {SQLStringResources.Limit} ROWS ONLY";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.Offset}", offset);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.Limit}", limit);
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
