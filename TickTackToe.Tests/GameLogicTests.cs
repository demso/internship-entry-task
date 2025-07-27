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
    // private readonly GameService _gameService;
    //
    // public GameLogicTests()
    // {
    //     _gameService = new GameService();
    // }
    //
    // [Fact]
    // public void CreateNewGame_ReturnsValidGame()
    // {
    //     // Act
    //     var game = _gameService.CreateNewGame();
    //
    //     // Assert
    //     Assert.NotEqual(Guid.Empty, game.Id);
    //     Assert.Equal("X", game.CurrentPlayer);
    //     Assert.Equal(0, game.MoveCount);
    //     Assert.Null(game.Winner);
    //     Assert.False(game.IsDraw);
    //     Assert.Equal(3, game.Board.Length);
    //     Assert.All(game.Board, row => Assert.All(row, cell => Assert.Null(cell))); 
    // }
    // [Fact]
    // public void MakeMove_ValidMove_UpdatesGameState()
    // {
    //     // Arrange
    //     var game = _gameService.CreateNewGame();
    //     var position = new Position
    //     {
    //         Column = 0,
    //         Row = 0
    //     };
    //     
    //
    //     // Act
    //     _gameService.MakeMove(game, "X", position);
    //
    //     // Assert
    //     Assert.Equal("X", game.Board[0][0]);
    //     Assert.Equal("O", game.CurrentPlayer);
    //     Assert.Equal(1, game.MoveCount);
    //     Assert.Null(game.Winner);
    //     Assert.False(game.IsDraw);
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