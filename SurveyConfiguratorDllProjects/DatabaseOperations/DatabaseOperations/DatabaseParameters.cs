namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Constant Values to be used to interact with database
    /// </summary>
    public static class DatabaseParameters
    {
        public const string cPARAMETER_QUESTION_ID = "@id";
        public const string cPARAMETER_QUESTION_TEXT = "@text";
        public const string cPARAMETER_QUESTION_TYPE = "@type";
        public const string cPARAMETER_QUESTION_ORDER = "@order";
        public const string cPARAMETER_QUESTION_START_VALUE = "@startValue";
        public const string cPARAMETER_QUESTION_END_VALUE = "@endValue";
        public const string cPARAMETER_QUESTION_START_CAPTION = "@startCaption";
        public const string cPARAMETER_QUESTION_END_CAPTION = "@endCaption";
        public const string cPARAMETER_QUESTION_FACES_NUMBER = "@numberOfFaces";
        public const string cPARAMETER_QUESTION_STARS_NUMBER = "@numberOfStars";
        public const string cTABLE_QUESTION = "Question";
        public const string cTABLE_SLIDER_QUESTION = "SliderQuestion";
        public const string cTABLE_SMILEY_QUESTION = "SmileyQuestion";
        public const string cTABLE_STARS_QUESTION = "StarQuestion";
        public const string cCOLUMN_QUESTION_ID = "QuestionId";
        public const string cCOLUMN_QUESTION_TEXT = "QuestionText";
        public const string cCOLUMN_QUESTION_ORDER = "QuestionOrder";
        public const string cCOLUMN_TYPE_ID = "TypeId";
        public const string cCOLUMN_FACES_NUMBER = "NumOfFaces";
        public const string cCOLUMN_STARS_NUMBER = "NumOfStars";
        public const string cCOLUMN_START_VALUE = "StartValue";
        public const string cCOLUMN_END_VALUE = "EndValue";
        public const string cCOLUMN_START_CAPTION = "StartValueCaption";
        public const string cCOLUMN_END_CAPTION = "EndValueCaption";
        public const string cOFFSET = "@offset";
        public const string cLIMIT = "@limit";

    }
}
