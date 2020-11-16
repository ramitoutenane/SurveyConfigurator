using System;

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
            try
            {
                NumberOfStars = numberOfStars;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Stars question constructor to initialize new smiley question
        /// </summary>
        /// <param name="other">Stars question object to copy it's properties</param>
        /// <param name="id">New question id</param>
        public int NumberOfStars
        {
            get => nNumberOfStars;
            set { nNumberOfStars = value; }
        }
        public override string ToString() => $"{base.ToString()}\nNumber of Stars: {NumberOfStars}";
        /// <summary>
        /// Check if question is valid
        /// </summary>
        /// <returns>true if valid , false otherwise</returns>
        public override bool IsValid()
        {
            try
            {
                if (NumberOfStars < QuestionValidationValues.cSTARS_NUMBER_MIN)
                    return false;
                if (NumberOfStars > QuestionValidationValues.cSTARS_NUMBER_MAX)
                    return false;
                return base.IsValid();
            }
            catch
            {
                return false;
            }
        }
        public override Question CopyWithNewId(int id)
        {
            try
            {
                return new StarsQuestion(Text, Order, NumberOfStars, id);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
    }
}
