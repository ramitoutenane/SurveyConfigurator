using System;
using System.Drawing;
using System.Windows.Forms;

namespace SurveyConfiguratorApp
{
    public partial class QuestionProperties : Form
    {
        public Question question { get; private set; }
        /// <summary>
        /// QuestionProperties form constructor to initialize new QuestionProperties form
        /// </summary>
        public QuestionProperties()
        {
            try
            {
                InitializeComponent();
                PopulateQuestionTypes();
                typeComboBox.SelectedIndex = 0;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// QuestionProperties form constructor to initialize QuestionProperties form and populate with question data
        /// </summary>
        /// <param name="question">question to populate form with it's data</param>
        public QuestionProperties(Question question) : this()
        {
            try
            {
                if (question != null)
                {
                    // populate components with general question data
                    this.question = question;
                    questionTextBox.Text = question.Text;
                    orderNumericUpDown.Value = question.Order;
                    typeComboBox.SelectedIndex = (int)question.Type - 1;
                    typeComboBox.Enabled = false;
                    //populate components with specific question type data
                    if (question is SliderQuestion tSlider)
                    {
                        startCaptionTextBox.Text = tSlider.StartValueCaption;
                        startValueNumericUpDown.Value = tSlider.StartValue;
                        endCaptionTextBox.Text = tSlider.EndValueCaption;
                        endValueNumericUpDown.Value = tSlider.EndValue;
                    }
                    else if (question is SmileyQuestion tSmiley)
                    {
                        smileyNumericUpDown.Value = tSmiley.NumberOfFaces;
                    }
                    else if (question is StarsQuestion tStars)
                    {
                        starsNumericUpDown.Value = tStars.NumberOfStars;
                    }
                }
                else
                {
                    throw new ArgumentException(MessageStringValues.cQUESTION_TYPE_Exception);
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// QuestionProperties form Load event handler
        /// </summary>
        private void QuestionProperties_Load(object sender, EventArgs e)
        {
            try
            {
                //set numericUpDown controls maximum to max int value
                orderNumericUpDown.Maximum = int.MaxValue;
                startValueNumericUpDown.Maximum = int.MaxValue;
                endValueNumericUpDown.Maximum = int.MaxValue;
                smileyNumericUpDown.Maximum = int.MaxValue;
                starsNumericUpDown.Maximum = int.MaxValue;

                // set numericUpDown controls minimum to min int value
                orderNumericUpDown.Minimum = int.MinValue;
                startValueNumericUpDown.Minimum = int.MinValue;
                endValueNumericUpDown.Minimum = int.MinValue;
                smileyNumericUpDown.Minimum = int.MinValue;
                starsNumericUpDown.Minimum = int.MinValue;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// typeComboBox change selection event handler
        /// </summary>
        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //hide all group boxes of question types
                sliderGroupBox.Visible = false;
                smileyGroupBox.Visible = false;
                starsGroupBox.Visible = false;
                //question group box location point
                Point groupBoxLocation = new Point(10, 230);
                //check selected question and show it's group box
                switch (typeComboBox.SelectedIndex)
                {
                    case 0:
                        smileyGroupBox.Visible = true;
                        smileyGroupBox.Location = groupBoxLocation;
                        break;
                    case 1:
                        sliderGroupBox.Visible = true;
                        sliderGroupBox.Location = groupBoxLocation;
                        break;
                    case 2:
                        starsGroupBox.Visible = true;
                        starsGroupBox.Location = groupBoxLocation;
                        break;
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// questionTextBox text change event handler
        /// </summary>
        private void questionTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //update question length character counter
                currentCharCount.Text = questionTextBox.Text.Length.ToString();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        /// <summary>
        /// saveButton Click event handler
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                //validate the properties
                if (IsValidQuestion())
                {
                    string questionText = questionTextBox.Text.TrimEnd();
                    int questionOrder = (int)orderNumericUpDown.Value;
                    int id = -1;
                    //if question already exist (update form), get a copy of it's id
                    //if (add form) question id is -1, if update form question id != -1
                    if (question != null && question.Id > 0)
                    {
                        id = question.Id;
                    }
                    //based on selected question type create new question, and save it to question public reference
                    switch (typeComboBox.SelectedIndex)
                    {
                        case 0:
                            question = new SmileyQuestion(questionText, questionOrder, (int)smileyNumericUpDown.Value, id);
                            break;
                        case 1:
                            question = new SliderQuestion(questionText, questionOrder, (int)startValueNumericUpDown.Value,
                                (int)endValueNumericUpDown.Value, startCaptionTextBox.Text.TrimEnd(), endCaptionTextBox.Text.TrimEnd(), id);
                            break;
                        case 2:
                            question = new StarsQuestion(questionText, questionOrder, (int)starsNumericUpDown.Value, id);
                            break;
                    }
                    //return OK result to parent form
                    DialogResult = DialogResult.OK;

                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        /// <summary>
        /// numericUpDown validation event handler
        /// </summary>
        private void numericUpDown_Validation(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //check which numericUpDown triggered the event
                //validate it's value, if invalid change it to valid state and show error message to user
                if (sender == orderNumericUpDown)
                {
                    if (orderNumericUpDown.Value < QuestionValidationValues.cQUESTION_ORDER_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_ORDER_ERROR} {QuestionValidationValues.cQUESTION_ORDER_MIN}");
                        orderNumericUpDown.Value = QuestionValidationValues.cQUESTION_ORDER_MIN;
                    }
                }
                else if (sender == startValueNumericUpDown)
                {
                    if (startValueNumericUpDown.Value < QuestionValidationValues.cSTART_VALUE_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_START_VALUE_ERROR} {QuestionValidationValues.cSTART_VALUE_MIN}");
                        orderNumericUpDown.Value = QuestionValidationValues.cSTART_VALUE_MIN;
                    }
                }
                else if (sender == endValueNumericUpDown)
                {
                    if (endValueNumericUpDown.Value > QuestionValidationValues.cEND_VALUE_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_END_VALUE_ERROR} {QuestionValidationValues.cEND_VALUE_MAX}");
                        orderNumericUpDown.Value = QuestionValidationValues.cEND_VALUE_MAX;
                    }
                }
                else if (sender == starsNumericUpDown)
                {
                    if (starsNumericUpDown.Value < QuestionValidationValues.cSTARS_NUMBER_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_STARS_NUMBER_ERROR}{QuestionValidationValues.cSTARS_NUMBER_MIN}");
                        starsNumericUpDown.Value = QuestionValidationValues.cSTARS_NUMBER_MIN;
                    }
                    if (starsNumericUpDown.Value > QuestionValidationValues.cSTARS_NUMBER_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_STARS_NUMBER_ERROR} {QuestionValidationValues.cSTARS_NUMBER_MAX}");
                        starsNumericUpDown.Value = QuestionValidationValues.cSTARS_NUMBER_MAX;
                    }
                }
                else if (sender == smileyNumericUpDown)
                {
                    if (smileyNumericUpDown.Value < QuestionValidationValues.cFACES_NUMBER_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MIN}");
                        smileyNumericUpDown.Value = QuestionValidationValues.cFACES_NUMBER_MIN;
                    }
                    if (smileyNumericUpDown.Value > QuestionValidationValues.cFACES_NUMBER_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MAX}");
                        smileyNumericUpDown.Value = QuestionValidationValues.cFACES_NUMBER_MAX;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Populate QuestionTypes combo box with question types from string resources based on form language
        /// </summary>
        private void PopulateQuestionTypes()
        {
            try
            {
                typeComboBox.Items.Add(Properties.StringResources.QUESTION_TYPE_SMILEY);
                typeComboBox.Items.Add(Properties.StringResources.QUESTION_TYPE_SLIDER);
                typeComboBox.Items.Add(Properties.StringResources.QUESTION_TYPE_STARS);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Check if all question properties are valid based on question type
        /// </summary>
        /// <returns>true if all properties are valid, false otherwise</returns>
        private bool IsValidQuestion()
        {
            try
            {
                //check general question properties
                if (questionTextBox.Text.TrimEnd().Length == 0)
                {
                    Main.ShowError(Properties.StringResources.EMPTY_QUESTION_ERROR);
                    questionTextBox.Focus();
                    return false;
                }
                if (orderNumericUpDown.Value < QuestionValidationValues.cQUESTION_ORDER_MIN)
                {
                    Main.ShowError($"{Properties.StringResources.MIN_ORDER_ERROR} {QuestionValidationValues.cQUESTION_ORDER_MIN}");
                    questionTextBox.Focus();
                    return false;
                }
                //check the type of question and it's properties based on question type selection
                if (typeComboBox.SelectedIndex == (int)QuestionType.Slider - 1)
                {
                    if (startCaptionTextBox.Text.TrimEnd().Length == 0)
                    {
                        Main.ShowError(Properties.StringResources.EMPTY_START_CAPTION);
                        startCaptionTextBox.Focus();
                        return false;
                    }
                    if (endCaptionTextBox.Text.TrimEnd().Length == 0)
                    {
                        Main.ShowError(Properties.StringResources.EMPTY_END_CAPTION);
                        endCaptionTextBox.Focus();
                        return false;
                    }
                    if (startValueNumericUpDown.Value < QuestionValidationValues.cSTART_VALUE_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_START_VALUE_ERROR} {QuestionValidationValues.cSTART_VALUE_MIN}");
                        startValueNumericUpDown.Focus();
                        return false;
                    }
                    if (endValueNumericUpDown.Value > QuestionValidationValues.cEND_VALUE_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_END_VALUE_ERROR} {QuestionValidationValues.cEND_VALUE_MAX}");
                        endValueNumericUpDown.Focus();
                        return false;
                    }
                    if (startValueNumericUpDown.Value >= endValueNumericUpDown.Value)
                    {
                        Main.ShowError(Properties.StringResources.START_LARGER_THAN_END_ERROR);
                        startValueNumericUpDown.Focus();
                        return false;
                    }
                }
                else if (typeComboBox.SelectedIndex == (int)QuestionType.Stars - 1)
                {
                    if (starsNumericUpDown.Value < QuestionValidationValues.cSTARS_NUMBER_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_STARS_NUMBER_ERROR}{QuestionValidationValues.cSTARS_NUMBER_MIN}");
                        starsNumericUpDown.Focus();
                        return false;
                    }
                    if (starsNumericUpDown.Value > QuestionValidationValues.cSTARS_NUMBER_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_STARS_NUMBER_ERROR} {QuestionValidationValues.cSTARS_NUMBER_MAX}");
                        starsNumericUpDown.Focus();
                        return false;
                    }
                }
                else if (typeComboBox.SelectedIndex == (int)QuestionType.Smiley - 1)
                {
                    if (smileyNumericUpDown.Value < QuestionValidationValues.cFACES_NUMBER_MIN)
                    {
                        Main.ShowError($"{Properties.StringResources.MIN_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MIN}");
                        smileyNumericUpDown.Focus();
                        return false;
                    }
                    if (smileyNumericUpDown.Value > QuestionValidationValues.cFACES_NUMBER_MAX)
                    {
                        Main.ShowError($"{Properties.StringResources.MAX_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MAX}");
                        smileyNumericUpDown.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
    }
}
