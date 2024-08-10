using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Account_and_Condition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    condition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    condition_description = table.Column<string>(type: "text", nullable: true),
                    order_by = table.Column<int>(type: "integer", nullable: true),
                    condition_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.condition_id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAccounts",
                columns: table => new
                {
                    payment_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    payment_account_name = table.Column<string>(type: "text", nullable: true),
                    account_type = table.Column<string>(type: "text", nullable: true),
                    account_bank_id = table.Column<Guid>(type: "uuid", nullable: true),
                    account_brn_id = table.Column<Guid>(type: "uuid", nullable: true),
                    account_no = table.Column<string>(type: "text", nullable: true),
                    account_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAccounts", x => x.payment_account_id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectNo",
                columns: table => new
                {
                    cus_no_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    year = table.Column<string>(type: "text", nullable: true),
                    allocated = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectNo", x => x.cus_no_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_condition_id_org_id_business_id",
                table: "Conditions",
                columns: new[] { "condition_id", "org_id", "business_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAccounts_payment_account_id_org_id_business_id",
                table: "PaymentAccounts",
                columns: new[] { "payment_account_id", "org_id", "business_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNo_cus_no_id",
                table: "ProjectNo",
                column: "cus_no_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNo_org_id_business_id_year",
                table: "ProjectNo",
                columns: new[] { "org_id", "business_id", "year" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "PaymentAccounts");

            migrationBuilder.DropTable(
                name: "ProjectNo");
        }
    }
}
