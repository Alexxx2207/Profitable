using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class AddExchangeFinantialInstruments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListsTicketSymbols");

            migrationBuilder.DropTable(
                name: "TicketSymbols");

            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    GUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.GUID);
                });

            migrationBuilder.CreateTable(
                name: "FinancialInstruments",
                columns: table => new
                {
                    GUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TickerSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInstruments", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_FinancialInstruments_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialInstruments_MarketTypes_MarketTypeId",
                        column: x => x.MarketTypeId,
                        principalTable: "MarketTypes",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListsFinancialInstruments",
                columns: table => new
                {
                    GUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FinancialInstrumentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsFinancialInstruments", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_ListsFinancialInstruments_FinancialInstruments_FinancialInstrumentId",
                        column: x => x.FinancialInstrumentId,
                        principalTable: "FinancialInstruments",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListsFinancialInstruments_Lists_ListId",
                        column: x => x.ListId,
                        principalTable: "Lists",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInstruments_ExchangeId",
                table: "FinancialInstruments",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInstruments_MarketTypeId",
                table: "FinancialInstruments",
                column: "MarketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListsFinancialInstruments_FinancialInstrumentId",
                table: "ListsFinancialInstruments",
                column: "FinancialInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ListsFinancialInstruments_ListId",
                table: "ListsFinancialInstruments",
                column: "ListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListsFinancialInstruments");

            migrationBuilder.DropTable(
                name: "FinancialInstruments");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.CreateTable(
                name: "TicketSymbols",
                columns: table => new
                {
                    GUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MarketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSymbols", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_TicketSymbols_MarketTypes_MarketTypeId",
                        column: x => x.MarketTypeId,
                        principalTable: "MarketTypes",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListsTicketSymbols",
                columns: table => new
                {
                    GUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketSymbolGUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsTicketSymbols", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_ListsTicketSymbols_Lists_ListId",
                        column: x => x.ListId,
                        principalTable: "Lists",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListsTicketSymbols_TicketSymbols_TicketSymbolGUID",
                        column: x => x.TicketSymbolGUID,
                        principalTable: "TicketSymbols",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListsTicketSymbols_ListId",
                table: "ListsTicketSymbols",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_ListsTicketSymbols_TicketSymbolGUID",
                table: "ListsTicketSymbols",
                column: "TicketSymbolGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSymbols_MarketTypeId",
                table: "TicketSymbols",
                column: "MarketTypeId");
        }
    }
}
