using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class QuotationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationProduct",
                columns: table => new
                {
                    quotation_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    discount = table.Column<float>(type: "real", nullable: false),
                    amount_before_vat = table.Column<decimal>(type: "numeric", nullable: false),
                    sum_of_discount = table.Column<decimal>(type: "numeric", nullable: false),
                    real_price_msrp = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationProduct", x => x.quotation_product_id);
                    table.ForeignKey(
                        name: "FK_QuotationProduct_Product_product_id",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationProduct_Quotation_quotation_id",
                        column: x => x.quotation_id,
                        principalTable: "Quotation",
                        principalColumn: "quotation_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProduct_product_id",
                table: "QuotationProduct",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProduct_quotation_id",
                table: "QuotationProduct",
                column: "quotation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationProduct");
        }
    }
}
