using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ControlTypePickListOptions
    {
        public short ControlTypeFieldId { get; set; }
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        public short DisplayOrder { get; set; }
    }
}
