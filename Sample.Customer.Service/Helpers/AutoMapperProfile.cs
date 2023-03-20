using AutoMapper;
using Sample.Admin.Model;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;
using DataModel = Sample.Customer.Service.Infrastructure.DataModels;
using DomainModel = Sample.Customer.Model;

namespace Sample.Customer.Service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Groups, DomainModel.GroupsModel>();
            CreateMap<GroupRights, DomainModel.GroupRightsModel>();
            CreateMap<DataModel.ValidationTypes, DomainModel.ValidationTypes>();
            CreateMap<PasswordPolicyVM, PasswordPolicyVM>();
            CreateMap<DataModel.Users, DomainModel.UsersModel>();
            CreateMap<DomainModel.UsersModel, DataModel.Users>();
            CreateMap<DomainModel.UserVM, DomainModel.UsersModel>();
            CreateMap<DomainModel.UsersModel, DomainModel.UserVM>();
            CreateMap<FormsMasterScreenControlsModel, ScreenControlsVM>();
            CreateMap<FormsMasterScreenControlsModel, Fields>();
            CreateMap<AddUpdateFlightReservationVM, AddFlightReservationVM>();
            CreateMap<Reservation, FlightAndParkingReservationVM>();
            CreateMap<FlightReservation, FlightReservationVM>();
            CreateMap<ParkingReservation, ParkingReservationVM>();
            CreateMap<ReservationVehicle, ReservationVehicleVM>();
            CreateMap<Airports, AirportsVM>();
            CreateMap<Address, AddressVM>();
            CreateMap<ParkingProvidersLocations, ParkingProvidersLocationsVM>();

            #region[Vehicle Category & Features]
            CreateMap<DataModel.VehicleCategory, Model.VehicleCategory>().ReverseMap();
            CreateMap<DataModel.VehicleFeatures, Model.VehicleFeatures>().ReverseMap();
            #endregion

        }
    }   
}
