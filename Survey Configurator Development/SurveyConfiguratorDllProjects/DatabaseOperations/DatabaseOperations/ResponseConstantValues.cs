using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOperations
{
    public static class ResponseConstantValues
    {
        public const int cDEFAULT_STATUS_CODE = 0;
        public const int cSUCCESS_STATUS_CODE = 1;
        public const int cFAIL_STATUS_CODE = -1;
        public const int cGENERAL_ERROR_STATUS_CODE = -2;
        public const string cINSERT_SUCCESS_MESSAGE = "Question inserted successfully";
        public const string cINSERT_FAIL_MESSAGE = "Question has not been inserted";
        public const string cINSERT_ERROR_MESSAGE = "An error occurred while inserting question";
        public const string cUPDATE_SUCCESS_MESSAGE = "Question updated successfully";
        public const string cUPDATE_FAIL_MESSAGE = "Question has not been updated";
        public const string cUPDATE_ERROR_MESSAGE = "An error occurred while updating question";
        public const string cDELETE_SUCCESS_MESSAGE = "Question deleted successfully";
        public const string cDELETE_FAIL_MESSAGE = "Question has not been deleted";
        public const string cDELETE_ERROR_MESSAGE = "An error occurred while deleting question";
    }
}
