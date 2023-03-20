using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class AgreementTemplate
    {
        public AgreementTemplate()
        {
            ParkingReservation = new HashSet<ParkingReservation>();
            RentalReservation = new HashSet<RentalReservation>();
        }

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
        public long AccountId { get; set; }

        public virtual AgreementType AgreementType { get; set; }
        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
        public virtual ICollection<RentalReservation> RentalReservation { get; set; }
    }
}
