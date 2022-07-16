using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profitable.Data.Migrations
{
    public partial class CreateNonclusteredIndexOnTickerSymbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListsFinancialInstruments_FinancialInstruments_FinancialInstrumentId",
                table: "ListsFinancialInstruments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialInstruments",
                table: "FinancialInstruments");

            migrationBuilder.AlterColumn<string>(
                name: "TickerSymbol",
                table: "FinancialInstruments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "FinancialInstruments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "IX_TickerSymbol",
                table: "FinancialInstruments",
                column: "TickerSymbol")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddForeignKey(
                name: "FK_ListsFinancialInstruments_FinancialInstruments_FinancialInstrumentId",
                table: "ListsFinancialInstruments",
                column: "FinancialInstrumentId",
                principalTable: "FinancialInstruments",
                principalColumn: "TickerSymbol",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListsFinancialInstruments_FinancialInstruments_FinancialInstrumentId",
                table: "ListsFinancialInstruments");

            migrationBuilder.DropPrimaryKey(
                name: "IX_TickerSymbol",
                table: "FinancialInstruments");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "FinancialInstruments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TickerSymbol",
                table: "FinancialInstruments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialInstruments",
                table: "FinancialInstruments",
                column: "GUID");

            migrationBuilder.AddForeignKey(
                name: "FK_ListsFinancialInstruments_FinancialInstruments_FinancialInstrumentId",
                table: "ListsFinancialInstruments",
                column: "FinancialInstrumentId",
                principalTable: "FinancialInstruments",
                principalColumn: "GUID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
