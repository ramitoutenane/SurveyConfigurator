﻿using System;
using System.Collections.Generic;
using System.Data.Common;


namespace SurveyConfiguratorApp
{
    interface IQueryable<T, K> where T : class where K : DbConnection
    {
        //an interface to support retrieve behavior on database
        List<T> SelectAll(K connection, int offsit = 0, int limit = 0);
        T Select(K connection, int id);
    }
}
