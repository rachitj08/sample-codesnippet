using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class VehicleCategory
    {
        public VehicleCategory()
        {
            RentalReservation = new HashSet<RentalReservation>();
            Vehicles = new HashSet<Vehicles>();
        }

        public long VehicleCategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<RentalReservation> RentalReservation { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}
