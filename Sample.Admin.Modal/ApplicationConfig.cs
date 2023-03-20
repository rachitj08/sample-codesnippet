using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class ApplicationConfig
    {
        public string ApplicationName { get; set; }
        public string CopyrightDescription { get; set; }
        public string DefaultLogo { get; set; }
        public string DefaultOrgName { get; set; }
        public Dictionary<string, string> DefaultCss { get; set; }
    }
}
