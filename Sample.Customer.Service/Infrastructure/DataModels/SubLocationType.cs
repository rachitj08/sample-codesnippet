using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class SubLocationType
    {
        public long SubLocationTypeId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? Status { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
