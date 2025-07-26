using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Data;
using TickTackToe.Api.Dtos;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Handlers;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Mapping;

namespace TickTackToe.Api.Repositories;

class GameRepositoryAsync(GameContext dbContext, ILogger<Program> log) : IGameRepositoryAsync{
    public async Task<Game?> GetGameByIdAsync(int id) {
        return await dbContext.Games.FindAsync(id);
    }
    public async Task<IReadOnlyCollection<Game>> GetAllGamesAsync() {
        return await dbContext.Games
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task AddGameAsync(Game game) {
        await dbContext.Games.AddAsync(game);
        await dbContext.SaveChangesAsync();
    }
    public async Task UpdateGameAsync(Game game) {
        dbContext.Update(game);
        await dbContext.SaveChangesAsync();
    }
}