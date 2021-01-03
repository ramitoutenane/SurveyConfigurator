using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace SurveyConfiguratorWeb.Helpers
{
    public static class Helper
    {
        public static string MD5CheckSum(string pInput)
        {
            try
            {
                // Use input string to calculate MD5 hash
                using (System.Security.Cryptography.MD5 tMD5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] tInputBytes = System.Text.Encoding.ASCII.GetBytes(pInput);
                    byte[] tHashBytes = tMD5.ComputeHash(tInputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder tStringBuilder = new StringBuilder();
                    for (int i = 0; i < tHashBytes.Length; i++)
                    {
                        tStringBuilder.Append(tHashBytes[i].ToString("X2"));
                    }
                    return tStringBuilder.ToString();
                }
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return String.Empty;
            }
        }
        public static bool ChangeCulture(string pSelectedCalture)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(pSelectedCalture);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                return true;
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return false;
            }

        }

    }
}