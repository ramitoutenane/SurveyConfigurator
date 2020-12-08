using System;

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
        /// <summary>
        /// Get a copy of the current question with new id
        /// </summary>
        /// <param name="id">the new id</param>
        /// <returns></returns>
        public override BaseQuestion CopyWithNewId(int id)
        {
            try
            {
                return new StarsQuestion(Text, Order, NumberOfStars, id);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
            }
        }

        public override bool Equals(object pObj)
        {
            try
            {
                return Equals(pObj as StarsQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public bool Equals(StarsQuestion pOther)
        {
            try
            {
                return pOther != null &&
                       base.Equals(pOther) &&
                       nNumberOfStars == pOther.nNumberOfStars;
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
                return hashCode;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return new Random().Next(int.MaxValue, int.MaxValue);
            }
        }
        #endregion
    }
}
