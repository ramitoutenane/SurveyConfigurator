using System;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Smiley question class that extends general question and adds smiley question properties 
    /// </summary>
    public class SmileyQuestion : BaseQuestion
    {
        #region Variable deceleration
        private int mNumberOfFaces;
        #endregion
        #region Constructor
        /// <summary>
        /// Smiley question constructor to initialize new smiley question
        /// </summary>
        /// <param name="pText">Question text</param>
        /// <param name="pOrder">Question order</param>
        /// <param name="pNumberOfFaces">Number of smiley faces on smiley question</param>
        /// <param name="pId">Question id</param>
        public SmileyQuestion(string pText, int pOrder, int pNumberOfFaces, int pId = -1) : base(pText, pOrder, QuestionType.Smiley, pId)
        {
            try
            {
                NumberOfFaces = pNumberOfFaces;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Properties definition
        public int NumberOfFaces
        {
            get => mNumberOfFaces;
            set { mNumberOfFaces = value; }
        }
        #endregion
        #region Methods
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                return new SmileyQuestion(Text, Order, NumberOfFaces, pId);
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
                return Equals(pObj as SmileyQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public bool Equals(SmileyQuestion pOther)
        {
            try
            {
                return pOther != null &&
                       base.Equals(pOther) &&
                       mNumberOfFaces == pOther.mNumberOfFaces;
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
                int hashCode = -1437452496;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + mNumberOfFaces.GetHashCode();
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

