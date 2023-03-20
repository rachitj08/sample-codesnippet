using System;

namespace Sample.Admin.Model.Account.Domain
{
    public class Group
    {
        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Group Created on Date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Group Created by User Id
        /// </summary>
        public long CreatedBy { get; set; }
        /// <summary>
        /// Group Updated on Date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Group Updated by User ID
        /// </summary>
        public long? UpdatedBy { get; set; }
        /// <summary>
        /// Account id 
        /// </summary>
        public long AccountId { get; set; }
    }
}
