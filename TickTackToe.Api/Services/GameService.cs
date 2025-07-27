using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Interfaces;

namespace TickTackToe.Api.Services;

public class GameService : IGameService {
    public bool IsInBounds(int row, int col, int boardSize) => row >= 0 && row < boardSize && col >= 0 && col < boardSize;
    //проверка является ли точка частью выигрышной комбинации
    public bool CheckWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
        return CheckVerticalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckHorizontalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckDiagonalWinCondition(row, col, playerType, winCodition, boardSize, board);
    }
    //проверка является ли точка частью выигрышной комбинации в вертикальном направлении
    public bool CheckVerticalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
        int streak = 0;
        for (int r = row-winCodition; r < row+winCodition; r++) {
            if (!IsInBounds(r, col, boardSize))
                continue;
            if (board[r][col].Equals(playerType)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
    }
    //проверка является ли точка частью выигрышной комбинации в горизонтальном направлении
    public bool CheckHorizontalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
        int streak = 0;
        for (int c = col-winCodition; c < col+winCodition; c++) {
            if (!IsInBounds(row, c, boardSize))
                continue;
            if (board[row][c].Equals(playerType)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
    }
    //проверка является ли точка частью выигрышной комбинации в диагональном напралении
    public bool CheckDiagonalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
        int streak = 0;
        for (int c = col-winCodition, r = row-winCodition; c < col+winCodition && r < row+winCodition; c++, r++) {
            if (!IsInBounds(r, c, boardSize))
                continue;
            if (board[r][c].Equals(playerType)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }

        streak = 0;
        for (int c = col-winCodition, r = row+winCodition; c < col+winCodition && r >= row-winCodition; c++, r--) {
            if (!IsInBounds(r, c, boardSize))
                continue;
            if (board[r][c].Equals(playerType)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
        
    }
    public bool CheckDraw(string[][] board) {
        return board.All(subArr => !Array.Exists(subArr, string.IsNullOrEmpty));
    }
    public Game? CreateGame(int boardSize, int winCondition) {
        if (boardSize < 3 || winCondition < 1 || winCondition > boardSize) 
            return null;
        
        var newBoard = new string[boardSize][];

        for (var i = 0; i < boardSize; i++) {
            newBoard[i] = new string[boardSize];
            for (var j = 0; j < boardSize; j++) {
                newBoard[i][j] = string.Empty;
            }
        }
        
        return new Game {
            WhoseTurn = Player.X,
            TurnNumber = 0,
            BoardSize = boardSize,
            Board = newBoard,
            GameResult = GameResult.None,
            GameState = GameState.InProgress,
            WinCondition = winCondition
        };
    }
}