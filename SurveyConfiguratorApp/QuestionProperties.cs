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
            MessageBox.Show(typeComboBox.SelectedIndex.ToString());
            sliderGroupBox.Visible = false;
            smileyGroupBox.Visible = false;
            starsGroupBox.Visible = false;

            switch (typeComboBox.SelectedIndex)
            {
                case 0:
                    smileyGroupBox.Visible = true;
                    smileyGroupBox.Location = new Point(8, 225);
                    break;
                case 1:
                    sliderGroupBox.Visible = true;
                    sliderGroupBox.Location = new Point(8, 225);
                    break;
                case 2:
                    starsGroupBox.Visible = true;
                    starsGroupBox.Location = new Point(8, 225);
                    break;
            }
        }
    }
}
