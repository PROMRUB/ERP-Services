using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class BusinessLogoWithRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Quotation_business_id",
                table: "Quotation",
                column: "business_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Businesses_business_id",
                table: "Quotation",
                column: "business_id",
                principalTable: "Businesses",
                principalColumn: "business_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Businesses_business_id",
                table: "Quotation");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_business_id",
                table: "Quotation");
        }
    }
}
