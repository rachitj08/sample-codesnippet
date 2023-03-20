namespace Sample.Admin.HttpAggregator.Config.UrlsConfig
{
    /// <summary>
    /// BaseUrlsConfig
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
    }

    /// <summary>
    /// BaseConfig
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
