using System.Collections.Generic;

namespace Sample.Customer.Model
{
    /// <summary>
    /// Tenant based JwtBearer Token Settings
    /// </summary>
    public class TenantJwtBearerSettingsModel
    {

        /// <summary>
        /// Authority Detail
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Audience Detail
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Valid Audiences
        /// </summary>
        public List<string> ValidAudience { get; set; }

        /// <summary>
        /// Valid Issuer
        /// </summary>
        public List<string> ValidIssuer { get; set; }

        /// <summary>
        /// Is Validate Issuer
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Is Validate Audience
        /// </summary>
        public bool ValidateAudience { get; set; }

        public string IssuerSigningKey { get; set; }
    }
}
