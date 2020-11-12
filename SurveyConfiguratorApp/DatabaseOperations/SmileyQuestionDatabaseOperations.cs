using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SurveyConfiguratorApp.DatabaseOperations;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on smiley question table
    /// </summary>
    class SmileyQuestionDatabaseOperations : ICUDable<SmileyQuestion>, IQueryable<SmileyQuestion>
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
        public SmileyQuestionDatabaseOperations(string connectionString)
        {
            mConnectionString = connectionString;
            mQuestionDatabaseOperation = new QuestionDatabaseOperations(connectionString);
        }
        /// <summary>
        /// Insert smiley question into database smiley question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Insert(SmileyQuestion data)
        {
            // insert general question into question table and get question id to be used as foreign key
            int tQuestionId = mQuestionDatabaseOperation.Insert(data);
            // question id is auto increment key that starts from 1, if question is inserted successfully the returned id is larger than 1
            // if id is less than 1 exit insert method to avoid foreign key reference error
            if (tQuestionId < 1)
                return tQuestionId;
            string tCommandString = $"INSERT INTO smiley_question (question_id, num_of_faces) OUTPUT INSERTED.question_id " +
                $"VALUES ({SQLStringResources.QuestionId}, {SQLStringResources.QuestionFacesNumber})";
            try
            {
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", tQuestionId);
                        tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionFacesNumber}", data.NumberOfFaces);
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
        /// update smiley question in database smiley question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(SmileyQuestion data)
        {
            // update general question into question table and get update result, if updated continue to update specific question properties, exit from update otherwise
            if (!mQuestionDatabaseOperation.Update(data))
            {
                return false;
            }
            string tCommandString = $"UPDATE smiley_question SET num_of_faces = {SQLStringResources.QuestionFacesNumber} WHERE question_id = {SQLStringResources.QuestionId} ";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", data.Id);
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionFacesNumber}", data.NumberOfFaces);
                    return tCommand.ExecuteNonQuery() > 0;
                }
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
            string tQueryString = $"SELECT question_text, question_order, num_of_faces, question.question_id, type_id " +
                $"FROM question, smiley_question WHERE question.question_id = {SQLStringResources.QuestionId} AND question.question_id = smiley_question.question_id";

            using (SqlConnection tConnection = new SqlConnection(mConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                {
                    tCommand.Parameters.AddWithValue($"{SQLStringResources.QuestionId}", id);
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
        /// <summary>
        /// Select all smiley questions in given range from database
        /// </summary>
        /// <param name="offset">Number of questions to skip before starting to return objects from the database</param>
        /// <param name="limit">Number of questions to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved questions</returns>
        public List<SmileyQuestion> SelectAll(int offset = 0, int limit = 0)
        {
            string tQueryString = $"SELECT question_text, question_order, num_of_faces, question.question_id, type_id " +
                $"FROM question, smiley_question WHERE question.question_id = smiley_question.question_id " +
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
    }
}