using System;

namespace SurveyConfiguratorApp
{
    public class SmileyQuestion : Question
    {
        //Smiley faces question class that extends general question and adds Smiley faces question properties 

        private int numberOfFaces;
        public SmileyQuestion(string text, int order, int numberOfFaces, int id = -1) : base(text, order, QuestionType.Smiley, id)
        {
            NumberOfFaces = numberOfFaces;
        }
        public SmileyQuestion(SmileyQuestion other, int id) : this(other.Text, other.Order, other.NumberOfFaces, id) { }
        public int NumberOfFaces
        {
            get => numberOfFaces;
            set
            {
                if (value < 2 || value > 5)
                    throw new ArgumentOutOfRangeException("Number of Faces must be between 2 and 5");
                numberOfFaces = value;
            }
        }

        public override string ToString() => $"{base.ToString()}\nNumber of Faces: {NumberOfFaces}";
    }
}
