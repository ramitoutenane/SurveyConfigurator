using System;
using System.Collections.Generic;
using System.Data.Common;

namespace SurveyConfiguratorApp
{
    public interface ICUDable<T, K> where T : class where K : DbConnection
    {
        int Insert(K connection, T data);
        bool Update(K connection, T data);
        bool Delete(K connection, int id);

    }
}
