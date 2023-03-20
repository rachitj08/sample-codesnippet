using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingProvidersLocationsSubLocationsQrBkp
    {
        public long? ParkingProvidersLocationSubLocationId { get; set; }
        public long? ParkingProviderLocationId { get; set; }
        public string SubLocationType { get; set; }
        public string QrcodePath { get; set; }
        public long? BeaconId { get; set; }
        public string SubLocationName { get; set; }
        public long? ActivityCodeId { get; set; }
        public string QrcodeEncryptedValue { get; set; }
        public string QrcodeMapping { get; set; }
    }
}
