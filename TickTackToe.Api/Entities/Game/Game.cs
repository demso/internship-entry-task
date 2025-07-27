using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Entities.Game;

public class Game {
    public int Id { get; init; }
    public Player WhoseTurn { get; set; } = Player.X;
    public int TurnNumber { get; set; }
    public int BoardSize { get; init; }
    public required string[][] Board { get; init; }
    public GameResult GameResult { get; set; } = GameResult.None;
    public GameState GameState { get; set; } = GameState.InProgress;
    public int WinCondition { get; init; }
}