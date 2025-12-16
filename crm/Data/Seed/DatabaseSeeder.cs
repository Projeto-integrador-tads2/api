using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Enums;

namespace Data.Seed
{
    /// <summary>
    /// Responsável por popular o banco de dados com dados iniciais
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Executa o seed de todos os dados iniciais
        /// </summary>
        public static async Task SeedAsync(AppDbContext context, ILogger logger)
        {
            try
            {
                await SeedAdminUserAsync(context, logger);
                await SeedDefaultStepColumnsAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao executar seed do banco de dados");
                throw;
            }
        }

        private static async Task SeedDefaultStepColumnsAsync(AppDbContext context, ILogger logger)
        {
            var defaultColumns = new[]
            {
                new { Name = "Análise de Perfil", Color = "#2196F3", Order = 1 },
                new { Name = "Conversa com o Cliente", Color = "#4CAF50", Order = 2 },
                new { Name = "Negociação", Color = "#FFC107", Order = 3 },
                new { Name = "Fechamento", Color = "#F44336", Order = 4 }
            };

            foreach (var col in defaultColumns)
            {
                var exists = await context.StepColumn.AnyAsync(x => x.Name == col.Name && x.IsDefault);
                if (!exists)
                {
                    var stepColumn = new StepColumnModel(col.Name, col.Order, col.Color);
                    stepColumn.GetType().GetProperty("IsDefault")?.SetValue(stepColumn, true);
                    context.StepColumn.Add(stepColumn);
                    logger.LogInformation($"Coluna padrão '{col.Name}' criada.");
                }
            }
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Cria um usuário admin padrão se não existir
        /// </summary>
        private static async Task SeedAdminUserAsync(AppDbContext context, ILogger logger)
        {
            const string adminEmail = "admin@crm.com";
            const string adminPassword = "Admin@123";

            var adminExists = await context.User.AnyAsync(u => u.Email == adminEmail);

            if (adminExists)
            {
                logger.LogInformation("✅ Usuário admin já existe no banco");
                return;
            }

            logger.LogInformation("🔨 Criando usuário admin padrão...");

            var passwordHasher = new PasswordHasher<object>();
            var tempUser = new { Email = adminEmail };
            var hashedPassword = passwordHasher.HashPassword(tempUser, adminPassword);

            var adminUser = new UserModel(
                name: "Administrador",
                email: adminEmail,
                password: hashedPassword,
                phone: "00000000000",
                profilePicture: null,
                role: UserRole.Admin
            );

            context.User.Add(adminUser);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Método auxiliar para adicionar outros seeds no futuro
        /// Exemplo: Seed de empresas, produtos, etc.
        /// </summary>
        private static async Task SeedCompaniesAsync(AppDbContext context, ILogger logger)
        {
            await Task.CompletedTask;
        }
    }
}
