using System.ComponentModel.DataAnnotations;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Dtos;

public record GameDto(
    int Id, 
    Player WhoseTurn, //"O" or "X"
    int TurnNumber, 
    [Required] string[][] Board,
    GameResult GameResult,
    GameState GameState,
    int WinCondition
); 