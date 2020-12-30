using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Slider question class that extends general question and adds slider question properties 
    /// </summary>
    public class SliderQuestion : BaseQuestion
    {
        #region Variable deceleration
        private int mStartValue;
        private int mEndValue;
        private string mStartValueCaption;
        private string mEndValueCaption;
        #endregion
        #region Constructor
        public SliderQuestion() : base(QuestionType.Slider) 
        {
            StartValue = QuestionValidationValues.cSTART_VALUE_MIN;
            EndValue = QuestionValidationValues.cEND_VALUE_MAX;
        }
        /// <summary>
        /// Slider question constructor to initialize new slider question
        /// </summary>
        /// <param name="pText">Question text</param>
        /// <param name="pOrder">Question order</param>
        /// <param name="pStartValue">Slider start value</param>
        /// <param name="pEndValue">Slider end value</param>
        /// <param name="pStartValueCaption">Slider start value caption</param>
        /// <param name="pEndValueCaption">Slider end value caption</param>
        /// <param name="pId">Question id</param>
        public SliderQuestion(string pText, int pOrder, int pStartValue, int pEndValue, string pStartValueCaption, string pEndValueCaption, int pId = -1) : base(pText, pOrder, QuestionType.Slider, pId)
        {
            try
            {
                StartValue = pStartValue;
                EndValue = pEndValue;
                StartValueCaption = pStartValueCaption;
                EndValueCaption = pEndValueCaption;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Properties definition
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_START_VALUE_ERROR")]
        [Range(QuestionValidationValues.cSTART_VALUE_MIN, QuestionValidationValues.cEND_VALUE_MAX - 1, ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "START_VALUE_ERROR")]
        public int StartValue
        {
            get => mStartValue;
            set { mStartValue = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_END_VALUE_ERROR")]
        [Range(QuestionValidationValues.cSTART_VALUE_MIN + 1, QuestionValidationValues.cEND_VALUE_MAX, ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "END_VALUE_ERROR")]
        public int EndValue
        {
            get => mEndValue;
            set { mEndValue = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_START_CAPTION_ERROR")]
        [MaxLength(QuestionValidationValues.cSTART_CAPTION_LENGTH, ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "START_CAPTION_LENGTH_ERROR")]
        public string StartValueCaption
        {
            get => mStartValueCaption;
            set { mStartValueCaption = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_END_CAPTION_ERROR")]
        [MaxLength(QuestionValidationValues.cEND_CAPTION_LENGTH, ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "END_CAPTION_LENGTH_ERROR")]
        public string EndValueCaption
        {
            get => mEndValueCaption;
            set { mEndValueCaption = value; }
        }
        #endregion
        #region Methods
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

        public override bool Equals(object pObject)
        {
            try
            {
                return Equals(pObject as SliderQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public bool Equals(SliderQuestion pQuestion)
        {
            try
            {
                return pQuestion != null &&
                       base.Equals(pQuestion) &&
                       mStartValue == pQuestion.mStartValue &&
                       mEndValue == pQuestion.mEndValue &&
                       mStartValueCaption == pQuestion.mStartValueCaption &&
                       mEndValueCaption == pQuestion.mEndValueCaption;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public override int GetHashCode()
        {
            try
            {
                int hashCode = 850309230;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + mStartValue.GetHashCode();
                hashCode = hashCode * -1521134295 + mEndValue.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mStartValueCaption);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mEndValueCaption);
                return Math.Abs(hashCode);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return int.MinValue;
            }
        }
        #endregion
    }
}
