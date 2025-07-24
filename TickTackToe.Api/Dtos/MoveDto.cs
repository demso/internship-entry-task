using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record MoveDto(
    [Range(0, int.MaxValue)] int GameId,
    [Range(0, int.MaxValue)] int PlayerId,
    [Range(0, int.MaxValue)] int Row,
    [Range(0, int.MaxValue)] int Column
);