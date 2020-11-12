namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Stars question class that extends general question and adds stars question properties 
    /// </summary>
    public class StarsQuestion : Question
    {
        private int nNumberOfStars;
        /// <summary>
        /// Stars question constructor to initialize new stars question
        /// </summary>
        /// <param name="text">Question text</param>
        /// <param name="order">Question order</param>
        /// <param name="numberOfStars">Number of stars on stars question</param>
        /// <param name="id">Question id</param>
        public StarsQuestion(string text, int order, int numberOfStars, int id = -1) : base(text, order, QuestionType.Stars, id)
        {
            NumberOfStars = numberOfStars;
        }
        /// <summary>
        /// Stars question constructor to initialize new smiley question
        /// </summary>
        /// <param name="other">Stars question object to copy it's properties</param>
        /// <param name="id">New question id</param>
        public StarsQuestion(StarsQuestion other, int id) : this(other.Text, other.Order, other.NumberOfStars, id) { }

        public int NumberOfStars
        {
            get => nNumberOfStars;
            set { nNumberOfStars = value; }
        }
        public override string ToString() => $"{base.ToString()}\nNumber of Stars: {NumberOfStars}";
    }
}
