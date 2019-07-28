using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using AWS.Logger.Log4net;
using log4net;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.IdentityModel.Protocols;
using SmartApartmentDataTestUtility.Encryption.AES;

namespace SmartApartmentDataTestUtility.Logger
{
    public class SmartApartmentLogger
    {
        #region Private variables

        //Config path
        private static readonly string _configFilePath = "SmartApartmentLogger.config";

        //log4net base adapter
        private static readonly ILog _loggerAdapter = LogManager.GetLogger(nameof(SmartApartmentLogger));

        //log4net log pattern. Set this up in SmartApartmentLogger.config
        private readonly string _logPattern;

        //AWS LogGroup. Set this up in SmartApartmentLogger.config
        private readonly string _logGroup;

        //AWS REgion. Set this up in SmartApartmentLogger.config
        private readonly string _awsRegion;

        //Basic login credentials for AWS. Set key and secret in SmartApartmentLogger.config
        private readonly BasicAWSCredentials _awsCredentials;

        //log4net AWS Appender.
        private AWSAppender _awsAppender;

        private static readonly string _encryptionKey = "2S5%'d+hta%,.;JA}pX0J7^%A4bh${l'";

        #endregion Private variables

        #region Properties

        private static SmartApartmentLogger Instance { get; set; }

        #endregion Properties

        #region Constructor

        static SmartApartmentLogger()
        {
            var configFileMap = new ExeConfigurationFileMap();
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var fileName = Path.Combine(basePath, _configFilePath);
            configFileMap.ExeConfigFilename = fileName;
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            if (!File.Exists(configFileMap.ExeConfigFilename))
            {
                throw new FileNotFoundException("SmartApartmentLogger.config not found. Please create one before initializing logger");
            }

            Instance = new SmartApartmentLogger(fileName,
                config.AppSettings.Settings["LogPattern"]?.Value,
                config.AppSettings.Settings["LogGroup"]?.Value,
                config.AppSettings.Settings["AWSRegion"]?.Value,
                config.AppSettings.Settings["AWSKey"]?.Value,
                AESEncryption.Decrypt(config.AppSettings.Settings["AWSSecret"]?.Value, _encryptionKey));
        }

        private SmartApartmentLogger(string fileName, string logPattern, string logGroup, string awsRegion, string awsKey, string awsSecret)
        {
            XmlConfigurator.Configure(new FileInfo(fileName));
            _logPattern = logPattern;
            _logGroup = logGroup;
            _awsRegion = awsRegion;
            _awsCredentials = CreateCredentials(awsKey, awsSecret);
            Setup();
        }

        #endregion Constructor

        #region Private Methods

        /// <summary>
        /// Sets up wrapper to work with log4net
        /// </summary>
        private void Setup()
        {
            _awsAppender = CreateAppender(_awsCredentials);
            var hierarchy = (Hierarchy)_loggerAdapter.Logger.Repository;
            var logger = hierarchy.GetLogger(nameof(SmartApartmentLogger)) as log4net.Repository.Hierarchy.Logger;
            logger.AddAppender(_awsAppender);
        }

        /// <summary>
        /// Writes the log to AWS using log4net adapter
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="customParameters"></param>
        private void WriteLog(ELogType logType, string message, params string[] customParameters)
        {
            _loggerAdapter.Info(message.GenerateJSONLogMessage(logType, customParameters));
            //_loggerAdapter.Info(GenerateJSONLogMessage(logType, message, customParameters));
        }

        /// <summary>
        /// Creates basic login credentials for AWS API
        /// </summary>
        /// <param name="awsKey"></param>
        /// <param name="awsSecret"></param>
        /// <returns></returns>
        private BasicAWSCredentials CreateCredentials(string awsKey, string awsSecret)
        {
            if (awsKey == null || awsSecret == null)
            {
                throw new ArgumentNullException(
                    "args",
                    "Please pass AWS API key and secret key as command line arguments"
                );
            }

            return new BasicAWSCredentials(awsKey, awsSecret);
        }

        /// <summary>
        /// Creates an AWS Appender for log4net base adapter
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        private AWSAppender CreateAppender(AWSCredentials credentials)
        {
            var patternLayout = new PatternLayout
            {
                ConversionPattern = _logPattern
            };
            patternLayout.ActivateOptions();

            var appender = new AWSAppender
            {
                Layout = patternLayout,
                Credentials = credentials,
                LogGroup = _logGroup,
                Region = _awsRegion
            };

            appender.ActivateOptions();
            return appender;
        }

        #endregion Private Methods

        #region Static Methods

        /// <summary>
        /// Writes a custom message to AWS CloudWatch. Use customParameters to add fields like 'CustomerCode:150'
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="customParameters"></param>
        public static void LogAsync(ELogType logType, string message, params string[] customParameters)
        {
            Instance?.WriteLog(logType, message, customParameters);
        }

        /// <summary>
        /// Gets SmartApartmentLogger log4net wrapper
        /// </summary>
        /// <returns></returns>
        public static ILog GetWrapper()
        {
            return _loggerAdapter;
        }

        #endregion Static Methods
    }

    #region Enums

    /// <summary>
    /// Enum of available log types
    /// </summary>
    public enum ELogType
    {
        Info,
        Debug,
        Error,
        Warning,
        Exception
    }

    #endregion Enums
}
