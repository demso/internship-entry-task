using TickTackToe.Api.Data;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;
using TickTackToe.Api.Mapping;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        //GET /games
        app.MapGet("games", (GameContext dbContext) => dbContext.Games);
        
        //GET /games/{id}
        app.MapGet("games/{id}", (int id, GameContext dbContext) => {
                Game? game = dbContext.Games.Find(id);
                return game is null ? Results.NotFound() : Results.Ok(game.ToDto());
            })
            .WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("games", (CreateGameDto newGame, GameContext dbContext) => {
            Game game = newGame.ToEntity();
                
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
        })
        .WithParameterValidation();

        //PUT games/{id}/move
        app.MapPut("games/{id}/move", ProcessMove)
            .WithParameterValidation();
        
        return app;
    }
    
    private static IResult ProcessMove(int id, MoveDto move,  GameContext dbContext) {
        Game? game = dbContext.Games.Find(id);
        
        if (game is null)
            return Results.BadRequest("Нет такой игры");
        
        var player = GameMapping.StringToPlayer(move.Player);
        
        if (game.GameState.Equals(GameState.Finished)) 
            return Results.BadRequest("Игра окончена");
        if (player is null)
            return Results.BadRequest("Неверный тип игрока");
        if (!player.Equals(game.WhoseTurn)) 
            return Results.BadRequest($"Не ваш ход, ходит [{game.WhoseTurn}]");
        if (game.Board[move.Row][move.Column].Trim().Length > 0)
            return Results.BadRequest("Клетка занята");
        
        game.Board[move.Row][move.Column] = player.ToString()!;
        game.TurnNumber += 1;

        Move curMove = move.ToEntity();
        curMove.Game = game;
        
        dbContext.Moves.Add(curMove);
        
        if (GameHandler.CheckWinCondition(move.Row, move.Column, player.ToString()!, game.WinCondition, game.BoardSize, game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = player.Equals(Player.X) ? GameResult.WinX : GameResult.WinO;
        } 
        if (GameHandler.CheckDraw(game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = GameResult.Draw;
        } else {
            game.WhoseTurn = player.Equals(Player.X) ? Player.O : Player.X;
        }
        
        dbContext.SaveChanges();
    
        return Results.Ok(game.ToDto());
    }
}