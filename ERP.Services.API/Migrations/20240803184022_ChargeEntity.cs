using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class ChargeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_issued_by",
                table: "Quotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_sale_person_id",
                table: "Quotation");

            migrationBuilder.DropColumn(
                name: "payment_condition",
                table: "QuotationProject");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_condition_id",
                table: "QuotationProject",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_OrganizationsUsers_issued_by",
                table: "Quotation",
                column: "issued_by",
                principalTable: "OrganizationsUsers",
                principalColumn: "org_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_OrganizationsUsers_sale_person_id",
                table: "Quotation",
                column: "sale_person_id",
                principalTable: "OrganizationsUsers",
                principalColumn: "org_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_OrganizationsUsers_issued_by",
                table: "Quotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_OrganizationsUsers_sale_person_id",
                table: "Quotation");

            migrationBuilder.DropColumn(
                name: "payment_condition_id",
                table: "QuotationProject");

            migrationBuilder.AddColumn<string>(
                name: "payment_condition",
                table: "QuotationProject",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Users_issued_by",
                table: "Quotation",
                column: "issued_by",
                principalTable: "Users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Users_sale_person_id",
                table: "Quotation",
                column: "sale_person_id",
                principalTable: "Users",
                principalColumn: "user_id");
        }
    }
}
