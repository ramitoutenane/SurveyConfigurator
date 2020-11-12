using System.Collections.Generic;
using System.Data.Common;
namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support query behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    interface IQueryable<T> where T : class
    {
        /// <summary>
        /// Select all objects in given range from repository
        /// </summary>
        /// <param name="offset">Number of objects to skip before starting to return objects from the repository</param>
        /// <param name="limit">Number of objects to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved objects</returns>
        List<T> SelectAll(int offset, int limit = 0);
        /// <summary>
        /// Select specific object from the repository
        /// </summary>
        /// <param name="id">Id of object to be selected</param>
        /// <returns>The selected object if exist, null otherwise</returns>
        T Select(int id);
    }
}
