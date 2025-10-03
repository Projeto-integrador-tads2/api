using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class CompanyCardModel : BaseEFEntity
    {
        public Guid UserId { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid StepColumnId { get; private set; }
        
        [ForeignKey("UserId")]
        public virtual UserModel User { get; private set; }
        
        [ForeignKey("CompanyId")]
        public virtual CompanyModel Company { get; private set; }
        
        [ForeignKey("StepColumnId")]
        public virtual StepColumnModel StepColumn { get; private set; }
        
        public virtual ICollection<ObservationModel> Observations { get; private set; } = new List<ObservationModel>();
        public virtual ICollection<HistoryModel> Histories { get; private set; } = new List<HistoryModel>();

        public CompanyCardModel(Guid userId, Guid companyId, Guid stepColumnId)
        {
            Guard.Against.Default(userId, nameof(userId));
            Guard.Against.Default(companyId, nameof(companyId));
            Guard.Against.Default(stepColumnId, nameof(stepColumnId));
            
            UserId = userId;
            CompanyId = companyId;
            StepColumnId = stepColumnId;
        }

        private CompanyCardModel()
        {
        }

        public void Update(Guid userId, Guid companyId, Guid stepColumnId)
        {
            Guard.Against.Default(userId, nameof(userId));
            Guard.Against.Default(companyId, nameof(companyId));
            Guard.Against.Default(stepColumnId, nameof(stepColumnId));
            
            UserId = userId;
            CompanyId = companyId;
            StepColumnId = stepColumnId;
            SetUpdatedAt();
        }

        public void MoveToColumn(Guid newStepColumnId)
        {
            Guard.Against.Default(newStepColumnId, nameof(newStepColumnId));
            
            StepColumnId = newStepColumnId;
            SetUpdatedAt();
        }
    }
}