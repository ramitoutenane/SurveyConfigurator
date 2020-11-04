using System;

namespace SurveyConfiguratorApp
{
    public enum QuestionType
    {
        //Enumeration to define types of questions provided by the system

        Smiley = 1,
        Slider = 2,
        Stars = 3
    }

    public abstract class Question
    {
        //Abstract class to represent General question with required attributes

        private string text;
        private int order;
        public readonly QuestionType type;

        protected Question(string text, int order, QuestionType type, int id)
        {
            Text = text;
            Order = order;
            this.type = type;
            ID = id;
        }

        public string Text
        {
            get => text;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Question text cannot be null or empty");

                value = value.Trim();
                if (value.Length < 1 || value.Length > 4000)
                    throw new ArgumentOutOfRangeException("Question length must be between 1 and 4000");

                text = value;
            }
        }

        public int Order
        {
            get => order;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Question order must be larger than 0");
                order = value;
            }
        }

        public int ID { get; set; }

        public override string ToString() => $"ID: {ID}\nType: {type}\nQuestion: {Text}\nOrder: {Order}";
    }
}
