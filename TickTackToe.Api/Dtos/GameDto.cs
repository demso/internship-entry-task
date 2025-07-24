namespace TickTackToe.Api.Dtos;

public record GameDto(
    Guid Id, 
    string WhoseTurn, //"O" or "X"
    uint TurnNumber, 
    uint BoardSize, 
    string[] Board, //0 - empty, 1 - circle, 2 - cross
    string GameResult,
    string State, //0 - not started, 1 - in progress, 2 - finished
    uint WinCondition
); 