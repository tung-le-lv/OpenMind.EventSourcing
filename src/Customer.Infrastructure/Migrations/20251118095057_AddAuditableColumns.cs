using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionUserId",
                table: "CustomerReadModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CustomerReadModel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "CustomerReadModel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "CustomerReadModel",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionUserId",
                table: "CustomerReadModel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerReadModel");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CustomerReadModel");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CustomerReadModel");
        }
    }
}
