using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerProductId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_SellerProductId",
                table: "CartItems",
                column: "SellerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_SellerProducts_SellerProductId",
                table: "CartItems",
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
                name: "FK_CartItems_SellerProducts_SellerProductId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_SellerProductId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SellerProductId",
                table: "CartItems");
        }
    }
}
