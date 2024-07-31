using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class SetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_IssuedByUserUserId",
                table: "Quotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_sale_person_id",
                table: "Quotation");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_IssuedByUserUserId",
                table: "Quotation");

            migrationBuilder.DropColumn(
                name: "IssuedByUserUserId",
                table: "Quotation");

            migrationBuilder.AlterColumn<Guid>(
                name: "sale_person_id",
                table: "Quotation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_issued_by",
                table: "Quotation",
                column: "issued_by");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_issued_by",
                table: "Quotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Users_sale_person_id",
                table: "Quotation");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_issued_by",
                table: "Quotation");

            migrationBuilder.AlterColumn<Guid>(
                name: "sale_person_id",
                table: "Quotation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IssuedByUserUserId",
                table: "Quotation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_IssuedByUserUserId",
                table: "Quotation",
                column: "IssuedByUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Users_IssuedByUserUserId",
                table: "Quotation",
                column: "IssuedByUserUserId",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Users_sale_person_id",
                table: "Quotation",
                column: "sale_person_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
