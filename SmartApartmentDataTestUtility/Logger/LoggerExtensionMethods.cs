using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CloudWatchLogs.Model;
using log4net;
using Newtonsoft.Json.Linq;

namespace SmartApartmentDataTestUtility.Logger
{
    public static class LoggerExtensionMethods
    {
        /// <summary>
        /// Generates custom JSON log message. To log custom fields add them to params in 'Key:Value' format
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GenerateJSONLogMessage(this string message, ELogType logType, params string[] args)
        {
            JObject logMessage = new JObject
            {
                { "Timestamp", DateTime.UtcNow.ToString() },
                { "Computer Name", System.Environment.MachineName },
                { "Level", Enum.GetName(typeof(ELogType), logType) }
            };

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string field = args[i];
                    string[] keyValuePair = field.Split(':');
                    try
                    {
                        logMessage.Add(keyValuePair[0], keyValuePair[1]);

                        if (keyValuePair[0].Equals("Message") || keyValuePair[1].Equals("Timestamp"))
                            throw new InvalidParameterException("Can't use key message as a custom key");
                    }
                    catch (IndexOutOfRangeException exception)
                    {
                        logMessage.Add("Error" + 1, $"Field : {field} is not properly formatted. Use key:value format");
                    }
                    catch (InvalidParameterException exception)
                    {
                        logMessage.Add("Field" + i, keyValuePair[1]);
                    }
                }
            }

            logMessage.Add("Message", message);
            return logMessage.ToString();
        }

        #region Custom ILog message adapter extensions
        public static void CustomError(this ILog logger, string message, params object[] args)
        {
            List<string> messageArgs = new List<string>();

            if (args != null)
            {
                foreach (object obj in args)
                {
                    if (obj is Exception)
                    {
                        messageArgs.Add("Exception:" + (obj as Exception).Message);
                    }
                    else if (obj.GetType() == typeof(string))
                    {
                        messageArgs.Add(obj as string);
                    }
                    else
                    {
                        throw new InvalidParameterException("Non supported argument type passed.");
                    }
                }
            }

            logger.Error(message.GenerateJSONLogMessage(ELogType.Error, messageArgs.ToArray()));
        }

        public static void CustomInfo(this ILog logger, string message, params object[] args)
        {
            List<string> messageArgs = new List<string>();

            if (args != null)
            {
                foreach (object obj in args)
                {
                    if (obj is Exception)
                    {
                        messageArgs.Add("Exception:" + (obj as Exception).Message);
                    }
                    else if (obj.GetType() == typeof(string))
                    {
                        messageArgs.Add(obj as string);
                    }
                    else
                    {
                        throw new InvalidParameterException("Non supported argument type passed.");
                    }
                }
            }

            logger.Info(message.GenerateJSONLogMessage(ELogType.Info, messageArgs.ToArray()));
        }

        public static void CustomWarning(this ILog logger, string message, params object[] args)
        {
            List<string> messageArgs = new List<string>();

            if (args != null)
            {
                foreach (object obj in args)
                {
                    if (obj is Exception)
                    {
                        messageArgs.Add("Exception:" + (obj as Exception).Message);
                    }
                    else if (obj.GetType() == typeof(string))
                    {
                        messageArgs.Add(obj as string);
                    }
                    else
                    {
                        throw new InvalidParameterException("Non supported argument type passed.");
                    }
                }
            }

            logger.Info(message.GenerateJSONLogMessage(ELogType.Warning, messageArgs.ToArray()));
        }

        public static void CustomDebug(this ILog logger, string message, params object[] args)
        {
            List<string> messageArgs = new List<string>();

            if (args != null)
            {
                foreach (object obj in args)
                {
                    if (obj is Exception)
                    {
                        messageArgs.Add("Exception:" + (obj as Exception).Message);
                    }
                    else if (obj.GetType() == typeof(string))
                    {
                        messageArgs.Add(obj as string);
                    }
                    else
                    {
                        throw new InvalidParameterException("Non supported argument type passed.");
                    }
                }
            }

            logger.Debug(message.GenerateJSONLogMessage(ELogType.Debug, messageArgs.ToArray()));
        }
        #endregion
    }
}
