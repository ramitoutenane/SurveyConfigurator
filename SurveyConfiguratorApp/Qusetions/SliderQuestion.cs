using System;

namespace SurveyConfiguratorApp
{
    public class SliderQuestion : Question
    {
        //Slider question class that extends general question and adds Slider question properties 

        private int startValue = 0;
        private int endValue = 100;
        private string startValueCaption;
        private string endValueCaption;

        public SliderQuestion(string text, int order, int startValue, int endValue, string startValueCaption, string endValueCaption, int id = -1) : base(text, order, QuestionType.Slider, id)
        {
            if (startValue > endValue)
                throw new ArgumentOutOfRangeException($"Start value must be less than end value");
            StartValue = startValue;
            EndValue = endValue;
            StartValueCaption = startValueCaption;
            EndValueCaption = endValueCaption;
        }
        public SliderQuestion(SliderQuestion other, int id): this(other.Text, other.Order, other.StartValue, other.EndValue, other.StartValueCaption, other.EndValueCaption, id){ }

        public int StartValue
        {
            get => startValue;
            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException("Start value must be between 0 and 99");
                if (value > EndValue)
                    throw new ArgumentOutOfRangeException($"Start value must be less than end value ({EndValue})");
                startValue = value;
            }
        }
        public int EndValue
        {
            get => endValue; set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentOutOfRangeException("End Value must be between 1 and 100");
                if (value < startValue)
                    throw new ArgumentOutOfRangeException($"End value must be less than start value ({StartValue})");
                endValue = value;
            }
        }
        public string StartValueCaption
        {
            get => startValueCaption;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Caption cannot be null or empty");

                value = value.Trim();
                if (value.Length < 1 || value.Length > 255)
                    throw new ArgumentOutOfRangeException("Caption length must be between 1 and 255");

                startValueCaption = value;
            }
        }

        public string EndValueCaption
        {
            get => endValueCaption;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Caption cannot be null or empty");

                value = value.Trim();
                if (value.Length < 1 || value.Length > 255)
                    throw new ArgumentOutOfRangeException("Caption length must be between 1 and 255");

                endValueCaption = value;
            }
        }

        public override string ToString() => $"{base.ToString()}\n{StartValueCaption}: {StartValue}\n{EndValueCaption}:{EndValue}";
    }
}
