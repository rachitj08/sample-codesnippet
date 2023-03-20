using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Vehicles
    {
        public Vehicles()
        {
            ReservationVehicle = new HashSet<ReservationVehicle>();
            Shuttle = new HashSet<Shuttle>();
            UserVehicles = new HashSet<UserVehicles>();
            VehicleFeaturesMapping = new HashSet<VehicleFeaturesMapping>();
            VehicleTagMapping = new HashSet<VehicleTagMapping>();
            VehiclesEventLog = new HashSet<VehiclesEventLog>();
            VehiclesMediaStorage = new HashSet<VehiclesMediaStorage>();
        }

        public long VehicleId { get; set; }
        public long? VehicleCategoryId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string RegistrationState { get; set; }
        public string LicensePlate { get; set; }
        public string Vinnumber { get; set; }
        public string Logo { get; set; }
        public string Color { get; set; }
        public int? NumberOfDoors { get; set; }
        public bool? IsTransmissionAutomatic { get; set; }
        public bool? IsConvertable { get; set; }
        public int? NumberOfBags { get; set; }
        public int? NumberOfPassenger { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }
        public string Ticket { get; set; }
        public long? VehicleStateId { get; set; }

        public virtual VehicleCategory VehicleCategory { get; set; }
        public virtual ICollection<ReservationVehicle> ReservationVehicle { get; set; }
        public virtual ICollection<Shuttle> Shuttle { get; set; }
        public virtual ICollection<UserVehicles> UserVehicles { get; set; }
        public virtual ICollection<VehicleFeaturesMapping> VehicleFeaturesMapping { get; set; }
        public virtual ICollection<VehicleTagMapping> VehicleTagMapping { get; set; }
        public virtual ICollection<VehiclesEventLog> VehiclesEventLog { get; set; }
        public virtual ICollection<VehiclesMediaStorage> VehiclesMediaStorage { get; set; }
    }
}
