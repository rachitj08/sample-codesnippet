using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    /// <summary>
    /// Base Urls Config
    /// </summary>
    public class CustomerBaseUrlsConfig
    {
        /// <summary>
        /// To set Admin API url
        /// </summary>
        public string AdminAPI { get; set; }

        /// <summary>
        /// To set Customer API url
        /// </summary>
        public string CustomerAPI { get; set; }

        /// <summary>
        /// To set StorageRootPath url
        /// </summary>
        public string StorageRootPath { get; set; }

        /// <summary>
        /// To set StorageClient url
        /// </summary>
        public string StorageClient { get; set; }
    }
}
