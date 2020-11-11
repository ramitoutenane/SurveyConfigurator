using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp
{
    interface IRepository<T> where T : class
    {
        //an interface to support Repository behavior
        List<T> Items { get; }
        SortOrder SortOrder { get; }
        SortMethod OrderingMethod { get; }
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        T Select(int id);
        List<T> Refresh();
        void OrderList(SortMethod OrderingMethod, SortOrder SortOrder);
        bool IsConnected();
    }
}
