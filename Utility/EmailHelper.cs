using Google.Apis.Auth.OAuth2;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Utilities.EmailHelper
{
    /// <summary>
    /// EmailHelper object that implement methods  for sending the email.
    /// </summary>
    /// <remarks></remarks>
    public class EmailHelper : IEmailHelper
    {
        private readonly SmtpConfiguration smtpConfiguration;
        public EmailHelper(IOptions<SmtpConfiguration> config)
        {
            this.smtpConfiguration = config.Value;
        }
        public EmailHelper()
        { }
        /// <summary>
        /// SendEmail use to send email 
        /// </summary>
        /// <param name="emailTo">Email To</param>
        /// <param name="emailCc">Email CC</param>
        /// <param name="emailSubject">Email Subject</param>
        /// <param name="emailBody">Email Body</param>
        /// <param name="filePath">Attachment File Path list</param>
        public bool EmailSenderOld(string emailTo, string emailSubject, string emailBody, string emailCc = "", List<string> filePath = null)
        {
            bool isEmailSend = false;
            try
            {
                Console.WriteLine("SendMail Method called."+emailTo);
                var mailMessage = GetMailMessage(smtpConfiguration, emailTo, emailSubject);
                mailMessage.Body = emailBody;
                if (!string.IsNullOrWhiteSpace(emailCc))
                {
                    string[] CCId = emailCc.Split(',');
                    foreach (string CCEmail in CCId)
                        mailMessage.CC.Add(new MailAddress(CCEmail));
                }
                if (filePath != null && filePath.Count > 0)
                {
                    foreach (string path in filePath)
                        mailMessage.Attachments.Add(new Attachment(path));
                }
               
                var smtpClient = new SmtpClient(smtpConfiguration.SmtpServer, Convert.ToInt32(smtpConfiguration.EmailPort));
                smtpClient.UseDefaultCredentials = true;
                smtpClient.UseDefaultCredentials = smtpConfiguration.UseDefaultCredentials;
                smtpClient.Host = smtpConfiguration.SmtpServer;
                smtpClient.EnableSsl = smtpConfiguration.EnableSsl;
                smtpClient.Credentials = new NetworkCredential(smtpConfiguration.SmtpUserName, smtpConfiguration.SmtpPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
                isEmailSend = true;
                Console.WriteLine("SendMail Method End." + smtpConfiguration.SmtpUserName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
            return isEmailSend;
        }
        /// <summary>
        ///GetMailMessage use to get mail message by using the email settings  
        /// </summary>
        /// <param name="smtpSetting">smtpconfiguration setting</param>
        /// <param name="emailTo">Email Send to id</param>
        /// <param name="emailSubject">Email Subject</param>
        /// <returns>MailMessage</returns>       
        public MailMessage GetMailMessage(SmtpConfiguration smtpSetting, string emailTo, string emailSubject)
        {
            var mailMessage = new MailMessage(smtpSetting.EmailFromAddress, emailTo)
            {
                Subject = emailSubject,
                IsBodyHtml = smtpSetting.IsBodyHtml,

            };
            return mailMessage;
        }
        /// <summary>
        /// To populate body text from Email Template file
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="replaceText">Key value pair to replace</param>
        /// <returns>string</returns>
        public string PopulateBody(string filePath, Dictionary<string, string> replaceText)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = reader.ReadToEnd();
            }
            foreach (KeyValuePair<string, string> keyValuePair in replaceText)
                body = body.Replace(keyValuePair.Key, keyValuePair.Value);
            return body;
        }

        public bool SendMail(string emailTo, string emailSubject, string emailBody, string emailCc = "", List<string> filePath = null)
        {
            bool isEmailSend = false;
            try
            {
                string serviceAccount = smtpConfiguration.ServiceAccount;
                // G Suite user email address
                var gsuiteUser = smtpConfiguration.GSuiteUser;
                string privateKey = smtpConfiguration.PrivateKey; //"-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQC6m4/IZG3fenV1\nobCQ24YgXGBEtZzSKLe/lYq75TmyUIdhEJDeg9eTR4N9oRzdt2yMo06qVIUDvXt1\nqKOl6xvGapihLJBv2IqnAgPxcjVGs7QrwOhsPqzz4KnjAUggZhBOtojAzOvjiRrZ\nd7W6wTNoVzWM4/lY1ILNDT4h26V6YMQ4k0fI9ShRvi23iEYBQXAwkuwXKRX3Btb0\nPK9R2pz3BHNDvgS7XHovBx94SaK3+31iWzfqwSTKOtdQrXM8XXXgSsNUGjOI2YB3\nh7uvKSLNKmWFLE+jpBp01m2n+qLqRY7bAEP/IyiH4aUStsywJ8cUizHIbbkVOzUH\nTYft1RjHAgMBAAECggEACpVPcNboJXqHpAEbR04whzAs39Qk/+CUch1x7/j3296D\nnMQ9Loqmhv714Ur8eGfEuR0x8KjLDLplgK5+7hQI4l+h3YkS624cPvrSkUj7eIh6\nS7RjsQIysOWW/P2/nk/rjEa2gRZ5lvW1F0MsOxFBcEXEM3GrpbwTInMmGzvDPDyd\nDF4M3LQK7gfLcmxF6hF90//k3lbr2JAumIt1wbFbh/hthUdYCE2LjPP4WDMPP97o\n2UG+5cfhS6UA7YWZ/TnuHSk2iTzSZVcBofqRHD6zSaik3qOi4S/vtCXG5sv1crsJ\nb3/kU3pdgk0StdQMfN1kUeLbQSkVn86vBi+8wYfXYQKBgQDih+y3qD84UmCoxCTG\n1nNwzyNUTeTqI5e0suIPYmK1uW83jq+cTAucOeOMBMDmGBMNGa+OTfq+64gkE9c3\nvXDBC+NbvXTRjzpI5+o7PG5+kFXBL6t+VXudEqkBFgKbFHpmu9BehikxCniBXNrX\nvsrUa26llQrqtpeXhzspCskyGQKBgQDS4hYnpB8lNCIMB6hFK5foc1zWn7MgY4T+\n5tQFfxXbxlpFhwb/Rs6QmxAzlbKwu9IxBm43Dlideaqw3BRkRaVCV7oOabl15gEN\nLxjkC4wOEITZCyRXW9t8GT6hNeRf2YnZ9eN06h7gAbWy2Z9EAgUAP87+fkyN9X6P\n4blWoga93wKBgAGEdZJ5XwsKijk3LyPh1d4glRUAfJMQ7/g25orB1vVuq48MtqJu\nY/mE7cBHW99us9n3nRRv80kKHNxZAa0O6M1yPfCkYT7yHarVWt1Qt1DDuL9ViWmM\nOwq/UECAnD3bskIz6d5oOahpcIs1SwX88jDgE1qql+sgk/1g24WX2+cpAoGAJJ5F\nopSJI/itgNFDkt9C/YYqtMn+XL1rNqf2YQ30Sbljq3cK7ADrlYiXg52W7cFvejvX\nn4/KK24ZCaTtlYvSana5RA9vuN07iYKIG/E1XKvZpaadpN26ew4XJf8prMsleMeN\nu5t7yQIC/w9y7Gg9XhG2a9KakHv0gh98A2MXMJkCgYBCeAC7MWVO4mv3x3RRi16U\nHhlUW66Jzuo+ONBdpHiNSBY74sN+cjQPN77lulqvSIt9zYaWkAdPVIUU91ZDtXWv\nZlhTUhAMcOgXl8WxafcbAh1Tc+UmuLlQtiyXGfm3pkhakRxW7QeLXAC002t3zPi5\ntpyTI6P29R7B3TBMmTsBRw==\n-----END PRIVATE KEY-----\n";
                var serviceAccountCredentialInitializer = new ServiceAccountCredential.Initializer(serviceAccount)
                {
                    User = gsuiteUser,
                    Scopes = new[] { "https://mail.google.com/" }
                }.FromPrivateKey(privateKey);

                var credential = new ServiceAccountCredential(serviceAccountCredentialInitializer);
                if (!credential.RequestAccessTokenAsync(CancellationToken.None).Result)
                    throw new InvalidOperationException("Access token failed.");

              
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    // use the access token
                    var oauth2 = new SaslMechanismOAuth2(gsuiteUser, credential.Token.AccessToken);
                    client.Authenticate(oauth2);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(smtpConfiguration.GSuiteUser, gsuiteUser));
                    message.To.Add(new MailboxAddress("", emailTo));
                    message.Subject = emailSubject;

                    var builder = new BodyBuilder();
                    builder.HtmlBody = emailBody;


                    if (filePath != null && filePath.Count > 0)
                    {
                        foreach (string path in filePath)
                            builder.Attachments.Add(path);
                    }

                    message.Body = builder.ToMessageBody();



                    //message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    //{
                    //    Text = emailBody,

                    //};


                    client.Send(message);
                    client.Disconnect(true);
                    isEmailSend = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email error : MimeMessage " + ex.Message);
                //endDate = DateTime.UtcNow;
                //EmailLogging(startDate, endDate, false, ex.Message);
            }
            return isEmailSend;
        }

    }
}
