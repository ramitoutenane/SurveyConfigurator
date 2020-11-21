using System;
using System.Windows.Forms;
using System.Configuration;
using SortOrder = System.Data.SqlClient.SortOrder;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Enumeration to define types of sorting methods provided by the system
    /// </summary>
    public enum SortMethod
    {
        ByQuestionID,
        ByQuestionOrder,
        ByQuestionType,
        ByQuestionText
    }
    public partial class Main : Form
    {
        private IRepository<Question> mQuestionManager;
        private List<Question> mQuestionList;
        private SortMethod mSortMethod;
        private SortOrder mSortOrder;
        private string mCurrentLanguage;
        private Dictionary<string, SortMethod> mColumnSortMethod;
        private Dictionary<string, string> mCultureTable;

        /// <summary>
        /// Main form constructor to initialize new Main form
        /// </summary>
        public Main()
        {
            try
            {
                InitializeComponent();
                //set default sorting criteria 
                mSortMethod = SortMethod.ByQuestionText;
                mSortOrder = SortOrder.Ascending;
                //set SortMethod based on related DataGridViewColumn
                mColumnSortMethod = new Dictionary<string, SortMethod>
                {
                    {QuestionTextColumn.ToString(), SortMethod.ByQuestionText},
                    {QuestionOrderColumn.ToString(), SortMethod.ByQuestionOrder},
                    {QuestionTypeColumn.ToString(), SortMethod.ByQuestionType}
                };
                //initialize dictionary of language and it's culture representation
                mCultureTable = new Dictionary<string, string>
                {
                    {Properties.StringResources.ENGLISH_LANGUAGE,"en"},
                    {Properties.StringResources.ARABIC_LANGUAGE,"ar"}
                };
                //populate typeComboBox
                languageComboBox.DataSource = new List<string>(mCultureTable.Keys);

                //set default from language
                languageComboBox.SelectedItem = Properties.StringResources.ENGLISH_LANGUAGE;
                mCurrentLanguage = Properties.StringResources.ENGLISH_LANGUAGE;
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
                DataGridViewColumn tClickedColumn = questionDataGridView.Columns[e.ColumnIndex];
                SetSortMethod(mColumnSortMethod[tClickedColumn.ToString()]);

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
        private void questionDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs clickedCell)
        {
            try
            {
                //if a row is selected, index is not -1, show question properties dialog with selected question properties loaded to it
                if (clickedCell.RowIndex != -1)
                    ShowEditForm(questionDataGridView.Rows[clickedCell.RowIndex].DataBoundItem as Question);
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
                using (QuestionProperties tPropertiesDialog = new QuestionProperties(mQuestionManager))
                {
                    //check if result of dialog is OK 
                    if (tPropertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                            RefreshList();
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
                int tSelectedRow = -1;
                if (questionDataGridView.SelectedRows.Count == 1)
                    tSelectedRow = questionDataGridView.SelectedRows[0].Index;

                //if a row is selected, index is not -1
                if (tSelectedRow >= 0)
                {
                    //show question properties dialog with selected question properties loaded to it
                    ShowEditForm(questionDataGridView.Rows[tSelectedRow].DataBoundItem as Question);
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
                DialogResult tDialogResult = MessageBox.Show(Properties.StringResources.DELETE_CONFIRMATION_MESSAGE, Properties.StringResources.DELETE_CONFIRMATION_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //check if result of dialog is YES
                if (tDialogResult == DialogResult.Yes)
                {
                    //check if row is selected and get it's index
                    int tSelectedRow = -1;
                    if (questionDataGridView.SelectedRows.Count == 1)
                        tSelectedRow = questionDataGridView.SelectedRows[0].Index;

                    //if a row is selected, index is not -1
                    if (tSelectedRow >= 0)
                    {
                        //delete question from source, if deleted successfully reload data to grid view , throw error otherwise
                        Cursor.Current = Cursors.WaitCursor;
                        if (mQuestionManager.Delete(mQuestionList[tSelectedRow].Id))
                        {
                            RefreshList();
                            Cursor.Current = Cursors.Default;
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            throw new Exception(MessageStringValues.cDELETE_ERROR);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.DELETE_ERROR);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        /// <summary>
        /// refreshButton Click event handler
        /// </summary>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //refresh data from source, if refreshed successfully reload data to grid view , throw error otherwise
                if (mQuestionManager.SelectAll() != null)
                {
                    RefreshList();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    throw new Exception(MessageStringValues.cREFRESH_ERROR);
                }
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
                ShowError(Properties.StringResources.REFRESH_ERROR);
            }
            finally
            {
                Cursor.Current = Cursors.Default ;
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
                string tLanguageComboBoxSelectedItem = languageComboBox.SelectedItem.ToString();
                //check selected language and change current language variables according to selection
                string tComboBoxSelectedLanguge = mCultureTable[tLanguageComboBoxSelectedItem];
                if (tComboBoxSelectedLanguge == mCultureTable[Properties.StringResources.ENGLISH_LANGUAGE])
                {
                    if (mCurrentLanguage != mCultureTable[Properties.StringResources.ENGLISH_LANGUAGE])
                    {
                        tChanged = true;
                        mCurrentLanguage = mCultureTable[Properties.StringResources.ENGLISH_LANGUAGE];
                        RightToLeft = RightToLeft.No;
                    }
                }
                else if (tComboBoxSelectedLanguge == mCultureTable[Properties.StringResources.ARABIC_LANGUAGE])
                {
                    if (mCurrentLanguage != mCultureTable[Properties.StringResources.ARABIC_LANGUAGE])
                    {
                        tChanged = true;
                        mCurrentLanguage = mCultureTable[Properties.StringResources.ARABIC_LANGUAGE];
                    }
                }
                //if language is changed from current language to new one, change culture and reinitialize form components and data
                if (tChanged)
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(mCurrentLanguage);
                    this.Controls.Clear();
                    InitializeComponent();
                    InitializeData();
                    languageComboBox.SelectedItem = tLanguageComboBoxSelectedItem;
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
                SortQuestions();
                //bind questionDataGridView to local question list
                questionDataGridView.DataSource = mQuestionList;
                //update questionDataGridView binding context
                CurrencyManager tCurrencyManager = (CurrencyManager)questionDataGridView.BindingContext[mQuestionList];
                if (tCurrencyManager != null)
                {
                    tCurrencyManager.Refresh();
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
                using (QuestionProperties tPropertiesDialog = new QuestionProperties(mQuestionManager, question))
                {
                    //check if result of dialog is OK 
                    if (tPropertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                            RefreshList();
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
                    case SortMethod.ByQuestionID:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Id).ToList();
                        break;
                    case SortMethod.ByQuestionOrder:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Order).ToList();
                        break;
                    case SortMethod.ByQuestionText:
                        tSortedList = mQuestionList.OrderBy(Item => Item.Text).ToList();
                        break;
                    case SortMethod.ByQuestionType:
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
                // get connection data from configuration file to create database settings object
                string tDataSource = ConfigurationManager.AppSettings[DatabaseStringValues.cDATABASE_SERVER];
                string tInitialCatalog = ConfigurationManager.AppSettings[DatabaseStringValues.cDATABASE_NAME];
                string tUserID = ConfigurationManager.AppSettings[DatabaseStringValues.cDATABASE_USER];
                string tPassword = ConfigurationManager.AppSettings[DatabaseStringValues.cDATABASE_PASSWORD];
                DatabaseSettings tDatabaseSettings = new DatabaseSettings(tDataSource, tInitialCatalog, tUserID, tPassword);

                //initialize new question manger to manage question repository and connection
                mQuestionManager = new QuestionManager(tDatabaseSettings);
                if (mQuestionManager == null)
                    throw new NullReferenceException(MessageStringValues.cQUESTION_MANAGER_NULL_EXCEPTION);
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
