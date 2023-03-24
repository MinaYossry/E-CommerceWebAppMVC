using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixReportsAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Customers_CustomerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Products_ProductId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CustomerId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Reports",
                newName: "ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ProductId",
                table: "Reports",
                newName: "IX_Reports_ReviewId");

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSolved",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SellerId",
                table: "Reviews",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Reviews_ReviewId",
                table: "Reports",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Sellers_SellerId",
                table: "Reviews",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Reviews_ReviewId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Sellers_SellerId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_SellerId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsSolved",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reports",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReviewId",
                table: "Reports",
                newName: "IX_Reports_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CustomerId",
                table: "Reports",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Customers_CustomerId",
                table: "Reports",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Products_ProductId",
                table: "Reports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
