using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddColumntoQuotationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount_before_vat",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "real_price_msrp",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sum_of_discount",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_before_vat",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "real_price_msrp",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "sum_of_discount",
                table: "QuotationProduct");
        }
    }
}
