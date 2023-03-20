using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Applications
    {
        public Applications()
        {
            ApplicationGroupMapping = new HashSet<ApplicationGroupMapping>();
        }

        public long ApplicationsId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<ApplicationGroupMapping> ApplicationGroupMapping { get; set; }
    }
}
