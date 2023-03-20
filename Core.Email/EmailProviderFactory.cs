using Core.Email.Enums;
using Core.Email.EmailNotification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Email.Factories
{
    public class EmailProviderFactory
    {
        public EmailProvider GetEmailProvider(EmailSenderConfig emailSenderConfig)
        {
            EmailProvider provider;

            switch (emailSenderConfig.EmailProviderName)
            {
                // Twilio uses sendgrid to send email
                case EmailProviderName.Twilio:
                case EmailProviderName.Sendgrid:
                    provider = new SendgridEmailProvider();
                    provider.ApiKey = emailSenderConfig.ApiKey;
                    return provider;
                    //break;
                case EmailProviderName.GmailOAuth:
                    provider = new GmailOAuthEmailProvider();
                    provider.GmailPrivateKey = emailSenderConfig.GmailPrivateKey;
                    provider.GmailServiceAccountEmail = emailSenderConfig.GmailServiceAccountEmail;
                    provider.FromEmailAddress = emailSenderConfig.FromEmailAddress;
                    provider.FromEmailDisplayName = emailSenderConfig.FromEmailDisplayName;
                    return provider;
                    //break;
                default:
                    break;
            }

            throw new NotImplementedException("This EmailProviderName is not implemented.");
        }
    }
}
