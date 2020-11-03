using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class StarsQuestionDatabaseOperations : IRepository<StarsQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        StarsQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }

        public bool Delete(SqlConnection connection, int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(SqlConnection connection, StarsQuestion data)
        {
            throw new NotImplementedException();
        }

        public StarsQuestion Select(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public List<StarsQuestion> SelectAll(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public bool Update(SqlConnection connection, StarsQuestion data)
        {
            throw new NotImplementedException();
        }
    }
}
