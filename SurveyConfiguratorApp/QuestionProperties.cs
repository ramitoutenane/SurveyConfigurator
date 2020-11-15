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
                typeComboBox.DataSource = Enum.GetValues(typeof(QuestionType));
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.showError(MessageStringResources.cGENERAL_ERROR);
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
                    throw new ArgumentException(MessageStringResources.cQUESTION_TYPE_Exception);

                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                Main.showError(MessageStringResources.cGENERAL_ERROR);
            }
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sliderGroupBox.Visible = false;
                smileyGroupBox.Visible = false;
                starsGroupBox.Visible = false;
                Point groupBoxLocation = new Point(10, 195);
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
                Main.showError(MessageStringResources.cGENERAL_ERROR);
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
                showError(MessageStringResources.cGENERAL_ERROR);
    }

}
private bool isValidQuestion()
        {
            if (questionTextBox.Text.TrimEnd().Length == 0)
            {
                Main.showError("Question text can't be empty");
                questionTextBox.Focus();
                return false;
            }
            if (typeComboBox.SelectedIndex == 1)
            {
                if (startCaptionTextBox.Text.TrimEnd().Length == 0)
                {
                    Main.showError("Start caption text can't be empty");
                    startCaptionTextBox.Focus();
                    return false;
                }
                if (endCaptionTextBox.Text.TrimEnd().Length == 0)
                {
                    Main.showError("End caption text can't be empty");
                    endCaptionTextBox.Focus();
                    return false;
                }
            }
            return true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValidQuestion())
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
                Main.showError(MessageStringResources.cGENERAL_ERROR);
            }

        }
        public static void showError(string errorMessage)
        {
            try
            {
                MessageBox.Show(errorMessage, MessageStringResources.cERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
    }
}
