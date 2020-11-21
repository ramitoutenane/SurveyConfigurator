using System;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Slider question class that extends general question and adds slider question properties 
    /// </summary>
    public class SliderQuestion : Question
    {

        private int mStartValue;
        private int mEndValue;
        private string mStartValueCaption;
        private string mEndValueCaption;
        /// <summary>
        /// Slider question constructor to initialize new slider question
        /// </summary>
        /// <param name="text">Question text</param>
        /// <param name="order">Question order</param>
        /// <param name="startValue">Slider start value</param>
        /// <param name="endValue">Slider end value</param>
        /// <param name="startValueCaption">Slider start value caption</param>
        /// <param name="endValueCaption">Slider end value caption</param>
        /// <param name="id">Question id</param>
        public SliderQuestion(string text, int order, int startValue, int endValue, string startValueCaption, string endValueCaption, int id = -1) : base(text, order, QuestionType.Slider, id)
        {
            try
            {
                StartValue = startValue;
                EndValue = endValue;
                StartValueCaption = startValueCaption;
                EndValueCaption = endValueCaption;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }

        /// <summary>
        /// Slider question constructor to initialize new slider question
        /// </summary>
        /// <param name="other">Slider question object to copy it's properties</param>
        /// <param name="id">New question id</param>

        public int StartValue
        {
            get => mStartValue;
            set { mStartValue = value; }
        }
        public int EndValue
        {
            get => mEndValue;
            set { mEndValue = value; }
        }
        public string StartValueCaption
        {
            get => mStartValueCaption;
            set { mStartValueCaption = value; }
        }

        public string EndValueCaption
        {
            get => mEndValueCaption;
            set { mEndValueCaption = value; }
        }
        public override string ToString() => $"{base.ToString()}\n{StartValueCaption}: {StartValue}\n{EndValueCaption}:{EndValue}";
        /// <summary>
        /// Check if question is valid
        /// </summary>
        /// <returns>true if valid , false otherwise</returns>
        public override bool IsValid()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(StartValueCaption) || StartValueCaption.Length > QuestionValidationValues.cSTART_CAPTION_LENGTH)
                    return false;
                if (string.IsNullOrWhiteSpace(mEndValueCaption) || mEndValueCaption.Length > QuestionValidationValues.cEND_CAPTION_LENGTH)
                    return false;
                if (StartValue < QuestionValidationValues.cSTART_VALUE_MIN)
                    return false;
                if (EndValue > QuestionValidationValues.cEND_VALUE_MAX)
                    return false;
                if (StartValue > EndValue)
                    return false;
                return base.IsValid();
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Get a copy of the current question with new id
        /// </summary>
        /// <param name="id">the new id</param>
        /// <returns></returns>
        public override Question CopyWithNewId(int id)
        {
            try
            {
                return new SliderQuestion(Text, Order, StartValue, EndValue, StartValueCaption, EndValueCaption, id);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                return null;
            }
        }
    }
}
