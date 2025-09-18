using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Estimation_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "administrative_cost_est",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "buyunit_est",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "cost_est",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "currency_est",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "exchangerate_est",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "import_duty_est",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "incoterm_est",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "wht_po",
                table: "Product",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "administrative_cost_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "buyunit_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "cost_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "currency_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "exchangerate_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "import_duty_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "incoterm_est",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "wht_po",
                table: "Product");
        }
    }
}
