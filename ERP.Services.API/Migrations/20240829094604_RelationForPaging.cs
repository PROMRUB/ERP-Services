using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class RelationForPaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentAccounts_account_bank_id",
                table: "PaymentAccounts",
                column: "account_bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAccounts_account_brn_id",
                table: "PaymentAccounts",
                column: "account_brn_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAccounts_BankBranch_account_brn_id",
                table: "PaymentAccounts",
                column: "account_brn_id",
                principalTable: "BankBranch",
                principalColumn: "bank_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAccounts_Bank_account_bank_id",
                table: "PaymentAccounts",
                column: "account_bank_id",
                principalTable: "Bank",
                principalColumn: "bank_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAccounts_BankBranch_account_brn_id",
                table: "PaymentAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAccounts_Bank_account_bank_id",
                table: "PaymentAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PaymentAccounts_account_bank_id",
                table: "PaymentAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PaymentAccounts_account_brn_id",
                table: "PaymentAccounts");
        }
    }
}
