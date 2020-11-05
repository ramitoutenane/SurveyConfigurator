using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp
{
    interface IRepository<T> where T : class
    {
        List<T> Items { get; }
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        T Select(int id);
        List<T> Refresh();
    }
}
