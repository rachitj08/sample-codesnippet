using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Admin.Model.Account.New
{
    public class NewSubscription
    {
        /// <summary>
        /// Version identifier
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// Sub - Description added for subscriptions
        /// </summary>
        [StringLength(500, ErrorMessage = "Subscription description maximum length is 500")]
        public string Description { get; set; }

        /// <summary>
        /// subscription start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// subscription end date
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
