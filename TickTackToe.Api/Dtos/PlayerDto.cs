using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record PlayerDto(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int GameId
);