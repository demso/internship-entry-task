using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record CreateGameDto(
    [Required][Range(3, int.MaxValue)] int BoardSize,
    [Required][Range(3, int.MaxValue)] int WinCondition
);