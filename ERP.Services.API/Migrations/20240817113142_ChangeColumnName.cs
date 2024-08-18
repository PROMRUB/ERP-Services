using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "delivery_date",
                table: "QuotationProject",
                newName: "eth_sale_month");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "eth_sale_month",
                table: "QuotationProject",
                newName: "delivery_date");
        }
    }
}
