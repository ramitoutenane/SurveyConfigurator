namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Constant strings to be used to print messages to user
    /// </summary>
    public static class ErrorMessages
    {
        public const string cGENERAL_ERROR = "An Error occurred, Please try again or contact Admin";
        public const string cCONNECTION_ERROR = "Connection to database is not available";
        public const string cREFRESH_ERROR = "An Error occurred while refreshing list";
        public const string cINSERT_ERROR = "An Error occurred while inserting question";
        public const string cUPDATE_ERROR = "An Error occurred while updating question";
        public const string cDELETE_ERROR = "An Error occurred while deleting question";
        public const string cSELECT_ERROR = "An Error occurred while selecting question";
        public const string cSORT_ERROR = "An Error occurred while sorting list";
        public const string cERROR_BOX_TITLE = "Error";
        public const string cNO_QUESTION_ID = "Couldn't find a Question with given id";
        public const string cNO_QUESTION_SELECTED = "No question is Selected";
        public const string cQUESTION_VALIDATION_EXCEPTION = "Invalid Question attributes";
        public const string cQUESTION_TYPE_EXCEPTION = "Question type is not recognized";
        public const string cQUESTION_NULL_EXCEPTION = "Given Question is Null";
        public const string cCONNECTION_STRING_NULL_EXCEPTION = "Connection String is null";
        public const string cQUESTION_MANAGER_NULL_EXCEPTION = "Question manager reference is null";
    }
}
