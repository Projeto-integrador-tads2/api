using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations
{
    public class HistoryModelConfiguration : IEntityTypeConfiguration<HistoryModel>
    {
        public void Configure(EntityTypeBuilder<HistoryModel> builder)
        {
            builder
                .HasOne(h => h.FromStepColumn)
                .WithMany(s => s.HistoriesAsFrom)
                .HasForeignKey(h => h.FromStepColumnId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.ToStepColumn)
                .WithMany(s => s.HistoriesAsTo)
                .HasForeignKey(h => h.ToStepColumnId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.CompanyCard)
                .WithMany(c => c.Histories)
                .HasForeignKey(h => h.CompanyCardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(h => h.MovedByUser)
                .WithMany()
                .HasForeignKey(h => h.MovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClientCompanyConfiguration : IEntityTypeConfiguration<ClientModel>
    {
        public void Configure(EntityTypeBuilder<ClientModel> builder)
        {
            builder
                .HasOne(c => c.Company)
                .WithOne(co => co.Client)
                .HasForeignKey<ClientModel>(c => c.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
