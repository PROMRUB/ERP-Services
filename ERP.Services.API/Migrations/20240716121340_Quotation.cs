using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Quotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "inventory",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Quotation",
                columns: table => new
                {
                    quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quotation_no = table.Column<string>(type: "text", nullable: true),
                    edit_time = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quotation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sale_person_id = table.Column<Guid>(type: "uuid", nullable: false),
                    issued_by = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuedByUserUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    vat = table.Column<double>(type: "double precision", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    account_no = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotation", x => x.quotation_id);
                    table.ForeignKey(
                        name: "FK_Quotation_CustomerContact_customer_contact_id",
                        column: x => x.customer_contact_id,
                        principalTable: "CustomerContact",
                        principalColumn: "cus_con_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotation_Customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotation_Users_IssuedByUserUserId",
                        column: x => x.IssuedByUserUserId,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotation_Users_sale_person_id",
                        column: x => x.sale_person_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationProduct",
                columns: table => new
                {
                    quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationProduct", x => new { x.quotation_id, x.product_id });
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

            migrationBuilder.CreateTable(
                name: "QuotationProject",
                columns: table => new
                {
                    quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lead_time = table.Column<int>(type: "integer", nullable: false),
                    warranty = table.Column<int>(type: "integer", nullable: false),
                    payment_condition = table.Column<string>(type: "text", nullable: false),
                    po = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationProject", x => new { x.quotation_id, x.project_id });
                    table.ForeignKey(
                        name: "FK_QuotationProject_Project_project_id",
                        column: x => x.project_id,
                        principalTable: "Project",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationProject_Quotation_quotation_id",
                        column: x => x.quotation_id,
                        principalTable: "Quotation",
                        principalColumn: "quotation_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_customer_contact_id",
                table: "Quotation",
                column: "customer_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_customer_id",
                table: "Quotation",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_IssuedByUserUserId",
                table: "Quotation",
                column: "IssuedByUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_quotation_id",
                table: "Quotation",
                column: "quotation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_sale_person_id",
                table: "Quotation",
                column: "sale_person_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProduct_product_id",
                table: "QuotationProduct",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProduct_quotation_id_product_id",
                table: "QuotationProduct",
                columns: new[] { "quotation_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProject_project_id",
                table: "QuotationProject",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProject_quotation_id_project_id",
                table: "QuotationProject",
                columns: new[] { "quotation_id", "project_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationProduct");

            migrationBuilder.DropTable(
                name: "QuotationProject");

            migrationBuilder.DropTable(
                name: "Quotation");

            migrationBuilder.DropColumn(
                name: "inventory",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "Product");
        }
    }
}
