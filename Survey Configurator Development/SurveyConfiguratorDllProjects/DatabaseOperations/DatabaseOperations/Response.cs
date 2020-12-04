using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOperations
{
    /// <summary>
    /// Class to return operation response and status
    /// </summary>
    public class Response
    {
        #region Variable deceleration
        private ResponseStatus mStatus;
        private int mStatusCode;
        private string mStatusMessage;
        #endregion
        #region Constructors
        /// <summary>
        /// Response constructor to initialize new Response object
        /// </summary>
        /// <param name="pStatus">ResponseStatus value</param>
        /// <param name="pStatusCode">Response status code</param>
        /// <param name="pStatusMessage">Response status message</param>
        public Response(ResponseStatus pStatus, int pStatusCode, string pStatusMessage)
        {
            mStatus = pStatus;
            mStatusCode = pStatusCode;
            mStatusMessage = pStatusMessage;
        }
        #endregion
        #region Properties definition
        public ResponseStatus Status { get => mStatus;}
        public int StatusCode { get => mStatusCode;}
        public string StatusMessage { get => mStatusMessage;}
        #endregion
        #region Method
        public static Response DefaultResponse()
        {
            try
            {
                return new Response(ResponseStatus.Default, ResponseConstantValues.cDEFAULT_STATUS_CODE, "Default response");
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
