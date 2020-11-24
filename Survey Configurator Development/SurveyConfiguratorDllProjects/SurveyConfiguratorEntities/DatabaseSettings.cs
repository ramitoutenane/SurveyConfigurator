using System;
namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class to represent and store database connection settings
    /// </summary>
    public class DatabaseSettings
    {
        private string mDatabaseServer;
        private string mDatabaseName;
        private string mDatabaseUser;
        private string mDatabasePassword;
        /// <summary>
        /// DatabaseSettings constructor to initialize new DatabaseSettings object
        /// </summary>
        /// <param name="databaseServer">Database server</param>
        /// <param name="databaseName">Database name</param>
        /// <param name="databaseUser">Database user</param>
        /// <param name="databasePassword">Database user password</param>
        public DatabaseSettings(string databaseServer, string databaseName, string databaseUser, string databasePassword)
        {
            try
            {
                DatabaseServer = databaseServer;
                DatabaseName = databaseName;
                DatabaseUser = databaseUser;
                DatabasePassword = databasePassword;
            }
            catch (Exception error)
            {
                ErrorLogger.Log(error);
            }
        }
        public string DatabaseServer { get => mDatabaseServer; private set => mDatabaseServer = value; }
        public string DatabaseName { get => mDatabaseName; private set => mDatabaseName = value; }
        public string DatabaseUser { get => mDatabaseUser; private set => mDatabaseUser = value; }
        public string DatabasePassword { get => mDatabasePassword; private set => mDatabasePassword = value; }
    }
}