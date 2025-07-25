using System.ComponentModel.DataAnnotations;

namespace TickTackToe.Api.Dtos;

public record GameDto(
    [Required] int Id, 
    [Required] string WhoseTurn, //"O" or "X"
    int TurnNumber, 
    [Required] int BoardSize, 
    [Required] string[] Board, //0 - empty, 1 - circle, 2 - cross
    string GameResult,
    string State, //0 - not started, 1 - in progress, 2 - finished
    int WinCondition
); 