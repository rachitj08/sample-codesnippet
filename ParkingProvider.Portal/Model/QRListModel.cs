using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ParkingProvider.Portal.Model
{
    public class QRListModel
    {
        public string EncryptedData { get; set; }
        public string Name { get; set; }
        public string QRPath { get; set; }
    }
    public class QRDataModel
    {
        public SelectList ParkingProviderList { get; set; }
        public SelectList ParkingProviderLocationList { get; set; }
        public List<QRListModel> qRListModels { get; set; }

    }

}
