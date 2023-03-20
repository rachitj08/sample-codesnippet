using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class VehiclesVM
    {
        public long VehicleId { get; set; }
        public long VehicleCategoryId { get; set; }
        public DateTime Make { get; set; }
        public long Model { get; set; }
        public DateTime Year { get; set; }
        public long RegistrationState { get; set; }
        public string LicensePlate { get; set; }
        public string Vinnumber { get; set; }
        public string Logo { get; set; }
        public string Color { get; set; }
        public int NumberOfDoors { get; set; }
        public bool IsTransmissionAutomatic { get; set; }
        public bool IsConvertable { get; set; }
        public int? NumberOfBags { get; set; }
        public int? NumberOfPassenger { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }
    }

    public class VehicleDetailVM
    {
        public string CarTitle { get; set; }
        public string CarNumber { get; set; }
        public string CarImagePath { get; set; }
        public string CarColorName { get; set; } 
        public long UserId { get; set; } 
        public string VIN { get; set; }
        public string TagID { get; set; }
        public long VehicleId { get; set; }

    }


    public class CreateReservationVehicleReqVM
    {
        public long ReservationId { get; set; }
        public long VehicleId { get; set; }
    }
    public class CreateVehicleReqVM
    {
        public string VinNumber { get; set; }
        public string CarColor { get; set; }
        public string TabId { get; set; }
        public long ReservationId { get; set; }
        public string LicensePlateNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public int NumberOfDoors { get; set; }
        public int NumberOfBags { get; set; }
        public int NumberOfPassenger { get; set; }
        public bool Transmission { get; set; }
        public string Ticket { get; set; }
        public long VehicleStateId { get; set; }
    }
}
