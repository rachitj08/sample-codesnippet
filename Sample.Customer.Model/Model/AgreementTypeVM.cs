using System;

namespace Sample.Customer.Model
{
    public class AgreementTypeVM
    {
        public long AgreementTypeId { get; set; }
        public long Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
