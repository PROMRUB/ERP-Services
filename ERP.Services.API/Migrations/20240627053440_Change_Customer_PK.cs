using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Change_Customer_PK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_cus_custom_id_org_id_business_id",
                table: "Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_cus_id_cus_custom_id_org_id_business_id",
                table: "Customer",
                columns: new[] { "cus_id", "cus_custom_id", "org_id", "business_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_cus_id_cus_custom_id_org_id_business_id",
                table: "Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_cus_custom_id_org_id_business_id",
                table: "Customer",
                columns: new[] { "cus_custom_id", "org_id", "business_id" },
                unique: true);
        }
    }
}
