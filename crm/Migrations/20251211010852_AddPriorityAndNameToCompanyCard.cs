using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMvcSwagger.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityAndNameToCompanyCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Cards",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Cards");
        }
    }
}
