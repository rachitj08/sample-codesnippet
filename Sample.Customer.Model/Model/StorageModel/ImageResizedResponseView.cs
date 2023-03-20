using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.StorageModel
{
    public class ImageResizedResponseView
    {
        public bool IsImageSaved { get; set; }
        public bool IsImageResized { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
    }
}
