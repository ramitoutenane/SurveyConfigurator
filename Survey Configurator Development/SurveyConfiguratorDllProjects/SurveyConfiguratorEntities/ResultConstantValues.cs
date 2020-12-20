
namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Constant Values to be used in creating response objects
    /// </summary>
    public static class ResultConstantValues
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
        public const string cREFRESH_SUCCESS_MESSAGE = "Question list refreshed successfully";
        public const string cREFRESH_FAIL_MESSAGE = "Question list has not been refreshed";
        public const string cREFRESH_ERROR_MESSAGE = "An error occurred while refreshing question list";
        public const string cREAD_SUCCESS_MESSAGE = "Question list refreshed successfully";
        public const string cREAD_FAIL_MESSAGE = "Failed to read question";
        public const string cREAD_NOT_FOUND_MESSAGE = "Question not found";
        public const string cREAD_ERROR_MESSAGE = "An error occurred while reading question";
    }
}
