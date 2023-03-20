using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.StorageModel
{
    public class MediaUploadView
    {
        public long ReferenceId { get; set; }
        public long ReferenceType { get; set; }
        public long CreatedBy { get; set; }
        public long? ModuleId { get; set; }
        public long MediaTypeId { get; set; }
        public string ExternalLink { get; set; }
        public short Sequence { get; set; }
        public List<Document> Documents { get; set; }

    }
}
