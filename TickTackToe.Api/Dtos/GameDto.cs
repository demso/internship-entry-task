namespace TickTackToe.Api.Dtos;

public record GameDto(
    int Id, 
    int PlayerId1, 
    int PlayerId2, 
    bool Player1Type, //true - circle, false - cross
    int TurnNumber, 
    int BoardSize, 
    bool[] Board, //true - circle, false - cross
    int? WinnerId,
    int State, //0 - not started, 1 - in progress, 2 - finished
    int WinCondition
); 