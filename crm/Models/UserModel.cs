using Ardalis.GuardClauses;
using Enums;

namespace Models
{
    public class UserModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
        public string? ProfilePicture { get; private set; }
        public UserRole Role { get; private set; }

        public virtual ICollection<CompanyCardModel> Cards { get; private set; } = new List<CompanyCardModel>();
        public virtual ICollection<ObservationModel> Observations { get; private set; } = new List<ObservationModel>();

        public UserModel(string name, string email, string password, string phone, string? profilePicture = null, UserRole role = UserRole.User)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(email, nameof(email));
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.NullOrEmpty(password, nameof(password));
            Guard.Against.NullOrWhiteSpace(password, nameof(password));
            Guard.Against.NullOrEmpty(phone, nameof(phone));
            Guard.Against.NullOrWhiteSpace(phone, nameof(phone));

            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
            ProfilePicture = profilePicture;
            Role = role;
        }

        private UserModel()
        {
        }

        public void Update(string name, string email, string phone, string? profilePicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(email, nameof(email));
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.NullOrEmpty(phone, nameof(phone));
            Guard.Against.NullOrWhiteSpace(phone, nameof(phone));

            Name = name;
            Email = email;
            Phone = phone;
            if (profilePicture != null)
                ProfilePicture = profilePicture;
            SetUpdatedAt();
        }

        public void UpdatePassword(string password)
        {
            Guard.Against.NullOrEmpty(password, nameof(password));
            Guard.Against.NullOrWhiteSpace(password, nameof(password));

            Password = password;
            SetUpdatedAt();
        }

        public void UpdateProfilePicture(string? profilePicture)
        {
            ProfilePicture = profilePicture;
            SetUpdatedAt();
        }

        public void UpdateRole(UserRole role)
        {
            Role = role;
            SetUpdatedAt();
        }

        public bool HasPermission(Resource resource, Permission permission)
        {
            if (Role == UserRole.Admin)
                return true;

            return GetUserPermissions(resource).HasFlag(permission);
        }

        private Permission GetUserPermissions(Resource resource)
        {
            return resource switch
            {
                Resource.Users => Permission.CanView,
                Resource.Clients => Permission.All,
                Resource.Cards => Permission.All,
                Resource.Companies => Permission.CanView,
                Resource.StepColumns => Permission.All,
                Resource.Observations => Permission.All,
                Resource.History => Permission.CanView,
                _ => Permission.None
            };
        }
    }
}
