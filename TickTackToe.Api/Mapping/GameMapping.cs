using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;

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