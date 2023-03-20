using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AccountServices
    {
        public int AccountServiceId { get; set; }
        public long AccountId { get; set; }
        public int ServiceId { get; set; }
        public string DbServer { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; }
        public string DbSchema { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
