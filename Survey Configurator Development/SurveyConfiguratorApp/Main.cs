using System;
using System.Windows.Forms;
using System.Configuration;
using SortOrder = System.Data.SqlClient.SortOrder;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using SurveyConfiguratorEntities;
using QuestionManaging;

namespace SurveyConfiguratorApp
{
    #region SortMethod Enumeration
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
    #endregion
    public partial class Main : Form
    {
        #region Initialize Main form
        private QuestionManager mQuestionManager;
        private List<BaseQuestion> mQuestionList;
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
                // get connection data from configuration file to create database settings object
                string tDatabaseServer = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_SERVER];
                string tDatabaseName = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_NAME];
                string tDatabaseUser = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_USER];
                string tDatabasePassword = ConfigurationManager.AppSettings[ConstantStringResources.cDATABASE_PASSWORD];
                DatabaseSettings tDatabaseSettings = new DatabaseSettings(tDatabaseServer, tDatabaseName, tDatabaseUser, tDatabasePassword);

                //initialize new question manger to manage question repository and connection
                mQuestionManager = new QuestionManager(tDatabaseSettings);

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
                    {ConstantStringResources.cENGLISH_LANGUAGE,ConstantStringResources.cENGLISH_CULTURE},
                    {ConstantStringResources.cARABIC_LANGUAGE,ConstantStringResources.cARABIC_CULTURE}
                };

                //populate typeComboBox
                languageComboBox.DataSource = new List<string>(mCultureTable.Keys);

            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                StartAutoRefreshThread();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        /// <summary>
        /// initialize connection and question manager
        /// </summary>
        private void InitializeData()
        {
            try
            {
                if (mQuestionManager == null)
                {
                    ErrorLogger.Log(new NullReferenceException(ErrorMessages.cQUESTION_MANAGER_NULL_EXCEPTION));
                    ShowError(Properties.StringResources.GENERAL_ERROR);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                //refresh data from source, if refreshed successfully reload data to grid view , show pError otherwise
                if (!mQuestionManager.IsConnected())
                {
                    ErrorLogger.Log(new Exception(ErrorMessages.cCONNECTION_ERROR));
                    ShowError(Properties.StringResources.CONNECTION_ERROR);
                    return;
                }
                if (mQuestionManager.SelectAll() != null)
                {
                    RefreshList();
                }
                else
                {
                    ErrorLogger.Log(new Exception(ErrorMessages.cREFRESH_ERROR));
                    ShowError(Properties.StringResources.REFRESH_ERROR);
                    return;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
        #region questionDataGridView event handling
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                    ShowEditForm(questionDataGridView.Rows[clickedCell.RowIndex].DataBoundItem as BaseQuestion);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        #endregion
        #region Buttons event handling
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                    ShowEditForm(questionDataGridView.Rows[tSelectedRow].DataBoundItem as BaseQuestion);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                //check if row is selected and get it's index
                int tSelectedRow = -1;
                if (questionDataGridView.SelectedRows.Count == 1)
                    tSelectedRow = questionDataGridView.SelectedRows[0].Index;

                //if a row is selected, index is not -1
                if (tSelectedRow >= 0)
                {
                    BaseQuestion tSelectedQuestion = mQuestionList[tSelectedRow];
                    //show confirmation dialog to user
                    DialogResult tDialogResult = MessageBox.Show(Properties.StringResources.DELETE_CONFIRMATION_MESSAGE, Properties.StringResources.DELETE_CONFIRMATION_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //check if result of dialog is YES
                    if (tDialogResult == DialogResult.Yes)
                    {
                        //delete question from source, if deleted successfully reload data to grid view , show pError otherwise
                        Cursor.Current = Cursors.WaitCursor;
                        if (!mQuestionManager.IsConnected())
                        {
                            ErrorLogger.Log(new Exception(ErrorMessages.cCONNECTION_ERROR));
                            ShowError(Properties.StringResources.CONNECTION_ERROR);
                            return;
                        }
                        if (mQuestionManager.Delete(tSelectedQuestion.Id))
                        {
                            RefreshList();
                            Cursor.Current = Cursors.Default;
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            ErrorLogger.Log(new Exception(ErrorMessages.cDELETE_ERROR));
                            ShowError(Properties.StringResources.DELETE_ERROR);
                            return;
                        }
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                InitializeData();
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.REFRESH_ERROR);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
        #region Change language
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
                switch (tComboBoxSelectedLanguge)
                {
                    case ConstantStringResources.cENGLISH_CULTURE:
                        if (mCurrentLanguage != mCultureTable[ConstantStringResources.cENGLISH_LANGUAGE])
                        {
                            tChanged = true;
                            mCurrentLanguage = mCultureTable[ConstantStringResources.cENGLISH_LANGUAGE];
                            RightToLeft = RightToLeft.No;
                        }
                        break;

                    case ConstantStringResources.cARABIC_CULTURE:
                        if (mCurrentLanguage != mCultureTable[ConstantStringResources.cARABIC_LANGUAGE])
                        {
                            tChanged = true;
                            mCurrentLanguage = mCultureTable[ConstantStringResources.cARABIC_LANGUAGE];
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
                    languageComboBox.SelectedItem = tLanguageComboBoxSelectedItem;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.GENERAL_ERROR);
            }
        }
        #endregion
        #region Sort questions list
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.SORT_ERROR);
            }

        }
        /// <summary>
        /// Change list sort method
        /// </summary>
        /// <param name="pNewSortMethod">list Sort Method</param>
        private void SetSortMethod(SortMethod pNewSortMethod)
        {
            try
            {
                // if the new sort method is the same as old one , just toggle sort order, else change sort method to new one with Ascending order
                if (pNewSortMethod == mSortMethod)
                    ToggleSortOrder();
                else
                {
                    mSortOrder = SortOrder.Ascending;
                    mSortMethod = pNewSortMethod;
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
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
                List<BaseQuestion> tSortedList = new List<BaseQuestion>(mQuestionList.Count);
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.SORT_ERROR);
            }
        }
        #endregion
        #region Helper methods
        /// <summary>
        /// Refresh questionDataGridView to show the latest changes
        /// </summary>
        private void RefreshList()
        {
            try
            {
                //get data from source
                mQuestionList = mQuestionManager.QuestionsList;
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
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                addButton.Enabled = false;

                ShowError(Properties.StringResources.REFRESH_ERROR);
            }

        }
        /// <summary>
        /// Show question properties dialog with question data loaded to it
        /// </summary>
        /// <param name="pQuestion">Question object to be loaded to properties form</param>
        private void ShowEditForm(BaseQuestion pQuestion)
        {
            try
            {
                //show question properties dialog
                using (QuestionProperties tPropertiesDialog = new QuestionProperties(mQuestionManager, pQuestion))
                {
                    //check if result of dialog is OK 
                    if (tPropertiesDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshList();
                    }
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                ShowError(Properties.StringResources.UPDATE_ERROR);

            }
        }
        /// <summary>
        /// Show custom Error message box to user
        /// </summary>
        /// <param name="pErrorMessage">pError message to be shown to user</param>
        public static void ShowError(string pErrorMessage)
        {
            try
            {
                MessageBox.Show(pErrorMessage, Properties.StringResources.ERROR_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Database thread
        private delegate void RefreshFromDatabaseDelegate();
        /// <summary>
        /// register delegate and time interval of auto refresh
        /// </summary>
        private void StartAutoRefreshThread()
        {
            try
            {
                //get refresh interval from config file, if invalid or less than 20000 milliseconds then set to 20000 milliseconds
                string tConfigRefreshInterval = ConfigurationManager.AppSettings[ConstantStringResources.cAUTO_REFRESH_INTERVAL];
                int tAutoRefreshInterval;
                if (!int.TryParse(tConfigRefreshInterval, out tAutoRefreshInterval) || tAutoRefreshInterval < 20000)
                {
                    tAutoRefreshInterval = 20000;
                }

                //call auto refresh method from question manager
                if (mQuestionManager != null)
                {
                    mQuestionManager.AutoRefreshEventHandler += RefreshFromAnotherThread;
                    mQuestionManager.StartAutoRefresh(tAutoRefreshInterval);
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        /// <summary>
        /// Refresh questions data grid view from another thread
        /// </summary>
        private void RefreshFromAnotherThread()
        {
            try
            {
                RefreshFromDatabaseDelegate pRefreshFromDatabaseDelegate = RefreshIfChanged;
                questionDataGridView.BeginInvoke(pRefreshFromDatabaseDelegate);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }

        /// <summary>
        /// Refresh question data grid view if local question list and database questions are different
        /// </summary>
        public void RefreshIfChanged()
        {
            try
            {
                //if refreshed successfully reload data to grid view, show error otherwise
                if (mQuestionManager.SelectAll() != null)
                {
                    List<BaseQuestion> tQuestionList = mQuestionManager.QuestionsList;
                    //refresh question list if its not up to date.
                    if (IsChanged(tQuestionList))
                        RefreshList();
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }

        }
        /// <summary>
        /// Check if local question list content and source content are equal
        /// </summary>
        /// <param name="pSourceQuestionList">source list to compare to local list</param>
        /// <returns>true if equal, false otherwise</returns>
        public bool IsChanged(List<BaseQuestion> pSourceQuestionList)
        {
            try { 
                if (mQuestionList.Count != pSourceQuestionList.Count)
                    return true;
                List<BaseQuestion> tOrderedSourceQuestionList = pSourceQuestionList.OrderBy(tQuestion => tQuestion.Id).ToList();
                List<BaseQuestion> tOrderedLocalQuestionList = mQuestionList.OrderBy(tQuestion => tQuestion.Id).ToList();
                for (int i = 0; i < tOrderedLocalQuestionList.Count; i++)
                {
                    if (!tOrderedLocalQuestionList[i].Equals(tOrderedSourceQuestionList[i]))
                        return true;
                }
                return false;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return true;
            }
        }

        #endregion
    }
}
