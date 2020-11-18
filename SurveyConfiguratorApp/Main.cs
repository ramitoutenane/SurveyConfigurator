using System;
using System.Windows.Forms;
using System.Configuration;
using SortOrder = System.Data.SqlClient.SortOrder;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        private IMaintainable<Question> mQuestionManager;
        private List<Question> mQuestionList;
        private SortMethod mSortMethod;
        private SortOrder mSortOrder;
        private string mConnectionString;
        private string mCurrentLanguage;
        private int mCurrentLanguageIndex;
        /// <summary>
        /// Main form constructor to initialize new Main form
        /// </summary>
        public Main()
        {
            try
            {
                InitializeComponent();
                //set default from language
                mCurrentLanguage = "en";
                mCurrentLanguageIndex = 0;
                languageComboBox.SelectedIndex = mCurrentLanguageIndex;
                //set default sorting criteria 
                mSortMethod = SortMethod.ByQuestionText;
                mSortOrder = SortOrder.Ascending;

            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// Main form Load event handler
        /// </summary>
        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeData();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// questionDataGridView ColumnHeaderMouseClick event handler
        /// </summary>
        private void questionDataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //check which column has been clicked and change sort criteria based on it
                switch (e.ColumnIndex)
                {
                    case 0:
                        SetSortMethod(SortMethod.ByQuestionText);
                        break;
                    case 1:
                        SetSortMethod(SortMethod.ByType);
                        break;
                    case 2:
                        SetSortMethod(SortMethod.ByOrder);
                        break;
                }
                //sort question list and load new data to grid view 
                SortQuestions();
                questionDataGridView.DataSource = mQuestionList;
                questionDataGridView.Refresh();
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }

        }
        /// <summary>
        /// questionDataGridView change selected row event handler
        /// </summary>
        private void questionDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //enable edit and delete button only if exactly one row is selected, disable otherwise
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
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// questionDataGridView cell double click event handler
        /// </summary>
        private void questionDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if a row is selected, index is not -1, show question properties dialog with selected question properties loaded to it
                if (e.RowIndex != -1)
                    ShowEditForm(questionDataGridView.Rows[e.RowIndex].DataBoundItem as Question);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// addButton Click event handler
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                //show new empty question properties dialog
                using (QuestionProperties propertiesDialog = new QuestionProperties())
                {
                    //check if result of dialog is OK 
                    if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        //insert new question to source, if inserted successfully reload data to grid view , throw error otherwise
                        if (mQuestionManager.Insert(propertiesDialog.question))
                            RefreshList();
                        else
                            throw new Exception(MessageStringValues.cREFRESH_ERROR);
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.INSERT_ERROR);

            }
        }
        /// <summary>
        /// editButton Click event handler
        /// </summary>
        private void editButton_Click(object sender, EventArgs e)
        {
            try
            {
                //check if one row is selected and get it's index
                int selectedRow = -1;
                if (questionDataGridView.SelectedRows.Count == 1)
                    selectedRow = questionDataGridView.SelectedRows[0].Index;

                //if a row is selected, index is not -1
                if (selectedRow >= 0)
                {
                    //show question properties dialog with selected question properties loaded to it
                    ShowEditForm(questionDataGridView.Rows[selectedRow].DataBoundItem as Question);
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.UPDATE_ERROR);

            }
        }
        /// <summary>
        /// deleteButton Click event handler
        /// </summary>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                //show confirmation dialog to user
                DialogResult dialogResult = MessageBox.Show(Properties.StringResources.DELETE_CONFIRMATION_MESSAGE, Properties.StringResources.DELETE_CONFIRMATION_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //check if result of dialog is YES
                if (dialogResult == DialogResult.Yes)
                {
                    //check if row is selected and get it's index
                    int selectedRow = -1;
                    if (questionDataGridView.SelectedRows.Count == 1)
                        selectedRow = questionDataGridView.SelectedRows[0].Index;

                    //if a row is selected, index is not -1
                    if (selectedRow >= 0)
                    {
                        //delete question from source, if deleted successfully reload data to grid view , throw error otherwise
                        if (mQuestionManager.Delete(mQuestionList[selectedRow].Id))
                            RefreshList();
                        else
                            throw new Exception(MessageStringValues.cDELETE_ERROR);
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.DELETE_ERROR);

            }
        }
        /// <summary>
        /// refreshButton Click event handler
        /// </summary>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                //refresh data from source, if refreshed successfully reload data to grid view , throw error otherwise
                if (mQuestionManager.Refresh() != null)
                    RefreshList();
                else
                    throw new Exception(MessageStringValues.cREFRESH_ERROR);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.REFRESH_ERROR);
            }
        }
        /// <summary>
        /// languageComboBox change selection event handler
        /// </summary>
        private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool tChanged = false;
                //check selected index and change current language variables according to selection
                switch (languageComboBox.SelectedIndex)
                {
                    case 0:
                        if (mCurrentLanguage != "en")
                        {
                            tChanged = true;
                            mCurrentLanguage = "en";
                            mCurrentLanguageIndex = 0;
                        }
                        break;
                    case 1:
                        if (mCurrentLanguage != "ar")
                        {
                            tChanged = true;
                            mCurrentLanguage = "ar";
                            mCurrentLanguageIndex = 1;
                        }
                        break;
                }
                //if language is changed from current language to new one, change culture and reinitialize form components and data
                if (tChanged)
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(mCurrentLanguage);
                    this.Controls.Clear();
                    InitializeComponent();
                    InitializeData();
                    languageComboBox.SelectedIndex = mCurrentLanguageIndex;
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// Refresh questionDataGridView to show the latest changes
        /// </summary>
        private void RefreshList()
        {
            try
            {
                //get data from source
                mQuestionList = mQuestionManager.Items;
                //bind questionDataGridView to local question list
                questionDataGridView.DataSource = mQuestionList;
                //update questionDataGridView binding context
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

                ShowError(Properties.StringResources.REFRESH_ERROR);
            }

        }
        /// <summary>
        /// Show question properties dialog with question data loaded to it
        /// </summary>
        /// <param name="question">Question object to be loaded to properties form</param>
        private void ShowEditForm(Question question)
        {
            try
            {
                //show question properties dialog
                using (QuestionProperties propertiesDialog = new QuestionProperties(question))
                {
                    //check if result of dialog is OK 
                    if (propertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        //update question on source, if updated successfully reload data to grid view , throw error otherwise
                        if (mQuestionManager.Update(propertiesDialog.question))
                            RefreshList();
                        else
                            throw new Exception(MessageStringValues.cUPDATE_ERROR);
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.UPDATE_ERROR);

            }
        }
        /// <summary>
        /// Show custom Error message box to user
        /// </summary>
        /// <param name="errorMessage">error message to be shown to user</param>
        public static void ShowError(string errorMessage)
        {
            try
            {
                MessageBox.Show(errorMessage, Properties.StringResources.ERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        /// <summary>
        /// Toggle SortOrder between Ascending and Descending
        /// </summary>
        private void ToggleSortOrder()
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
                ShowError(Properties.StringResources.SORT_ERROR);
            }

        }
        /// <summary>
        /// Change list sort method
        /// </summary>
        /// <param name="newSortMethod">list Sort Method</param>
        private void SetSortMethod(SortMethod newSortMethod)
        {
            try
            {
                // if the new sort method is the same as old one , just toggle sort order, else change sort method to new one with Ascending order
                if (newSortMethod == mSortMethod)
                    ToggleSortOrder();
                else
                {
                    mSortOrder = SortOrder.Ascending;
                    mSortMethod = newSortMethod;
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.SORT_ERROR);
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
                ShowError(Properties.StringResources.SORT_ERROR);
            }
        }
        /// <summary>
        /// initialize connection and question manager
        /// </summary>
        private void InitializeData()
        {
            try
            {
                // create connection string from Configuration file data
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = ConfigurationManager.AppSettings["DatabaseServer"],
                    InitialCatalog = ConfigurationManager.AppSettings["DatabaseName"],
                    UserID = ConfigurationManager.AppSettings["DatabaseUser"],
                    Password = ConfigurationManager.AppSettings["DatabasePassword"]
                };
                mConnectionString = tBuilder.ConnectionString;

                //initialize new question manger to manage question repository and connection
                if (mConnectionString != null)
                    mQuestionManager = new QuestionManager(mConnectionString);
                else
                    throw new NullReferenceException("Connection String is null");

                if (mQuestionManager == null)
                    throw new NullReferenceException("Question manager reference is null");
                refreshButton.PerformClick();

            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
    }
}
