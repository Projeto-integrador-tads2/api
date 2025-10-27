using MediatR;
using Data;
using Dtos;
using Models;
using Interfaces;

namespace Commands
{
    // Command
    public class UploadUserProfilePictureCommand : IRequest<FileUploadDto>
    {
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
        public string? CustomFileName { get; set; }
    }

    // Handler
    public class UploadUserProfilePictureCommandHandler : IRequestHandler<UploadUserProfilePictureCommand, FileUploadDto>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly AppDbContext _context;
        private readonly ILogger<UploadUserProfilePictureCommandHandler> _logger;

        public UploadUserProfilePictureCommandHandler(
            IFileStorageService fileStorageService,
            AppDbContext context,
            ILogger<UploadUserProfilePictureCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _context = context;
            _logger = logger;
        }

        public async Task<FileUploadDto> Handle(UploadUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            ValidateFile(request.File);

            var user = await _context.User.FindAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado");

            try
            {
                var fileUrl = await _fileStorageService.UploadFileAsync(
                    request.File, 
                    "profile-pictures", 
                    request.CustomFileName
                );

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    var oldFileKey = ExtractFileKeyFromUrl(user.ProfilePicture);
                    await _fileStorageService.DeleteFileAsync(oldFileKey);
                }

                user.UpdateProfilePicture(fileUrl);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Profile picture uploaded for user {UserId}: {FileUrl}", request.UserId, fileUrl);

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
                _logger.LogError(ex, "Error uploading profile picture for user {UserId}", request.UserId);
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
            return uri.AbsolutePath.TrimStart('/');
        }
    }
}
