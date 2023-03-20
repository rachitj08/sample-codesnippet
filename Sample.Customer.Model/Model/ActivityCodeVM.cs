using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class ActivityCodeVM
    {
        public ActivityCodeVM()
        {
        }

        public long ActivityCodeId { get; set; }
        public string Code { get; set; }
        public long Odering { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EncryptedCode { get; set; }
        public string QRCode { get; set; }
    }
}
