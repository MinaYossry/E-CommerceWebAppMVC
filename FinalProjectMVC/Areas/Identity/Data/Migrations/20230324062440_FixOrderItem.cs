using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerProductId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SellerProductId",
                table: "OrderItems",
                column: "SellerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_SellerProducts_SellerProductId",
                table: "OrderItems",
                column: "SellerProductId",
                principalTable: "SellerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict,
                onUpdate: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_SellerProducts_SellerProductId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SellerProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SellerProductId",
                table: "OrderItems");
        }
    }
}
