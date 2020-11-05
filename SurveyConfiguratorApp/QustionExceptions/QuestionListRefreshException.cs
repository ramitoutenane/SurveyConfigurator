using System;


namespace SurveyConfiguratorApp
{
    public class QuestionListRefreshException : QuestionOperationException
    {
        public QuestionListRefreshException() : base("An Error Occurred While Refreshing Questions List.") { }
        public QuestionListRefreshException(string message) : base(message) {}
        public QuestionListRefreshException(string message, QuestionOperationException inner) : base(message, inner) { }
    }
}
