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
            InitializeComponent();
            typeComboBox.DataSource = Enum.GetValues(typeof(QuestionType));
        }
        public QuestionProperties(Question question) : this()
        {
            if (question != null)
            {
                this.question = question;
                questionTextBox.Text = question.Text;
                orderNumericUpDown.Value = question.Order;
                typeComboBox.SelectedIndex = (int)question.Type - 1;
                typeComboBox.Enabled = false;

                if (question is SliderQuestion slider)
                {
                    startCaptionTextBox.Text = slider.StartValueCaption;
                    startValueNumericUpDown.Value = slider.StartValue;
                    endCaptionTextBox.Text = slider.EndValueCaption;
                    endValueNumericUpDown.Value = slider.EndValue;
                }
                else if (question is SmileyQuestion smiley)
                {
                    smileyNumericUpDown.Value = smiley.NumberOfFaces;
                }
                else if (question is StarsQuestion stars)
                {
                    starsNumericUpDown.Value = stars.NumberOfStars;
                }
            }
            else
            {
                Main.showError("Invalid Question Type");
            }
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

        private void startValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            endValueNumericUpDown.Minimum = startValueNumericUpDown.Value + 1;
        }

        private void endValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            startValueNumericUpDown.Maximum = endValueNumericUpDown.Value - 1;
        }

        private void questionTextBox_TextChanged(object sender, EventArgs e)
        {
            currentCharCount.Text = questionTextBox.Text.Length.ToString();
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
            if (isValidQuestion())
            {
                string questionText = questionTextBox.Text.TrimEnd();
                int questionOrder = (int)orderNumericUpDown.Value;
                int id = -1;
                if (question != null && question.Id > 0)
                {
                    id = question.Id;
                }
                try
                {
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
                catch (Exception ex)
                {
                    Main.showError(ex.Message);
                }
            }
        }
    }
}
