using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Address
    {
        public Address()
        {
            Airports = new HashSet<Airports>();
            ParkingProvidersLocations = new HashSet<ParkingProvidersLocations>();
            UserDrivingLicense = new HashSet<UserDrivingLicense>();
        }

        public long AddressId { get; set; }
        public long AccountId { get; set; }
        public string Streat1 { get; set; }
        public string Streat2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<Airports> Airports { get; set; }
        public virtual ICollection<ParkingProvidersLocations> ParkingProvidersLocations { get; set; }
        public virtual ICollection<UserDrivingLicense> UserDrivingLicense { get; set; }
    }
}
