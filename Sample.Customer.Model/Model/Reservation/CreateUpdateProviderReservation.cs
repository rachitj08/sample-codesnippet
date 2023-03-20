using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class UpsertReservationVM
    {
        //For Reservation
         public long ReservationId { get; set; }
        public long ProviderLocationId { get; set; }
        public DateTime DepaurtureDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public long? SourceId { get; set; }
        public string Comment { get; set; }
        public string BookingConfirmationNo { get; set; }
        public string CommingFrom { get; set; }
        public bool IsCancel { get; set; }
        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }
        public string VehicleKeyLocation { get; set; }
        public string VehicleLocation { get; set; }
        public string ValletLocation { get; set; }
        public string PaymentMode { get; set; }
        public bool IsSystemRate { get; set; }
        public bool IsCustomRate { get; set; }
        public decimal CustomRate { get; set; }
        public string CustomRateType { get; set; }
    }
    public class UpsertUserVM
    {
        //For User
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileCode { get; set; }
        public string MobileNumber { get; set; }
        public long UserId { get; set; }
        public long  Country { get; set; }
        public string CountryName { get; set; }
        public long State { get; set; }
        public string StateName { get; set; }
        public long City { get; set; }
        public string CityName { get; set; }
        
    }
    public class UpsertVehicleVM
    {
        //To Create Vehicle
        public long VehicleId { get; set; }
        public string LicensePlateNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string CarColor { get; set; }
        public string Ticket { get; set; }
        public long? VehicleStateId { get; set; }
    }

    public partial class ReservationPaymentHistoryVM
    {
        public long ReservationPaymentHistoryId { get; set; }
        public long ReservationId { get; set; }
        public string PaymentType { get; set; }
        public string SourceName { get; set; }
        public string PaymentMode { get; set; }
        public bool? IsSystemRate { get; set; }
        public bool? IsCustomRate { get; set; }
        public decimal BaseRate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmout { get; set; }
        public DateTime PaymentOn { get; set; }
       
    }
    public class UpsertProviderReservationVM
    {
        public UpsertReservationVM UpsertReservationVM { get; set; }
        public UpsertUserVM UpsertUserVM { get; set; }
        public UpsertVehicleVM UpsertVehicleVM { get; set; }
        public List<ReservationPaymentHistoryVM> ReservationPaymentHistoryVM { get; set; }

    }
}
