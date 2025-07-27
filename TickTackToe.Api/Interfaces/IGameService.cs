using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Interfaces;

public interface IGameService {
    public bool IsInBounds(int row, int col, int boardSize);

    public bool CheckWinCondition(int row, int col, string playerType, int winCodition, int boardSize,
        string[][] board);

    public bool CheckVerticalWinCondition(int row, int col, string playerType, int winCodition, int boardSize,
        string[][] board);

    public bool CheckHorizontalWinCondition(int row, int col, string playerType, int winCodition, int boardSize,
        string[][] board);

    public bool CheckDiagonalWinCondition(int row, int col, string playerType, int winCodition, int boardSize,
        string[][] board);

    public bool CheckDraw(string[][] board);
    public Game? CreateGame(int boardSize, int winCondition);
    
}