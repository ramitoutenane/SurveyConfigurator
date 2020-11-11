using System;


namespace SurveyConfiguratorApp
{
    public class QuestionSelectException : QuestionOperationException
    {
        public QuestionSelectException() : base("An Error Occurred While Selecting Question.") { }
        public QuestionSelectException(string message) : base(message) { }
        public QuestionSelectException(string message, QuestionOperationException inner) : base(message, inner) { }
    }
}
