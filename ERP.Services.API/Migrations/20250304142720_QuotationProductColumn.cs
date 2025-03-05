using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class QuotationProductColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cost_estimate",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "cost_estimate_percent",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "cost_estimate_profit",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "cost_estimate_profit_percent",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "estimate_price",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "latest_cost",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "profit",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "profit_percent",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost_estimate",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "cost_estimate_percent",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "cost_estimate_profit",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "cost_estimate_profit_percent",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "estimate_price",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "latest_cost",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "profit",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "profit_percent",
                table: "QuotationProduct");
        }
    }
}
