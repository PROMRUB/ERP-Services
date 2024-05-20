using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Business_And_Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "alley",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "building",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_type",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "floor",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moo",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "org_status",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "room_no",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "village",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    business_id = table.Column<Guid>(type: "uuid", nullable: false),
                    business_custom_id = table.Column<string>(type: "text", nullable: true),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_type = table.Column<string>(type: "text", nullable: true),
                    business_name = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    business_logo = table.Column<string>(type: "text", nullable: true),
                    tax_id = table.Column<string>(type: "text", nullable: true),
                    branch_id = table.Column<string>(type: "text", nullable: true),
                    building = table.Column<string>(type: "text", nullable: true),
                    room_no = table.Column<string>(type: "text", nullable: true),
                    floor = table.Column<string>(type: "text", nullable: true),
                    village = table.Column<string>(type: "text", nullable: true),
                    moo = table.Column<string>(type: "text", nullable: true),
                    house_no = table.Column<string>(type: "text", nullable: true),
                    road = table.Column<string>(type: "text", nullable: true),
                    alley = table.Column<string>(type: "text", nullable: true),
                    provice = table.Column<string>(type: "text", nullable: true),
                    district = table.Column<string>(type: "text", nullable: true),
                    sub_district = table.Column<string>(type: "text", nullable: true),
                    post_code = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    business_description = table.Column<string>(type: "text", nullable: true),
                    business_created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    business_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.business_id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    cus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cus_custom_id = table.Column<string>(type: "text", nullable: true),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    business_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cus_type = table.Column<string>(type: "text", nullable: true),
                    cus_name = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    tax_id = table.Column<string>(type: "text", nullable: true),
                    branch_id = table.Column<string>(type: "text", nullable: true),
                    building = table.Column<string>(type: "text", nullable: true),
                    room_no = table.Column<string>(type: "text", nullable: true),
                    floor = table.Column<string>(type: "text", nullable: true),
                    village = table.Column<string>(type: "text", nullable: true),
                    moo = table.Column<string>(type: "text", nullable: true),
                    house_no = table.Column<string>(type: "text", nullable: true),
                    road = table.Column<string>(type: "text", nullable: true),
                    alley = table.Column<string>(type: "text", nullable: true),
                    provice = table.Column<string>(type: "text", nullable: true),
                    district = table.Column<string>(type: "text", nullable: true),
                    sub_district = table.Column<string>(type: "text", nullable: true),
                    post_code = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    cus_description = table.Column<string>(type: "text", nullable: true),
                    cus_created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cus_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.cus_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_business_custom_id",
                table: "Businesses",
                column: "business_custom_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_cus_custom_id",
                table: "Customer",
                column: "cus_custom_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropColumn(
                name: "alley",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "building",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "business_type",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "floor",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "moo",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "org_status",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "room_no",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "village",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "website",
                table: "Organizations");
        }
    }
}
