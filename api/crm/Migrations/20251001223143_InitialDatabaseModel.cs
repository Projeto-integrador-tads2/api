using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMvcSwagger.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "User",
                newName: "Phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "User",
                newName: "Telefone");
        }
    }
}
