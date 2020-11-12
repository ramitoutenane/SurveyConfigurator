using System.Collections.Generic;
namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support repository maintaining behavior
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    interface IRepository<T> where T : class
    {
        /// <summary>
        /// List of objects to be maintained 
        /// </summary>
        List<T> Items { get; }
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="item">Object to be inserted</param>
        void Insert(T item);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="item">The new object to be update</param>
        void Update(T item);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="id">The id of object to be deleted</param>
        void Delete(int id);
        /// <summary>
        /// Select specific object from the repository
        /// </summary>
        /// <param name="id">Id of object to be selected</param>
        /// <returns>The selected object if exist, null otherwise</returns>
        T Select(int id);
        /// <summary>
        /// Synchronize local list with latest version from source
        /// </summary>
        /// <returns>The new refreshed List </returns>
        List<T> Refresh();
    }
}
