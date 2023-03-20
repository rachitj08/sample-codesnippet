using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Admin.Model
{
    public class GroupsModel
    {
        public long GroupId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public List<GroupRightsModel> GroupRights { get; set; }
    }

    public class GroupsVM
    {
        public long GroupId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public List<GroupRightsVM> GroupRights { get; set; }
    }
}
