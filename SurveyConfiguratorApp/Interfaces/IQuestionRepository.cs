using System.Collections.Generic;
namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support repository maintaining behavior
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        /// List of objects to be maintained 
        /// </summary>
        List<Question> QuestionsList { get; }
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="question">Object to be inserted</param>
        bool Insert(Question question);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="question">The new object to be update</param>
        /// <returns>true if inserted, false otherwise</return 
        bool Update(Question question);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="id">The id of object to be deleted</param>
        /// <returns>true if updated, false otherwise</return
        bool Delete(int id);
        /// <summary>
        /// Synchronize local list with latest version from source
        /// </summary>
        /// <returns>The new refreshed List </returns>
        List<Question> SelectAll();
    }
}
