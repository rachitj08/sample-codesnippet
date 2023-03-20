using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ParkingProvider.Portal.Model
{
    public class QRModel
    {
       
        public SelectList ParkingProviderList { get; set; }
        public SelectList ParkingProviderLocationList { get; set; }
        public SelectList SubLocationList { get; set; }
        public SelectList SubLocationTypeList { get; set; }
        public SelectList ActivityCodeList { get; set; }       
        public SelectList ParkingSpotIdList { get; set; }

        public string ParkingProvider { get; set; }
        public string ParkingProviderLocation { get; set; }
        public string SubLocation { get; set; }
        public string SubLocationType { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public string ParkingSpotId { get; set; }
        public string QRString { get; set; }
        public string EncryptedData { get; set; }
        public string SavedPath { get; set; }
        public long ActivityId { get; set; }
        public string QRMappingCode { get; set; }
    }
}
