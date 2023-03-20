using Common.Model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sample.Customer.Model.Model.ParkingProvider;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository.ParkingProvider
{
    public class ParkingProviderRepository : RepositoryBase<ParkingProviders>, IParkingProviderRepository
    {
        public ParkingProviderRepository(CloudAcceleratorContext context) : base(context)
        {

        }
        public IEnumerable<DropDownMaster> GetParkingProvider()
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""ProviderId"" as ""Id"",""Name"" FROM customer.""ParkingProviders"" ORDER BY ""ProviderId"" ASC ";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }

        public IEnumerable<DropDownMaster> GetParkingProviderLocation(long providerId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""ParkingProvidersLocationId"" as ""Id"",""Name"" FROM customer.""ParkingProvidersLocations"" ORDER BY ""ParkingProvidersLocationId"" ASC ";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }

        public IEnumerable<DropDownMaster> GetSubLocation(long parkingProviderlocationId)
        {
            List<DropDownMaster> subLocations = new List<DropDownMaster>();

            subLocations.Add(new DropDownMaster { Id = "1", Name = "Vehicle Key Location Scanned" });
            subLocations.Add(new DropDownMaster { Id = "5", Name = "Arrive At Parking" });
            subLocations.Add(new DropDownMaster { Id = "6", Name = "Parked At Vallet" });
            subLocations.Add(new DropDownMaster { Id = "8", Name = "Boarded Shuttle To Terminal" });
            subLocations.Add(new DropDownMaster { Id = "9", Name = "Vallet Location Scanned" });
            subLocations.Add(new DropDownMaster { Id = "10", Name = "Vehicle Scanned By Vallet" });
            subLocations.Add(new DropDownMaster { Id = "11", Name = "Vehicle Key Scanned" });
            subLocations.Add(new DropDownMaster { Id = "12", Name = "Vehicle Storage Location Scanned" });
            subLocations.Add(new DropDownMaster { Id = "14", Name = "Request Shuttle" });
            subLocations.Add(new DropDownMaster { Id = "15", Name = "Boarded Shuttle To Parking" });
            subLocations.Add(new DropDownMaster { Id = "16", Name = "Vehicle Retrival Key Scanned" });
            subLocations.Add(new DropDownMaster { Id = "17", Name = "Vehicle Retrival Vallet Location Scanned" });
            subLocations.Add(new DropDownMaster { Id = "19", Name = "Vehicle Pickedup By Customer" });
            subLocations.Add(new DropDownMaster { Id = "20", Name = "Vehicle Left Parking / Journey Completed" });
            subLocations.Add(new DropDownMaster { Id = "23", Name = "Begin Journey" });
            return subLocations;
           
        }

        public IEnumerable<DropDownMaster> GetActivityCode()
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""Code"" as ""Id"",""Description"" as ""Name"" from customer.""ActivityCode"" WHERE ""ScanType""='QRCode' order by ""Odering"" ASC";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }
        public IEnumerable<DropDownMaster> GetParkingSpotId(long subLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""ParkingSpotId"" as ""Id"",""Name""   FROM customer.""ParkingSpots"" order by ""ParkingSpotId"" ASC";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }
        public IEnumerable<DropDownMaster> GetParkingSpotType(long accountId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select ""ParkingSpotTypeId"" as ""Id"",""Name"" from customer.""ParkingSpotType"" where ""AccountId""={accountId}";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }
        public IEnumerable<DropDownMaster> GetParkingSpotByLocationandSpot(long locationId,long spotType,long accountId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select ""ParkingSpotId"" as ""Id"",""Name""   FROM customer.""ParkingSpots"" as spot
join customer.""ParkingProvidersLocationsSubLocations"" as sub
ON spot.""ParkingProvidersLocationSubLocationId""=sub.""ParkingProvidersLocationSubLocationId""
Where ""ParkingProviderLocationId""={locationId} and  COALESCE(spot.""IsOccupied"",false)=false AND spot.""ParkingSpotTypeId""={spotType} and spot.""AccountId""={accountId}
order by ""ParkingSpotId"" ASC";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }
        public IEnumerable<QRListVM> GetQRList(long parkingProviderId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""QRCodeMapping"" as ""EncryptedData"",""QRCodePath"" as ""QRPath"",""SubLocationName"" as ""Name"" FROM customer.""ParkingProvidersLocationsSubLocations"" WHERE ""ParkingProviderLocationId""={parkingProviderId} and ""QRCodeEncryptedValue"" is not null";
                return connection.Query<QRListVM>(querySearch).ToList();
            }
        }
        public List<Common.Model.SubLocationType> GetSubLocationType()
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"Select ""SubLocationTypeId"" , ""Name"",""Code"",""Status"" from customer.""SubLocationType"" where ""Status""=true ";
                return connection.Query<Common.Model.SubLocationType>(querySearch).ToList();
            }
        }

        public IEnumerable<DropDownMaster> GetSource()
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"SELECT ""SourceId"" as ""Id"",""Name"" as ""Name"" from customer.""Source"" order by ""SourceId"" DESC";
                return connection.Query<DropDownMaster>(querySearch).ToList();
            }
        }

    }
}
