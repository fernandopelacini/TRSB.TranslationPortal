using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _03.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslationProcessingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessingStartedAt",
                table: "TranslationRequests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessingStartedAt",
                table: "TranslationRequests");
        }
    }
}
