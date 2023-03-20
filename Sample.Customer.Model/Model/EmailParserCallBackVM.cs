using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class EmailParserCallBackVM
    {
        public string MsgId { get; set; }
        public string RequestId { get; set; }
        public long? AccountId { get; set; }
        public long LoggedUserId { get; set; }
    }

    public class EmailParserCallBackResponse
    {
        public string UserData { get; set; }
    }
    
    public class EmailParserReservationDetails
    {
        public long accountId { get; set; }
        public Guid MessageId { get; set; }
    }
}
