using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Admin.Model
{
    public class AccountServicesModel
    {
        public int AccountServiceId { get; set; }
        public long Account { get; set; }
        public int Service { get; set; }
        public string DbServer { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; }
        public string DbSchema { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AccountServicesVM
    {
        public int AccountServiceId { get; set; }
        public long AccountId { get; set; }
        public long? Account { get; set; }
        public string AccountName { get; set; }
        public int Service { get; set; }
        public string ServiceName { get; set; }
        public string DbServer { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; }
        public string DbSchema { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsolationType { get; set; }
    }
}
