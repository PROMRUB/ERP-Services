using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Services.API.Migrations
{
    /// <inheritdoc />
    public partial class Customer_Contact_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerContact_cus_con_id",
                table: "CustomerContact");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "CustomerContact",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContact_cus_con_id_org_id_business_id_cus_id_user_id",
                table: "CustomerContact",
                columns: new[] { "cus_con_id", "org_id", "business_id", "cus_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerContact_cus_con_id_org_id_business_id_cus_id_user_id",
                table: "CustomerContact");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "CustomerContact");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContact_cus_con_id",
                table: "CustomerContact",
                column: "cus_con_id",
                unique: true);
        }
    }
}
