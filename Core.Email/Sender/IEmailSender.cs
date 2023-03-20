using System;
using Core.Email.Models;

namespace Core.Email.EmailNotification
{
    public interface IEmailSender
    {
        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="emailData">EmailRequestModel with required data</param>
        (bool status, string responseJson) SendEmail(EmailRequestModel emailData);
    }
}