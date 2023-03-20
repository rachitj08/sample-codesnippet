using Common.Model;
using Core.SMS;
using Core.SMS.Enums;
using System;

namespace Sample.Customer.Service
{
    public interface ISMSHelper
    {
        bool SendSMS(SMSConfig smsConfig, string mobileNo, string message);
        bool SendOTPSMS(SMSConfig smsConfig, string mobileNo, string message);
    }
    public class SMSHelper : ISMSHelper
    {  
        public bool SendSMS(SMSConfig smsConfig, string mobileNo, string message)
        {
            if (string.IsNullOrEmpty(smsConfig.SMSProvider.ProviderName))
            {
                //Console.WriteLine("SMSProviderName is required. Set it in appsettings.json file.");
                return false;
            }
            var smsProviderName = (SMSProviderName)Enum.Parse(typeof(SMSProviderName), smsConfig.SMSProvider.ProviderName);

            var smsConfigDetails = new SMSSenderConfig()
            {
                AccountId = smsConfig.SMSProvider.AccountId,
                ApiKey = smsConfig.SMSProvider.ApiKey,
                BaseUrl = smsConfig.SMSProvider.BaseUrl,
                FromMobileNumber = smsConfig.SMSProvider.FromMobileNumber,
            }; 

            var smsSender = new SMSSender(smsConfigDetails, smsProviderName);
            var res = smsSender.SendSMS(mobileNo, message);
            return res.status;
        }

        public bool SendOTPSMS(SMSConfig smsConfig, string mobileNo, string message)
        {
            if (!string.IsNullOrWhiteSpace(smsConfig.OTPConfig.DefaultOTP))
            {
                return true;
            }

            return SendSMS(smsConfig, mobileNo, message);
        }
    }
}
