using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class UserBasicDataModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public long AccountId { get; set; }
    }
}
