using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public class BaseDataModel
    {
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
