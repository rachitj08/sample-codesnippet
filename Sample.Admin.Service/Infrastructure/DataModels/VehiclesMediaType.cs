using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class VehiclesMediaType
    {
        public long VehiclesMediaTypeId { get; set; }
        public string MediaType { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
    }
}
