using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AuthenticationTypes
    {
        public int AuthenticationTypeId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string HelpUrl { get; set; }
    }
}
