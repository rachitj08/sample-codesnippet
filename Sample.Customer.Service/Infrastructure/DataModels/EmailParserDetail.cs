using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class EmailParserDetail
    {
        public long EmailParserDetailId { get; set; }
        public long AccountId { get; set; }
        public Guid MessageId { get; set; }
        public string RequestId { get; set; }
        public string Mobile { get; set; }
        public string MobileCode { get; set; }
        public short Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public string ProcessMessage { get; set; }
    }
}
