using Dapper;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingProvidersSubLocationsRepository : RepositoryBase<ParkingProvidersLocationsSubLocations>, IParkingProvidersSubLocationsRepository
    {
        public ParkingProvidersSubLocationsRepository(CloudAcceleratorContext context) : base(context)
        {
            
        }


        public bool CheckLocationExist(long sublocationId,string subLocationType)
        {
           
            return Any(x=> x.ParkingProvidersLocationSubLocationId==sublocationId && x.SubLocationType==subLocationType);
        }

        //Entry Activity Code
        public async Task<ParkingProvidersLocationsSubLocations> GetSubLocationId(string activityCode, long parkingProviderLocationId, string subLocationType)
        {
            return await SingleAsnc(x => x.ParkingProviderLocationId == parkingProviderLocationId 
                    && x.ActivityCode.Code.ToLower().Trim() == activityCode.ToLower().Trim()
                    && x.SubLocationType == subLocationType);
        }
        public async Task<ParkingProvidersLocationsSubLocations> GetSubLocationByQRCodeMapping(string qrCode)
        {
            return await SingleAsnc(x => x.QrcodeMapping == qrCode);
        }
       

        public IQueryable<ParkingProvidersLocationsSubLocations> GetSubLocationByParkingProvide(long parkingProviderLocationId)
        {
            return GetQuery(x => x.ParkingProviderLocationId == parkingProviderLocationId);
        }
        public ParkingProvidersLocationsSubLocations CheckIfQRCodeExist(string SubLocationType,long activityCodeId,long parkingProviderLocationId)
        {
            return Single(x => x.ParkingProviderLocationId == parkingProviderLocationId && x.SubLocationType==SubLocationType && x.ActivityCodeId==activityCodeId);
        }
        public int UpdateQRData(QRUploadVM qRModel,string uploadPath,long activityId,string mapingCode)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"UPDATE  customer.""ParkingProvidersLocationsSubLocations"" SET  ""QRCodeEncryptedValue""='{qRModel.EncryptedValue}',  ""QRCodeMapping""='{mapingCode}' ,  ""QRCodePath""='{uploadPath}' 
WHERE ""SubLocationType""='{qRModel.SubLocationType}' and ""ActivityCodeId"" = '{activityId}' and ""ParkingProviderLocationId"" ={Convert.ToInt64(qRModel.ProviderLocationId)}";
                return connection.Execute(querySearch);
            }
        }
        public int SaveQRData(QRUploadVM qRModel, string uploadPath, long activityId, string mapingCode,string activityName)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"INSERT INTO customer.""ParkingProvidersLocationsSubLocations""(""ParkingProviderLocationId"", ""SubLocationType"", ""QRCodePath"", ""BeaconId"", ""SubLocationName"", ""ActivityCodeId"", ""QRCodeEncryptedValue"",""QRCodeMapping"") 
VALUES({Convert.ToInt64(qRModel.ProviderLocationId)}, '{qRModel.SubLocationType}', '{uploadPath}', 0, '{activityName}', {activityId}, '{qRModel.EncryptedValue}','{mapingCode}')";
                return connection.Execute(querySearch);
            }
        }
    }
}
