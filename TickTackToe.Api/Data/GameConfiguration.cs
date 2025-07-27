using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Data;

public class GameConfiguration : IEntityTypeConfiguration<Game> {
    public void Configure(EntityTypeBuilder<Game> builder) {
        builder.Property(g => g.Board).HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<string?[][]>(v, (JsonSerializerOptions?)null)!,
                new ValueComparer<string?[][]>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.Select(x => x.ToArray()).ToArray()));
    }
}