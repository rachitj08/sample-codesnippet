using Core.SMS.Enums;
using Core.SMS.Factories;
using System;

namespace Core.SMS
{
    public class SMSSender : ISMSSender
    {
        private readonly string apiKey;
        private readonly string accountId;
        private readonly string baseUrl;

        private readonly string fromPhoneNumber;
        private readonly SMSProviderName smsProviderName;

        private readonly SMSSenderConfig smsSenderConfig;

        /// <summary>
        /// Instantiate the class
        /// </summary>
        /// <param name="smsSenderConfig">new SMSSenderConfig for injecting API Key, Account ID and baseUrl</param>
        public SMSSender(SMSSenderConfig smsSenderConfig, SMSProviderName smsProviderName = SMSProviderName.Twilio)
        {
            this.apiKey = smsSenderConfig.ApiKey;
            this.accountId = smsSenderConfig.AccountId;
            this.baseUrl = smsSenderConfig.BaseUrl;
            this.smsProviderName = smsProviderName;
            this.fromPhoneNumber = smsSenderConfig.FromMobileNumber;
            this.smsSenderConfig = smsSenderConfig;
        }

        public (bool status, string responseJson) SendSMS(string mobileNumber, string smsContent)
        {
            SMSProviderFactory factory = new SMSProviderFactory();
            SMSProvider smsProvider = factory.GetSMSProvider(smsSenderConfig, smsProviderName);

            try
            {
                return smsProvider.SendSMS(mobileNumber, smsContent);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            

            
        }
    }
}
