namespace SurveyConfiguratorApp
{/// <summary>
 /// Abstract class to represent General question with required attributes
 /// </summary>
    public abstract class Question
    {

        private string mText;
        private int mOrder;
        private readonly int mId;

        public readonly QuestionType Type;
        /// <summary>
        /// Question constructor to initialize common data between question types
        /// </summary>
        /// <param name="text">Question text</param>
        /// <param name="order">Question order</param>
        /// <param name="type">Question type</param>
        /// <param name="id">Question id</param>
        protected Question(string text, int order, QuestionType type, int id)
        {
            Text = text;
            Order = order;
            Type = type;
            mId = id;
        }

        public string Text
        {
            get => mText;
            set { mText = value; }
        }

        public int Order
        {
            get => mOrder;
            set { mOrder = value; }
        }

        public int Id { get => mId; }

        public override string ToString() => $"Id: {Id}\nType: {Type}\nQuestion: {Text}\nOrder: {Order}";
    }
}
