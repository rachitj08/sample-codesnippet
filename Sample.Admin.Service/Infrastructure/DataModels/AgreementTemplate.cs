using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AgreementTemplate
    {
        public long AgreementTemplateId { get; set; }
        public long AgreementVersion { get; set; }
        public long AgreementTypeId { get; set; }
        public long StateId { get; set; }
        public DateTime AgreementEffectiveDate { get; set; }
        public DateTime AgreementExpirationDate { get; set; }
        public string TemplateText { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual AgreementType AgreementType { get; set; }
        public virtual State State { get; set; }
    }
}
