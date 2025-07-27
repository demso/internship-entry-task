using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Services;

namespace TickTackToe.Tests;

// [Fact] // Обычный тест
// [Trait("Category", "Integration")] // Категоризация
// [Theory] // Параметризованный тест
// [InlineData(1, 2, 3)] 
// [MemberData(nameof(TestData))] // Данные из метода
// [ClassData(typeof(TestDataClass))] // Данные из класса
// [Fact(Timeout = 1000)] // Таймаут
// [Fact(Skip = "Reason")] // Пропуск теста

public class GameLogicTests {
    private readonly IGameService _gameService = new GameService();
    
    [Fact]
    public void CreateGame_ReturnsValidGame()
    {
        // Arrange
        var game = _gameService.CreateGame(3,3);
    
        // Assert
        Assert.NotNull(game);
        Assert.Equal(Player.X, game.WhoseTurn);
        Assert.Equal(0, game.TurnNumber);
        Assert.Equal(GameResult.None, game.GameResult);
        Assert.Equal(GameState.InProgress, game.GameState);
        Assert.Equal(3, game.WinCondition);
        Assert.Equal(3, game.BoardSize);
        Assert.Equal(3, game.Board.Length);
        Assert.Equal(3, game.Board[0].Length);
        Assert.False(CheckAllWinCondition(game.WinCondition, game.Board));
        Assert.False(_gameService.CheckDraw(game.Board));
        Assert.All(game.Board, row => Assert.All(row, cell => Assert.Equal("", cell))); 
    }
    [Fact]
    public void CheckWinCondition_ValidMove_NoWin()
    {
        // Arrange
        var game = _gameService.CreateGame(3, 3);
        var row = 1;
        var column = 1;
    
        // Act
        game.Board[row][column] = Player.X.ToString();
    
        // Assert
        Assert.Equal(Player.X.ToString(), game.Board[row][column]);
        Assert.False(CheckAllWinCondition(game.WinCondition, game.Board));
        Assert.False(_gameService.CheckDraw(game.Board));
    }
    [Fact]
    public void CheckWinCondition_Win()
    {
        // Assert
        Assert.True(CheckAllWinCondition(3, new string[][]{ 
            ["", "X", ""], 
            ["O", "X", ""], 
            ["", "X", ""]
        }));
        Assert.True(CheckAllWinCondition(3, new string[][] {
            ["",  "", "X", "", "X"], 
            ["O",  "", "",  "", ""], 
            ["",  "", "X", "", ""], 
            ["",  "", "X", "", "O"], 
            ["X", "", "X", "", ""]
        }));
        Assert.True(CheckAllWinCondition(3, new string[][] {
            ["",  "",  "X"],
            ["O", "O", "O"],
            ["X",  "",  ""]
        }));
        Assert.True(CheckAllWinCondition(4, new string[][] {
            ["",  "",  "",  "O",  ""],
            ["",  "",  "",  "",  ""],
            ["X",  "X",  "X",  "X",  ""],
            ["",  "",  "",  "",  ""],
            ["",  "",  "",  "",  ""] 
        }));
        Assert.True(CheckAllWinCondition(3, new string[][] {
            ["", "", "O"],
            ["", "O", ""],
            ["O", "", ""]
        }));
        Assert.True(CheckAllWinCondition(3, new string[][] {
            ["X", "", "X"],
            ["", "X", ""],
            ["O", "", "X"]
        }));
        Assert.True(CheckAllWinCondition(3, new string[][] {
            ["",  "",  "",  "",  ""],
            ["",  "",  "",  "X",  ""],
            ["",  "",  "X",  "",  ""],
            ["",  "X",  "",  "",  ""],
            ["X",  "",  "",  "",  ""]
        }));
        Assert.True(CheckAllWinCondition(3 , new string[][] {
            ["", "", "", "", ""],
            ["", "X", "", "", ""],
            ["", "", "X", "", ""],
            ["", "", "", "X", ""],
            ["", "", "", "", ""]
        }));
    }
    [Fact]
    public void CheckWinCondition_NoWin() {
        //Assert
        Assert.False(CheckAllWinCondition(3, new string[][] {
            ["X", "X", ""],
            ["O", "X", ""],
            ["O", "O", ""]
        }));
        Assert.False(CheckAllWinCondition(3, new string[][] {
            ["", "", "O"],
            ["", "", ""],
            ["O", "", ""]
        }));
        Assert.False(CheckAllWinCondition(3, new string[][] {
            ["", "", "X"],
            ["", "X", ""],
            ["O", "", "X"]
        }));
        Assert.False(CheckAllWinCondition(3, new string[][] {
            ["",  "",  "",  "",  ""],
            ["",  "",  "",  "X",  ""],
            ["",  "",  "X",  "",  ""],
            ["",  "",  "",  "",  ""],
            ["X",  "",  "",  "",  ""]
        }));
        Assert.False(CheckAllWinCondition(3 , new string[][] {
            ["", "", "", "", ""],
            ["", "X", "", "", ""],
            ["", "", "", "", ""],
            ["", "", "", "X", ""],
            ["", "", "", "", ""]
        }));
    }

    [Fact]
    public void CheckDrawCondition_Draw() {
        Assert.True(_gameService.CheckDraw(new string[][]{ 
            ["X", "X", "O"], 
            ["O", "X", "O"], 
            ["X", "O", "X"]
        }));
        Assert.True(_gameService.CheckDraw(new string[][]{ 
            ["X", "O", "O", "X", "O"],
            ["X", "O", "X", "O", "X"],
            ["O", "X", "X", "O", "X"],
            ["X", "O", "O", "X", "O"],
            ["X", "O", "X", "O", "X"]
        }));
    }
    
    [Fact]
    public void CheckDrawCondition_NoDraw() {
        Assert.False(_gameService.CheckDraw(new string[][]{ 
            ["", "X", ""], 
            ["O", "X", ""], 
            ["", "X", ""]
        }));
        Assert.False(_gameService.CheckDraw(new string[][]{ 
            ["", "", "", "", ""],
            ["", "X", "", "", ""],
            ["", "", "X", "", ""],
            ["", "", "", "X", ""],
            ["", "", "", "", ""]
        }));
        Assert.False(_gameService.CheckDraw(new string[][]{ 
            ["",  "",  "X"],
            ["O", "O", "O"],
            ["X",  "",  ""]
        }));
    }
    
    //проверить есть ли выигрышная комбинация в какой-либо клетке игры
    private bool CheckAllWinCondition(int winCondition, string[][] board) {
        for (var row = 0; row < board.Length; row++) {
            for (var column = 0; column < board.Length; column++) {
                if (_gameService.CheckWinCondition(row, column, Player.X.ToString(), winCondition, board.Length, board))
                    return true;
                if (_gameService.CheckWinCondition(row, column, Player.O.ToString(), winCondition, board.Length, board))
                    return true;
            }
        }
        return false;
    }
}