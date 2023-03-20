using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Service.Infrastructure.Repository.ParkingProvider;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public static  class Bootstrapper
    {
        public static string defaultConnectionString;
        static Bootstrapper()
        {

        }

        public static void ConfigureServices(this IServiceCollection services, string defaultConnection = "Server=172.29.17.138; Database=SampleQA; user id=postgres; password=userpassdb;")
        {
            defaultConnectionString = defaultConnection;
            services.AddScoped<DbContextOptions>((serviceProvider) =>
             {
                 string connectionString = defaultConnection;

                 var efServices = new ServiceCollection();
                 efServices.AddEntityFrameworkNpgsql();

                 return new DbContextOptionsBuilder<CloudAcceleratorContext>()
                     .UseInternalServiceProvider(efServices.BuildServiceProvider())
                     .UseNpgsql(connectionString)
                     .Options;
             });
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRightsRepository, UserRightsRepository>();
            services.AddScoped<IUserGroupMappingRepository, UserGroupMappingRepository>();
            services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
            services.AddScoped<IFormsMasterRepository, FormsMasterRepository>();
            services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();
            services.AddScoped<IAuthenticationConfigKeysValuesRepository, AuthenticationConfigKeysValuesRepository>();
            services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IUserVehiclePreferenceCategoryRepository, UserVehiclePreferenceCategoryRepository>();
            services.AddScoped<IUserVehiclePreferenceFeaturesRepository, UserVehiclePreferenceFeaturesRepository>();

            services.AddScoped<IVehicleCategoryRepository, VehicleCategoryRepository>();
            services.AddScoped<IVehicleFeaturesRepository, VehicleFeaturesRepository>();
            services.AddScoped<IParkingHeadsRepository, ParkingHeadsRepository>();
            services.AddScoped<IParkingProvidersLocationsRepository, ParkingProvidersLocationsRepository>();
            services.AddScoped<IAirportsParkingRepository, AirportsParkingRepository>();
            services.AddScoped<IFlightReservationRepository, FlightReservationRepository>();
            services.AddScoped<IAirportsRepository, AirportsRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IReservationActivityCodeRepository, ReservationActivityCodeRepository>();
            services.AddScoped<IActivityCodeRepository, ActivityCodeRepository>();
            services.AddScoped<ITripPaxAndBagsRepository, TripPaxAndBagsRepository>();


            services.AddScoped<IEmailParserDetailRepository, EmailParserDetailRepository>();
            services.AddScoped<IParkingReservationRepository, ParkingReservationRepository>();
            services.AddScoped<IParkingProvidersSubLocationsRepository, ParkingProvidersSubLocationsRepository>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();
            services.AddScoped<IUserVehiclesRepository, UserVehiclesRepository>();
            services.AddScoped<IVehicleFeaturesMappingRepository, VehicleFeaturesMappingRepository>();
            services.AddScoped<IReservationVehicleRepository, ReservationVehicleRepository>();
            services.AddScoped<IVehicleTagMappingRepository, VehicleTagMappingRepository>();
            services.AddScoped<IParkingSpotsRepository, ParkingSpotsRepository>();
            services.AddScoped<IParkingHeadsRateRepository, ParkingHeadsRateRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPaymentDetailsRepository, PaymentDetailsRepository>();
            services.AddScoped<IAirlineRepository, AirlineRepository>();
            services.AddScoped<IParkingProviderRepository, ParkingProviderRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IReservationPaymentHistoryRepository, ReservationPaymentHistoryRepository>();
            //services.AddScoped<IParkingHeadsCustomRateRepository, ParkingHeadsCustomRateRepository>();
        }
    }
}
