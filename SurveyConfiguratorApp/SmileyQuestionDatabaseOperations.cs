using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SmileyQuestionDatabaseOperations : IRepository<SmileyQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        SmileyQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }

        public bool Delete(SqlConnection connection, int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(SqlConnection connection, SmileyQuestion data)
        {
            throw new NotImplementedException();
        }

        public SmileyQuestion Select(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public List<SmileyQuestion> SelectAll(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public bool Update(SqlConnection connection, SmileyQuestion data)
        {
            throw new NotImplementedException();
        }
    }
}
