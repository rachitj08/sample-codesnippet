using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class DataSourceFieldValidation
    {
        public int DataSourceFieldValidationId { get; set; }
        public long FieldId { get; set; }
        public int ValidationTypeId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
