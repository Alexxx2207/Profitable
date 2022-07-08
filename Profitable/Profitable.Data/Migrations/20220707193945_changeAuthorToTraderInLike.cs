using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class changeAuthorToTraderInLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_AuthorId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Likes",
                newName: "TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_AuthorId",
                table: "Likes",
                newName: "IX_Likes_TraderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_TraderId",
                table: "Likes",
                column: "TraderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_TraderId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "TraderId",
                table: "Likes",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_TraderId",
                table: "Likes",
                newName: "IX_Likes_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_AuthorId",
                table: "Likes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
