using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class ClientModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Guid CompanyId { get; private set; }

        [ForeignKey("CompanyId")]
        public virtual CompanyModel Company { get; private set; }

        public ClientModel(string name, string email, string phone, Guid companyId)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(email, nameof(email));
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.NullOrEmpty(phone, nameof(phone));
            Guard.Against.NullOrWhiteSpace(phone, nameof(phone));
            Guard.Against.Default(companyId, nameof(companyId));

            Name = name;
            Email = email;
            Phone = phone;
            CompanyId = companyId;
        }

        private ClientModel()
        {
        }

        public void Update(string name, string email, string phone)
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
            SetUpdatedAt();
        }

        public void AssignCompany(Guid companyId)
        {
            Guard.Against.Default(companyId, nameof(companyId));
            CompanyId = companyId;
            SetUpdatedAt();
        }
    }
}
