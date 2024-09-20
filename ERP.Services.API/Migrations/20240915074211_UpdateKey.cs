using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuotationProduct",
                table: "QuotationProduct");

            migrationBuilder.AddColumn<Guid>(
                name: "quotation_product_id",
                table: "QuotationProduct",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuotationProduct",
                table: "QuotationProduct",
                column: "quotation_product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuotationProduct",
                table: "QuotationProduct");

            migrationBuilder.DropColumn(
                name: "quotation_product_id",
                table: "QuotationProduct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuotationProduct",
                table: "QuotationProduct",
                columns: new[] { "quotation_id", "product_id" });
        }
    }
}
