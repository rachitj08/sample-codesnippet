using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Core.SMS
{
    public class TwilioSMSProvider : SMSProvider
    {
        public override (bool status, string responseJson) SendSMS(string mobileNumber, string smsContent)
        {
            if (string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(AccountId) || string.IsNullOrEmpty(FromPhoneNumber))
                throw new Exception("BaseUrl, AccountId, From phone number or ApiKey of the SMS provider can't be null.");

            try
            {
                TwilioClient.Init(AccountId, ApiKey);

                var message = MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber(FromPhoneNumber),
                    body: smsContent,
                    to: new Twilio.Types.PhoneNumber(mobileNumber)
                );

                if(message.Status == MessageResource.StatusEnum.Accepted || message.Status == MessageResource.StatusEnum.Queued)
                {
                    return (true, Newtonsoft.Json.JsonConvert.SerializeObject(message));
                }
                else
                {
                    return (false, Newtonsoft.Json.JsonConvert.SerializeObject(message));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
