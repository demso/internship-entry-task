using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record CreateGameDto(
    [Range(3, int.MaxValue)] int BoardSize,
    [Range(3, int.MaxValue)] int WinCondition
);