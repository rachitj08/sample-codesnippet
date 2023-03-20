using Core.Email.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Core.Email.EmailNotification
{
    public class SendgridEmailProvider : EmailProvider
    {
        public async override Task<(bool status, string responseJson)> SendEmail(EmailRequestModel emailRequestModel)
        {
            if (string.IsNullOrEmpty(ApiKey))
                throw new Exception("ApiKey of the Email provider can't be null.");

            try
            {
                var client = new SendGridClient(new SendGridClientOptions{ ApiKey = ApiKey, HttpErrorAsException = true });
                var from = new EmailAddress(emailRequestModel.From);
                var subject = emailRequestModel.Subject;
                var to = new EmailAddress(emailRequestModel.To[0]);
                var plainTextContent = emailRequestModel.Content;
                var htmlContent = emailRequestModel.HtmlContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                

                if(response.IsSuccessStatusCode)
                {
                    return (true, Newtonsoft.Json.JsonConvert.SerializeObject(response.Body));
                }
                else
                {
                    return (false, Newtonsoft.Json.JsonConvert.SerializeObject(response.Body));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
