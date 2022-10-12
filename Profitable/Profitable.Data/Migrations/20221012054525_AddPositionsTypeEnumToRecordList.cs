using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class AddPositionsTypeEnumToRecordList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionsType",
                table: "PositionsRecordLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionsType",
                table: "PositionsRecordLists");
        }
    }
}
