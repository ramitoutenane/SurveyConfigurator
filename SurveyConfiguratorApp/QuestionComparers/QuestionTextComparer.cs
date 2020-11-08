using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.QuestionComparers
{
    class QuestionTextComparer : Comparer<Question>
    {
        public override int Compare(Question x, Question y)
        {
            return x.Text.CompareTo(y.Text);
        }
    }
}
