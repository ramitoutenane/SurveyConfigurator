using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to support database operations on question table
    /// </summary>
    public class QuestionDatabaseOperations : ICUDable<Question>
    {
        private readonly string mConnectionString;
        /// <summary>
        /// QuestionDatabaseOperations constructor to initialize new QuestionDatabaseOperations object
        /// </summary>
        /// <param name="connectionString">SQL database connection string</param>
        public QuestionDatabaseOperations(string connectionString)
        {
            try
            {
                mConnectionString = connectionString;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Insert question into database question table
        /// </summary>
        /// <param name="data">question to be inserted</param>
        /// <returns>inserted question id</returns>
        public int Create(Question data)
        {
            try
            {
                string tCommandString = $"INSERT INTO {SQLStringValues.cTABLE_QUESTION} ({SQLStringValues.cCOLUMN_QUESTION_TEXT}, {SQLStringValues.cCOLUMN_QUESTION_ORDER}, {SQLStringValues.cCOLUMN_TYPE_ID}) OUTPUT INSERTED.{SQLStringValues.cCOLUMN_QUESTION_ID} " +
                    $"VALUES ({SQLStringValues.cPARAMETER_QUESTION_TEXT}, {SQLStringValues.cPARAMETER_QUESTION_ORDER}, {SQLStringValues.cPARAMETER_QUESTION_TYPE})";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_TYPE}", (int)data.Type);
                        tConnection.Open();
                        return (int)tCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return -1;
            }
        }
        /// <summary>
        /// update question in database question table
        /// </summary>
        /// <param name="data">question to be updated</param>
        /// <returns>true if question updated, false otherwise</returns>
        public bool Update(Question data)
        {
            try
            {
                string tCommandString = $"UPDATE {SQLStringValues.cTABLE_QUESTION} SET {SQLStringValues.cCOLUMN_QUESTION_TEXT} = {SQLStringValues.cPARAMETER_QUESTION_TEXT}, " +
                    $"{SQLStringValues.cCOLUMN_QUESTION_ORDER} = {SQLStringValues.cPARAMETER_QUESTION_ORDER} WHERE {SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cPARAMETER_QUESTION_ID} ";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_TEXT}", data.Text);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ORDER}", data.Order);
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ID}", data.Id);
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
        /// Delete question from database
        /// </summary>
        /// <param name="data">The id of question to be deleted</param>
        /// <returns>true if question deleted, false otherwise</returns>
        public bool Delete(int id)
        {
            try
            {
                string tCommandString = $"DELETE FROM {SQLStringValues.cTABLE_QUESTION} WHERE {SQLStringValues.cCOLUMN_QUESTION_ID} = {SQLStringValues.cPARAMETER_QUESTION_ID}";
                using (SqlConnection tConnection = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(tCommandString, tConnection))
                    {
                        tCommand.Parameters.AddWithValue($"{SQLStringValues.cPARAMETER_QUESTION_ID}", id);
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
    }
}
