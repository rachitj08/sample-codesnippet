using Microsoft.AspNetCore.Http;

namespace Core.BlobStorageService
{
    public class FileModel
    {
        public IFormFile ImageFile { get; set; }
        public string FileName { get; set; }
        public string BlobContainerName { get; set; }
    }
}
