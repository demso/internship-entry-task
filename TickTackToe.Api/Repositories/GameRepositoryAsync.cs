using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Data;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Mapping;

namespace TickTackToe.Api.Repositories;

class GameRepositoryAsync(GameContext dbContext, ILogger<Program> log) : IGameRepositoryAsync{
    public async Task<Game> GetGameByIdAsync(int id) {
        throw new NotImplementedException();
    }
    public async Task<IReadOnlyCollection<Game>> GetAllGamesAsync() {
        return await dbContext.Games
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Game> CreateGameAsync(int boardSize, int winCondition) {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdateGameAsync(Game game) {
        throw new NotImplementedException();
    }
}