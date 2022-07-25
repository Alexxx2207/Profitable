using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class ImageTypeProfilePic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "AspNetUsers");
        }
    }
}
