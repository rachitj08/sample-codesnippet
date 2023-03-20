using Core.Email.Enums;

namespace Core.Email.EmailNotification
{
    public class EmailSenderConfig
    {
        public string ApiKey { get; set; }
        public string GmailPrivateKey { get; set; }

        public string FromEmailAddress { get; set; }
        public string FromEmailDisplayName { get; set; }
        public string GmailServiceAccountEmail { get; set; }

        public EmailProviderName EmailProviderName { get; set; }
    }
}
