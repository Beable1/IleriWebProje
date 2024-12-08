using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace İleriWeb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class migProductIdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_OrderDetails_OrderDetailId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_OrderDetailId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Product",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrderDetailId",
                table: "Product",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_OrderDetails_OrderDetailId",
                table: "Product",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
