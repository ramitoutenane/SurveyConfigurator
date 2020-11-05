using System;


namespace SurveyConfiguratorApp
{
    public class QuestionInsertException : QuestionOperationException
    {
        public QuestionInsertException() : base("An Error Occurred While Inserting Question.") { }
        public QuestionInsertException(string message) : base(message) {}
        public QuestionInsertException(string message, QuestionOperationException inner) : base(message, inner) { }
    }
}
