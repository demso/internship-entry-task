using System.ComponentModel.DataAnnotations;
using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Dtos;

public record MoveDto(
    [Required] Player Player,
    [Required][Range(0, int.MaxValue)] int Row,
    [Required][Range(0, int.MaxValue)] int Column
);