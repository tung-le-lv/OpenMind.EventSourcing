using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerReadModel",
                table: "CustomerReadModel");

            migrationBuilder.RenameTable(
                name: "CustomerReadModel",
                newName: "CustomerReadModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerReadModels",
                table: "CustomerReadModels",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerReadModels",
                table: "CustomerReadModels");

            migrationBuilder.RenameTable(
                name: "CustomerReadModels",
                newName: "CustomerReadModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerReadModel",
                table: "CustomerReadModel",
                column: "Id");
        }
    }
}
