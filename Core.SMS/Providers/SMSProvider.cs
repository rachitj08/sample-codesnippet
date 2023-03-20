namespace Core.SMS
{
    public abstract class SMSProvider
    {
        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public string AccountId { get; set; }

        public string FromPhoneNumber { get; set; }

        public abstract (bool status, string responseJson) SendSMS(string mobileNumber, string smsContent);
    }
}
