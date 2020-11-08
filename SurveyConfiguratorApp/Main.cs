using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using SurveyConfiguratorApp.QuestionComparers;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        List<Question> questionList;
        public Main()
        {
            InitializeComponent();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            questionList = new List<Question>();
            questionList.Add(new StarsQuestion("Question 1", 1, 8, 1));
            questionList.Add(new SmileyQuestion("Question 3", 3, 4, 3));
            questionList.Add(new StarsQuestion("Question 5", 5, 6, 5));
            questionList.Add(new SmileyQuestion("Question 2", 2, 3, 2));
            questionDataGridView.DataSource = questionList;

        }

        private void questionBindingSource2_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void questionDataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    questionList.Sort(new QuestionTextComparer());
                    break;
                case 1:
                    questionList.Sort(new QuestionTypeComparer());
                    break;
                case 2:
                    questionList.Sort(new QuestionOrderComparer()); 
                    break;
            }
            questionDataGridView.Refresh();

        }

        private void questionDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
