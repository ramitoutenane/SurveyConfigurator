using System;
using System.Collections.Generic;
using System.Data.Common;

namespace SurveyConfiguratorApp
{
    public interface IRepository<T , K> where T : class where K : DbConnection
    {
        List<T> SelectAll(K connection);
        T Select(K connection);
        bool Insert(K connection, T data);
        bool Update(K connection, T data);
        bool Delete(K connection, int id);

    }
}
