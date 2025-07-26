using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Data;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Handlers;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Mapping;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";
    const float DIRTY_TRICK_CHANCE = 10; // вероятность смены символа игрока на символ противника
    const int TURN_COUNT = 3; // частота срабатывания вероятности (сколько ходов до смены символа)
    private static readonly int BOARD_SIZE = int.Parse(Environment.GetEnvironmentVariable("BOARD_SIZE")!); // размер поля игры
    private static readonly int WIN_CONDITION = int.Parse(Environment.GetEnvironmentVariable("WIN_CONDITION")!); // сколько должно быть символов одного типа в ряд для победы

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        //GET /health
        app.MapGet("health", () => Results.Ok("Сервер онлайн"));
        
        //GET /games
        app.MapGet("games", async (IGameRepositoryAsync rep) => 
             Results.Ok((await rep.GetAllGamesAsync()).Select(g => g.ToDto())));
        
        //GET /games/{id}
        app.MapGet("games/{id}",async  (int id, IGameRepositoryAsync rep) => {
                Game? game = await rep.GetGameByIdAsync(id);
                return game is null ? Results.NotFound("Данной игры нет в базе") : Results.Ok(game.ToDto());
            })
            .WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("games", async (IGameRepositoryAsync rep, ILogger<Program> log) => {
            Game? game = GameHandler.CreateGame(BOARD_SIZE, WIN_CONDITION);

            if (game is null) {
                string err =
                    $"Невозможно создать игру. BOARD_SIZE должно быть >= 3, WIN_CONDITION >= 1, WIN_CONDITION <= BOARD_SIZE, а сейчас BOARD_SIZE: {BOARD_SIZE}, WIN_CONDITION: {WIN_CONDITION}";
                log.LogError(err);
                return Results.BadRequest(err);
            }

            await rep.AddGameAsync(game);
            
            log.LogInformation($"Игра с ID: {game.Id} создана");
            
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
            })
            .WithParameterValidation();

        //PUT games/{id}/move
        app.MapPut("games/{id}/move", ProcessMoveAwait)
            .WithParameterValidation();
        
        return app;
    }
    
    private static async Task<IResult> ProcessMoveAwait(int id, MoveDto move,  IGameRepositoryAsync rep, ILogger<Program> log) {
        Game? game = await rep.GetGameByIdAsync(id);
            
        if (game is null)
            return Results.BadRequest("Нет такой игры");
        
        var player = move.Player;
        
        if (game.GameState.Equals(GameState.Finished)) 
            return Results.BadRequest($"Игра окончена, результат {game.GameResult}");
        if (!player.Equals(game.WhoseTurn)) 
            return Results.BadRequest($"Не ваш ход, ходит {game.WhoseTurn}");
        if (!GameHandler.IsInBounds(move.Row, move.Column, game.BoardSize))
            return Results.BadRequest("Клетка вне поля");
        if (game.Board[move.Row][move.Column].Length > 0)
            return Results.BadRequest("Клетка занята");

        log.LogInformation($"{player} сделал ход на {move.Row}, {move.Column}");
        
        if (game.TurnNumber > 0 && game.TurnNumber % TURN_COUNT == 0) {
            var trickTest = new Random().NextSingle() <= DIRTY_TRICK_CHANCE / 100f;
            if (trickTest) {
                player = player.Equals(Player.X) ? Player.O : Player.X;
                log.LogWarning($"Символ игрока изменен на противоположный: {player}");
            }
        }

        game.Board[move.Row][move.Column] = player.ToString();
        game.TurnNumber += 1;
        
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

        await rep.UpdateGameAsync(game);
    
        return Results.Ok(game.ToDto());
    }
}