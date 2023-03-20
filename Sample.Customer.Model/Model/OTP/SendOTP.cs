using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class SendMobileOtpMainVM : SendMobileOtpVM
    {
        public string OtpType { get; set; }
        public string ImeiNumber { get; set; }
    }

    public class SendMobileOtpVM 
    {
        public string CountryCode { get; set; }
        public string ContactNumber { get; set; }
    }

    public class SendEmailOtpVM
    {
        public string Email { get; set; }
    }
}
