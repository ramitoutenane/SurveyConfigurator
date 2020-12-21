using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyConfiguratorWeb.Models
{
    #region SortMethod Enumeration
    /// <summary>
    /// Enumeration to define types of sorting methods provided by the system
    /// </summary>
    public enum SortMethod
    {
        ByQuestionID,
        ByQuestionOrder,
        ByQuestionType,
        ByQuestionText
    }
    #endregion
}