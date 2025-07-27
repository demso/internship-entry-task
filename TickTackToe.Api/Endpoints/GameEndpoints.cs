using Microsoft.AspNetCore.Mvc;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Mapping;
using TickTackToe.Api.Services;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    // вероятность смены символа игрока на символ противника
    private const float DirtyTrickChance = 10; 
    // частота срабатывания вероятности (сколько ходов до смены символа)
    private const int TurnCount = 3; 
    // размер поля игры
    private static readonly int BoardSize = int.Parse(Environment.GetEnvironmentVariable("BOARD_SIZE")!); 
    // сколько должно быть символов одного типа в ряд для победы
    private static readonly int WinCondition = int.Parse(Environment.GetEnvironmentVariable("WIN_CONDITION")!); 
    private const string GetGameEndpointName = "GetGame";

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
            Game? game = GameService.CreateGame(BoardSize, WinCondition);

            if (game is null) {
                string err =
                    $"Невозможно создать игру. BOARD_SIZE должно быть >= 3, WIN_CONDITION >= 1, WIN_CONDITION <= BOARD_SIZE, а сейчас BOARD_SIZE: {BoardSize}, WIN_CONDITION: {WinCondition}";
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
    
    private static async Task<IResult> ProcessMoveAwait(HttpResponse response, [FromHeader(Name = "If-Match")] string? ifMatchHeader, 
            int id, MoveDto move,  IGameRepositoryAsync rep, ILogger<Program> log) {
        Game? game = await rep.GetGameByIdAsync(id);
            
        if (game is null)
            return Results.BadRequest("Нет такой игры");
        
        var currentETag = ETagService.GenerateETag(game);
        
        if (ifMatchHeader != null && ifMatchHeader != currentETag)
        {
            log.LogError("Etag не совпадает");
            return Results.Json(
                data: "Необходимо обновить состояние игры у клиента.",
                statusCode: StatusCodes.Status412PreconditionFailed
            );
        }
        
        var player = move.Player;
        
        if (game.GameState.Equals(GameState.Finished)) 
            return Results.BadRequest($"Игра окончена, результат {game.GameResult}");
        if (!player.Equals(game.WhoseTurn)) 
            return Results.BadRequest($"Не ваш ход, ходит {game.WhoseTurn}");
        if (!GameService.IsInBounds(move.Row, move.Column, game.BoardSize))
            return Results.BadRequest("Клетка вне поля");
        if (game.Board[move.Row][move.Column].Length > 0)
            return Results.BadRequest("Клетка занята");

        log.LogInformation($"{player} сделал ход на {move.Row}, {move.Column}");
        
        if (game.TurnNumber > 0 && game.TurnNumber % TurnCount == 0) {
            var trickTest = new Random().NextSingle() <= DirtyTrickChance / 100f;
            if (trickTest) {
                player = player.Equals(Player.X) ? Player.O : Player.X;
                log.LogWarning($"Символ игрока изменен на противоположный: {player}");
            }
        }

        game.Board[move.Row][move.Column] = player.ToString();
        game.TurnNumber += 1;
        
        if (GameService.CheckWinCondition(move.Row, move.Column, player.ToString(), game.WinCondition, game.BoardSize, game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = player.Equals(Player.X) ? GameResult.WinX : GameResult.WinO;
            log.LogInformation($"{player} выйграли!");
        } 
        if (GameService.CheckDraw(game.Board)) {
            game.GameState = GameState.Finished;
            game.GameResult = GameResult.Draw;
            log.LogInformation("Ничья!");
        } else {
            game.WhoseTurn = game.WhoseTurn.Equals(Player.X) ? Player.O : Player.X;
        }

        await rep.UpdateGameAsync(game);
        
        response.Headers.ETag = ETagService.GenerateETag(game);
    
        return Results.Ok(game.ToDto());
    }
}