using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerece.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_StockQuantity_Positive",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Label",
                table: "Products",
                column: "Label",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_StockQuantity_Positive",
                table: "Products",
                sql: "[StockQuantity] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Label",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_StockQuantity_Positive",
                table: "Products");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_StockQuantity_Positive",
                table: "Products",
                sql: "[StockQuantity] > 0");
        }
    }
}
