using System;
using System.Collections.Generic;
using Sample.Customer.Model;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingHeadsRateRepository
    {
        List<ParkingHeadsRateDetailVM> GetParkingHeadsRateList(DateTime applicableDate, long parkingProviderLocationId, long accountId);
        List<ParkingHeadsRateDetailVM> GetParkingCustomHeadsRateList(DateTime applicableDate, long parkingProviderLocationId, long accountId, long reservationId);
    }
}
