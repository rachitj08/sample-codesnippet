using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Customer.Model
{
    public class GroupsModel
    {
        public long GroupId { get; set; }
        public string Description { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public List<GroupRightsModel> GroupRights { get; set; }
    }

    public class GroupsVM
    {
        public long GroupId { get; set; }
        public string Description { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public List<GroupRightsVM> GroupRights { get; set; }
    }
}
