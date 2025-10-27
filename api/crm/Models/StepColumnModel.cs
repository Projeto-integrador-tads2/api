using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Models
{
    public class StepColumnModel : BaseEFEntity
    {
        public string Name { get; private set; }
        public int Order { get; private set; }
        public string Color { get; private set; }
        public bool IsActive { get; private set; }
        
        public virtual ICollection<CompanyCardModel> Cards { get; private set; } = new List<CompanyCardModel>();
        public virtual ICollection<HistoryModel> HistoriesAsFrom { get; private set; } = new List<HistoryModel>();
        public virtual ICollection<HistoryModel> HistoriesAsTo { get; private set; } = new List<HistoryModel>();

        public StepColumnModel(string name, int order, string color)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NegativeOrZero(order, nameof(order));
            Guard.Against.NullOrEmpty(color, nameof(color));
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$"))
            {
                throw new ArgumentException("Color must be in hex format (#RRGGBB)", nameof(color));
            }
            
            Name = name;
            Order = order;
            Color = color;
            IsActive = true;
        }

        private StepColumnModel()
        {
        }

        public void Update(string name, string color)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(color, nameof(color));
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$"))
            {
                throw new ArgumentException("Color must be in hex format (#RRGGBB)", nameof(color));
            }
            
            Name = name;
            Color = color;
            SetUpdatedAt();
        }

        public void UpdateOrder(int newOrder)
        {
            Guard.Against.NegativeOrZero(newOrder, nameof(newOrder));
            
            Order = newOrder;
            SetUpdatedAt();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}
