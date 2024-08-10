using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Customer_Contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerContact",
                columns: table => new
                {
                    cus_con_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cus_firstname = table.Column<string>(type: "text", nullable: true),
                    cus_lastname = table.Column<string>(type: "text", nullable: true),
                    cus_tel_no = table.Column<string>(type: "text", nullable: true),
                    cus_ext_no = table.Column<string>(type: "text", nullable: true),
                    cus_mobille_no = table.Column<string>(type: "text", nullable: true),
                    cus_mail = table.Column<string>(type: "text", nullable: true),
                    cus_con_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContact", x => x.cus_con_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContact_cus_con_id",
                table: "CustomerContact",
                column: "cus_con_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerContact");
        }
    }
}
