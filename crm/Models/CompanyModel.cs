using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class CompanyModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public string CompanyPicture { get; private set; }

        public virtual ICollection<ClientModel> Clients { get; private set; } = new List<ClientModel>();
        public virtual ICollection<CompanyCardModel> Cards { get; private set; } = new List<CompanyCardModel>();

        public CompanyModel(string name, string cnpj, string? companyPicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(cnpj, nameof(cnpj));
            Guard.Against.NullOrWhiteSpace(cnpj, nameof(cnpj));

            Name = name;
            Cnpj = cnpj;
            CompanyPicture = companyPicture;
        }

        private CompanyModel()
        {
        }

        public void Update(string name, string cnpj, string? companyPicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(cnpj, nameof(cnpj));
            Guard.Against.NullOrWhiteSpace(cnpj, nameof(cnpj));

            Name = name;
            Cnpj = cnpj;
            if (companyPicture != null)
                CompanyPicture = companyPicture;
            SetUpdatedAt();
        }

        public void UpdateCompanyPicture(string? companyPicture)
        {
            CompanyPicture = companyPicture;
            SetUpdatedAt();
        }
    }
}
