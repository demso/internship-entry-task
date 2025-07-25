namespace TickTackToe.Api.Handlers;

public static class GameHandler {
    public static bool CheckWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
        return CheckVerticalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckHorizontalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckDiagonalWinCondition(row, col, playerType, winCodition, boardSize, board);
    }
    //public static int GetIndexAt(int row, int col, int boardSize) => row * boardSize + col;
    private static bool IsInBounds(int row, int col, int boardSize) => row >= 0 && row < boardSize && col >= 0 && col < boardSize;
    public static bool CheckVerticalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
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
    public static bool CheckHorizontalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
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

    public static bool CheckDiagonalWinCondition(int row, int col, string playerType, int winCodition, int boardSize, string[][] board) {
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

    public static bool CheckDraw(string[][] board) {
        return board.All(subArr => !Array.Exists(subArr, string.IsNullOrEmpty));
    }
}