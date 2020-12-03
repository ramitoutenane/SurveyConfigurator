using System;
namespace SurveyConfiguratorEntities
{
    /// <summary>
    /// Class to represent and store database connection settings
    /// </summary>
    public class DatabaseSettings
    {
        #region Variable deceleration
        private string mDatabaseServer;
        private string mDatabaseName;
        private string mDatabaseUser;
        private string mDatabasePassword;
        #endregion
        #region Constructor 
        /// <summary>
        /// DatabaseSettings constructor to initialize new DatabaseSettings object
        /// </summary>
        /// <param name="pDatabaseServer">Database server</param>
        /// <param name="pDatabaseName">Database name</param>
        /// <param name="pDatabaseUser">Database user</param>
        /// <param name="pDatabasePassword">Database user password</param>
        public DatabaseSettings(string pDatabaseServer, string pDatabaseName, string pDatabaseUser, string pDatabasePassword)
        {
            try
            {
                DatabaseServer = pDatabaseServer;
                DatabaseName = pDatabaseName;
                DatabaseUser = pDatabaseUser;
                DatabasePassword = pDatabasePassword;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
            }
        }
        #endregion
        #region Properties definition
        public string DatabaseServer { get => mDatabaseServer; private set => mDatabaseServer = value; }
        public string DatabaseName { get => mDatabaseName; private set => mDatabaseName = value; }
        public string DatabaseUser { get => mDatabaseUser; private set => mDatabaseUser = value; }
        public string DatabasePassword { get => mDatabasePassword; private set => mDatabasePassword = value; }
        #endregion
    }
}