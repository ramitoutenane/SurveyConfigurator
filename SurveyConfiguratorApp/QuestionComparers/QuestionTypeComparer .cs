using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.QuestionComparers
{
    class QuestionTypeComparer : Comparer<Question>
    {
        public override int Compare(Question x, Question y)
        {
            return x.Type.CompareTo(y.Type);
        }
    }
}
