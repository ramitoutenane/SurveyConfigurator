using System;
using System.ComponentModel.DataAnnotations;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Stars question class that extends general question and adds stars question properties 
    /// </summary>
    public class StarsQuestion : BaseQuestion
    {
        #region Variable deceleration
        private int nNumberOfStars;
        #endregion
        #region Constructor
        public StarsQuestion() : base(QuestionType.Stars) { }
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Properties definition
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_STARS_NUMBER_ERROR")]
        [Range(QuestionValidationValues.cSTARS_NUMBER_MIN, QuestionValidationValues.cSTARS_NUMBER_MAX, ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "STARS_NUMBER_ERROR")]
        public int NumberOfStars
        {
            get => nNumberOfStars;
            set { nNumberOfStars = value; }
        }
        #endregion
        #region Methods
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

        public override bool Equals(object pObject)
        {
            try
            {
                return Equals(pObject as StarsQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public bool Equals(StarsQuestion pQuestion)
        {
            try
            {
                return pQuestion != null &&
                       base.Equals(pQuestion) &&
                       nNumberOfStars == pQuestion.nNumberOfStars;
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
                int hashCode = 49537828;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + nNumberOfStars.GetHashCode();
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

