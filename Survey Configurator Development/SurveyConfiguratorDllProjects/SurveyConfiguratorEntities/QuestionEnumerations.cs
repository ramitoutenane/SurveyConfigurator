using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Enumeration to define types of questions provided by the system
    /// </summary>
    public enum QuestionType
    {
        Smiley = 1,
        Slider = 2,
        Stars = 3
    }
    /// <summary>
    /// Enumeration to define types of response status provided by the system
    /// </summary>
    public enum ResultValue
    {
        Default,
        Fail,
        Success,
        Error
    }
}
