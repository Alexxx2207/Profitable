using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class ChangeCreatedToLasUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ListCreatedOn",
                table: "PositionsRecordLists",
                newName: "LastUpdated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "PositionsRecordLists",
                newName: "ListCreatedOn");
        }
    }
}
