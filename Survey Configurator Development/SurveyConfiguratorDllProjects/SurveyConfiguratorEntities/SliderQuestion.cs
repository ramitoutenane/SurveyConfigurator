using System;
using System.Collections.Generic;

namespace SurveyConfiguratorApp
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
        /// <summary>
        /// Get a copy of the current question with new id
        /// </summary>
        /// <param name="pId">the new id</param>
        /// <returns></returns>
        public override BaseQuestion CopyWithNewId(int pId)
        {
            try
            {
                return new SliderQuestion(Text, Order, StartValue, EndValue, StartValueCaption, EndValueCaption, pId);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
            }
        }

        public override bool Equals(object pObj)
        {
            return Equals(pObj as SliderQuestion);
        }

        public bool Equals(SliderQuestion pOther)
        {
            return pOther != null &&
                   base.Equals(pOther) &&
                   mStartValue == pOther.mStartValue &&
                   mEndValue == pOther.mEndValue &&
                   mStartValueCaption == pOther.mStartValueCaption &&
                   mEndValueCaption == pOther.mEndValueCaption;
        }

        public override int GetHashCode()
        {
            int hashCode = 850309230;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + mStartValue.GetHashCode();
            hashCode = hashCode * -1521134295 + mEndValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mStartValueCaption);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mEndValueCaption);
            return hashCode;
        }
        #endregion
    }
}
