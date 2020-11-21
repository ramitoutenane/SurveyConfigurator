using System;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Enumeration to define types of questions provided by the system
    /// </summary>
    public enum QuestionType
    {
        Smiley = 1,
        Slider = 2,
        Stars = 3
    }

    /// <summary>
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
            try
            {
                Text = text;
                Order = order;
                Type = type;
                mId = id;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
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
        public string TypeString { get => Type.ToString(); }
        public override string ToString() => $"Id: {Id}\nType: {TypeString}\nQuestion: {Text}\nOrder: {Order}";
        /// <summary>
        /// Check if question is valid
        /// </summary>
        /// <returns>true if valid , false otherwise</returns>
        public virtual bool IsValid()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Text) || Text.Length > QuestionValidationValues.cQUESTION_TEXT_LENGTH)
                    return false;
                if (Order < QuestionValidationValues.cQUESTION_ORDER_MIN)
                    return false;
                return true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
        /// <summary>
        /// Get a copy of the current question with new id
        /// </summary>
        /// <param name="id">the new id</param>
        /// <returns></returns>
        public abstract Question CopyWithNewId(int id);
    }
}
