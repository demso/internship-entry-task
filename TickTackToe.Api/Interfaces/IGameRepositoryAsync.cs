using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Interfaces;

public interface IGameRepositoryAsync
{
    Task<Game> GetGameByIdAsync(int id);
    Task<IReadOnlyCollection<Game>> GetAllGamesAsync();
    Task<Game> CreateGameAsync(int boardSize, int winCondition);
    Task<bool> UpdateGameAsync(Game game);
}