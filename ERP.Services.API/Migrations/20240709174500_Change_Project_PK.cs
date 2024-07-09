using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Change_Project_PK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_project_cus_Id",
                table: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Project_project_id_business_id_cus_id_project_cus_Id",
                table: "Project",
                columns: new[] { "project_id", "business_id", "cus_id", "project_cus_Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_project_id_business_id_cus_id_project_cus_Id",
                table: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Project_project_cus_Id",
                table: "Project",
                column: "project_cus_Id",
                unique: true);
        }
    }
}
