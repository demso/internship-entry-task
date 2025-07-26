using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;

namespace TickTackToe.Api.Mapping;

//TODO remove move table and entity

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