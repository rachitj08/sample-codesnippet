using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.StorageModel
{
    public class MediaResponseView
    {
        public List<string> Data { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsProfileUploaded { get; set; }
        public string FilePath { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public long MediaTypeId { get; set; }
        public string ExternalLink { get; set; }
        public short Sequence { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
