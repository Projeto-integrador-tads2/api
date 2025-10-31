using MediatR;
using Data;
using Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class RegisterServiceWithPictureCommand : IRequest<RegisterServiceWithPictureCommandResponse>
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Picture { get; set; }
        public string? CustomFileName { get; set; }
    }

    public class RegisterServiceWithPictureCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RegisterServiceWithPictureCommandHandler : IRequestHandler<RegisterServiceWithPictureCommand, RegisterServiceWithPictureCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<RegisterServiceWithPictureCommandHandler> _logger;

        public RegisterServiceWithPictureCommandHandler(
            AppDbContext context,
            IFileStorageService fileStorageService,
            ILogger<RegisterServiceWithPictureCommandHandler> logger)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<RegisterServiceWithPictureCommandResponse> Handle(RegisterServiceWithPictureCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Services.AnyAsync(s => s.Name == request.Name, cancellationToken))
                throw new Exception("Serviço com este nome já cadastrado");

            string? servicePictureUrl = null;

            try
            {
                if (request.Picture != null)
                {
                    ValidateFile(request.Picture);

                    servicePictureUrl = await _fileStorageService.UploadFileAsync(
                        request.Picture,
                        "service-pictures",
                        request.CustomFileName
                    );

                    _logger.LogInformation("Service picture uploaded: {FileUrl}", servicePictureUrl);
                }

                var service = new ServiceModel(request.Name, request.Description, servicePictureUrl);

                _context.Services.Add(service);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Service registered successfully with ID: {ServiceId}", service.Id);

                return new RegisterServiceWithPictureCommandResponse
                {
                    ServiceId = service.Id.ToString(),
                    Name = service.Name,
                    Description = service.Description,
                    ServicePicture = service.ServicePicture,
                    Message = "Serviço registrado com sucesso"
                };
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(servicePictureUrl))
                {
                    try
                    {
                        var fileKey = ExtractFileKeyFromUrl(servicePictureUrl);
                        await _fileStorageService.DeleteFileAsync(fileKey);
                        _logger.LogInformation("Rolled back uploaded picture due to error");
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogError(deleteEx, "Error rolling back uploaded picture");
                    }
                }

                _logger.LogError(ex, "Error registering service");
                throw;
            }
        }

        private void ValidateFile(IFormFile file)
        {
            if (file.Length == 0)
                throw new ArgumentException("Arquivo vazio");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Apenas imagens são permitidas (JPEG, PNG, GIF, WEBP)");

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
