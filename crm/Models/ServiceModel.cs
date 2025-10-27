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

        public ServiceModel(string name, string description, string? servicePicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Name = name;
            Description = description;
            ServicePicture = servicePicture;
        }

        private ServiceModel() { }

        public void Update(string name, string description, string? servicePicture = null)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Name = name;
            Description = description;
            ServicePicture = servicePicture;
        }
    }
}
