using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.QuestionComparers
{
    class QuestionOrderComparer : Comparer<Question>
    {
        public override int Compare(Question x, Question y)
        {
            return x.Order.CompareTo(y.Order);
        }
    }
}
