namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support Create , Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface ICUDable<T> where T : class
    {
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="data">Object to be inserted</param>
        /// <returns>Object id in repository</returns>
        int Create(T data);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="data">The new object to be update</param>
        /// <returns>true if objected updated, false otherwise</returns>
        bool Update(T data);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="data">The id of object to be deleted</param>
        /// <returns>true if objected deleted, false otherwise</returns>
        bool Delete(int id);
    }
}
