using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Mapping;

public static class GameMapping {
    public static Game ToEntity(this CreateGameDto newGame) {
        var newBoard = new string[newGame.BoardSize][];
        var arrLine = new string[newGame.BoardSize];
        Array.Fill(arrLine, string.Empty);
        Array.Fill(newBoard, arrLine);
        return new Game() {
            WhoseTurn = Player.X,
            TurnNumber = 0,
            BoardSize = newGame.BoardSize,
            Board = newBoard,
            GameResult = GameResult.None,
            GameState = GameState.InProgress,
            WinCondition = newGame.WinCondition
        };
    }

    public static GameDto ToDto(this Game game) {
        return new GameDto(
            game.Id,
            game.WhoseTurn,
            game.TurnNumber,
            game.BoardSize,
            game.Board,
            game.GameResult,
            game.GameState,
            game.WinCondition
            );
    }

    public static Player? StringToPlayer(string s) {
        return s switch {
            "X" => Player.X,
            "O" => Player.O,
            _ => null
        };
    }

    public static Move ToEntity(this MoveDto moveDto, int gameId) {
        return new Move() {
            GameId = gameId,
            Player = moveDto.Player,
            Row = moveDto.Row,
            Column = moveDto.Column
        };
    }
    
    public static MoveDto ToDto(this Move move) {
        return new MoveDto(
            move.Player, 
            move.Row, 
            move.Column
            );
    }
}