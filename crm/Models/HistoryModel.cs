using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace Models
{
    public class HistoryModel : BaseEFEntity
    {
        public DateTime MovedAt { get; private set; }
        public Guid CompanyCardId { get; private set; }
        public Guid FromStepColumnId { get; private set; }
        public Guid ToStepColumnId { get; private set; }
        public Guid MovedByUserId { get; private set; }

        [ForeignKey("CompanyCardId")]
        [InverseProperty("Histories")]
        public virtual CompanyCardModel CompanyCard { get; private set; }
        
        [ForeignKey("FromStepColumnId")]
        [InverseProperty("HistoriesAsFrom")]
        public virtual StepColumnModel FromStepColumn { get; private set; }
        
        [ForeignKey("ToStepColumnId")]
        [InverseProperty("HistoriesAsTo")]
        public virtual StepColumnModel ToStepColumn { get; private set; }
        
        [ForeignKey("MovedByUserId")]
        public virtual UserModel MovedByUser { get; private set; }

        public HistoryModel(Guid companyCardId, Guid fromStepColumnId, Guid toStepColumnId, Guid movedByUserId)
        {
            Guard.Against.Default(companyCardId, nameof(companyCardId));
            Guard.Against.Default(fromStepColumnId, nameof(fromStepColumnId));
            Guard.Against.Default(toStepColumnId, nameof(toStepColumnId));
            Guard.Against.Default(movedByUserId, nameof(movedByUserId));
            
            CompanyCardId = companyCardId;
            FromStepColumnId = fromStepColumnId;
            ToStepColumnId = toStepColumnId;
            MovedByUserId = movedByUserId;
            MovedAt = DateTime.UtcNow;
        }

        private HistoryModel()
        {
        }
    }
}