using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class PaymentAccoontCallNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_PaymentAccounts_account_no",
                table: "Quotation");

            migrationBuilder.AlterColumn<Guid>(
                name: "account_no",
                table: "Quotation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_PaymentAccounts_account_no",
                table: "Quotation",
                column: "account_no",
                principalTable: "PaymentAccounts",
                principalColumn: "payment_account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_PaymentAccounts_account_no",
                table: "Quotation");

            migrationBuilder.AlterColumn<Guid>(
                name: "account_no",
                table: "Quotation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_PaymentAccounts_account_no",
                table: "Quotation",
                column: "account_no",
                principalTable: "PaymentAccounts",
                principalColumn: "payment_account_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
