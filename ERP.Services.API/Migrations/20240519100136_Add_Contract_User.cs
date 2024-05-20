using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Contract_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "OrganizationsUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tel_no",
                table: "OrganizationsUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "OrganizationsUsers");

            migrationBuilder.DropColumn(
                name: "tel_no",
                table: "OrganizationsUsers");
        }
    }
}
