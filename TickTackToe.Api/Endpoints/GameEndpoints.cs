using TickTackToe.Api.Dtos;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";

    static int PlayerCount = 0;

    private static readonly List<int> playerIds = new List<int>();

    private static readonly List<GameDto> games = new List<GameDto>();
    private static readonly List<MoveDto> moves = new List<MoveDto>();
    //HashSet<PlayerDto> players = new HashSet<PlayerDto>();

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        app.MapGet("games", () => games);

        app.MapGet("games/{id:guid}", (Guid id) => {
                GameDto? game = games.Find(x => x.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("games", (CreateGameDto newGame) => {
            // if(newGame.BoardSize <= 0)
            //     return Results.BadRequest("Invalid board size");
            GameDto game = new(
                Guid.NewGuid(), 
                nameof(Player.X),
                0,
                newGame.BoardSize,
                new string[newGame.BoardSize * newGame.BoardSize],
                nameof(GameResult.None),
                nameof(GameStatus.NotStarted),
                newGame.WinCondition
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        })
        .WithParameterValidation();

        app.MapPut("games/{id}/move", (int id, MoveDto move) => { //game id
            GameDto game = ProcessMove(id, move);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        app.MapPut("games/{id}/join", (Guid id) => {
            var index = games.FindIndex(game => game.Id == id);
            
            if (index == -1) 
                return Results.NotFound();
            
            var game = games[index];
            return Results.Ok(game);
        });
        
        return app;
    }
    
    private static GameDto ProcessMove(Guid id, MoveDto move) {
        var index = games.FindIndex(game => game.Id == id);
        var game = games[index];
        var newBoard = (int[])game.Board.Clone();
        bool playerType = game.Player1Type;
        if (game.PlayerId1 == move.PlayerId) {
            playerType = game.Player1Type;
        } else {
            playerType = !game.Player1Type;
        }
        newBoard[game.BoardSize * move.Row + move.Column] = playerType ? 1 : 2;
        
        moves.Add(move);
        
        var winCondition = game.WinCondition;
        var boardSize = game.BoardSize;
        
        if (GameHandler.CheckWinCondition(move.Row, move.Column, playerType, winCondition, boardSize, newBoard)) {
            games[index] = new GameDto(
                id,
                -1,
                game.PlayerId1,
                game.PlayerId2,
                game.Player1Type,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                move.PlayerId,
                2,
                game.WinCondition
            );
        } else {
            var playerNextMove = game.PlayerId1 == move.PlayerId ? game.PlayerId2 : game.PlayerId1;
            games[index] = new GameDto(
                id,
                playerNextMove,
                game.PlayerId1,
                game.PlayerId2,
                game.Player1Type,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                game.GameResult,
                game.State,
                game.WinCondition
            );
        }
        return game;
    }
}