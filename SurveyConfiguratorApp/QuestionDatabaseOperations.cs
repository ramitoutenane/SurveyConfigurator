﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp
{
    public class QuestionDatabaseOperations : IRepository<Question, SqlConnection>
    {
        public bool Delete(SqlConnection connection, int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(SqlConnection connection, Question data)
        {
            throw new NotImplementedException();
        }

        public Question Select(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public List<Question> SelectAll(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public bool Update(SqlConnection connection, Question data)
        {
            throw new NotImplementedException();
        }
    }
}
