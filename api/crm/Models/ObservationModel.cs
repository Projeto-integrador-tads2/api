using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class ObservationModel : BaseEFEntity
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public Guid CompanyCardId { get; private set; }
      
        [ForeignKey("UserId")]
        public virtual UserModel User { get; private set; }

        [ForeignKey("CompanyCardId")]
        public virtual CompanyCardModel CompanyCard { get; private set; }

        public ObservationModel(string title, string content, Guid userId, Guid companyCardId, bool isInternal = false)
        {
            Guard.Against.NullOrEmpty(title, nameof(title));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Guard.Against.NullOrEmpty(content, nameof(content));
            Guard.Against.NullOrWhiteSpace(content, nameof(content));
            Guard.Against.Default(userId, nameof(userId));
            Guard.Against.Default(companyCardId, nameof(companyCardId));
            
            Title = title;
            Content = content;
            UserId = userId;
            CompanyCardId = companyCardId;
        }

        private ObservationModel()
        {
        }

        public void Update(string title, string content)
        {
            Guard.Against.NullOrEmpty(title, nameof(title));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Guard.Against.NullOrEmpty(content, nameof(content));
            Guard.Against.NullOrWhiteSpace(content, nameof(content));
            
            Title = title;
            Content = content;
            SetUpdatedAt();
        }
    }
}
