using System;
using System.Drawing;
using System.Windows.Forms;

namespace SurveyConfiguratorApp
{
    public partial class QuestionProperties : Form
    {
        public Question question { get; private set; }
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
        public QuestionProperties(Question question) : this()
        {
            try
            {
                if (question != null)
                {
                    this.question = question;
                    questionTextBox.Text = question.Text;
                    orderNumericUpDown.Value = question.Order;
                    typeComboBox.SelectedIndex = (int)question.Type - 1;
                    typeComboBox.Enabled = false;

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
        private void QuestionProperties_Load(object sender, EventArgs e)
        {
            try
            {
                orderNumericUpDown.Maximum = int.MaxValue;
                startValueNumericUpDown.Maximum = int.MaxValue;
                endValueNumericUpDown.Maximum = int.MaxValue;
                smileyNumericUpDown.Maximum = int.MaxValue;
                starsNumericUpDown.Maximum = int.MaxValue;

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
        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sliderGroupBox.Visible = false;
                smileyGroupBox.Visible = false;
                starsGroupBox.Visible = false;
                Point groupBoxLocation = new Point(10, 230);
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

        private void questionTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                currentCharCount.Text = questionTextBox.Text.Length.ToString();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        private bool IsValidQuestion()
        {
            try
            {
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidQuestion())
                {
                    string questionText = questionTextBox.Text.TrimEnd();
                    int questionOrder = (int)orderNumericUpDown.Value;
                    int id = -1;
                    if (question != null && question.Id > 0)
                    {
                        id = question.Id;
                    }

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
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        private void ValidateNumericUpDown(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
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
    }
}
