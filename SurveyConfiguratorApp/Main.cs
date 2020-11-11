using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using SortOrder = System.Data.SqlClient.SortOrder;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        IRepository<Question> questionManager;
        SortMethod sortMethod;
        SortOrder sortOrder;

        public Main()
        {
            InitializeComponent();
            sortMethod = SortMethod.ByQuestionText;
            sortOrder = SortOrder.Ascending;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["surveyConnection"].ConnectionString;
                if (connectionString != null)
                    questionManager = new QuestionManager(connectionString);

            }
            catch (Exception ex)
            {
                showError("Invalid Connection string , Please check configuration file");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (questionManager != null)
                refreshButton.PerformClick();
            else
                Application.Exit();
        }

        private void questionDataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    setSortMethod(SortMethod.ByQuestionText);
                    break;
                case 1:
                    setSortMethod(SortMethod.ByType);
                    break;
                case 2:
                    setSortMethod(SortMethod.ByOrder);
                    break;
            }
            questionManager.OrderList(sortMethod, sortOrder);
            refreshList();

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                questionManager.Refresh();
                refreshList();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
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
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete Question?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedRow = -1;
                if (questionDataGridView.SelectedRows.Count > 0)
                    selectedRow = questionDataGridView.SelectedRows[0].Index;

                if (selectedRow >= 0)
                {
                    try
                    {
                        questionManager.Delete(questionManager.Items[selectedRow].ID);
                        refreshList();
                    }
                    catch (Exception ex)
                    {
                        showError(ex.Message);
                    }
                }
                else
                    showError("No question is Selected");
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            QuestionProperties propertiesDialog = new QuestionProperties();
            if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    questionManager.Insert(propertiesDialog.question);
                    refreshList();
                }
                catch (Exception ex)
                {
                    showError(ex.Message);
                }
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
                    try
                    {
                        questionManager.Update(propertiesDialog.question);
                        refreshList();
                    }
                    catch (Exception ex)
                    {
                        showError(ex.Message);
                    }
                }
                propertiesDialog.Dispose();
            }
            else
                showError("No question is Selected");
        }

        private void refreshList()
        {
            if (questionManager.IsConnected())
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
        public static void showError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toggleSortOrder()
        {
            if (sortOrder == SortOrder.Ascending)
                sortOrder = SortOrder.Descending;
            else
                sortOrder = SortOrder.Ascending;
        }
        private void setSortMethod(SortMethod newSortMethod)
        {
            if (newSortMethod == sortMethod)
                toggleSortOrder();
            else
                sortMethod = newSortMethod;
        }
    }
}
