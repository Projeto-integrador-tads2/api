using ApiMvcSwagger.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Data.Configurations;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<CompanyCardModel> Cards { get; set; }
        public DbSet<CompanyModel> Company { get; set; }
        public DbSet<HistoryModel> Histories { get; set; }
        public DbSet<ObservationModel> Observations { get; set; }
        public DbSet<StepColumnModel> StepColumn { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<ServiceModel> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HistoryModelConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCompanyConfiguration());
        }
    }
}
