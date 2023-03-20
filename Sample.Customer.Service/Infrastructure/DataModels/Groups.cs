using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Groups
    {
        public Groups()
        {
            ApplicationGroupMapping = new HashSet<ApplicationGroupMapping>();
            GroupRights = new HashSet<GroupRights>();
            UserGroupMappings = new HashSet<UserGroupMappings>();
        }

        public long GroupId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }

        public virtual ICollection<ApplicationGroupMapping> ApplicationGroupMapping { get; set; }
        public virtual ICollection<GroupRights> GroupRights { get; set; }
        public virtual ICollection<UserGroupMappings> UserGroupMappings { get; set; }
    }
}
