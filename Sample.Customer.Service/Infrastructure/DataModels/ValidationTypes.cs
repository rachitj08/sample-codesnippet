using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ValidationTypes
    {
        public int ValidationTypeId { get; set; }
        public string ValidationType { get; set; }
        public string ValidationRule { get; set; }
        public string ErrorMessage { get; set; }
    }
}
