using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Create_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationsUsers_org_custom_id",
                table: "OrganizationsUsers");

            migrationBuilder.DropIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrganizationsUsers_org_custom_id",
                table: "OrganizationsUsers",
                column: "org_custom_id");

            migrationBuilder.CreateIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers",
                column: "org_custom_id",
                unique: true);
        }
    }
}
