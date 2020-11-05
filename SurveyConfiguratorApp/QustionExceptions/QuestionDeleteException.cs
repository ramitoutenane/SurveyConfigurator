using System;


namespace SurveyConfiguratorApp
{
    public class QuestionDeleteException : QuestionOperationException
    {
        public QuestionDeleteException() : base("An Error Occurred While Deleting Question.") { }
        public QuestionDeleteException(string message) : base(message) {}
        public QuestionDeleteException(string message, QuestionOperationException inner) : base(message, inner) { }
    }
}
