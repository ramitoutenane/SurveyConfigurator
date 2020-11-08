using System;

namespace SurveyConfiguratorApp
{
    public class StarsQuestion : Question
    {
        //Stars question class that extends general question and adds Stars question properties 

        private int numberOfStars;
        public StarsQuestion(string text, int order, int numberOfStars, int id = -1) : base(text, order, QuestionType.Stars, id)
        {
            NumberOfStars = numberOfStars;
        }
        public StarsQuestion(StarsQuestion other, int id) : this(other.Text, other.Order, other.NumberOfStars, id) { }

        public int NumberOfStars
        {
            get => numberOfStars;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentOutOfRangeException("Number of Faces must be between 1 and 10");
                numberOfStars = value;
            }
        }
        public override string ToString() => $"{base.ToString()}\nNumber of Stars: {NumberOfStars}";
    }
}
