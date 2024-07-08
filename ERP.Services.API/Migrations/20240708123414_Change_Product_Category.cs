using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Change_Product_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_cat_cus_id",
                table: "ProductCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_cat_id_org_id_business_id_cat_cus_id",
                table: "ProductCategory",
                columns: new[] { "cat_id", "org_id", "business_id", "cat_cus_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_cat_id_org_id_business_id_cat_cus_id",
                table: "ProductCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_cat_cus_id",
                table: "ProductCategory",
                column: "cat_cus_id",
                unique: true);
        }
    }
}
