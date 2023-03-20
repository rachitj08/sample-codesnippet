using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class ScannedVM
    {
        public long ReservationId { get; set; }
        public string ScannedData { get; set; }
        public string ScannedBy { get; set; }
        public string ActivityDoneBy { get; set; }
        public string QRCodeMapping { get; set; }
    }
}
