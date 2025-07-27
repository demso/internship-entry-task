using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TickTackToe.Api.Data;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Repositories;
using TickTackToe.Api.Services;

namespace TickTackToe.Tests;

public class GameRepositoryTests {
    private readonly IGameService _gameService;
    private readonly GameContext _dbContext;
    private readonly IGameRepositoryAsync _gameRepository;

    public GameRepositoryTests()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<GameContext>(options =>
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));
        services.AddScoped<IGameRepositoryAsync, GameRepositoryAsync>();
        services.AddScoped<IGameService, GameService>();
        
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<GameContext>();
        _gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
        _gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepositoryAsync>();
    }
    
    [Fact]
    public async Task AddGameAsync_SavesGame()
    {
        //Arrange
        var game = _gameService.CreateGame(3, 3);
        
        // Act
        await _gameRepository.AddGameAsync(game!);
        
        // Assert
        var savedGame = await _dbContext.Games.FindAsync(1);
    
        Assert.NotNull(savedGame);
        Assert.Equal(Player.X, savedGame.WhoseTurn);
        Assert.Equal(0, savedGame.TurnNumber);
        Assert.Equal(GameResult.None, savedGame.GameResult);
        Assert.Equal(GameState.InProgress, savedGame.GameState);
    }
    
    [Fact]
    public async Task UpdateGameAsync_UpdatesGame()
    {
        // Arrange
        var game = _gameService.CreateGame(4, 4);
        await _gameRepository.AddGameAsync(game!);
        game!.Board[1][1] = Player.X.ToString();
    
        // Act 
        await _gameRepository.UpdateGameAsync(game);
        
        // Assert
        var updatedGame = await _dbContext.Games.FindAsync(game.Id);
        
        Assert.Equal(Player.X.ToString(), updatedGame!.Board[1][1]);
        Assert.Equal(4, updatedGame.WinCondition);
        Assert.Equal(4, updatedGame.BoardSize);
    }

    [Fact]
    public async Task GetAllGames_ReturnsAllGames() {
        
        // Arrange
        var game = _gameService.CreateGame(5, 5);
        await _gameRepository.AddGameAsync(game!);
        game = _gameService.CreateGame(6, 6);
        await _gameRepository.AddGameAsync(game!);
        
        // Act
        var games = await _gameRepository.GetAllGamesAsync();

        // Assert
        Assert.Equal(2, games.Count);
    }
}