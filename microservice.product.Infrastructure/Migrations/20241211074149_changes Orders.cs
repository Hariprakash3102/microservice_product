using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace microservice.product.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changesOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Product_ProductsProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductsProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductsProductId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Product_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Product_ProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ProductsProductId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductsProductId",
                table: "Orders",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Product_ProductsProductId",
                table: "Orders",
                column: "ProductsProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
