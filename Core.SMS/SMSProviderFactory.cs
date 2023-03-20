using Core.SMS.Enums;
using System;

namespace Core.SMS.Factories
{
    public class SMSProviderFactory
    {
        public SMSProvider GetSMSProvider(SMSSenderConfig smsSenderConfig, SMSProviderName smsProviderName)
        {
            SMSProvider provider;

            switch (smsProviderName)
            {
                case SMSProviderName.Twilio:
                    provider = new TwilioSMSProvider();
                    provider.ApiKey = smsSenderConfig.ApiKey;
                    provider.BaseUrl = smsSenderConfig.BaseUrl;
                    provider.AccountId = smsSenderConfig.AccountId;
                    provider.FromPhoneNumber = smsSenderConfig.FromMobileNumber;
                    return provider;
                    //break;
                default:
                    break;
            }

            throw new NotImplementedException("This SMSProviderName is not implemented.");
        }
    }
}
