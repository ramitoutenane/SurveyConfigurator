using System;

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
            try
            {
                NumberOfFaces = numberOfFaces;
            }catch(Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        public int NumberOfFaces
        {
            get => mNumberOfFaces;
            set { mNumberOfFaces = value; }
        }
        public override string ToString() => $"{base.ToString()}\nNumber of Faces: {NumberOfFaces}";
        /// <summary>
        /// Check if question is valid
        /// </summary>
        /// <returns>true if valid , false otherwise</returns>
        public override bool IsValid()
        {
            try
            {
                if (NumberOfFaces < QuestionValidationValues.cFACES_NUMBER_MIN)
                    return false;
                if (NumberOfFaces > QuestionValidationValues.cFACES_NUMBER_MAX)
                    return false;
                return base.IsValid();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return false;
            }
        }
        public override Question CopyWithNewId(int id)
        {
            try
            {
                return new SmileyQuestion(Text, Order,NumberOfFaces, id);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
    }
}
