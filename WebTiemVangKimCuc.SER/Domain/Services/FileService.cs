using Azure.Storage;
using Azure.Storage.Blobs;
using System.Runtime.CompilerServices;
using WebTiemVangKimCuc.SER.ViewModel.Blob;

namespace WebTiemVangKimCuc.SER.Domain.Services
{
    public interface IFileService
    {
        Task<List<BlobDto>> ListAsync();
        Task<BlobDto?> UploadAsync(IFormFile file);
        Task<object?> DeleteAsync(string fileName);
    }
    public class FileService : IFileService
    {
        private readonly string _storageAccount = "";
        private readonly string _key = "";
        private readonly BlobContainerClient _filesContainer;
        private readonly ILogger _logger;

        public FileService(ILogger logger)
        {
            DotNetEnv.Env.Load();

            _storageAccount = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT");
            _key = Environment.GetEnvironmentVariable("BLOB_KEY");

            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("images");
            _logger = logger;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            List<BlobDto> files = new List<BlobDto>();

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            return files;
        }

        public async Task<BlobDto?> UploadAsync (IFormFile file)
        {
            try
            {
                BlobClient client = _filesContainer.GetBlobClient(file.FileName);

                await using (Stream? data = file.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                var result = new BlobDto
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name,
                    ContentType = file.ContentType
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(UploadAsync)} function error on {nameof(FileService)}");
                throw;
            }  
        }

        public async Task<object?> DeleteAsync (string fileName)
        {
            try
            {
                BlobClient client = _filesContainer.GetBlobClient(fileName);

                var result = new BlobDto
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name,
                };

                if (await client.ExistsAsync())
                {
                    await client.DeleteAsync();
                }

                return result; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(DeleteAsync)} function error on {nameof(FileService)}");
                throw;
            }
        }
    }
}
