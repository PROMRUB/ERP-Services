using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Change_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_product_custom_Id",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_id_org_id_business_id_product_custom_Id",
                table: "Product",
                columns: new[] { "product_id", "org_id", "business_id", "product_custom_Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_product_id_org_id_business_id_product_custom_Id",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_custom_Id",
                table: "Product",
                column: "product_custom_Id",
                unique: true);
        }
    }
}
