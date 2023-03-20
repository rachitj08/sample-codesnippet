using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class UsersGroupsMapping
    {
        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// User Group Mapping Id
        /// </summary>
        public long UsersGroupsMappingId { get; set; }
        /// <summary>
        /// User Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupId { get; set; }
    }
}
