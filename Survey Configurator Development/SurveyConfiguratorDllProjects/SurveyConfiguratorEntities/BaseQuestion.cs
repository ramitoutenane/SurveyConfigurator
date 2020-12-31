using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
        public BaseQuestion(QuestionType pType)
        {
            Type = pType;
            Order = QuestionValidationValues.cQUESTION_ORDER_MIN;
        }
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
        [Required(ErrorMessageResourceType =typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_QUESTION_ERROR")]
        [MaxLength(QuestionValidationValues.cQUESTION_TEXT_LENGTH,ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "QUESTION_LENGTH_ERROR")]
        [DataType(DataType.MultilineText)]
        public string Text
        {
            get => mText;
            set {mText = value;}
        }
        [Required(ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "EMPTY_ORDER_ERROR")]
        [Range(QuestionValidationValues.cQUESTION_ORDER_MIN,int.MaxValue,ErrorMessageResourceType = typeof(Properties.ValidationMessages), ErrorMessageResourceName = "ORDER_ERROR")]
        public int Order
        {
            get => mOrder;
            set { mOrder = value; }
        }

        public int Id { get => mId; set { mId = value; } }
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

        public override bool Equals(object pObject)
        {
            try
            {
                return Equals(pObject as BaseQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public bool Equals(BaseQuestion pQuestion)
        {
            try
            {
                return pQuestion != null &&
                    mId == pQuestion.mId &&
                    mText == pQuestion.mText &&
                    mOrder == pQuestion.mOrder &&
                    Type == pQuestion.Type;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }

        public override int GetHashCode()
        {
            try { 
            int hashCode = 124979793;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mText);
            hashCode = hashCode * -1521134295 + mOrder.GetHashCode();
            hashCode = hashCode * -1521134295 + mId.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
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
