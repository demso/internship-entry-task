using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record CreateGameDto(
    int BoardSize, //если равно 0, то берется значение из константы на сервере
    int WinCondition
);