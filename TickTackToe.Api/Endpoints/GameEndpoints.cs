using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Data;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;
using TickTackToe.Api.Mapping;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";
    const float SWITCH_CHANCE = 10f;
    const int TURN_COUNT = 3;

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        //GET /health
        app.MapGet("health", () => Results.Ok("Сервер онлайн"));
        
        //GET /games
        app.MapGet("games", (GameContext dbContext) => 
            dbContext.Games
                     .Select(game => game.ToDto())
                     .AsNoTracking());
        
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
    
    private static IResult ProcessMove(int id, MoveDto move,  GameContext dbContext, ILogger<Program> log) {
        Game? game = dbContext.Games.Find(id);
        
        if (game is null)
            return Results.BadRequest("Нет такой игры");
        
        var player = move.Player;
        
        if (game.GameState.Equals(GameState.Finished)) 
            return Results.BadRequest("Игра окончена");
        if (!player.Equals(game.WhoseTurn)) 
            return Results.BadRequest($"Не ваш ход, ходит [{game.WhoseTurn}]");
        if (!GameHandler.IsInBounds(move.Row, move.Column, game.BoardSize))
            return Results.BadRequest("Клетка вне поля");
        if (game.Board[move.Row][move.Column].Length > 0)
            return Results.BadRequest("Клетка занята");

        log.LogInformation($"{player} сделал ход на {move.Row}, {move.Column}");
        
        if (game.TurnNumber > 0 && game.TurnNumber % TURN_COUNT == 0) {
            var switchTest = new Random().NextSingle() <= SWITCH_CHANCE / 100f;
            if (switchTest) {
                player = player.Equals(Player.X) ? Player.O : Player.X;
                log.LogWarning($"Символ игрока изменен на противоположный: {player}");
            }
        }

        game.Board[move.Row][move.Column] = player.ToString();
        game.TurnNumber += 1;

        Move curMove = move.ToEntity(game.Id);
        curMove.Game = game;
        
        dbContext.Moves.Add(curMove);
        
        if (GameHandler.CheckWinCondition(move.Row, move.Column, player.ToString(), game.WinCondition, game.BoardSize, game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = player.Equals(Player.X) ? GameResult.WinX : GameResult.WinO;
            log.LogInformation($"{player} выйграли!");
        } 
        if (GameHandler.CheckDraw(game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = GameResult.Draw;
            log.LogInformation("Ничья!");
        } else {
            game.WhoseTurn = player.Equals(Player.X) ? Player.O : Player.X;
        }
        
        dbContext.SaveChanges();
    
        return Results.Ok(game.ToDto());
    }
}