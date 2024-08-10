using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class PaymentAccountRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentAccountEntityPaymentAccountId",
                table: "Quotation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_PaymentAccountEntityPaymentAccountId",
                table: "Quotation",
                column: "PaymentAccountEntityPaymentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_PaymentAccounts_PaymentAccountEntityPaymentAccoun~",
                table: "Quotation",
                column: "PaymentAccountEntityPaymentAccountId",
                principalTable: "PaymentAccounts",
                principalColumn: "payment_account_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_PaymentAccounts_PaymentAccountEntityPaymentAccoun~",
                table: "Quotation");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_PaymentAccountEntityPaymentAccountId",
                table: "Quotation");

            migrationBuilder.DropColumn(
                name: "PaymentAccountEntityPaymentAccountId",
                table: "Quotation");
        }
    }
}
