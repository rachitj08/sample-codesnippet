using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model.ParkingProvider;

namespace Sample.Customer.Service.Infrastructure.Repository.ParkingProvider
{
    public interface IParkingProviderRepository
    {
        IEnumerable<DropDownMaster> GetParkingProvider();
        IEnumerable<DropDownMaster> GetParkingProviderLocation(long providerId);
        IEnumerable<DropDownMaster> GetSubLocation(long parkingProviderlocationId);
        IEnumerable<DropDownMaster> GetActivityCode();
        IEnumerable<DropDownMaster> GetSource();
        IEnumerable<DropDownMaster> GetParkingSpotId(long subLocationId);
        IEnumerable<QRListVM> GetQRList(long parkingProviderId);

        List<SubLocationType> GetSubLocationType();

        IEnumerable<DropDownMaster> GetParkingSpotByLocationandSpot(long locationId, long spotType, long accountId);

        IEnumerable<DropDownMaster> GetParkingSpotType(long accountId);

    }
}
