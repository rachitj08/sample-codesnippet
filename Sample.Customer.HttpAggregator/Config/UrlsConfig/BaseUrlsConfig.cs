namespace Sample.Customer.HttpAggregator.Config.UrlsConfig
{
    /// <summary>
    /// Base Urls Config
    /// </summary>
    public class BaseUrlsConfig
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

        /// <summary>
        /// To set Payment url
        /// </summary>
        public string PaymentAPI { get; set; }
    }

    /// <summary>
    /// Base Config
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// To set ClientId for token api
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// To set ClientSecret for token api
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// To set Scope for token api
        /// </summary>
        public string Scope { get; set; }
    }
}
