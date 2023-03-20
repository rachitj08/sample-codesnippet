using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class Subscription
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public long SubscriptionId { get; set; }
        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// Version Id
        /// </summary>
        public int VersionId { get; set; }
        /// <summary>
        /// Subscription Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// subscription start date
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Subscription end Date
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Subscription Cancelled on date
        /// </summary>
        public DateTime? CancelledOn { get; set; }
        /// <summary>
        /// Subscripton Cancelled Reason
        /// </summary>
        public string CancelledReason { get; set; }
        /// <summary>
        /// Created on Date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Created by User Id
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Updated on Date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Update by User Id
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Cancelled
        /// </summary>
        public bool Cancelled { get; set; }
    }
}
