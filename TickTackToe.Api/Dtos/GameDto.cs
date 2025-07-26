using System.ComponentModel.DataAnnotations;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Dtos;

public record GameDto(
    [Required] int Id, 
    [Required] Player WhoseTurn, //"O" or "X"
    int TurnNumber, 
    [Required] int BoardSize, 
    [Required] string[][] Board, //0 - empty, 1 - circle, 2 - cross
    GameResult GameResult,
    GameState GameState, //0 - not started, 1 - in progress, 2 - finished
    int WinCondition
); 