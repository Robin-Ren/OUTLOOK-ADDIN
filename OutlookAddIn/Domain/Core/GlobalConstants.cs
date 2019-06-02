using System.Configuration;

namespace OutlookAddin.Domain
{
    public static class GlobalConstants
    {
        // App config settings
        public static string WebApiBaseUri = ConfigurationManager.AppSettings["webApiBaseUri"];
        public static string WebApiCondoId = ConfigurationManager.AppSettings["webApiCondoId"];

        /// <summary>
        /// Name of the standard config file
        /// </summary>
        public static readonly string ConfigFile = @"OutlookAddin.config";

        /// <summary>
        /// Log file to capture errors in the admin tool for help in troubleshooting.
        /// </summary>
        public static readonly string ErrorLogFile = @"OutlookAddin.log";

        #region Connection Strings
        public static readonly string OutlookAddinConnectionStringName = "OutlookAddinConnectionString";
        #endregion

        #region Error Notification Email property names
        public static readonly string ErrorEmailHostName = "ErrorMailHost";

        public static readonly string ErrorEmailFromAddressName = "ErrorMailFromAddress";

        public static readonly string ErrorEmailToAddressName = "ErrorMailToAddress";

        public static readonly string ErrorEmailUserNameName = "ErrorEmailUserName";

        public static readonly string ErrorEmailPasswordName = "ErrorEmailPassword";

        public static readonly string ErrorEmailPortName = "ErrorEmailPort";

        public static readonly string ErrorEmailRequireSSLName = "ErrorEmailRequireSSL";

        #endregion

        #region Messages
        public static readonly string LoginFailedMessage = @"User ID or password is incorrect!";
        #endregion
    }
}