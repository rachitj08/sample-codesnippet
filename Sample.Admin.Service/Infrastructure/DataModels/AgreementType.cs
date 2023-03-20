using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AgreementType
    {
        public AgreementType()
        {
            AgreementTemplate = new HashSet<AgreementTemplate>();
        }

        public long AgreementTypeId { get; set; }
        public long Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<AgreementTemplate> AgreementTemplate { get; set; }
    }
}
