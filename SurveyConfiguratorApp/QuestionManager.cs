using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp
{
    public class QuestionManager : IRepository<Question>
    {
        private readonly SliderQuestionDatabaseOperations sliderSQL = new SliderQuestionDatabaseOperations();
        private readonly SmileyQuestionDatabaseOperations smileySQL = new SmileyQuestionDatabaseOperations();
        private readonly StarsQuestionDatabaseOperations starsSQL = new StarsQuestionDatabaseOperations();
        public List<Question> Items { get; private set; }
        QuestionManager()
        {
            Items = new List<Question>();
        }

        public List<Question> Refresh()
        {
            throw new NotImplementedException();

        }
        public Question Select(int id)
        {
            throw new NotImplementedException();

        }
        public void Insert(Question item)
        {
            throw new NotImplementedException();
        }
        public void Update(Question item)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }


    }
}
