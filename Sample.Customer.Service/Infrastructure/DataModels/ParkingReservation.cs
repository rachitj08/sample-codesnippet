using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingReservation
    {
        public ParkingReservation()
        {
            Invoice = new HashSet<Invoice>();
        }

        public long ParkingReservationId { get; set; }
        public long AirportsParkingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsConcentedToRent { get; set; }
        public long IsActive { get; set; }
        public long? AgreementTemplateId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public long? ReservationId { get; set; }
        public long? ParkingProvidersLocationId { get; set; }
        public string VehicleKeyLocation { get; set; }
        public string VehicleLocation { get; set; }
        public bool IsParked { get; set; }
        public string ValletLocation { get; set; }
        public string Comment { get; set; }
        public string BookingConfirmationNo { get; set; }
        public long? SourceId { get; set; }
        public string Source { get; set; }
        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }

        public virtual AgreementTemplate AgreementTemplate { get; set; }
        public virtual AirportsParking AirportsParking { get; set; }
        public virtual ParkingProvidersLocations ParkingProvidersLocation { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
