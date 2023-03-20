using Core.Email.Models;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Email.EmailNotification
{
    public class GmailOAuthEmailProvider : EmailProvider
    {
        public async override Task<(bool status, string responseJson)> SendEmail(EmailRequestModel emailRequestModel)
        {
            if (string.IsNullOrEmpty(GmailPrivateKey) || string.IsNullOrEmpty(GmailServiceAccountEmail))
                throw new Exception("Gmail private Key or service account email address can't be null.");
            
            if (string.IsNullOrEmpty(FromEmailAddress))
                throw new Exception("From email address can't be null.");

            try
            {
                var serviceAccountCredentialInitializer = new ServiceAccountCredential.Initializer(GmailServiceAccountEmail)
                {
                    User = FromEmailAddress,
                    Scopes = new[] { "https://mail.google.com/" }
                }.FromPrivateKey(GmailPrivateKey);

                var credential = new ServiceAccountCredential(serviceAccountCredentialInitializer);
                if (!credential.RequestAccessTokenAsync(CancellationToken.None).Result)
                    throw new InvalidOperationException("Access token failed.");

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    var oauth2 = new SaslMechanismOAuth2(FromEmailAddress, credential.Token.AccessToken);
                    client.Authenticate(oauth2);

                    if (credential.Token.IsExpired(Google.Apis.Util.SystemClock.Default))
                    {
                        //
                    }

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(FromEmailDisplayName ?? "Support", FromEmailAddress));
                    foreach (string toEmail in emailRequestModel.To)
                    {
                        message.To.Add(new MailboxAddress(toEmail, toEmail));
                    }
                    
                    if(emailRequestModel.CC != null && emailRequestModel.CC.Length > 0)
                    {
                        foreach (string ccEmail in emailRequestModel.CC)
                        {
                            message.Cc.Add(new MailboxAddress(ccEmail, ccEmail));
                        }
                    }
                    
                    if(emailRequestModel.BCC != null && emailRequestModel.BCC.Length > 0)
                    {
                        foreach (string bccEmail in emailRequestModel.BCC)
                        {
                            message.Bcc.Add(new MailboxAddress(bccEmail, bccEmail));
                        }
                    }
                    
                    message.Subject = emailRequestModel.Subject;

                    message.Body = new TextPart("plain")
                    {
                        Text = emailRequestModel.Content,

                    };
                    if(emailRequestModel.IsHtmlBody == true)
                    {
                        message.Body = new TextPart(TextFormat.Html)
                        {
                            Text = emailRequestModel.Content,

                        };
                    }
                    await client.SendAsync(message);
                    client.Disconnect(true);
                    return (true, "Success");
                }
            }
            catch (SmtpCommandException smptEx) 
            {
                // 
                return (false, smptEx.Message);
            }
            catch (Exception ex)
            {

                //throw;
                return (false, ex.Message);
            }
        }
    }
}
