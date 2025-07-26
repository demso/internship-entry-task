using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TickTackToe.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WhoseTurn = table.Column<int>(type: "INTEGER", nullable: false),
                    TurnNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Board = table.Column<string>(type: "TEXT", nullable: false),
                    GameResult = table.Column<int>(type: "INTEGER", nullable: false),
                    GameState = table.Column<int>(type: "INTEGER", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
