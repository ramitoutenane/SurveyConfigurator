using QuestionManaging;
using SurveyConfiguratorEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SurveyConfiguratorApp
{
    public partial class QuestionProperties : Form
    {
        #region QuestionTypeTranslation inner class 
        /// <summary>
        /// private inner class to connect question Type with it's translated name
        /// </summary>
        private class QuestionTypeTranslation
        {
            public readonly QuestionType Type;
            public readonly string TypeName;

            public QuestionTypeTranslation(QuestionType pType, string pTypeName)
            {
                try
                {
                    Type = pType;
                    TypeName = pTypeName;
                }catch(Exception pError)
                {
                    ErrorLogger.Log(pError);
                }
            }

            public override string ToString() => TypeName;

        }
        #endregion
        #region Initialize QuestionProperties form
        private readonly QuestionManager mQuestionManager;
        private BaseQuestion mQuestion;
        private List<QuestionTypeTranslation> mQuestionTypeList;
        /// <summary>
        /// QuestionProperties form constructor to initialize new QuestionProperties form
        /// </summary>
        public QuestionProperties(QuestionManager pQuestionManager)
        {
            try
            {
                InitializeComponent();
                mQuestionManager = pQuestionManager;

                //populate typeComboBox
                mQuestionTypeList = new List<QuestionTypeTranslation>()
                {
                    new QuestionTypeTranslation(QuestionType.Smiley,Properties.StringResources.QUESTION_TYPE_SMILEY),
                    new QuestionTypeTranslation(QuestionType.Slider,Properties.StringResources.QUESTION_TYPE_SLIDER),
                    new QuestionTypeTranslation(QuestionType.Stars,Properties.StringResources.QUESTION_TYPE_STARS)
                };
                typeComboBox.DataSource = mQuestionTypeList;
                typeComboBox.SelectedIndex = 0;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// QuestionProperties form constructor to initialize QuestionProperties form and populate with question data
        /// </summary>
        /// <param name="pQuestion">question to populate form with it's data</param>
        public QuestionProperties(QuestionManager pQuestionManager, BaseQuestion pQuestion) : this(pQuestionManager)
        {
            try
            {
                if (pQuestion != null)
                {
                    //populate components with general question data
                    mQuestion = pQuestion;
                    questionTextBox.Text = pQuestion.Text;
                    orderNumericUpDown.Value = pQuestion.Order;
                    typeComboBox.SelectedItem = mQuestionTypeList.Find(tQuestion => tQuestion.Type == pQuestion.Type);
                    typeComboBox.Enabled = false;
                    //populate components with specific question type data
                    switch (pQuestion.Type)
                    {
                        case QuestionType.Slider:
                            SliderQuestion tSlider = pQuestion as SliderQuestion;
                            startCaptionTextBox.Text = tSlider.StartValueCaption;
                            startValueNumericUpDown.Value = tSlider.StartValue;
                            endCaptionTextBox.Text = tSlider.EndValueCaption;
                            endValueNumericUpDown.Value = tSlider.EndValue;
                            break;
                        case QuestionType.Smiley:
                            SmileyQuestion tSmiley = pQuestion as SmileyQuestion;
                            smileyNumericUpDown.Value = tSmiley.NumberOfFaces;

                            break;
                        case QuestionType.Stars:
                            StarsQuestion tStars = pQuestion as StarsQuestion;
                            starsNumericUpDown.Value = tStars.NumberOfStars;
                            break;
                        default:
                            ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_TYPE_EXCEPTION));
                            ShowError(Properties.StringResources.GENERAL_ERROR);
                            return;
                    }
                }
                else
                {
                    ErrorLogger.Log(new ArgumentException(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                    ShowError(Properties.StringResources.GENERAL_ERROR);
                    return;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        #endregion
        #region Controls event handling
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
                QuestionTypeTranslation tSelectedQuestion = typeComboBox.SelectedItem as QuestionTypeTranslation;
                switch (tSelectedQuestion.Type)
                {
                    case QuestionType.Smiley:
                        smileyGroupBox.Visible = true;
                        smileyGroupBox.Location = tGroupBoxLocation;
                        break;
                    case QuestionType.Slider:
                        sliderGroupBox.Visible = true;
                        sliderGroupBox.Location = tGroupBoxLocation;

                        break;
                    case QuestionType.Stars:
                        starsGroupBox.Visible = true;
                        starsGroupBox.Location = tGroupBoxLocation;

                        break;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                bool tSaved = false;
                //validate the properties
                if (IsValidQuestion())
                {
                    string tQuestionText = questionTextBox.Text.TrimEnd();
                    int tQuestionOrder = (int)orderNumericUpDown.Value;
                    int tId = -1;
                    //if question already exist (update form), get a copy of it's id
                    //if (add form) question id is -1, if update form question id != -1
                    if (mQuestion != null && mQuestion.Id > 0)
                    {
                        tId = mQuestion.Id;
                    }
                    //based on selected question type create new question, and save it to question public reference
                    QuestionTypeTranslation tSelectedQuestion = typeComboBox.SelectedItem as QuestionTypeTranslation;
                    switch (tSelectedQuestion.Type)
                    {
                        case QuestionType.Smiley:
                            mQuestion = new SmileyQuestion(tQuestionText, tQuestionOrder, (int)smileyNumericUpDown.Value, tId);
                            break;
                        case QuestionType.Slider:
                            mQuestion = new SliderQuestion(tQuestionText, tQuestionOrder, (int)startValueNumericUpDown.Value,
                                    (int)endValueNumericUpDown.Value, startCaptionTextBox.Text.TrimEnd(), endCaptionTextBox.Text.TrimEnd(), tId);
                            break;
                        case QuestionType.Stars:
                            mQuestion = new StarsQuestion(tQuestionText, tQuestionOrder, (int)starsNumericUpDown.Value, tId);
                            break;
                    }

                    //if question is not null check it's id to determine whether to insert it or update it
                    if (mQuestion == null)
                    {
                        ErrorLogger.Log(new Exception(ErrorMessages.cQUESTION_NULL_EXCEPTION));
                        ShowError(Properties.StringResources.GENERAL_ERROR);
                        return;

                    }
                    Cursor.Current = Cursors.WaitCursor;
                    if (mQuestion.Id >= 0)
                        tSaved = UpdateQuestion(mQuestion);
                    else
                        tSaved = InsertQuestion(mQuestion);
                    Cursor.Current = Cursors.Default;

                }
                if (tSaved)
                    DialogResult = DialogResult.OK;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
        #region NumericUpDown validation
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                        startValueNumericUpDown.Value = QuestionValidationValues.cSTART_VALUE_MIN;
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                        endValueNumericUpDown.Value = QuestionValidationValues.cEND_VALUE_MAX;
                    }
                    if (endValueNumericUpDown.Value <= startValueNumericUpDown.Value)
                    {
                        ShowError(Properties.StringResources.START_LARGER_THAN_END_ERROR);
                        endValueNumericUpDown.Value = startValueNumericUpDown.Value + 1;
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Question validation
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
                QuestionTypeTranslation tSelectedQuestion = typeComboBox.SelectedItem as QuestionTypeTranslation;
                switch (tSelectedQuestion.Type)
                {
                    case QuestionType.Smiley:
                        return IsValidSmileyQuestion();
                    case QuestionType.Slider:
                        return IsValidSliderQuestion();
                    case QuestionType.Stars:
                        return IsValidStarsQuestion();
                    default:
                        return false;
                }

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
                return false;
            }
        }
        #endregion
        #region Helper methods
        /// <summary>
        /// Show custom Error message box to user
        /// </summary>
        /// <param name="pErrorMessage">pError message to be shown to user</param>
        public static void ShowError(string pErrorMessage)
        {
            try
            {
                MessageBox.Show(pErrorMessage, Properties.StringResources.ERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        /// <summary>
        /// Insert question into repository
        /// </summary>
        /// <param name="pQuestion">The question to be inserted</param>
        /// <returns>true if inserted, false otherwise</returns>
        private bool InsertQuestion(BaseQuestion pQuestion)
        {
            try
            {
                if (!mQuestionManager.IsConnected())
                {
                    ErrorLogger.Log(ErrorMessages.cCONNECTION_ERROR, new StackTrace());
                    ShowError(Properties.StringResources.CONNECTION_ERROR);
                    return false;
                }
                if (mQuestionManager.Create(pQuestion).Value == ResultValue.Success)
                    return true;
                else
                {
                    ShowError(Properties.StringResources.INSERT_ERROR);
                    return false;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        /// <summary>
        /// Update question in repository
        /// </summary>
        /// <param name="pQuestion">The question to be updated</param>
        /// <returns>true if updated, false otherwise</returns>
        private bool UpdateQuestion(BaseQuestion pQuestion)
        {
            try
            {
                if (!mQuestionManager.IsConnected())
                {
                    ErrorLogger.Log(ErrorMessages.cCONNECTION_ERROR, new StackTrace());
                    ShowError(Properties.StringResources.CONNECTION_ERROR);
                    return false;
                }
                if (mQuestionManager.Update(pQuestion).Value == ResultValue.Success)
                    return true;
                else
                {
                    ShowError(Properties.StringResources.UPDATE_ERROR);
                    return false;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }
        }
        #endregion
    }
}
