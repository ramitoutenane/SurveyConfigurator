namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support Insert , Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface IDatabaseProcessable<T> where T : Question
    {
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="question">Object to be inserted</param>
        /// <returns>Object id in repository</returns>
        int Insert(T question);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="question">The new object to be update</param>
        /// <returns>true if objected updated, false otherwise</returns>
        bool Update(T question);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="id">The id of object to be deleted</param>
        /// <returns>true if objected deleted, false otherwise</returns>
        bool Delete(int id);
    }
}
