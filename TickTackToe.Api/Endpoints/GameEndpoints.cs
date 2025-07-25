using TickTackToe.Api.Dtos;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";

    static int GameCount = 0;

    private static readonly List<GameDto> games = new List<GameDto>();
    private static readonly List<MoveDto> moves = new List<MoveDto>();

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        //app.MapGet("games", () => games);

        app.MapGet("games/{id}", (int id) => {
                GameDto? game = games.Find(x => x.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("games", (CreateGameDto newGame) => {
            var newBoard = new string[newGame.BoardSize * newGame.BoardSize];
            Array.Fill(newBoard, string.Empty);
            GameDto game = new(
                GameCount++, 
                nameof(Player.X),
                0,
                newGame.BoardSize,
                newBoard,
                nameof(GameResult.None),
                nameof(GameStatus.InProgress),
                newGame.WinCondition
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        })
        .WithParameterValidation();

        app.MapPut("games/{id}/move", ProcessMove);
        
        return app;
    }
    
    private static IResult ProcessMove(int id, MoveDto move) {
        var index = games.FindIndex(game => game.Id == id);
        var game = games[index];
        var newBoard = (string[])game.Board.Clone();
        var playerType = move.Player;

        if (game.State.Equals(nameof(GameStatus.Finished))) {
            return Results.BadRequest("Игра окончена");
        }
        if (!playerType.Equals(game.WhoseTurn)) {
            return Results.BadRequest("Не ваш ход");
        }
        
        newBoard[game.BoardSize * move.Row + move.Column] = playerType;
        
        moves.Add(move);
        
        var winCondition = game.WinCondition;
        var boardSize = game.BoardSize;
        
        if (GameHandler.CheckWinCondition(move.Row, move.Column, playerType, winCondition, boardSize, newBoard)) {
            var gameResult = playerType.Equals(nameof(Player.X)) ? nameof(GameResult.WinX) : nameof(GameResult.WinO);
            games[index] = new GameDto(
                id,
                game.WhoseTurn,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                gameResult,
                nameof(GameStatus.Finished),
                game.WinCondition
            );
        } else {
            if (GameHandler.CheckDraw(newBoard)) {
                games[index] = new GameDto(
                    id,
                    game.WhoseTurn,
                    game.TurnNumber + 1,
                    game.BoardSize,
                    newBoard,
                    nameof(GameResult.Draw),
                    nameof(GameStatus.Finished),
                    game.WinCondition
                );
                
            }
            var nextTurnPlayer = playerType.Equals(nameof(Player.X)) ? nameof(Player.O) : nameof(Player.X);
            games[index] = new GameDto(
                id,
                nextTurnPlayer,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                game.GameResult,
                game.State,
                game.WinCondition
            );
        }
        return Results.Ok(games[index]);
    }
}