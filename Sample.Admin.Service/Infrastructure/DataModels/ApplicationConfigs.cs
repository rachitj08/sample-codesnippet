using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class ApplicationConfigs
    {
        public long ApplicationConfigId { get; set; }
        public string ApplicationName { get; set; }
        public string CopyrightDescription { get; set; }
        public string DefaultLogo { get; set; }
        public string DefaultOrgName { get; set; }
        public string DefaultCss { get; set; }
    }
}
