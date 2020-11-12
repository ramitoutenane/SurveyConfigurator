namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Smiley question class that extends general question and adds smiley question properties 
    /// </summary>
    public class SmileyQuestion : Question
    {
        private int mNumberOfFaces;
        /// <summary>
        /// Smiley question constructor to initialize new smiley question
        /// </summary>
        /// <param name="text">Question text</param>
        /// <param name="order">Question order</param>
        /// <param name="numberOfFaces">Number of smiley faces on smiley question</param>
        /// <param name="id">Question id</param>
        public SmileyQuestion(string text, int order, int numberOfFaces, int id = -1) : base(text, order, QuestionType.Smiley, id)
        {
            NumberOfFaces = numberOfFaces;
        }
        /// <summary>
        /// Smiley question constructor to initialize new smiley question
        /// </summary>
        /// <param name="other">Smiley question object to copy it's properties</param>
        /// <param name="id">New question id</param>
        public SmileyQuestion(SmileyQuestion other, int id) : this(other.Text, other.Order, other.NumberOfFaces, id) { }
        public int NumberOfFaces
        {
            get => mNumberOfFaces;
            set { mNumberOfFaces = value; }
        }

        public override string ToString() => $"{base.ToString()}\nNumber of Faces: {NumberOfFaces}";
    }
}
