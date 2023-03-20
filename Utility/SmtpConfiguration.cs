namespace Utilities
{
    /// <summary>
    /// SmtpConfiguration object that contains properties   for sending the email.
    /// </summary>
    /// <remarks></remarks>
    public class SmtpConfiguration
    {
        /// <summary>
        /// SmtpServer
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// SmtpUserName
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// SmtpPassword
        /// </summary>
        public string SmtpPassword { get; set; }
         
        /// <summary>
        /// EmailFromAddress
        /// </summary>
        public string EmailFromAddress { get; set; }
         
        /// <summary>
        /// EmailPort
        /// </summary>
        public string EmailPort { get; set; }

        /// <summary>
        /// IsBodyHtml
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// EnableSsl
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// UseDefaultCredentials
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// ServiceAccount For new mail MailKit
        /// </summary>
        public string ServiceAccount { get; set; }
        /// <summary>
        /// GSuiteUser MailKit
        /// </summary>
        public string GSuiteUser { get; set; }
        /// <summary>
        /// PrivateKey MailKit
        /// </summary>
        public string PrivateKey { get; set; }

    }
}
