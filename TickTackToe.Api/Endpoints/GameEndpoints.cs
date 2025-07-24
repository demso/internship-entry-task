using TickTackToe.Api.Dtos;

namespace TickTackToe.Api.Endpoints;

public static class GameEndpoints {
    const string GetGameEndpointName = "GetGame";

    static int PlayerCount = 0;

    private static readonly List<int> playerIds = new List<int>();

    private static readonly List<GameDto> games = new List<GameDto>();
    private static readonly List<MoveDto> moves = new List<MoveDto>();
    //HashSet<PlayerDto> players = new HashSet<PlayerDto>();

    public static WebApplication MapGameEndpoints(this WebApplication app) {
        app.MapGet("games", () => games);

        app.MapGet("games/{id}", (int id) => {
                GameDto? game = games.Find(x => x.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("games", (CreateGameDto newGame) => {
            // if(newGame.BoardSize <= 0)
            //     return Results.BadRequest("Invalid board size");
            GameDto game = new(
                games.Count + 1,
                0,
                ++PlayerCount,
                -1,
                true, // circle
                0,
                newGame.BoardSize,
                new int[newGame.BoardSize * newGame.BoardSize],
                null,
                0,
                newGame.WinCondition
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        })
        .WithParameterValidation();

        app.MapPut("games/{id}/move", (int id, MoveDto move) => { //game id
            GameDto game = ProcessMove(id, move);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        app.MapPut("games/{id}/join", (int id) => {
            var index = games.FindIndex(game => game.Id == id);
            
            if (index == -1) 
                return Results.NotFound();
            
            var game = games[index];
            if (game.PlayerId1 < 0) {
                games[index] = game with { Id = id, PlayerId1 = ++PlayerCount };
            } else if (game.PlayerId2 < 0) {
                games[index] = game with { Id = id, PlayerId2 = ++PlayerCount };
            }
            return Results.Ok(game);
        });
        
        return app;
    }
    
    private static GameDto ProcessMove(int id, MoveDto move) {
        var index = games.FindIndex(game => game.Id == id);
        var game = games[index];
        var newBoard = (int[])game.Board.Clone();
        bool playerType = game.Player1Type;
        if (game.PlayerId1 == move.PlayerId) {
            playerType = game.Player1Type;
        } else {
            playerType = !game.Player1Type;
        }
        newBoard[game.BoardSize * move.Row + move.Column] = playerType ? 1 : 2;
        
        moves.Add(move);
        
        var winCondition = game.WinCondition;
        var boardSize = game.BoardSize;
        
        if (CheckWinCondition(move.Row, move.Column, playerType, winCondition, boardSize, newBoard)) {
            games[index] = new GameDto(
                id,
                -1,
                game.PlayerId1,
                game.PlayerId2,
                game.Player1Type,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                move.PlayerId,
                2,
                game.WinCondition
            );
        } else {
            var playerNextMove = game.PlayerId1 == move.PlayerId ? game.PlayerId2 : game.PlayerId1;
            games[index] = new GameDto(
                id,
                playerNextMove,
                game.PlayerId1,
                game.PlayerId2,
                game.Player1Type,
                game.TurnNumber + 1,
                game.BoardSize,
                newBoard,
                game.WinnerId,
                game.State,
                game.WinCondition
            );
        }
        return game;
    }

    public static bool CheckWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, int[] board) {
        return CheckVerticalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckHorizontalWinCondition(row, col, playerType, winCodition, boardSize, board)
               || CheckDiagonalWinCondition(row, col, playerType, winCodition, boardSize, board);
    }
    private static int GetIndexAt(int row, int col, int boardSize) => row * boardSize + col;
    private static bool IsInBounds(int row, int col, int boardSize) => row >= 0 && row < boardSize && col >= 0 && col < boardSize;
    public static bool CheckVerticalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, int[] board) {
        int streak = 0;
        for (int r = row-winCodition; r < row+winCodition; r++) {
            if (!IsInBounds(r, col, boardSize))
                continue;
            if (board[GetIndexAt(r, col, boardSize)] == (playerType ? 1 : 2)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
    }
    public static bool CheckHorizontalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, int[] board) {
        int streak = 0;
        for (int c = col-winCodition; c < col+winCodition; c++) {
            if (!IsInBounds(row, c, boardSize))
                continue;
            if (board[GetIndexAt(row, c, boardSize)] == (playerType ? 1 : 2)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
    }
    public static bool CheckDiagonalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, int[] board) {
        int streak = 0;
        for (int c = col-winCodition, r = row-winCodition; c < col+winCodition && r < row+winCodition; c++, r++) {
            if (!IsInBounds(r, c, boardSize))
                continue;
            if (board[GetIndexAt(r, c, boardSize)] == (playerType ? 1 : 2)) {
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
            if (board[GetIndexAt(r, c, boardSize)] == (playerType ? 1 : 2)) {
                streak++;
                if (streak >= winCodition)
                    return true;
            } else {
                streak = 0;
            }
        }
        return false;
        
    }
}