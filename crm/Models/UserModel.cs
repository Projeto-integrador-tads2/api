using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace Models
{
    public class UserModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
        public string? ProfilePicture { get; private set; }

        public virtual ICollection<CompanyCardModel> Cards { get; private set; } = new List<CompanyCardModel>();
        public virtual ICollection<ObservationModel> Observations { get; private set; } = new List<ObservationModel>();

        public UserModel(string name, string email, string password, string phone, string? profilePicture = null)
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
    }
}
