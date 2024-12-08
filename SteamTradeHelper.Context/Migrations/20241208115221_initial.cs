using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamTradeHelper.Context.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SteamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryCount = table.Column<int>(type: "int", nullable: true),
                    IsTradingBackground = table.Column<bool>(type: "bit", nullable: false),
                    IsTradingEmotion = table.Column<bool>(type: "bit", nullable: false),
                    IsTradingCard = table.Column<bool>(type: "bit", nullable: false),
                    IsTradingFoilCard = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTradeable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBuyTradeable = table.Column<bool>(type: "bit", nullable: false),
                    IsSellTradeable = table.Column<bool>(type: "bit", nullable: false),
                    InventoryCount = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<int>(type: "int", nullable: true),
                    SellPrice = table.Column<int>(type: "int", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bots_SteamId",
                table: "Bots",
                column: "SteamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_GameId",
                table: "Cards",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ItemId",
                table: "Cards",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_AppId",
                table: "Games",
                column: "AppId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bots");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
