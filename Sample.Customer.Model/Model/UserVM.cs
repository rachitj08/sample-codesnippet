using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class UserVM: UsersModel
    {
        public List<UserGroupMappingModel> UserGroups { get; set; }
        public long UserId { get; set; }
    }
}
