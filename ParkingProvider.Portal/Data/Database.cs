using Npgsql;
using System.Collections.Generic;
using System.Data;
using ParkingProvider.Portal.Model;
using Dapper;
using System.Linq;
using System;

namespace ParkingProvider.Portal.Data
{
    public interface IDatabaseContext
    {
        IEnumerable<Model.DropDownMaster> GetParkingProvider();
        IEnumerable<Model.DropDownMaster> GetParkingProviderLocation(long providerId);
        IEnumerable<Model.DropDownMaster> GetSubLocation(long parkingProviderlocationId);
        IEnumerable<Model.DropDownMaster> GetActivityCode();
        IEnumerable<Model.DropDownMaster> GetParkingSpotId(long subLocationId);
        int UpdateQRData(string encryptedKey, long subLocationId);
        IEnumerable<QRListModel> GetQRList(long parkingProviderId);
        int SaveQRData(QRModel qRModel);
        long GetActivityIdByCode(string code);
        string GetActivityNameByCode(string code);
        int CheckSubLocationExist(QRModel qRModel);
        int UpdateQRData(QRModel qRModel);
    }
    public class DatabaseContext : IDatabaseContext
    {
        string connectionString = "Server=172.29.17.138; Database=YodaQA; user id=postgres; password=userpassdb;";


        public IEnumerable<Model.DropDownMaster> GetParkingProvider()
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"SELECT ""ProviderId"" as ""Id"",""Name"" FROM customer.""ParkingProviders"" ORDER BY ""ProviderId"" ASC ";
                return connection.Query<Model.DropDownMaster>(querySearch).ToList();
            }
        }

        public IEnumerable<Model.DropDownMaster> GetParkingProviderLocation(long providerId)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"SELECT ""ParkingProvidersLocationId"" as ""Id"",""Name"" FROM customer.""ParkingProvidersLocations"" ORDER BY ""ParkingProvidersLocationId"" ASC ";
                return connection.Query<Model.DropDownMaster>(querySearch).ToList();
            }
        }

        public IEnumerable<Model.DropDownMaster> GetSubLocation(long parkingProviderlocationId)
        {
            List<Model.DropDownMaster> subLocations = new List<Model.DropDownMaster>();

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
            //            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            //            {
            //                var querySearch = @$"select sub.""ParkingProvidersLocationSubLocationId"" as ""Id"",ac.""Name"" as ""Name"" from customer.""ParkingProvidersLocationsSubLocations"" as sub  inner join customer.""ActivityCode"" as ac on sub.""ActivityCodeId"" = ac.""ActivityCodeId"" where ac.""ScanType"" = 'QRCode'
            //and sub.""ParkingProviderLocationId""={parkingProviderlocationId} order by ""Odering""";
            //                return connection.Query<Model.DropDownMaster>(querySearch).ToList();
            //            }
        }

        public IEnumerable<Model.DropDownMaster> GetActivityCode()
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"SELECT ""Code"" as ""Id"",""Description"" as ""Name"" from customer.""ActivityCode"" WHERE ""ScanType""='QRCode' order by ""Odering"" ASC";
                return connection.Query<Model.DropDownMaster>(querySearch).ToList();
            }
        }
        public IEnumerable<Model.DropDownMaster> GetParkingSpotId(long subLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"SELECT ""ParkingSpotId"" as ""Id"",""Name""   FROM customer.""ParkingSpots"" order by ""ParkingSpotId"" ASC";
                return connection.Query<Model.DropDownMaster>(querySearch).ToList();
            }
        }

        public int UpdateQRData(string encryptedKey, long subLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"UPDATE customer.""ParkingProvidersLocationsSubLocations"" SET ""QRCodeEncryptedValue""='{encryptedKey}' WHERE  ""ParkingProvidersLocationSubLocationId""={subLocationId}";
                return connection.Execute(querySearch);
            }
        }

        public IEnumerable<QRListModel> GetQRList(long parkingProviderId)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"SELECT ""QRCodeMapping"" as ""EncryptedData"",""QRCodePath"" as ""QRPath"",""SubLocationName"" as ""Name"" FROM customer.""ParkingProvidersLocationsSubLocations"" WHERE ""ParkingProviderLocationId""={parkingProviderId} and ""QRCodeEncryptedValue"" is not null";
                return connection.Query<QRListModel>(querySearch).ToList();
            }
        }
        public int CheckSubLocationExist(QRModel qRModel)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$" SELECT ""ParkingProvidersLocationSubLocationId"" from customer.""ParkingProvidersLocationsSubLocations"" WHERE ""SubLocationType""='{qRModel.SubLocationType}' and ""ActivityCodeId""={qRModel.ActivityId} and ""ParkingProviderLocationId""={qRModel.ParkingProviderLocation} ";
                var result= connection.ExecuteScalar(querySearch);
                if (result != null)
                    return Convert.ToInt32(result);
                else return 0;
            }
        }
        public int UpdateQRData(QRModel qRModel)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"UPDATE  customer.""ParkingProvidersLocationsSubLocations"" SET  ""QRCodeEncryptedValue""='{qRModel.EncryptedData}',  ""QRCodeMapping""='{qRModel.QRMappingCode}' ,  ""QRCodePath""='{qRModel.SavedPath}' 
WHERE ""SubLocationType""='{qRModel.SubLocationType}' and ""ActivityCodeId"" = '{qRModel.ActivityId}' and ""ParkingProviderLocationId"" ={ Convert.ToInt64(qRModel.ParkingProviderLocation)}";
                return connection.Execute(querySearch);
            }
        }
        public int SaveQRData(QRModel qRModel)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"INSERT INTO customer.""ParkingProvidersLocationsSubLocations""(""ParkingProviderLocationId"", ""SubLocationType"", ""QRCodePath"", ""BeaconId"", ""SubLocationName"", ""ActivityCodeId"", ""QRCodeEncryptedValue"",""QRCodeMapping"") 
VALUES({Convert.ToInt64(qRModel.ParkingProviderLocation)}, '{qRModel.SubLocationType}', '{qRModel.SavedPath}', 0, '{qRModel.ActivityName}', {Convert.ToInt64(qRModel.ActivityId)}, '{qRModel.EncryptedData}','{qRModel.QRMappingCode}')";
                return connection.Execute(querySearch);
            }
        }
        public long GetActivityIdByCode(string code)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"select ""ActivityCodeId"" from customer.""ActivityCode"" where ""Code""='{code}'";
                return connection.QueryFirstOrDefault<long>(querySearch);
            }
        }
        public string GetActivityNameByCode(string code)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var querySearch = @$"select ""Name"" from customer.""ActivityCode"" where ""Code""='{code}'";
                return connection.QueryFirstOrDefault<string>(querySearch);
            }
        }


    }
}
