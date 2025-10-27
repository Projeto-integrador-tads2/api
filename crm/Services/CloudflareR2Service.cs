using Amazon.S3;
using Amazon.S3.Model;
using Interfaces;

namespace Services
{
    public class CloudflareR2Service : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = string.Empty;
        private readonly string _publicUrl = string.Empty;
        private readonly ILogger<CloudflareR2Service> _logger;

        public CloudflareR2Service(IConfiguration configuration, ILogger<CloudflareR2Service> logger)
        {
            var config = configuration.GetSection("CloudflareR2");

            var clientConfig = new AmazonS3Config
            {
                ServiceURL = $"https://{config["AccountId"]}.r2.cloudflarestorage.com",
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(
                config["AccessKeyId"],
                config["SecretAccessKey"],
                clientConfig
            );

            _bucketName = config["BucketName"] ?? throw new ArgumentNullException("CloudflareR2:BucketName");
            _publicUrl = config["PublicUrl"] ?? throw new ArgumentNullException("CloudflareR2:PublicUrl");
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder, string? fileName = null)
        {
            try
            {
                fileName ??= $"{Guid.NewGuid()}-{file.FileName}";
                var fileKey = $"{folder}/{fileName}";

                using var stream = file.OpenReadStream();

                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                await _s3Client.PutObjectAsync(request);
                var fileUrl = GetPublicUrl(fileKey);

                _logger.LogInformation("File uploaded successfully: {FileKey}", fileKey);
                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileKey)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                await _s3Client.DeleteObjectAsync(request);
                _logger.LogInformation("File deleted successfully: {FileKey}", fileKey);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileKey}", fileKey);
                return false;
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                var response = await _s3Client.GetObjectAsync(request);
                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file {FileKey}", fileKey);
                throw;
            }
        }

        public string GetPublicUrl(string fileKey)
        {
            return $"{_publicUrl}/{fileKey}";
        }
    }
}
