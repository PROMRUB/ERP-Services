using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AdddelivertyDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "delivery_date",
                table: "QuotationProject",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivery_date",
                table: "QuotationProject");
        }
    }
}
