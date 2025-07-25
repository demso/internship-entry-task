using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record MoveDto(
    [Range(0, int.MaxValue)] uint GameId,
    [Required] string Player,
    [Range(0, int.MaxValue)] int Row,
    [Range(0, int.MaxValue)] int Column
);