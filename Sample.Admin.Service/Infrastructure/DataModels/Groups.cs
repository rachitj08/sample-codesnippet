using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Groups
    {
        public Groups()
        {
            GroupRights = new HashSet<GroupRights>();
            UserGroupMappings = new HashSet<UserGroupMappings>();
        }

        public long GroupId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }

        public virtual ICollection<GroupRights> GroupRights { get; set; }
        public virtual ICollection<UserGroupMappings> UserGroupMappings { get; set; }
    }
}
