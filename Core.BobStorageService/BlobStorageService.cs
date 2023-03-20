using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.BlobStorageService
{
    public class BlobStorageService: IBlobStorageService
    {
        private BlobConfig _config;

        public BlobStorageService(BlobConfig config)
        {
            _config = config;
        }

        public async Task<string> UploadFileAsync(MemoryStream ms, string fileName, string containerName, string directory = "")
        {
            var storageAccount = CloudStorageAccount.Parse(_config.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            if (!String.IsNullOrEmpty(directory))
            {
                directory = directory + "/";
            }
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(directory + fileName);
            var fileArray = ms.ToArray();
            await blob.UploadFromByteArrayAsync(fileArray, 0, fileArray.Length);
            return blob.Uri.ToString();

        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName, string directory = "", string fileName = "")
        {
            var storageAccount = CloudStorageAccount.Parse(_config.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            var _fileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName;
            if (!string.IsNullOrEmpty(directory))
            {
                directory = directory + "/";
            }
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(directory + _fileName);
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileArray = ms.ToArray();
                await blob.UploadFromByteArrayAsync(fileArray, 0, fileArray.Length);
            }
            return blob.Uri.ToString();
        }

        public async Task<string> UploadItemImage(byte[] photobytes, string fileName, string containerName, string extension, string folderName)
        {
            string connectionString = _config.StorageConnectionString;
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);

            //TODO: enable it later
           await container.CreateIfNotExistsAsync();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
           await container.SetPermissionsAsync(containerPermissions);

            string blobImage = folderName + "/" + fileName + "."+ extension;

            CloudBlockBlob photo = container.GetBlockBlobReference(blobImage);
            await photo.UploadFromByteArrayAsync(photobytes, 0, photobytes.Length);
            return photo.Uri.ToString();
        }

        public async Task Delete(string fileName, string containerName, string extension, string folderName)
        {
            string connectionString = _config.StorageConnectionString;
            string blobImage = folderName + "/" + fileName + "." + extension;

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(blobImage);

            await blobClient.DeleteAsync();
        }

        //public async Task<BlobContentInfo> Upload(FileModel model)
        //{
        //    var blobContainer = _blobServiceClient.GetBlobContainerClient(model.BlobContainerName);

        //    var blobClient = blobContainer.GetBlobClient(model.ImageFile.FileName);

        //    var result = await blobClient.UploadAsync(model.ImageFile.OpenReadStream());

        //    return result;

        //}

        //public async Task<byte[]> Get(FileModel model)
        //{
        //    var blobContainer = _blobServiceClient.GetBlobContainerClient(model.BlobContainerName);

        //    var blobClient = blobContainer.GetBlobClient(model.FileName);
        //    var downloadContent = await blobClient.DownloadAsync();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        await downloadContent.Value.Content.CopyToAsync(ms);
        //        return ms.ToArray();
        //    }
        //}
    }
}
