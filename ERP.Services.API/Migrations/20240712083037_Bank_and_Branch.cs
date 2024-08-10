using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Bank_and_Branch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_code = table.Column<string>(type: "text", nullable: true),
                    bank_abbr = table.Column<string>(type: "text", nullable: true),
                    bank_name_th = table.Column<string>(type: "text", nullable: true),
                    bank_name_en = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.bank_id);
                });

            migrationBuilder.CreateTable(
                name: "BankBranch",
                columns: table => new
                {
                    bank_branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_code = table.Column<string>(type: "text", nullable: true),
                    bank_branch_code = table.Column<string>(type: "text", nullable: true),
                    bank_branch_name_th = table.Column<string>(type: "text", nullable: true),
                    bank_branch_name_en = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranch", x => x.bank_branch_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "BankBranch");
        }
    }
}
