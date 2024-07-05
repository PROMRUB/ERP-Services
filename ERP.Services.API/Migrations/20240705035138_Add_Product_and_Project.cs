using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Product_and_Project : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_cat_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_sub_cat_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_custom_Id = table.Column<string>(type: "text", nullable: true),
                    product_name = table.Column<string>(type: "text", nullable: true),
                    msrp = table.Column<decimal>(type: "numeric", nullable: true),
                    lw_price = table.Column<decimal>(type: "numeric", nullable: true),
                    product_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    cat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cat_cus_id = table.Column<string>(type: "text", nullable: true),
                    parent_cat_id = table.Column<string>(type: "text", nullable: true),
                    cat_name = table.Column<string>(type: "text", nullable: true),
                    cat_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.cat_id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_cus_Id = table.Column<string>(type: "text", nullable: true),
                    project_name = table.Column<string>(type: "text", nullable: true),
                    project_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.project_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_custom_Id",
                table: "Product",
                column: "product_custom_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_cat_cus_id",
                table: "ProductCategory",
                column: "cat_cus_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_project_cus_Id",
                table: "Project",
                column: "project_cus_Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
