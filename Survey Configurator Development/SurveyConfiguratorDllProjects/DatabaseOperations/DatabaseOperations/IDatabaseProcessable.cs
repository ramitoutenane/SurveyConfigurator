using SurveyConfiguratorEntities;

namespace DatabaseOperations
{
    /// <summary>
    /// Interface to support Insert , Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface IDatabaseProcessable<T> where T : BaseQuestion
    {
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="pQuestion">Object to be inserted</param>
        /// <returns>Object id in repository</returns>
        Result Insert(T pQuestion);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="pQuestion">The new object to be update</param>
        /// <returns>true if objected updated, false otherwise</returns>
        Result Update(T pQuestion);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="pId">The id of object to be deleted</param>
        /// <returns>true if objected deleted, false otherwise</returns>
        Result Delete(int pId);
        /// <summary>
        /// check if database connection is available
        /// </summary>
        /// <returns>true if connected, false otherwise</returns>
        bool IsConnected();
    }
}
