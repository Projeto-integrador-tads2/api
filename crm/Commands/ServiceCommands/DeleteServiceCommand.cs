using MediatR;
using Data;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class DeleteServiceCommand : IRequest<DeleteServiceCommandResponse>
    {
        [Required(ErrorMessage = "ServiceId é obrigatório")]
        public string ServiceId { get; set; } = string.Empty;
    }

    public class DeleteServiceCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, DeleteServiceCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly Interfaces.IFileStorageService _fileStorageService;
        private readonly ILogger<DeleteServiceCommandHandler> _logger;

        public DeleteServiceCommandHandler(
            AppDbContext context,
            Interfaces.IFileStorageService fileStorageService,
            ILogger<DeleteServiceCommandHandler> logger)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<DeleteServiceCommandResponse> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ServiceId, out var serviceId))
                throw new ArgumentException($"ID do serviço inválido: '{request.ServiceId}'. Deve ser um GUID válido.");

            var service = await _context.Services.FindAsync(new object[] { serviceId }, cancellationToken);

            if (service == null)
                throw new Exception($"Serviço não encontrado com ID: {serviceId}");

            if (!string.IsNullOrEmpty(service.ServicePicture))
            {
                try
                {
                    var fileKey = ExtractFileKeyFromUrl(service.ServicePicture);
                    await _fileStorageService.DeleteFileAsync(fileKey);
                    _logger.LogInformation("Service picture deleted from storage: {FileKey}", fileKey);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete service picture, but continuing with service deletion");
                }
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Service deleted successfully: {ServiceId}", serviceId);

            return new DeleteServiceCommandResponse
            {
                ServiceId = request.ServiceId,
                Message = "Serviço deletado com sucesso"
            };
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
