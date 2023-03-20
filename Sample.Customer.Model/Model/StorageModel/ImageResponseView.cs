using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.StorageModel
{
    public class ImageResponseView
    {
        public List<string> SavedPathList { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public long MediaTypeId { get; set; }
        public string ExternalLink { get; set; }
        public short Sequence { get; set; }
    }
}
