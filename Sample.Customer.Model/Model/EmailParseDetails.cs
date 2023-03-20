using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Model
{
    public class EmailParseDetails
    {
        public long EmailParseDetailId { get; set; }
        public Guid MessageId { get; set; }
        public string RequestIds { get; set; }
        public long UserId { get; set; }
    }
}
