using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IleriWeb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class migAddimagedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Product",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Product");
        }
    }
}
