using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class InComingOutGoingCarsVM
    {
        public List<CarDetailVM> InComingCars { get; set; }
        public List<CarDetailVM> OutGoingCars { get; set; }
    }
    public class CarDetailVM
    {
        public string CarTitle { get; set; }
        public string CarNumber { get; set; } 
        public string CarImagePath { get; set; }
        public string CarColorName { get; set; }
        public string CarParkedAreaLocNumber { get; set; }
        public string CarParkedKeyLocNumber { get; set; } 
        public string ActivityCode { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long AccountId { get; set; }
        public DateTime StartDateTime { get; set; }
        public long ReservationId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public long UserId { get; set; }
        public string LastActivityCode { get; set; }
        public string VIN { get; set; }
        public string TagID { get; set; }
        public long VehicleId { get; set; }
        public short AirportToParkingETAMin { get; set; }
        public DateTime? ReachedAtAirport { get; set; }
        public bool IsParked { get; set; }
        public string ValletLocation { get; set; }
        public string InvoicePath { get; set; }

    }

}
