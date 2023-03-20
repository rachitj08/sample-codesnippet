using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Core.BlobStorageService
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(MemoryStream ms, string fileName, string rootDirectory, string directory = "");
        Task<string> UploadFileAsync(IFormFile file, string rootDirectory, string directory = "", string fileName = "");
        Task<string> UploadItemImage(byte[] photobytes, string fileName, string containerName, string extension, string folderName);
        Task Delete(string fileName, string containerName, string extension, string folderName);
        //Task<BlobContentInfo> Upload(FileModel model);
        //Task<byte[]> Get(FileModel model);
    }
}
