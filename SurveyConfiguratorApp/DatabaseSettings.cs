using System;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to represent and store database connection settings
    /// </summary>
    public class DatabaseSettings
    {
        private string mDataSource;
        private string mInitialCatalog;
        private string mUserID;
        private string mPassword;
        /// <summary>
        /// DatabaseSettings constructor to initialize new DatabaseSettings object
        /// </summary>
        /// <param name="dataSource">Database server</param>
        /// <param name="initialCatalog">Database name</param>
        /// <param name="userID">Database user</param>
        /// <param name="password">Database user password</param>
        public DatabaseSettings(string dataSource, string initialCatalog, string userID, string password)
        {
            try
            {
                DataSource = dataSource;
                InitialCatalog = initialCatalog;
                UserID = userID;
                Password = password;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        public string DataSource { get => mDataSource; private set => mDataSource = value; }
        public string InitialCatalog { get => mInitialCatalog; private set => mInitialCatalog = value; }
        public string UserID { get => mUserID; private set => mUserID = value; }
        public string Password { get => mPassword; private set => mPassword = value; }
    }
}
