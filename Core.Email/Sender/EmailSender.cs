using Core.Email.Factories;
using Core.Email.Models;
using System;

namespace Core.Email.EmailNotification
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderConfig emailSenderConfig;

        public EmailSender(EmailSenderConfig emailSenderConfig)
        {
            this.emailSenderConfig = emailSenderConfig;
        }

        public (bool status, string responseJson) SendEmail(EmailRequestModel emailData)
        {
            EmailProviderFactory factory = new EmailProviderFactory();
            EmailProvider emailProvider = factory.GetEmailProvider(emailSenderConfig);

            try
            {
                var res = emailProvider.SendEmail(emailData);
                return res.Result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}