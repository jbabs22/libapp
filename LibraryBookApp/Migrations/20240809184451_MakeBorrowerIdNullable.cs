using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryBookApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeBorrowerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ApplicationUsers_BorrowerId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ApplicationUserId",
                table: "Books",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ApplicationUsers_ApplicationUserId",
                table: "Books",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ApplicationUsers_BorrowerId",
                table: "Books",
                column: "BorrowerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ApplicationUsers_ApplicationUserId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_ApplicationUsers_BorrowerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ApplicationUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Books");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ApplicationUsers_BorrowerId",
                table: "Books",
                column: "BorrowerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }
    }
}
