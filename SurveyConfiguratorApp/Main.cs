using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using SurveyConfiguratorApp.QuestionComparers;
using SortOrder = System.Data.SqlClient.SortOrder;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        QuestionManager questionManager;
        OrderingMethod orderingMethod;
        SortOrder sortOrder;

        public Main()
        {
            InitializeComponent();
            questionManager = new QuestionManager(ConfigurationManager.ConnectionStrings["surveyConnection"].ConnectionString);
            orderingMethod = OrderingMethod.ByQuestionText;
            sortOrder = SortOrder.Ascending;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            refreshButton.PerformClick();

        }

        private void questionDataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Toggle Sort
            if (sortOrder == SortOrder.Ascending)
                sortOrder = SortOrder.Descending;
            else
                sortOrder = SortOrder.Ascending;
            switch (e.ColumnIndex)
            {
                case 0:
                    orderingMethod = OrderingMethod.ByQuestionText;
                    break;
                case 1:
                    orderingMethod = OrderingMethod.ByType;
                    break;
                case 2:
                    orderingMethod = OrderingMethod.ByOrder;
                    break;
            }
            questionManager.OrderList(orderingMethod, sortOrder);
            questionDataGridView.DataSource = questionManager.Items;
            questionDataGridView.Refresh();

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            questionManager.Refresh();
            refreshList();
        }

        private void questionDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (questionDataGridView.SelectedRows.Count == 1)
            {
                editButton.Enabled = true;
                deleteButton.Enabled = true;
            }
            else
            {
                editButton.Enabled = false;
                deleteButton.Enabled = false;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int selectedRow = -1;
            if (questionDataGridView.SelectedRows.Count > 0)
                selectedRow = questionDataGridView.SelectedRows[0].Index;

            if (selectedRow >= 0)
            {
                questionManager.Delete(questionManager.Items[selectedRow].ID);
                refreshList();
            }
            else
                showError("No question is Selected");

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            QuestionProperties propertiesDialog = new QuestionProperties();
            if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
            {

            }
            else
            {

            }
            propertiesDialog.Dispose();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int selectedRow = -1;
            if (questionDataGridView.SelectedRows.Count > 0)
                selectedRow = questionDataGridView.SelectedRows[0].Index;

            if (selectedRow >= 0)
            {
                QuestionProperties propertiesDialog = new QuestionProperties(questionManager.Items[selectedRow]);
                if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
                {

                }
                else
                {

                }
                propertiesDialog.Dispose();
            }
            else
                showError("No question is Selected");
        }

        private void refreshList()
        {
            if (questionManager.isConnectedToDB())
            {
                try
                {
                    questionDataGridView.DataSource = questionManager.Items;
                    CurrencyManager currencyManager = (CurrencyManager)questionDataGridView.BindingContext[questionManager.Items];
                    if (currencyManager != null)
                    {
                        currencyManager.Refresh();
                    }
                    addButton.Enabled = true;
                }
                catch (QuestionListRefreshException ex)
                {
                    showError(ex.Message);
                    addButton.Enabled = false;
                }
            }
            else
            {
                string message = "Database Connection is not Available";
                showError(message);
            }

        }
        private void showError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
