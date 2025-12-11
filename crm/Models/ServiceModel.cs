using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace Models
{
    public class ServiceModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string? ServicePicture { get; private set; }
        public int ContractDuration { get; private set; }
        public decimal Value { get; private set; }

        public ServiceModel(string name, string description, int contractDuration, decimal value, string? servicePicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Guard.Against.NegativeOrZero(contractDuration, nameof(contractDuration));
            Guard.Against.Negative(value, nameof(value));
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            ContractDuration = contractDuration;
            Value = value;
            ServicePicture = servicePicture;
        }

        private ServiceModel() { }

        public void Update(string name, string description, int contractDuration, decimal value, string? servicePicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Guard.Against.NegativeOrZero(contractDuration, nameof(contractDuration));
            Guard.Against.Negative(value, nameof(value));
            Name = name;
            Description = description;
            ContractDuration = contractDuration;
            Value = value;
            ServicePicture = servicePicture;
            SetUpdatedAt();
        }
    }
}
