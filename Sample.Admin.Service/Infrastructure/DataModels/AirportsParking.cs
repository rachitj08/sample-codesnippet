using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AirportsParking
    {
        public long AirportsParkingId { get; set; }
        public long AirportId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }

        public virtual Airports Airport { get; set; }
    }
}
