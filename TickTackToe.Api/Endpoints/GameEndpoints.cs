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
            GameDto game = CheckWinner(id, move);
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
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });
        
        return app;
    }
    
    private static GameDto CheckWinner(int id, MoveDto move) {
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
        int winCondition = game.WinCondition;
        int boardSize = game.BoardSize;
        
        // if (GetIndexAt(,,boardSize))
        //     
        // for (int row = 0; row < game.BoardSize; row++) {
        //     for (int col = 0; col < game.BoardSize; col++) {
        //         if (newBoard[])
        //     }    
        // }
            
        games[index] = new GameDto(
            id,
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
        moves.Add(move);
        return game;
    }
    
    private static int GetIndexAt(int row, int col, int boardSize) => row * boardSize + col;
    private static bool IsInBounds(int row, int col, int boardSize) => row >= 0 && row < boardSize && col >= 0 && col < boardSize;

    private static bool Incrementor(int row, int col, bool playerType, int boardSize, int[] board, int winCondition, int rStep, int cStep) {
        int streak = 0;
        int cStart = cStep switch {
            > 0 => col - winCondition,
            < 0 => col + winCondition,
            _ => col
        };
        
        int rStart = rStep switch {
            > 0 => row - winCondition,
            < 0 => row + winCondition,
            _ => row
        };

        int rEnd = rStep switch {
            > 0 => row + winCondition,
            < 0 => row - winCondition,
            _ => row
        };

        int cEnd = cStep switch {
            > 0 => col + winCondition,
            < 0 => col - winCondition,
            _ => col
        };

        bool result = false;
        
        while (rStart < rEnd || cStart < cEnd) {
            if (rStart < rEnd) rStart += rStep;
            if (cStart < cEnd) cStart += cStep;
            if (!IsInBounds(rStart, col, boardSize))
                continue;
            if (board[GetIndexAt(rStart, col, boardSize)] == (playerType ? 1 : 2)) {
                streak++;
                if (streak >= winCondition) {
                    result = true;
                    break;
                }
            } else {
                streak = 0;
            }
        }
        return result;
    }

    public static bool CheckVirticalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, int[] board) {
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