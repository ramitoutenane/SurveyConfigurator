using System;
using System.Diagnostics;
using System.IO;
using System.Text;

/// <summary>
/// Class used to log pError to the location given in configuration
/// </summary>
public static class ErrorLogger
{
    /// <summary>
    /// Log pError message to log file
    /// </summary>
    /// <param name="pMessage">Error message to be logged</param>
    public static void Log(string pMessage)
    {
        try
        {
            // format log message by adding time and a separator line a the beginning 
            StringBuilder tLogMessage = new StringBuilder();
            tLogMessage.AppendLine($"Time: {DateTime.Now}");
            tLogMessage.AppendLine(new string('-', 50));
            tLogMessage.AppendLine(pMessage);

            // write message to pError.log file
            using (StreamWriter tStreamWriter = File.AppendText("error.log"))
            {
                tStreamWriter.WriteLine(tLogMessage.ToString());
            }
        }
        catch { }
    }
    /// <summary>
    /// Log exception to log file
    /// </summary>
    /// <param name="pError">Exception to be logged</param>
    public static void Log(Exception pError)
    {
        try
        {
            // format pError message to show required data
            StringBuilder tErrorMessage = new StringBuilder();
            tErrorMessage.AppendLine($"Message : {pError.Message}");
            tErrorMessage.AppendLine($"Stack : {pError.StackTrace}");

            // log pError
            Log(tErrorMessage.ToString());
        }
        catch { }
    }
    /// <summary>
    /// Log message with stack trace to log file
    /// </summary>
    /// <param name="pMessage">Message to be logged</param>
    /// <param name="pStackFrame">Stack trace to be logged</param>
    public static void Log(string pMessage, StackFrame pStackFrame)
    {
        try
        {
            // format error message to show required data
            StringBuilder tErrorMessage = new StringBuilder();
            tErrorMessage.AppendLine($"Message : {pMessage}");
            tErrorMessage.AppendLine($"Stack : at {pStackFrame.GetMethod()} in {pStackFrame.GetFileName()}:line {pStackFrame.GetFileLineNumber()} ");

            // log pError
            Log(tErrorMessage.ToString());
        }
        catch { }
    }
}
