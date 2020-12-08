﻿using SurveyConfiguratorEntities;
using System.Collections.Generic;
using DatabaseOperations;
namespace QuestionManaging
{
    /// <summary>
    /// Interface to support repository maintaining behavior
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        /// List of objects to be maintained 
        /// </summary>
        List<BaseQuestion> QuestionsList { get; }
        /// <summary>
        /// Insert object into repository
        /// </summary>
        /// <param name="pQuestion">Object to be inserted</param>
        Reslut Insert(BaseQuestion pQuestion);
        /// <summary>
        /// Update object on repository
        /// </summary>
        /// <param name="pQuestion">The new object to be update</param>
        /// <returns>true if inserted, false otherwise</return 
        Reslut Update(BaseQuestion pQuestion);
        /// <summary>
        /// Delete object from repository
        /// </summary>
        /// <param name="pId">The id of object to be deleted</param>
        /// <returns>true if updated, false otherwise</return
        Reslut Delete(int pId);
        /// <summary>
        /// Synchronize local list with latest version from source
        /// </summary>
        /// <returns>The new refreshed List </returns>
        Reslut RefreshQuestionList();
        /// <summary>
        /// Check if source connection is available
        /// </summary>
        /// <returns>true if connected, false otherwise</returns>
        bool IsConnected();

    }
}
