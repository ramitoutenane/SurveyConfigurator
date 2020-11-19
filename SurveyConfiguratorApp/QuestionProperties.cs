using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SurveyConfiguratorApp
{
    public partial class QuestionProperties : Form
    {
        private Dictionary<QuestionType, string> mQuestionTypeResources;
        public Question question { get; private set; }
        /// <summary>
        /// QuestionProperties form constructor to initialize new QuestionProperties form
        /// </summary>
        public QuestionProperties()
        {
            try
            {
                InitializeComponent();
                //initialize dictionary of question type and it's string resource to use as ComboBox DataSource
                mQuestionTypeResources = new Dictionary<QuestionType, string>
                {
                    {QuestionType.Smiley,Properties.StringResources.QUESTION_TYPE_SMILEY},
                    {QuestionType.Slider,Properties.StringResources.QUESTION_TYPE_SLIDER},
                    {QuestionType.Stars,Properties.StringResources.QUESTION_TYPE_STARS}
                };
                //populate typeComboBox
                typeComboBox.DataSource = new List<string>(mQuestionTypeResources.Values);
                typeComboBox.SelectedIndex = 0;

            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
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
                    //populate components with general question data
                    this.question = question;
                    questionTextBox.Text = question.Text;
                    orderNumericUpDown.Value = question.Order;
                    typeComboBox.SelectedItem = mQuestionTypeResources[question.Type];
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
                    throw new ArgumentException(MessageStringValues.cQUESTION_TYPE_EXCEPTION);
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
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
                ShowError(Properties.StringResources.GENERAL_ERROR);
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
                Point tGroupBoxLocation = new Point(10, 230);
                //check selected question and show it's group box
                string tSelectedQuestion = (string)typeComboBox.SelectedItem;
                if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Smiley])

                {
                    smileyGroupBox.Visible = true;
                    smileyGroupBox.Location = tGroupBoxLocation;
                }
                else if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Slider])
                {
                    sliderGroupBox.Visible = true;
                    sliderGroupBox.Location = tGroupBoxLocation;
                }
                else if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Stars])
                {
                    starsGroupBox.Visible = true;
                    starsGroupBox.Location = tGroupBoxLocation;
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
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
                ShowError(Properties.StringResources.GENERAL_ERROR);
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
                    string tQuestionText = questionTextBox.Text.TrimEnd();
                    int tQuestionOrder = (int)orderNumericUpDown.Value;
                    int tId = -1;
                    //if question already exist (update form), get a copy of it's id
                    //if (add form) question id is -1, if update form question id != -1
                    if (question != null && question.Id > 0)
                    {
                        tId = question.Id;
                    }
                    //based on selected question type create new question, and save it to question public reference
                    string tSelectedQuestion = (string)typeComboBox.SelectedItem;
                    if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Smiley])
                        question = new SmileyQuestion(tQuestionText, tQuestionOrder, (int)smileyNumericUpDown.Value, tId);

                    else if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Slider])
                        question = new SliderQuestion(tQuestionText, tQuestionOrder, (int)startValueNumericUpDown.Value,
                                (int)endValueNumericUpDown.Value, startCaptionTextBox.Text.TrimEnd(), endCaptionTextBox.Text.TrimEnd(), tId);

                    else if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Stars])
                        question = new StarsQuestion(tQuestionText, tQuestionOrder, (int)starsNumericUpDown.Value, tId);

                }
                //return OK result to parent form
                DialogResult = DialogResult.OK;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        /// <summary>
        /// orderNumericUpDown validation event handler
        /// </summary>
        private void orderNumericUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender == orderNumericUpDown)
                {
                    if (orderNumericUpDown.Value < QuestionValidationValues.cQUESTION_ORDER_MIN)
                    {
                        ShowError($"{Properties.StringResources.MIN_ORDER_ERROR} {QuestionValidationValues.cQUESTION_ORDER_MIN}");
                        orderNumericUpDown.Value = QuestionValidationValues.cQUESTION_ORDER_MIN;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// startValueNumericUpDown validation event handler
        /// </summary>
        private void startValueNumericUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender == startValueNumericUpDown)
                {
                    if (startValueNumericUpDown.Value < QuestionValidationValues.cSTART_VALUE_MIN)
                    {
                        ShowError($"{Properties.StringResources.MIN_START_VALUE_ERROR} {QuestionValidationValues.cSTART_VALUE_MIN}");
                        orderNumericUpDown.Value = QuestionValidationValues.cSTART_VALUE_MIN;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// endValueNumericUpDown validation event handler
        /// </summary>
        private void endValueNumericUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender == endValueNumericUpDown)
                {
                    if (endValueNumericUpDown.Value > QuestionValidationValues.cEND_VALUE_MAX)
                    {
                        ShowError($"{Properties.StringResources.MAX_END_VALUE_ERROR} {QuestionValidationValues.cEND_VALUE_MAX}");
                        orderNumericUpDown.Value = QuestionValidationValues.cEND_VALUE_MAX;
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// smileyNumericUpDown validation event handler
        /// </summary>
        private void smileyNumericUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender == smileyNumericUpDown)
                {
                    if (smileyNumericUpDown.Value < QuestionValidationValues.cFACES_NUMBER_MIN)
                    {
                        ShowError($"{Properties.StringResources.MIN_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MIN}");
                        smileyNumericUpDown.Value = QuestionValidationValues.cFACES_NUMBER_MIN;
                    }
                    if (smileyNumericUpDown.Value > QuestionValidationValues.cFACES_NUMBER_MAX)
                    {
                        ShowError($"{Properties.StringResources.MAX_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MAX}");
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
        /// starsNumericUpDown validation event handler
        /// </summary>
        private void starsNumericUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender == starsNumericUpDown)
                {
                    if (starsNumericUpDown.Value < QuestionValidationValues.cSTARS_NUMBER_MIN)
                    {
                        ShowError($"{Properties.StringResources.MIN_STARS_NUMBER_ERROR}{QuestionValidationValues.cSTARS_NUMBER_MIN}");
                        starsNumericUpDown.Value = QuestionValidationValues.cSTARS_NUMBER_MIN;
                    }
                    if (starsNumericUpDown.Value > QuestionValidationValues.cSTARS_NUMBER_MAX)
                    {
                        ShowError($"{Properties.StringResources.MAX_STARS_NUMBER_ERROR} {QuestionValidationValues.cSTARS_NUMBER_MAX}");
                        starsNumericUpDown.Value = QuestionValidationValues.cSTARS_NUMBER_MAX;
                    }
                }
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
                    ShowError(Properties.StringResources.EMPTY_QUESTION_ERROR);
                    questionTextBox.Focus();
                    return false;
                }
                if (orderNumericUpDown.Value < QuestionValidationValues.cQUESTION_ORDER_MIN)
                {
                    ShowError($"{Properties.StringResources.MIN_ORDER_ERROR} {QuestionValidationValues.cQUESTION_ORDER_MIN}");
                    questionTextBox.Focus();
                    return false;
                }
                //check the type of question and it's properties based on question type selection
                string tSelectedQuestion = (string)typeComboBox.SelectedItem;
                if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Slider])
                {
                    return IsValidSliderQuestion();
                }
                if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Stars])
                {
                    return IsValidStarsQuestion();
                }
                if (tSelectedQuestion == mQuestionTypeResources[QuestionType.Smiley])
                {
                    return IsValidSmileyQuestion();
                }
                return false;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
        /// <summary>
        /// Check if slider question properties are valid
        /// </summary>
        /// <returns>true if all properties are valid, false otherwise</returns>
        private bool IsValidSliderQuestion()
        {
            try
            {
                if (startCaptionTextBox.Text.TrimEnd().Length == 0)
                {
                    ShowError(Properties.StringResources.EMPTY_START_CAPTION);
                    startCaptionTextBox.Focus();
                    return false;
                }
                if (endCaptionTextBox.Text.TrimEnd().Length == 0)
                {
                    ShowError(Properties.StringResources.EMPTY_END_CAPTION);
                    endCaptionTextBox.Focus();
                    return false;
                }
                if (startValueNumericUpDown.Value < QuestionValidationValues.cSTART_VALUE_MIN)
                {
                    ShowError($"{Properties.StringResources.MIN_START_VALUE_ERROR} {QuestionValidationValues.cSTART_VALUE_MIN}");
                    startValueNumericUpDown.Focus();
                    return false;
                }
                if (endValueNumericUpDown.Value > QuestionValidationValues.cEND_VALUE_MAX)
                {
                    ShowError($"{Properties.StringResources.MAX_END_VALUE_ERROR} {QuestionValidationValues.cEND_VALUE_MAX}");
                    endValueNumericUpDown.Focus();
                    return false;
                }
                if (startValueNumericUpDown.Value >= endValueNumericUpDown.Value)
                {
                    ShowError(Properties.StringResources.START_LARGER_THAN_END_ERROR);
                    startValueNumericUpDown.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
        /// <summary>
        /// Check if stars question properties are valid
        /// </summary>
        /// <returns>true if all properties are valid, false otherwise</returns>
        private bool IsValidStarsQuestion()
        {
            try
            {
                if (starsNumericUpDown.Value < QuestionValidationValues.cSTARS_NUMBER_MIN)
                {
                    ShowError($"{Properties.StringResources.MIN_STARS_NUMBER_ERROR}{QuestionValidationValues.cSTARS_NUMBER_MIN}");
                    starsNumericUpDown.Focus();
                    return false;
                }
                if (starsNumericUpDown.Value > QuestionValidationValues.cSTARS_NUMBER_MAX)
                {
                    ShowError($"{Properties.StringResources.MAX_STARS_NUMBER_ERROR} {QuestionValidationValues.cSTARS_NUMBER_MAX}");
                    starsNumericUpDown.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
        /// <summary>
        /// Check if smiley question properties are valid
        /// </summary>
        /// <returns>true if all properties are valid, false otherwise</returns>
        private bool IsValidSmileyQuestion()
        {
            try
            {
                if (smileyNumericUpDown.Value < QuestionValidationValues.cFACES_NUMBER_MIN)
                {
                    ShowError($"{Properties.StringResources.MIN_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MIN}");
                    smileyNumericUpDown.Focus();
                    return false;
                }
                if (smileyNumericUpDown.Value > QuestionValidationValues.cFACES_NUMBER_MAX)
                {
                    ShowError($"{Properties.StringResources.MAX_FACES_NUMBER_ERROR} {QuestionValidationValues.cFACES_NUMBER_MAX}");
                    smileyNumericUpDown.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
        /// <summary>
        /// Show custom Error message box to user
        /// </summary>
        /// <param name="errorMessage">error message to be shown to user</param>
        public static void ShowError(string errorMessage)
        {
            try
            {
                MessageBox.Show(errorMessage, Properties.StringResources.ERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
    }
}
