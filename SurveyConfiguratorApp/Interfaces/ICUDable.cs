using System.Data.Common;

namespace SurveyConfiguratorApp
{
    public interface ICUDable<T, K> where T : class where K : DbConnection
    {
        //an interface to support Create , Update , Delete behavior on database
        int Insert(K connection, T data);
        bool Update(K connection, T data);
        bool Delete(K connection, int id);

    }
}
