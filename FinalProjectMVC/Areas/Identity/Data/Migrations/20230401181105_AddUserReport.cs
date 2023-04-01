using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApplicationUserId",
                table: "Reports",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ApplicationUserId",
                table: "Reports",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ApplicationUserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ApplicationUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reports");
        }
    }
}
