using System;


namespace SurveyConfiguratorApp
{
    public class QuestionUpdateException : QuestionOperationException
    {
        public QuestionUpdateException() : base("An Error Occurred While Updating Question.") { }
        public QuestionUpdateException(string message) : base(message) {}
        public QuestionUpdateException(string message, QuestionOperationException inner) : base(message, inner) { }
    }
}
