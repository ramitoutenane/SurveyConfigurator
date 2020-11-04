using System;
using System.Collections.Generic;
using System.Data.Common;


namespace SurveyConfiguratorApp
{
    interface IQueryable<T, K> where T : class where K : DbConnection
    {
        List<T> SelectAll(K connection, int offsit = 0, int limit = 0);
        T Select(K connection, int id);
    }
}
