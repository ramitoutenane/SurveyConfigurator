using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyConfiguratorApp
{
    public partial class QuestionProperties : Form
    {
        public QuestionProperties()
        {
            InitializeComponent();

        }
        public QuestionProperties(Question question):this()
        {
            MessageBox.Show($"Edit Form: question_id =  {question.ID}");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void QuestionProperties_Load(object sender, EventArgs e)
        {
            typeComboBox.DataSource = Enum.GetValues(typeof(QuestionType));
            typeComboBox.BackColor = Color.White;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
            questionTextBox.Text = questionTextBox.Text.TrimStart();
            questionTextBox.SelectionStart = questionTextBox.Text.Length;
            questionTextBox.SelectionLength = 0;
            currentCharCount.Text = questionTextBox.Text.Length.ToString();
        }
    }
}
