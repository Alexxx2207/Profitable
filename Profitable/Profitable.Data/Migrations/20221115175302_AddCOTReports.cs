using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class AddCOTReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COTReportedInstruments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COTReportedInstruments", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "COTReports",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatePublished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssetManagersLong = table.Column<long>(type: "bigint", nullable: false),
                    AssetManagersShort = table.Column<long>(type: "bigint", nullable: false),
                    LeveragedFundsLong = table.Column<long>(type: "bigint", nullable: false),
                    LeveragedFundsShort = table.Column<long>(type: "bigint", nullable: false),
                    COTReportedInstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COTReports", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_COTReports_COTReportedInstruments_COTReportedInstrumentId",
                        column: x => x.COTReportedInstrumentId,
                        principalTable: "COTReportedInstruments",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COTReports_COTReportedInstrumentId",
                table: "COTReports",
                column: "COTReportedInstrumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COTReports");

            migrationBuilder.DropTable(
                name: "COTReportedInstruments");
        }
    }
}
