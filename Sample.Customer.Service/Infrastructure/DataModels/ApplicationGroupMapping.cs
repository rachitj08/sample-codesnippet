using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ApplicationGroupMapping
    {
        public long ApplicationGroupMappingId { get; set; }
        public long ApplicationId { get; set; }
        public long GroupId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Groups Group { get; set; }
    }
}
