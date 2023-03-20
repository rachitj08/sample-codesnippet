namespace Sample.Admin.Model
{
    public class AuthenticationConfigKeyModel
    {
        public long AuthenticationConfigKeyId { get; set; }
        public string AuthenticationType { get; set; }
        public string ConfigKey { get; set; }
        public bool IsRequired { get; set; }
    }
}
