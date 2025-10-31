using MediatR;
using Data;
using Interfaces;

namespace Commands
{
    public class RemoveServicePictureCommand : IRequest<RemoveServicePictureCommandResponse>
    {
        public string ServiceId { get; set; } = string.Empty;
    }

    public class RemoveServicePictureCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class RemoveServicePictureCommandHandler : IRequestHandler<RemoveServicePictureCommand, RemoveServicePictureCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<RemoveServicePictureCommandHandler> _logger;

        public RemoveServicePictureCommandHandler(
            AppDbContext context,
            IFileStorageService fileStorageService,
            ILogger<RemoveServicePictureCommandHandler> logger)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<RemoveServicePictureCommandResponse> Handle(RemoveServicePictureCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ServiceId, out var serviceId))
                throw new ArgumentException($"ID do serviço inválido: '{request.ServiceId}'. Deve ser um GUID válido.");

            var service = await _context.Services.FindAsync(new object[] { serviceId }, cancellationToken);

            if (service == null)
                throw new Exception($"Serviço não encontrado com ID: {serviceId}");

            string? oldPictureUrl = service.ServicePicture;

            if (string.IsNullOrEmpty(oldPictureUrl))
            {
                return new RemoveServicePictureCommandResponse
                {
                    ServiceId = service.Id.ToString(),
                    Name = service.Name,
                    Description = service.Description,
                    Message = "Serviço não possui foto para remover"
                };
            }

            try
            {
                var fileKey = ExtractFileKeyFromUrl(oldPictureUrl);
                await _fileStorageService.DeleteFileAsync(fileKey);
                _logger.LogInformation("Service picture deleted from storage: {FileKey}", fileKey);

                service.Update(service.Name, service.Description, null);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Service picture removed successfully for service {ServiceId}", serviceId);

                return new RemoveServicePictureCommandResponse
                {
                    ServiceId = service.Id.ToString(),
                    Name = service.Name,
                    Description = service.Description,
                    Message = "Foto do serviço removida com sucesso"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing service picture for service {ServiceId}", serviceId);
                throw;
            }
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
