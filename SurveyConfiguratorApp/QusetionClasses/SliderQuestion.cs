using System;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Slider question class that extends general question and adds Slider question properties 
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
            StartValue = startValue;
            EndValue = endValue;
            StartValueCaption = startValueCaption;
            EndValueCaption = endValueCaption;
        }
        /// <summary>
        /// Slider question constructor to initialize new slider question
        /// </summary>
        /// <param name="other">Slider question object to copy it's properties</param>
        /// <param name="id">New question id</param>
        public SliderQuestion(SliderQuestion other, int id) : this(other.Text, other.Order, other.StartValue, other.EndValue, other.StartValueCaption, other.EndValueCaption, id) { }

        public int StartValue
        {
            get => mStartValue;
            set {mStartValue = value; }
        }
        public int EndValue
        {
            get => mEndValue; 
            set{mEndValue = value;}
        }
        public string StartValueCaption
        {
            get => mStartValueCaption;
            set {mStartValueCaption = value; }
        }

        public string EndValueCaption
        {
            get => mEndValueCaption;
            set{ mEndValueCaption = value;}
        }

        public override string ToString() => $"{base.ToString()}\n{StartValueCaption}: {StartValue}\n{EndValueCaption}:{EndValue}";
    }
}
