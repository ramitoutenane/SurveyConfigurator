using System;


namespace SurveyConfiguratorApp
{
    public class QuestionOperationException : Exception
    {
        public QuestionOperationException() : base("Question Opertion Error Occurred.") { }
        public QuestionOperationException(string message) : base(message) { }
        public QuestionOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
