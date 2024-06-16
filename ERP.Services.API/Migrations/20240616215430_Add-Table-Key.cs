using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTableKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerNo_cus_no_id",
                table: "CustomerNo",
                column: "cus_no_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerNo_cus_no_id",
                table: "CustomerNo");
        }
    }
}
