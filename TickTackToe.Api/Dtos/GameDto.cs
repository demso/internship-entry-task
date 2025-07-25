namespace TickTackToe.Api.Dtos;

public record GameDto(
    int Id, 
    string WhoseTurn, //"O" or "X"
    int TurnNumber, 
    int BoardSize, 
    string[] Board, //0 - empty, 1 - circle, 2 - cross
    string GameResult,
    string State, //0 - not started, 1 - in progress, 2 - finished
    int WinCondition
); 