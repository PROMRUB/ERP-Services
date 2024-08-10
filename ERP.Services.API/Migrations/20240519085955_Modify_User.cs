using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Modify_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "OrganizationsUsers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "OrganizationsUsers",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "roles_list",
                table: "OrganizationsUsers",
                newName: "last_name_th");

            migrationBuilder.AddColumn<string>(
                name: "first_name_th",
                table: "OrganizationsUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers",
                column: "org_custom_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers");

            migrationBuilder.DropColumn(
                name: "first_name_th",
                table: "OrganizationsUsers");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "OrganizationsUsers",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "OrganizationsUsers",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "last_name_th",
                table: "OrganizationsUsers",
                newName: "roles_list");

            migrationBuilder.CreateIndex(
                name: "OrgUser_Unique1",
                table: "OrganizationsUsers",
                columns: new[] { "org_custom_id", "user_id" },
                unique: true);
        }
    }
}
