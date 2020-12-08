using SurveyConfiguratorEntities;
using System.Collections.Generic;
using System.Data.Common;
namespace DatabaseOperations
{
    /// <summary>
    /// Interface to support query behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface IDatabaseSelectable<T> where T : BaseQuestion
    {
        /// <summary>
        /// Select all objects in given range from repository
        /// </summary>
        /// <param name="pOffset">Number of objects to skip before starting to return objects from the repository</param>
        /// <param name="pLimit">Number of objects to return after the offset has been processed</param>
        /// <returns>List that contains the retrieved objects</returns>
        List<T> SelectAll(int pOffset = 0, int pLimit = 0);
        /// <summary>
        /// Select specific object from the repository
        /// </summary>
        /// <param name="pId">Id of object to be selected</param>
        /// <returns>The selected object if exist, null otherwise</returns>
        T Read(int pId);
    }
}
