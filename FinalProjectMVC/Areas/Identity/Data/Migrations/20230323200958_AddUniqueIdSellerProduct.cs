using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIdSellerProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SellerProducts_SellerId",
                table: "SellerProducts");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerProducts_SellerId_ProductId",
                table: "SellerProducts",
                columns: new[] { "SellerId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SellerProducts_SellerId_ProductId",
                table: "SellerProducts");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SellerProducts_SellerId",
                table: "SellerProducts",
                column: "SellerId");
        }
    }
}
