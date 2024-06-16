using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerNo_org_id_business_id_character",
                table: "CustomerNo",
                columns: new[] { "org_id", "business_id", "character" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerNo_org_id_business_id_character",
                table: "CustomerNo");
        }
    }
}
