using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Utilities;

namespace Utilities.EmailHelper
{
    public interface IEmailHelper
    {
        /// <summary>
        /// SendEmail use to send email 
        /// </summary>
        /// <param name="emailTo">Email To</param>
        /// <param name="emailSubject">Email Subject</param>
        /// <param name="emailBody">Email Body</param>
        /// <param name="emailCc">Email CC</param>
        /// <param name="filePath">Attachment File Path list</param>
        bool SendMail(string emailTo, string emailSubject, string emailBody, string cc = "", List<string> filePath = null);
        /// <summary>
        ///GetMailMessage use to get mail message by using the email settings  
        /// </summary>
        /// <param name="smtpSetting">smtpconfiguration setting</param>
        /// <param name="emailTo">Email Send to id</param>
        /// <param name="emailSubject">Email Subject</param>
        /// <returns>MailMessage</returns>  
        MailMessage GetMailMessage(SmtpConfiguration smtpSetting, string emailTo, string emailSubject);

        /// <summary>
        /// To populate body text from Email Template file
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="replaceText">Key value pair to replace</param>
        /// <returns>string</returns>
        string PopulateBody(string filePath, Dictionary<string, string> replaceText);
        /// <summary>
        /// MailKit
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="cc"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool EmailSenderOld(string emailTo, string emailSubject, string emailBody, string cc = "", List<string> filePath = null);
    }
}
