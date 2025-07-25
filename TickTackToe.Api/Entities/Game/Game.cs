using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Entities.Game;

public class Game {
    public int Id { get; set; }
    public Player WhoseTurn { get; set; } = Player.X;
    public int TurnNumber { get; set; }
    public int BoardSize { get; set; }
    public required string[] Board { get; set; }
    public GameResult GameResult { get; set; } = GameResult.None;
    public GameState GameState { get; set; } = GameState.InProgress;
    public int WinCondition { get; set; }
}