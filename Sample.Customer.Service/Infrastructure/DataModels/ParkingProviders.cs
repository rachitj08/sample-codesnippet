using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingProviders
    {
        public ParkingProviders()
        {
            ParkingProvidersLocations = new HashSet<ParkingProvidersLocations>();
        }

        public long ProviderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<ParkingProvidersLocations> ParkingProvidersLocations { get; set; }
    }
}
