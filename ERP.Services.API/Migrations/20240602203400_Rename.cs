using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "provice",
                table: "Organizations",
                newName: "province");

            migrationBuilder.RenameColumn(
                name: "provice",
                table: "Customer",
                newName: "province");

            migrationBuilder.RenameColumn(
                name: "provice",
                table: "Businesses",
                newName: "province");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "province",
                table: "Organizations",
                newName: "provice");

            migrationBuilder.RenameColumn(
                name: "province",
                table: "Customer",
                newName: "provice");

            migrationBuilder.RenameColumn(
                name: "province",
                table: "Businesses",
                newName: "provice");
        }
    }
}
