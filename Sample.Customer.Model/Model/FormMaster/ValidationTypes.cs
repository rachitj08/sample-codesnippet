using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class ValidationTypes
    {
        public int ValidationTypeId { get; set; }
        public string ValidationType { get; set; }
        public string ValidationRule { get; set; }
        public string ErrorMessage { get; set; }
    }
}
