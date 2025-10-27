using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMvcSwagger.Migrations
{
    /// <inheritdoc />
    public partial class modelsRefatoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_StepColumn_StepColumn_Id",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_User_User_Id",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Clients_ClientId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Cards_Card_Id",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_StepColumn_Moved_To",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_StepColumn_StepColumn_Id",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_User_User_Id",
                table: "Observations");

            migrationBuilder.DropIndex(
                name: "IX_Company_ClientId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Observations");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Observations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "StepColumn_Id",
                table: "Observations",
                newName: "CompanyCardId");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_User_Id",
                table: "Observations",
                newName: "IX_Observations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_StepColumn_Id",
                table: "Observations",
                newName: "IX_Observations_CompanyCardId");

            migrationBuilder.RenameColumn(
                name: "Moved_To",
                table: "Histories",
                newName: "ToStepColumnId");

            migrationBuilder.RenameColumn(
                name: "Card_Id",
                table: "Histories",
                newName: "MovedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_Moved_To",
                table: "Histories",
                newName: "IX_Histories_ToStepColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_Card_Id",
                table: "Histories",
                newName: "IX_Histories_MovedByUserId");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Cards",
                newName: "StepColumnId");

            migrationBuilder.RenameColumn(
                name: "StepColumn_Id",
                table: "Cards",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_User_Id",
                table: "Cards",
                newName: "IX_Cards_StepColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_StepColumn_Id",
                table: "Cards",
                newName: "IX_Cards_CompanyId");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "User",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StepColumn",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "StepColumn",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)",
                oldMaxLength: 6)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StepColumn",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Observations",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Observations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyCardId",
                table: "Histories",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Histories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FromStepColumnId",
                table: "Histories",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Histories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPicture",
                table: "Company",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Clients",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cards",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_CompanyCardId",
                table: "Histories",
                column: "CompanyCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_FromStepColumnId",
                table: "Histories",
                column: "FromStepColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId",
                table: "Clients",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Company_CompanyId",
                table: "Cards",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_StepColumn_StepColumnId",
                table: "Cards",
                column: "StepColumnId",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_User_UserId",
                table: "Cards",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Cards_CompanyCardId",
                table: "Histories",
                column: "CompanyCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_StepColumn_FromStepColumnId",
                table: "Histories",
                column: "FromStepColumnId",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_StepColumn_ToStepColumnId",
                table: "Histories",
                column: "ToStepColumnId",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_User_MovedByUserId",
                table: "Histories",
                column: "MovedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Cards_CompanyCardId",
                table: "Observations",
                column: "CompanyCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_User_UserId",
                table: "Observations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Company_CompanyId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_StepColumn_StepColumnId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_User_UserId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Cards_CompanyCardId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_StepColumn_FromStepColumnId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_StepColumn_ToStepColumnId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_User_MovedByUserId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Cards_CompanyCardId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_User_UserId",
                table: "Observations");

            migrationBuilder.DropIndex(
                name: "IX_Histories_CompanyCardId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_FromStepColumnId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StepColumn");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "CompanyCardId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "FromStepColumnId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "CompanyPicture",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Observations",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "CompanyCardId",
                table: "Observations",
                newName: "StepColumn_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_UserId",
                table: "Observations",
                newName: "IX_Observations_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_CompanyCardId",
                table: "Observations",
                newName: "IX_Observations_StepColumn_Id");

            migrationBuilder.RenameColumn(
                name: "ToStepColumnId",
                table: "Histories",
                newName: "Moved_To");

            migrationBuilder.RenameColumn(
                name: "MovedByUserId",
                table: "Histories",
                newName: "Card_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_ToStepColumnId",
                table: "Histories",
                newName: "IX_Histories_Moved_To");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_MovedByUserId",
                table: "Histories",
                newName: "IX_Histories_Card_Id");

            migrationBuilder.RenameColumn(
                name: "StepColumnId",
                table: "Cards",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Cards",
                newName: "StepColumn_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_StepColumnId",
                table: "Cards",
                newName: "IX_Cards_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_CompanyId",
                table: "Cards",
                newName: "IX_Cards_StepColumn_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StepColumn",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "StepColumn",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Observations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Observations",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Observations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cards",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_ClientId",
                table: "Company",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_StepColumn_StepColumn_Id",
                table: "Cards",
                column: "StepColumn_Id",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_User_User_Id",
                table: "Cards",
                column: "User_Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Clients_ClientId",
                table: "Company",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Cards_Card_Id",
                table: "Histories",
                column: "Card_Id",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_StepColumn_Moved_To",
                table: "Histories",
                column: "Moved_To",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_StepColumn_StepColumn_Id",
                table: "Observations",
                column: "StepColumn_Id",
                principalTable: "StepColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_User_User_Id",
                table: "Observations",
                column: "User_Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
