using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class GroupsRight
    {
        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// Group Module Id
        /// </summary>
        public long GroupModuleId { get; set; }
        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// Module Id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Created On date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Created by Used Id
        /// </summary>
        public long CreatedBy { get; set; }
        /// <summary>
        /// Updated on date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Update By User Id
        /// </summary>
        public long? UpdatedBy { get; set; }

    }
}
