using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.QuestionComparers
{
    class QuestionIDComparer : Comparer<Question>
    {
        public override int Compare(Question x, Question y)
        {
            return x.ID.CompareTo(y.ID);
        }
    }
}
