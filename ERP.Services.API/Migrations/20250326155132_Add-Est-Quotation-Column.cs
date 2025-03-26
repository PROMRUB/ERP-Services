using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEstQuotationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "QuotationProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "exchange",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "incoterm",
                table: "QuotationProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "purchasing_price",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "exchange",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "incoterm",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "purchasing_price",
                table: "QuotationProduct");
        }
    }
}
