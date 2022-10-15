using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class AddAccountStatisticsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PositionsRecordLists",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionsRecordLists", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_PositionsRecordLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                    TradePositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
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
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuturesPositions");

            migrationBuilder.DropTable(
                name: "StocksPositions");

            migrationBuilder.DropTable(
                name: "TradePositions");

            migrationBuilder.DropTable(
                name: "PositionsRecordLists");
        }
    }
}
