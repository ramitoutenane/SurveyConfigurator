using System;
using System.Collections.Generic;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Abstract class to represent General question with required attributes
    /// </summary>
    public abstract class BaseQuestion : IEquatable<BaseQuestion>
    {
        #region Variable deceleration
        private string mText;
        private int mOrder;
        private int mId;
        public readonly QuestionType Type;
        #endregion
        #region Constructor
        /// <summary>
        /// Question constructor to initialize common data between question types
        /// </summary>
        /// <param name="pText">Question text</param>
        /// <param name="pOrder">Question order</param>
        /// <param name="pType">Question type</param>
        /// <param name="pId">Question id</param>
        protected BaseQuestion(string pText, int pOrder, QuestionType pType, int pId)
        {
            try
            {
                Text = pText;
                Order = pOrder;
                Type = pType;
                mId = pId;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Properties definition
        public string Text
        {
            get => mText;
            set { mText = value; }
        }

        public int Order
        {
            get => mOrder;
            set { mOrder = value; }
        }

        public int Id { get => mId; }
        public string TypeString { get => Type.ToString(); }
        #endregion
        #region Methods
        public override string ToString() => $"Id: {Id}\nType: {TypeString}\nQuestion: {Text}\nOrder: {Order}";
        /// <summary>
        /// Check if question is valid
        /// </summary>
        /// <returns>true if valid , false otherwise</returns>
        public virtual bool IsValid()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Text) || Text.Length > QuestionValidationValues.cQUESTION_TEXT_LENGTH)
                    return false;
                if (Order < QuestionValidationValues.cQUESTION_ORDER_MIN)
                    return false;
                return true;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Change current question id
        /// </summary>
        /// <param name="pId">the new id</param>
        public void ChangeId(int pId) => mId = pId;
        /// <summary>
        /// Get a copy of the current question with new id
        /// </summary>
        /// <param name="pId">the new id</param>
        /// <returns></returns>
        public abstract BaseQuestion CopyWithNewId(int pId);

        public override bool Equals(object pObj)
        {
            return Equals(pObj as BaseQuestion);
        }

        public bool Equals(BaseQuestion pOther)
        {
            return pOther != null &&
                mId == pOther.mId &&
                mText == pOther.mText &&
                mOrder == pOther.mOrder &&
                Type == pOther.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = 124979793;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mText);
            hashCode = hashCode * -1521134295 + mOrder.GetHashCode();
            hashCode = hashCode * -1521134295 + mId.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}
