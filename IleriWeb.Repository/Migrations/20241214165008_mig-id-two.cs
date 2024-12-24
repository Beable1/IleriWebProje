using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IleriWeb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class migidtwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Customers_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customers_CustomerId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustomerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "IdentityUserId",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdentityUserId",
                table: "Order",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_IdentityUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdentityUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityUserId = table.Column<int>(type: "integer", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IdentityUserId",
                table: "Customers",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Customers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customers_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
