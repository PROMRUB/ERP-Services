using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Inhand_And_Last_PO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cost_inhand",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "cost_last_po",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "currency_inhand",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "currency_last_po",
                table: "Product",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost_inhand",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "cost_last_po",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "currency_inhand",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "currency_last_po",
                table: "Product");
        }
    }
}
