using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IleriWeb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addbuilders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
