using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Enums;
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
    private readonly GameService _gameService;
    
    public GameLogicTests()
    {
        _gameService = new GameService();
    }
    
    //проверить есть ли выигрышная комбинация в какой-либо клетке игры
    bool CheckAllWinCondition(Game game) {
        for (var row = 0; row < game.BoardSize; row++) {
            for (var column = 0; column < game.BoardSize; column++) {
                if (_gameService.CheckWinCondition(row, column, game.WhoseTurn.ToString(), game.WinCondition, game.BoardSize, game.Board))
                    return true;
            }
        }
        return false;
    }
    
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
        Assert.False(CheckAllWinCondition(game));
        Assert.False(_gameService.CheckDraw(game.Board));
        Assert.All(game.Board, row => Assert.All(row, cell => Assert.Equal("", cell))); 
    }
    // [Fact]
    // public void CheckWinCondition_ValidTurn_NoWin()
    // {
    //     // Arrange
    //     var board1 = new string[][] {
    //         ["", "X", ""],
    //         ["O", "X", ""],
    //         ["", "X", ""]
    //     };
    //
    //     var board2 = new string[][] {
    //         ["", "", "X", "", "X"],
    //         ["O", "", "", "", ""],
    //         ["", "", "X", "", ""],
    //         ["", "", "X", "", "O"],
    //         ["X", "", "X", "", ""]
    //     };
    //     
    //     var board3 = 
    //     
    //     var game = _gameService.CreateGame(3, 3);
    //     var row = 1;
    //     var column = 1;
    //
    //     // Act
    //     game.Board[row][column] = Player.X.ToString();
    //     game.WhoseTurn = Player.O;
    //     game.TurnNumber += 1;
    //
    //     // Assert
    //     Assert.Equal(Player.X.ToString(), game.Board[row][column]);
    //     Assert.Equal(Player.O, game.WhoseTurn);
    //     Assert.Equal(1, game.TurnNumber);
    //     Assert.Equal(GameResult.None, game.GameResult);
    //     Assert.Equal(GameState.InProgress, game.GameState);
    //     Assert.False(CheckAllWinCondition(game));
    //     Assert.False(_gameService.CheckDraw(game.Board));
    // }
    //
    // [Fact]
    // public void MakeMove_WrongPlayer_ThrowsBusinessValidationException()
    // {
    //     // Arrange
    //     var game = _gameService.CreateNewGame(); // CurrentPlayer = "X"
    //     var position = new Position
    //     {
    //         Column = 0,
    //         Row = 0
    //     };
    //
    //     // Act & Assert
    //     var exception = Assert.Throws<BusinessValidationException>(() =>
    //         _gameService.MakeMove(game, "O", position));
    //     Assert.Equal(400, exception.StatusCode);
    //     Assert.Contains(exception.Errors, e => e.PropertyName == "Player" && e.ErrorMessage == "Not your turn.");
    // }
}