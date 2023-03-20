using System;
using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class OngoingUpcomingTripVM
    {
        public List<TripDetailVM> OngoingTrips { get; set; }
        public List<TripDetailVM> UpcomingTrips { get; set; }
        public List<TripDetailVM> CompletedTrips { get; set; }

    }

    public class TripDetailVM
    {
        public long ReservationId { get; set; }
        public long FlightReservationId { get; set; }
        public DateTime DepaurtureDateTime { get; set; }
        public string FlightNo { get; set; }
        public string DepaurtureCity { get; set; }
        public string DepaurtureAirportName { get; set; }
        public string DepaurtureAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string AirlineCode { get; set; }
        public string DepaurtureAirportAddress { get; set; }
        public string ParkingLocationAddress { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public string ActivityCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string InvoicePath { get; set; }
    }

}
