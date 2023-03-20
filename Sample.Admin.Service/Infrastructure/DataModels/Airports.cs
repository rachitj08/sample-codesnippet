using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Airports
    {
        public Airports()
        {
            AirportsParking = new HashSet<AirportsParking>();
        }

        public long AirportId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AddressId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<AirportsParking> AirportsParking { get; set; }
    }
}
