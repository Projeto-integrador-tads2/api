using MediatR;
using Data;
using Dtos;
using Models;
using Interfaces;

namespace Commands
{
    // Command
    public class UploadServicePictureCommand : IRequest<FileUploadDto>
    {
        public Guid ServiceId { get; set; }
        public IFormFile File { get; set; }
        public string? CustomFileName { get; set; }
    }

    // Handler
    public class UploadServicePictureCommandHandler : IRequestHandler<UploadServicePictureCommand, FileUploadDto>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly AppDbContext _context;
        private readonly ILogger<UploadServicePictureCommandHandler> _logger;

        public UploadServicePictureCommandHandler(
            IFileStorageService fileStorageService,
            AppDbContext context,
            ILogger<UploadServicePictureCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _context = context;
            _logger = logger;
        }

        public async Task<FileUploadDto> Handle(UploadServicePictureCommand request, CancellationToken cancellationToken)
        {
            ValidateFile(request.File);

            var service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
                throw new ArgumentException("Serviço não encontrado");

            try
            {
                var fileUrl = await _fileStorageService.UploadFileAsync(
                    request.File,
                    "service-pictures",
                    request.CustomFileName
                );

                if (!string.IsNullOrEmpty(service.ServicePicture))
                {
                    var oldFileKey = ExtractFileKeyFromUrl(service.ServicePicture);
                    await _fileStorageService.DeleteFileAsync(oldFileKey);
                }

                service.Update(service.Name, service.Description, fileUrl);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Service picture uploaded for service {ServiceId}: {FileUrl}", request.ServiceId, fileUrl);

                return new FileUploadDto
                {
                    FileUrl = fileUrl,
                    FileName = request.File.FileName,
                    FileSize = request.File.Length,
                    ContentType = request.File.ContentType,
                    UploadedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading service picture for service {ServiceId}", request.ServiceId);
                throw new ApplicationException("Erro ao fazer upload da imagem");
            }
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo não fornecido");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Apenas imagens são permitidas");

            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Arquivo muito grande (máximo 5MB)");
        }

        private string ExtractFileKeyFromUrl(string url)
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath.TrimStart('/');

            var bucketName = "crm-files";
            if (path.StartsWith($"{bucketName}/"))
            {
                path = path.Substring(bucketName.Length + 1);
            }

            return path;
        }
    }
}
