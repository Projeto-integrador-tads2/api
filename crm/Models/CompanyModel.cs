using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class CompanyModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public Guid ClientId { get; private set; }
        public ClientModel Client { get; private set; }

        public CompanyModel(string name, string cnpj, Guid clientId)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(cnpj, nameof(cnpj));
            Guard.Against.NullOrWhiteSpace(cnpj, nameof(cnpj));
            Guard.Against.Default(clientId, nameof(clientId));
            
            Name = name;
            Cnpj = cnpj;
            ClientId = clientId;
        }

        private CompanyModel()
        {
        }

        public void Update(string name, string cnpj, Guid clientId)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(cnpj, nameof(cnpj));
            Guard.Against.NullOrWhiteSpace(cnpj, nameof(cnpj));
            Guard.Against.Default(clientId, nameof(clientId));
            
            Name = name;
            Cnpj = cnpj;
            ClientId = clientId;
            SetUpdatedAt();
        }
    }
}