using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record MoveDto(
    [Required][Range(0, int.MaxValue)] int GameId,
    [Required] string Player,
    [Required][Range(0, int.MaxValue)] int Row,
    [Required][Range(0, int.MaxValue)] int Column
);