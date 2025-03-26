using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEstColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "administrative_costs",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "import_duty",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "wht",
                table: "QuotationProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "administrative_costs",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "import_duty",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "wht",
                table: "QuotationProduct");
        }
    }
}
