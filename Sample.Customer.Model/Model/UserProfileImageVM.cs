using Sample.Customer.Model.Model.StorageModel;

namespace Sample.Customer.Model
{
    public class UserProfileImageVM
    {
        public Document ProfileDocument { get; set; }
    }
    public class QRUploadVM
    {
        public long ProviderId { get; set; }
        public string ProviderLocationId { get; set; }
        public long SubLocationId { get; set; }
        public string SubLocationType { get; set; }
        public string ActivityCode { get; set; }
        public long SpotId { get; set; }
        public string EncryptedValue { get; set; }
        public Document ProfileDocument { get; set; }
    }
}
