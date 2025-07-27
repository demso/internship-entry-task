using Microsoft.EntityFrameworkCore;
using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Data;

public class GameContext(DbContextOptions<GameContext> options) : DbContext(options) {
    public DbSet<Game> Games => Set<Game>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
}