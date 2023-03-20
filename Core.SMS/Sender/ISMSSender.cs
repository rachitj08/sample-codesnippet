namespace Core.SMS
{
    public interface ISMSSender
    {
        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="mobileNumber">Mobile Number to SMS to</param>
        /// <param name="smsContent">Text Content of the SMS</param>
        (bool status, string responseJson) SendSMS(string mobileNumber, string smsContent);
    }
}
