using MediatR;
using Data;
using Models;
using Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class UpdateServiceWithPictureCommand : IRequest<UpdateServiceWithPictureCommandResponse>
    {
        public string ServiceId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Picture { get; set; }
        public string? CustomFileName { get; set; }
    }

    public class UpdateServiceWithPictureCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UpdateServiceWithPictureCommandHandler : IRequestHandler<UpdateServiceWithPictureCommand, UpdateServiceWithPictureCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UpdateServiceWithPictureCommandHandler> _logger;

        public UpdateServiceWithPictureCommandHandler(
            AppDbContext context,
            IFileStorageService fileStorageService,
            ILogger<UpdateServiceWithPictureCommandHandler> logger)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<UpdateServiceWithPictureCommandResponse> Handle(UpdateServiceWithPictureCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ServiceId, out var serviceId))
                throw new ArgumentException($"ID do serviço inválido: '{request.ServiceId}'. Deve ser um GUID válido.");

            var service = await _context.Services.FindAsync(new object[] { serviceId }, cancellationToken);

            if (service == null)
                throw new Exception($"Serviço não encontrado com ID: {serviceId}");

            string? oldPictureUrl = service.ServicePicture;
            string? newPictureUrl = oldPictureUrl;

            try
            {
                if (request.Picture != null)
                {
                    ValidateFile(request.Picture);

                    newPictureUrl = await _fileStorageService.UploadFileAsync(
                        request.Picture,
                        "service-pictures",
                        request.CustomFileName
                    );

                    _logger.LogInformation("New service picture uploaded: {FileUrl}", newPictureUrl);

                    if (!string.IsNullOrEmpty(oldPictureUrl))
                    {
                        try
                        {
                            var oldFileKey = ExtractFileKeyFromUrl(oldPictureUrl);
                            await _fileStorageService.DeleteFileAsync(oldFileKey);
                            _logger.LogInformation("Old service picture deleted");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete old picture, but continuing with update");
                        }
                    }
                }

                service.Update(request.Name, request.Description, newPictureUrl);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Service updated successfully: {ServiceId}", serviceId);

                return new UpdateServiceWithPictureCommandResponse
                {
                    ServiceId = service.Id.ToString(),
                    Name = service.Name,
                    Description = service.Description,
                    ServicePicture = service.ServicePicture,
                    Message = "Serviço atualizado com sucesso"
                };
            }
            catch (Exception ex)
            {
                if (newPictureUrl != null && newPictureUrl != oldPictureUrl)
                {
                    try
                    {
                        var fileKey = ExtractFileKeyFromUrl(newPictureUrl);
                        await _fileStorageService.DeleteFileAsync(fileKey);
                        _logger.LogInformation("Rolled back uploaded picture due to error");
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogError(deleteEx, "Error rolling back uploaded picture");
                    }
                }

                _logger.LogError(ex, "Error updating service {ServiceId}", serviceId);
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
