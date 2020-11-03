using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    class SliderQuestionDatabaseOperations : IRepository<SliderQuestion, SqlConnection>
    {
        private QuestionDatabaseOperations qda;
        SliderQuestionDatabaseOperations()
        {
            qda = new QuestionDatabaseOperations();
        }

        public bool Delete(SqlConnection connection, int id)
        {
            throw new NotImplementedException();
        }


        public bool Insert(SqlConnection connection, SliderQuestion data)
        {
            throw new NotImplementedException();
        }

        public SliderQuestion Select(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public List<SliderQuestion> SelectAll(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public bool Update(SqlConnection connection, SliderQuestion data)
        {
            throw new NotImplementedException();
        }
    }
}
