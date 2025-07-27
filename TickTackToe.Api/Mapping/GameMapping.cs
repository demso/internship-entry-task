using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Mapping;

public static class GameMapping {
    public static GameDto ToDto(this Game game) {
        return new GameDto(
            game.Id,
            game.WhoseTurn,
            game.TurnNumber,
            game.Board,
            game.GameResult,
            game.GameState,
            game.WinCondition
            );
    }
}