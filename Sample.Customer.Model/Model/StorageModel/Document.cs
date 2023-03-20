using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.StorageModel
{
    public class Document
    {
        public string FileName { get; set; }
        public string PicBase64 { get; set; }
        public string FileExtenstion { get; set; }
        public byte[] Filebytes { get; set; }
        public Guid FileGuid { get; set; }
        public string FileUniqueName { get; set; }
    }
}
