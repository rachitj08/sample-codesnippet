using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.Admin.Model
{
    public class ServiceModel
    {
        [DisplayName("Id")]
        public int ServiceId { get; set; }

        [DisplayName("Service Name"), Required(ErrorMessage = "Please enter Service name"), MaxLength(100, ErrorMessage = "Maximum Length is 100")]
        public string ServiceName { get; set; }

        [DisplayName("Service end point address"), Required(ErrorMessage = "Please enter End Point Base Address"), MinLength(1, ErrorMessage = "Minimum Length is 1"), MaxLength(255, ErrorMessage = "Maximum Length is 255")]
        public string EndPointBaseAddress { get; set; }
    }
}
