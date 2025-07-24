using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record CreateGameDto(
    [Range(3, int.MaxValue)] uint BoardSize,
    [Range(3, int.MaxValue)] uint WinCondition
);