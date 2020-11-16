using System;
using System.Windows.Forms;
using System.Configuration;
using SortOrder = System.Data.SqlClient.SortOrder;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        private readonly IMaintainable<Question> mQuestionManager;
        private List<Question> mQuestionList;
        private SortMethod mSortMethod;
        private SortOrder mSortOrder;
        private string mConnectionString;
        public Main()
        {
            try
            {
                InitializeComponent();
                mSortMethod = SortMethod.ByQuestionText;
                mSortOrder = SortOrder.Ascending;

                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder();
                tBuilder.DataSource = ConfigurationManager.AppSettings["DatabaseServer"];
                tBuilder.InitialCatalog = ConfigurationManager.AppSettings["DatabaseName"];
                tBuilder.UserID = ConfigurationManager.AppSettings["DatabaseUser"];
                tBuilder.Password = ConfigurationManager.AppSettings["DatabasePassword"];

                mConnectionString = tBuilder.ConnectionString;
                if (mConnectionString != null)
                    mQuestionManager = new QuestionManager(mConnectionString);
                else
                    throw new NullReferenceException("Connection String is null");
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cGENERAL_ERROR);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                if (mQuestionManager == null)
                    throw new NullReferenceException("Question manager reference is null");
                refreshButton.PerformClick();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cGENERAL_ERROR);
            }
        }

        private void questionDataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
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
                SortQuestions();
                questionDataGridView.DataSource = mQuestionList;
                questionDataGridView.Refresh();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cGENERAL_ERROR);
            }

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (mQuestionManager.Refresh() != null)
                    refreshList();
                else
                    throw new Exception(MessageStringValues.cREFRESH_ERROR);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cREFRESH_ERROR);
            }
        }

        private void questionDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cGENERAL_ERROR);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete Question?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    int selectedRow = -1;
                    if (questionDataGridView.SelectedRows.Count > 0)
                        selectedRow = questionDataGridView.SelectedRows[0].Index;

                    if (selectedRow >= 0)
                    {
                        if (mQuestionManager.Delete(mQuestionList[selectedRow].Id))
                            refreshList();
                        else
                            throw new Exception(MessageStringValues.cDELETE_ERROR);
                    }
                    else
                        showError("No question is Selected");
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cDELETE_ERROR);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (QuestionProperties propertiesDialog = new QuestionProperties())
                {
                    if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (mQuestionManager.Insert(propertiesDialog.question))
                            refreshList();
                        else
                            throw new Exception(MessageStringValues.cREFRESH_ERROR);
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cINSERT_ERROR);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = -1;
                if (questionDataGridView.SelectedRows.Count > 0)
                    selectedRow = questionDataGridView.SelectedRows[0].Index;

                if (selectedRow >= 0)
                {
                    ShowEditForm(questionDataGridView.Rows[selectedRow].DataBoundItem as Question);
                }
                else
                    showError("No question is Selected");
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cUPDATE_ERROR);
            }
        }

        private void refreshList()
        {
            try
            {
                mQuestionList = mQuestionManager.Items;
                questionDataGridView.DataSource = mQuestionList;
                CurrencyManager currencyManager = (CurrencyManager)questionDataGridView.BindingContext[mQuestionList];
                if (currencyManager != null)
                {
                    currencyManager.Refresh();
                }
                addButton.Enabled = true;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                addButton.Enabled = false;

                showError(MessageStringValues.cREFRESH_ERROR);

            }

        }
        private void ShowEditForm(Question question)
        {
            try
            {
                using (QuestionProperties propertiesDialog = new QuestionProperties(question))
                {
                    if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {

                        if (mQuestionManager.Update(propertiesDialog.question))
                            refreshList();
                        else
                            throw new Exception(MessageStringValues.cUPDATE_ERROR);
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cUPDATE_ERROR);
            }
        }
        public static void showError(string errorMessage)
        {
            try
            {
                MessageBox.Show(errorMessage, MessageStringValues.cERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }

        private void toggleSortOrder()
        {
            try
            {
                if (mSortOrder == SortOrder.Ascending)
                    mSortOrder = SortOrder.Descending;
                else
                    mSortOrder = SortOrder.Ascending;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cSORT_ERROR);
            }

        }
        private void setSortMethod(SortMethod newSortMethod)
        {
            try
            {
                if (newSortMethod == mSortMethod)
                    toggleSortOrder();
                else
                    mSortMethod = newSortMethod;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cSORT_ERROR);
            }

        }

        private void questionDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                    ShowEditForm(questionDataGridView.Rows[e.RowIndex].DataBoundItem as Question);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cGENERAL_ERROR);
            }
        }
        /// <summary>
        ///  Sort Items list according to given order and method
        /// </summary>
        public void SortQuestions()
        {
            try
            {
                // initialize temporary list with old list capacity to avoid list resizing.
                List<Question> tSortedList = new List<Question>(mQuestionList.Count);
                //Sort Items list according to given ordering method using linq
                switch (mSortMethod)
                {
                    case SortMethod.ByID:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Id).ToList();
                        break;
                    case SortMethod.ByOrder:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Order).ToList();
                        break;
                    case SortMethod.ByQuestionText:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Text).ToList();
                        break;
                    case SortMethod.ByType:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Type).ToList();
                        break;
                }
                // temporary ordered list is sorted in ascending order, if the required order is descending then reverse it
                if (mSortOrder == SortOrder.Descending)
                {
                    tSortedList.Reverse();
                }
                // set local Items list to the new sorted list
                mQuestionList = tSortedList;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                showError(MessageStringValues.cSORT_ERROR);
            }
        }
    }
}
