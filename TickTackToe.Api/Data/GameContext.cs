using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Entities;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Data;

public class GameContext(DbContextOptions<GameContext> options) : DbContext(options) {
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Move> Moves => Set<Move>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
}