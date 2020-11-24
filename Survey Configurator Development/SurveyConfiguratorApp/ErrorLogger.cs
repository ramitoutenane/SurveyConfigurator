using System;
using System.IO;
using System.Text;

namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Class used to log error to the location given in configuration
    /// </summary>
    public static class ErrorLogger
    {
        /// <summary>
        /// Log error message to log file
        /// </summary>
        /// <param name="message">Error message to be logged</param>
        public static void Log(string message)
        {
            try
           {
                // format log message by adding time and a separator line a the beginning 
                StringBuilder tLogMessage = new StringBuilder();
                tLogMessage.AppendLine($"Time: {DateTime.Now}");
                tLogMessage.AppendLine(new string('-',50));
                tLogMessage.AppendLine(message);

                // write message to error.log file
                using (StreamWriter tStreamWriter = File.AppendText("error.log"))
                {
                    tStreamWriter.WriteLine(tLogMessage.ToString());
                }
            }
            catch{}
        }
        /// <summary>
        /// Log exception to log file
        /// </summary>
        /// <param name="error">Exception to be logged</param>
        public static void Log(Exception error)
        {
            try
            {
                // format error message to show required data
                StringBuilder tErrorMessage = new StringBuilder();
                tErrorMessage.AppendLine($"Exception Type : {error.GetType()}");
                tErrorMessage.AppendLine($"Exception Message : {error.Message}");
                tErrorMessage.AppendLine($"Exception Stack : {error.StackTrace}");

                // log error
                Log(tErrorMessage.ToString());
            }
            catch {}
        }
    }
}
