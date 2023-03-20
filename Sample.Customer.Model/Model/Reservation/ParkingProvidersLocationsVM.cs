using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class ParkingProvidersLocationsVM
    {
        public long ParkingProvidersLocationId { get; set; }
        public long ProviderId { get; set; }
        public long AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public long Capcity { get; set; }
        public bool IsValet { get; set; }
        public bool IsSelf { get; set; }
        public bool? IsRentingTrunOn { get; set; }
        public short? AirportToParkingEtamin { get; set; }
        public string Name { get; set; }
        public int InTimeGapInMin { get; set; }
        public int OutTimeGapInMin { get; set; }
    }
}
