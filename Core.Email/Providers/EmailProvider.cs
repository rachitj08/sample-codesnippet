using Core.Email.Models;
using System.Threading.Tasks;

namespace Core.Email.EmailNotification
{
    public abstract class EmailProvider
    {
        public string ApiKey { get; set; }

        public string GmailPrivateKey { get; set; }

        public string FromEmailAddress { get; set; }

        public string FromEmailDisplayName { get; set; }
        
        public string GmailServiceAccountEmail { get; set; }

        public abstract Task<(bool status, string responseJson)> SendEmail(EmailRequestModel emailRequestModel);
    }
}
