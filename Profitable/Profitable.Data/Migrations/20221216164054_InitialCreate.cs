using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Guid);
                });

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
                name: "Exchanges",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "FuturesContracts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TickSize = table.Column<double>(type: "float", nullable: false),
                    TickValue = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturesContracts", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "MarketTypes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTypes", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Guid);
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
                    AssetManagersLongChange = table.Column<long>(type: "bigint", nullable: false),
                    AssetManagersShortChange = table.Column<long>(type: "bigint", nullable: false),
                    LeveragedFundsLongChange = table.Column<long>(type: "bigint", nullable: false),
                    LeveragedFundsShortChange = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "FinancialInstruments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TickerSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarketTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInstruments", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_FinancialInstruments_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialInstruments_MarketTypes_MarketTypeId",
                        column: x => x.MarketTypeId,
                        principalTable: "MarketTypes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationRole = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "PositionsRecordLists",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstrumentGroup = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionsRecordLists", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_PositionsRecordLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradePositions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionAddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantitySize = table.Column<double>(type: "float", nullable: false),
                    PositionsRecordListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryPrice = table.Column<double>(type: "float", nullable: false),
                    ExitPrice = table.Column<double>(type: "float", nullable: false),
                    RealizedProfitAndLoss = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePositions", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_TradePositions_PositionsRecordLists_PositionsRecordListId",
                        column: x => x.PositionsRecordListId,
                        principalTable: "PositionsRecordLists",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuturesPositions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    TradePositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuturesContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturesPositions", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_FuturesPositions_FuturesContracts_FuturesContractId",
                        column: x => x.FuturesContractId,
                        principalTable: "FuturesContracts",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuturesPositions_TradePositions_TradePositionId",
                        column: x => x.TradePositionId,
                        principalTable: "TradePositions",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocksPositions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradePositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyCommission = table.Column<double>(type: "float", nullable: false),
                    SellCommission = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocksPositions", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_StocksPositions_TradePositions_TradePositionId",
                        column: x => x.TradePositionId,
                        principalTable: "TradePositions",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COTReports_COTReportedInstrumentId",
                table: "COTReports",
                column: "COTReportedInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInstruments_ExchangeId",
                table: "FinancialInstruments",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInstruments_MarketTypeId",
                table: "FinancialInstruments",
                column: "MarketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FuturesPositions_FuturesContractId",
                table: "FuturesPositions",
                column: "FuturesContractId");

            migrationBuilder.CreateIndex(
                name: "IX_FuturesPositions_TradePositionId",
                table: "FuturesPositions",
                column: "TradePositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionsRecordLists_UserId",
                table: "PositionsRecordLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StocksPositions_TradePositionId",
                table: "StocksPositions",
                column: "TradePositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TradePositions_PositionsRecordListId",
                table: "TradePositions",
                column: "PositionsRecordListId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "COTReports");

            migrationBuilder.DropTable(
                name: "FinancialInstruments");

            migrationBuilder.DropTable(
                name: "FuturesPositions");

            migrationBuilder.DropTable(
                name: "StocksPositions");

            migrationBuilder.DropTable(
                name: "COTReportedInstruments");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.DropTable(
                name: "MarketTypes");

            migrationBuilder.DropTable(
                name: "FuturesContracts");

            migrationBuilder.DropTable(
                name: "TradePositions");

            migrationBuilder.DropTable(
                name: "PositionsRecordLists");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
