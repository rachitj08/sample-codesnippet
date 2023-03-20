using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class AuthenticationConfig
    {
        /// <summary>
        /// To Set Identity Authority Url
        /// </summary>
        public string AuthorityUrl { get; set; }
        public string Issuer { get; set; }

        /// <summary>
        /// To set ClientSecret for token api
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// To set Expiry Time(in Minutes)
        /// </summary>
        public int ExpiryTime { get; set; }

        /// <summary>
        /// To set Refresh Expiry Time(in Hours)
        /// </summary>
        public int RefreshExpiryTime { get; set; }
    }
}
