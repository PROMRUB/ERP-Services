using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QuotationProject_payment_condition_id",
                table: "QuotationProject",
                column: "payment_condition_id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationProject_Conditions_payment_condition_id",
                table: "QuotationProject",
                column: "payment_condition_id",
                principalTable: "Conditions",
                principalColumn: "condition_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationProject_Conditions_payment_condition_id",
                table: "QuotationProject");

            migrationBuilder.DropIndex(
                name: "IX_QuotationProject_payment_condition_id",
                table: "QuotationProject");
        }
    }
}
