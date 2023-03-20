using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingProvidersSubLocationsRepository
    {
        bool CheckLocationExist(long sublocationId, string subLocationType);

        Task<ParkingProvidersLocationsSubLocations> GetSubLocationId(string activityCode, long parkingProviderLocationId, string subLocationType);
        Task<ParkingProvidersLocationsSubLocations> GetSubLocationByQRCodeMapping(string qrCode);
        IQueryable<ParkingProvidersLocationsSubLocations> GetSubLocationByParkingProvide(long parkingProviderLocationId);
        ParkingProvidersLocationsSubLocations CheckIfQRCodeExist(string SubLocationType, long activityCodeId, long parkingProviderLocationId);
        int UpdateQRData(QRUploadVM qRModel, string uploadPath, long activityId, string mapingCode);
        int SaveQRData(QRUploadVM qRModel, string uploadPath, long activityId, string mapingCode, string activityName);
    }
}
