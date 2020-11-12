namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Stars question class that extends general question and adds Stars question properties 
    /// </summary>
    public class StarsQuestion : Question
    {

        private int nNumberOfStars;
        public StarsQuestion(string text, int order, int numberOfStars, int id = -1) : base(text, order, QuestionType.Stars, id)
        {
            NumberOfStars = numberOfStars;
        }
        public StarsQuestion(StarsQuestion other, int id) : this(other.Text, other.Order, other.NumberOfStars, id) { }

        public int NumberOfStars
        {
            get => nNumberOfStars;
            set{nNumberOfStars = value;}
        }
        public override string ToString() => $"{base.ToString()}\nNumber of Stars: {NumberOfStars}";
    }
}
