using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class ChangeCOTReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssetManagersLongChange",
                table: "COTReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AssetManagersShortChange",
                table: "COTReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LeveragedFundsLongChange",
                table: "COTReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LeveragedFundsShortChange",
                table: "COTReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetManagersLongChange",
                table: "COTReports");

            migrationBuilder.DropColumn(
                name: "AssetManagersShortChange",
                table: "COTReports");

            migrationBuilder.DropColumn(
                name: "LeveragedFundsLongChange",
                table: "COTReports");

            migrationBuilder.DropColumn(
                name: "LeveragedFundsShortChange",
                table: "COTReports");
        }
    }
}
