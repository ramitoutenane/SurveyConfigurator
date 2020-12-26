using System;

namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Class to return operation response and status
    /// </summary>
    public class Result
    {
        #region Variable deceleration
        private ResultValue mValue;
        private int mResultCode;
        private string mResultMessage;
        #endregion
        #region Constructors
        /// <summary>
        /// Response constructor to initialize new Response object
        /// </summary>
        /// <param name="pStatus">ResponseStatus value</param>
        /// <param name="pStatusCode">Response status code</param>
        /// <param name="pStatusMessage">Response status message</param>
        public Result(ResultValue pStatus, int pStatusCode, string pStatusMessage)
        {
            mValue = pStatus;
            mResultCode = pStatusCode;
            mResultMessage = pStatusMessage;
        }
        #endregion
        #region Properties definition
        public ResultValue Value { get => mValue;}
        public int ResultCode { get => mResultCode;}
        public string ResultMessage { get => mResultMessage;}
        #endregion
        #region Method
        public static Result DefaultResult()
        {
            try
            {
                return new Result(ResultValue.Default, ResultConstantValues.cDEFAULT_STATUS_CODE, "Default response");
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return null;
            }
}
        #endregion
    }
}
